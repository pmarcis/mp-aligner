//
//  Program.cs
//
//  Author:
//       Mārcis Pinnis <marcis.pinnis@gmail.com>
//
//  Copyright (c) 2013 Mārcis Pinnis
//
//  This program can be freely used only for scientific and educational purposes.
//
//  This program is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using MPAligner;

namespace CreateResources
{
	class MainClass
	{
		static NumberFormatInfo nfi;

		static void Main(string[] args)
		{
			nfi = new NumberFormatInfo();
			nfi.CurrencyDecimalSeparator = ".";
			nfi.NumberDecimalSeparator = ".";
			nfi.PercentDecimalSeparator = ".";
			string dictionaryFile = null;
			string srcStopWordFile = null;
			string trgStopWordFile = null;
			string filteredDictionaryFile = null;
			Dictionary<string, bool> srcStop = null;
			Dictionary<string, bool> trgStop = null;
			double threshold = 0;
			string outputFile = null;
			string excOutputFile = null;
			string sourceLang = null;
			string targetLang = null;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "-ot" && args.Length > i + 1)
				{
					outputFile = args[i + 1];
				}
				else if (args[i] == "-oe" && args.Length > i + 1)
				{
					excOutputFile = args[i + 1];
				}
				else if (args[i] == "-od" && args.Length > i + 1)
				{
					filteredDictionaryFile = args[i + 1];
				}
				else if (args[i] == "-sl" && args.Length > i + 1)
				{
					sourceLang = args[i + 1];
				}
				else if (args[i] == "-tl" && args.Length > i + 1)
				{
					targetLang = args[i + 1];
				}
				else if (args[i] == "-i" && args.Length > i + 1)
				{
					dictionaryFile = args[i + 1];
				}
				else if (args[i] == "-sStop" && args.Length > i + 1)
				{
					srcStopWordFile = args[i + 1];
					srcStop = ReadStopWords(srcStopWordFile);
				}
				else if (args[i] == "-tStop" && args.Length > i + 1)
				{
					trgStopWordFile = args[i + 1];
					trgStop = ReadStopWords(trgStopWordFile);
				}
				else if (args[i] == "-t" && args.Length > i + 1)
				{
					threshold = Convert.ToDouble(args[i + 1], nfi);
				}
			}
			
			if (string.IsNullOrWhiteSpace(outputFile)
			    ||string.IsNullOrWhiteSpace(excOutputFile)
			    ||string.IsNullOrWhiteSpace(filteredDictionaryFile)
			    ||string.IsNullOrWhiteSpace(sourceLang)
			    ||string.IsNullOrWhiteSpace(targetLang)
			    ||string.IsNullOrWhiteSpace(dictionaryFile)
			    ||string.IsNullOrWhiteSpace(srcStopWordFile)
			    ||string.IsNullOrWhiteSpace(trgStopWordFile)) {
				Console.WriteLine ("Usage: ./CreateResources.exe -i ./gizadict-in/lex.e2f -sl bg -tl en -ot ./bg-en-translit-raw-out.txt -oe ./bg-en-exc-dict-out -sStop ./STOP_BG.txt -tStop ./STOP_EN.txt -t 0.1 -od ./bg-en-filtered-dict-out");
				return;
			}

			//Simple style for transliteration purposes starts here
			Encoding utf8WithoutBom = new UTF8Encoding(false);
			StreamWriter sw = new StreamWriter(outputFile, false, utf8WithoutBom);
			sw.NewLine = "\n";
			StreamWriter swe = new StreamWriter(excOutputFile, false, utf8WithoutBom);
			swe.NewLine = "\n";
			StreamWriter swo = new StreamWriter(filteredDictionaryFile, false, utf8WithoutBom);
			swo.NewLine = "\n";
			if (!stemDictionary.ContainsKey(sourceLang))
			{
				stemDictionary.Add(sourceLang, new Dictionary<string, string>());
			}
			if (!stemDictionary.ContainsKey(targetLang))
			{
				stemDictionary.Add(targetLang, new Dictionary<string, string>());
			}
			StreamReader sr = new StreamReader(dictionaryFile, Encoding.UTF8);
			Console.Error.WriteLine("Reading dictionary file.");
			char[] sep = { ' ', '\t' };
			int counter = 0;

			Dictionary<string, Dictionary<string, bool>> srcStems = new Dictionary<string, Dictionary<string, bool>>();
			Dictionary<string, Dictionary<string, bool>> trgStems = new Dictionary<string, Dictionary<string, bool>>();
			Dictionary<string, Dictionary<string, bool>> srcToTrgStems = new Dictionary<string, Dictionary<string, bool>>();
			int writtenLines = 0;

			while (!sr.EndOfStream)
			{
				counter++;
				if (counter % 10000 == 0)
				{
					Console.Error.Write(".");
					if (counter % 500000 == 0)
					{
						Console.Error.WriteLine(" - " + counter.ToString());
					}
				}
				string line = sr.ReadLine().Trim();
				if (!string.IsNullOrWhiteSpace(line))
				{
					string[] arr = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
					if (arr!=null && arr.Length==3)
					{
						string srcText = arr[0].ToLower().Trim();
						string trgText = arr[1].ToLower().Trim();
						if (string.IsNullOrWhiteSpace(srcText) || string.IsNullOrWhiteSpace(trgText)) continue;
						if (!IsValidPhrase(srcText, sourceLang, trgText, targetLang)) continue;
						try
						{
							if (Convert.ToDouble(arr[2], nfi) >= threshold && arr[0] != arr[1])
							{
								if (srcStop == null || trgStop == null || (srcStop.ContainsKey(arr[0].ToLower()) && trgStop.ContainsKey(arr[1].ToLower())) || (!srcStop.ContainsKey(arr[0].ToLower()) && !trgStop.ContainsKey(arr[1].ToLower())))
								{
									if (Math.Min(arr[0].Length, arr[1].Length) < 4) //Just a precaution in order to filter out possible false alignments (no linguistic proof, but seems to improve quality).
									{
										if (Math.Min(arr[0].Length, arr[1].Length) / Math.Max(arr[0].Length, arr[1].Length) >= 0.6)
										{
											swo.WriteLine(line);
										}
									}
									else
									{
										swo.WriteLine(line);
									}
								}
							}
						}
						catch
						{
							Console.WriteLine("Error at line " + counter.ToString());
							continue;
						}
						string srcSimple = SimpleCharacterTransliteration.Transliterate(Stemmer(srcText,sourceLang));
						if (string.IsNullOrWhiteSpace(srcSimple)) continue;
						if (!srcStems.ContainsKey(srcSimple))
						{
							srcStems.Add(srcSimple, new Dictionary<string, bool>());
							srcStems[srcSimple].Add(srcText, true);
						}

						if (!srcToTrgStems.ContainsKey(srcSimple))
						{
							srcToTrgStems.Add(srcSimple, new Dictionary<string, bool>());
						}
						string trgSimple = SimpleCharacterTransliteration.Transliterate(Stemmer(trgText, targetLang));
						if (string.IsNullOrWhiteSpace(trgSimple)) continue;
						if (!trgStems.ContainsKey(trgSimple))
						{
							trgStems.Add(trgSimple, new Dictionary<string, bool>());
							trgStems[trgSimple].Add(trgText, true);
						}
						if (!srcToTrgStems[srcSimple].ContainsKey(trgSimple))
						{
							srcToTrgStems[srcSimple].Add(trgSimple, true);
						}
						string srcTransl = SimpleCharacterTransliteration.Transliterate(srcText);
						string trgTransl = SimpleCharacterTransliteration.Transliterate(trgText);
						double levenshtainDistance = LevenshteinDistance.Compute(srcTransl, trgTransl);
						double maxLen = Math.Max(srcTransl.Length, trgTransl.Length);
						double similarity = (maxLen - levenshtainDistance) / maxLen;
						if (srcText == trgText) continue;
						if ((similarity >= 0.7 || similarity >= 0.5 && sourceLang == "el" || similarity >= 0.5 && sourceLang == "ru"))
						{
							if (srcText.Replace(".", "").Length > 3 && trgText.Replace(".", "").Length > 3)
							{
								sw.WriteLine(srcText + "\t" + trgText);
								writtenLines++;
							}
						}
					}
				}
			}
			counter = 0;

			sw.Close();
			swo.Close();
			sr.Close();
			Console.Error.WriteLine();
			Console.Error.WriteLine("Dict file read successfully.");

			SplitTranslitRes(outputFile, sourceLang, targetLang, writtenLines);
			Console.Error.WriteLine();
			Console.Error.WriteLine("Tuning and Training sets split.");

			//Simple style for transliteration purposes ends here

			foreach (string srcStem in srcStems.Keys)
			{
				counter++;
				if (counter % 100 == 0)
				{
					Console.Write(".");
					if (counter % 5000 == 0)
					{
						Console.WriteLine(" " + counter.ToString());
					}
				}
				foreach (string trgStem in trgStems.Keys)
				{
					if (!srcToTrgStems[srcStem].ContainsKey(trgStem) && srcStem[0]==trgStem[0])
					{
						double levenshtainDistance = LevenshteinDistance.Compute(srcStem, trgStem);
						double maxLen = Math.Max(srcStem.Length, trgStem.Length);
						double similarity = (maxLen - levenshtainDistance) / maxLen;
						if (similarity >= 0.6)
						{
							foreach(string srcText in srcStems[srcStem].Keys)
							{
								foreach(string trgText in trgStems[trgStem].Keys)
								{
									swe.WriteLine(srcText + "\t" + trgText + "\t" + similarity.ToString("0.00",nfi));
								}
							}
						}
					}
				}
			}
			swe.Close();
		}

		public static void SplitTranslitRes(string inputFile, string srcLang, string trgLang, int count)
		{
			Encoding utf8WithoutBom = new UTF8Encoding(false);
			StreamWriter trainSrcOut = new StreamWriter(inputFile + ".train." + srcLang, false, utf8WithoutBom);
			trainSrcOut.NewLine = "\n";
			StreamWriter trainTrgOut = new StreamWriter(inputFile + ".train." + trgLang, false, utf8WithoutBom);
			trainTrgOut.NewLine = "\n";
			StreamWriter tuneSrcOut = new StreamWriter(inputFile + ".tune." + srcLang, false, utf8WithoutBom);
			tuneSrcOut.NewLine = "\n";
			StreamWriter tuneTrgOut = new StreamWriter(inputFile + ".tune." + trgLang, false, utf8WithoutBom);
			tuneTrgOut.NewLine = "\n";
			Dictionary<int,bool> randomRows = GetRandom10Percent(count);
			StreamReader sr = new StreamReader(inputFile, Encoding.UTF8);
			char[] sep = { '\t' };
			int counter = 0;
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine().Trim();
				string[] arr = line.Split(sep, StringSplitOptions.None);
				if (arr.Length == 2)
				{
					if (randomRows.ContainsKey(counter))
					{
						//For tuning set.
						tuneSrcOut.WriteLine(GetSpacedWord(arr[0]));
						tuneTrgOut.WriteLine(GetSpacedWord(arr[1]));
					}
					else
					{
						//For training set.
						trainSrcOut.WriteLine(GetSpacedWord(arr[0]));
						trainTrgOut.WriteLine(GetSpacedWord(arr[1]));
					}
				}
				counter++;
			}

			sr.Close();
			trainSrcOut.Close();
			trainTrgOut.Close();
			tuneSrcOut.Close();
			tuneTrgOut.Close();
		}

		private static string GetSpacedWord(string p)
		{
			StringBuilder res = new StringBuilder();
			for (int i = 0; i < p.Length; i++)
			{
				if (i == 0) res.Append(p[i]);
				else
				{
					res.Append(" ");
					res.Append(p[i]);
				}
			}
			return res.ToString();
		}

		private static Dictionary<int, bool> GetRandom10Percent(int count)
		{
			Dictionary<int, bool> res = new Dictionary<int, bool>();
			int tuningSetCount = count / 10;
			if (tuningSetCount > 2000) tuningSetCount = 2000;
			Random r = new Random(DateTime.Now.Millisecond);
			while (res.Count < tuningSetCount)
			{
				int num = r.Next(0, count);
				if (!res.ContainsKey(num)) res.Add(num, true);
			}
			return res;
		}

		public static Dictionary<string, Dictionary<string, string>> stemDictionary = new Dictionary<string, Dictionary<string, string>>();

		public static string Stemmer(string lowerCaseTerm, string language)
		{
			//if (!stemDictionary.ContainsKey(language))
			//{
			//    stemDictionary.Add(language, new Dictionary<string, string>());
			//}
			Stemmer stemmer = new RudeStemmer();
			if (language == "en")
			{
				stemmer = new ENLightStemmer();
			}
			else if (language == "lv")
			{
				stemmer = new LVLightStemmer();
			}
			else if (language == "ro")
			{
				stemmer = new ROLightStemmer();
			}
			else if (language == "lt")
			{
				stemmer = new LTLightStemmer();
			}
			else if (language == "et")
			{
				stemmer = new ETLightStemmer();
			}
			else if (language == "de")
			{
				stemmer = new DELightStemmer();
			}
			char[] sep = { ' ' };
			string res = "";
			string[] stringList = lowerCaseTerm.Split(sep, StringSplitOptions.RemoveEmptyEntries);
			foreach (string str in stringList)
			{
				string stem = "";
				if (stemDictionary[language].ContainsKey(str))
				{
					stem = stemDictionary[language][str];
				}
				else
				{
					stem = stemmer.StemString(str);
					stemDictionary[language].Add(str, stem);
				}
				if (string.IsNullOrWhiteSpace(res))
				{
					res = stem;
				}
				else
				{
					res += " " + stem;
				}
			}
			return res;
		}

		private static bool IsValidPhrase(string src, string srcLang, string trg, string trgLang)
		{
			if (IsValidPhrase(src, srcLang) && IsValidPhrase(trg, trgLang))
			{
				return true;
			}
			return false;
		}

		static Dictionary<string, bool> warningsPrinted = new Dictionary<string, bool>();
		private static bool IsValidPhrase(string p, string language)
		{
			string baseAlphabet = "";
			switch (language.ToUpper())
			{
				case "BG":
				baseAlphabet = ValidAlphabets.BG;
				break;
				case "CS":
				baseAlphabet = ValidAlphabets.CS;
				break;
				case "CA":
				baseAlphabet = ValidAlphabets.CA;
				break;
				case "CY":
				baseAlphabet = ValidAlphabets.CY;
				break;
				case "DA":
				baseAlphabet = ValidAlphabets.DA;
				break;
				case "DE":
				baseAlphabet = ValidAlphabets.DE;
				break;
				case "EL":
				baseAlphabet = ValidAlphabets.EL;
				break;
				case "EN":
				baseAlphabet = ValidAlphabets.EN;
				break;
				case "ES":
				baseAlphabet = ValidAlphabets.ES;
				break;
				case "ET":
				baseAlphabet = ValidAlphabets.ET;
				break;
				case "EU":
				baseAlphabet = ValidAlphabets.EU;
				break;
				case "FI":
				baseAlphabet = ValidAlphabets.FI;
				break;
				case "FR":
				baseAlphabet = ValidAlphabets.FR;
				break;
				case "GA":
				baseAlphabet = ValidAlphabets.GA;
				break;
				case "GD":
				baseAlphabet = ValidAlphabets.GD;
				break;
				case "GL":
				baseAlphabet = ValidAlphabets.GL;
				break;
				case "HR":
				baseAlphabet = ValidAlphabets.HR;
				break;
				case "HU":
				baseAlphabet = ValidAlphabets.HU;
				break;
				case "HI":
				baseAlphabet = ValidAlphabets.HI;
				break;
				case "IT":
				baseAlphabet = ValidAlphabets.IT;
				break;
				case "LT":
				baseAlphabet = ValidAlphabets.LT;
				break;
				case "LV":
				baseAlphabet = ValidAlphabets.LV;
				break;
				case "MT":
				baseAlphabet = ValidAlphabets.MT;
				break;
				case "NL":
				baseAlphabet = ValidAlphabets.NL;
				break;
				case "PL":
				baseAlphabet = ValidAlphabets.PL;
				break;
				case "PT":
				baseAlphabet = ValidAlphabets.PT;
				break;
				case "RO":
				baseAlphabet = ValidAlphabets.RO;
				break;
				case "RU":
				baseAlphabet = ValidAlphabets.RU;
				break;
				case "SK":
				baseAlphabet = ValidAlphabets.SK;
				break;
				case "SL":
				baseAlphabet = ValidAlphabets.SL;
				break;
				case "SV":
				baseAlphabet = ValidAlphabets.SV;
				break;
				case "TR":
				baseAlphabet = ValidAlphabets.TR;
				break;
				case "UR":
				baseAlphabet = ValidAlphabets.UR;
				break;
			}
			if (!warningsPrinted.ContainsKey("phraseValidation") && string.IsNullOrWhiteSpace(baseAlphabet))
			{
				warningsPrinted.Add("phraseValidation", true);
				Console.Error.WriteLine("Phrase validation not supported for the language " + language);
				throw new ArgumentException("Phrase validation not supported for the language " + language);
				//Console.Error.WriteLine("Using default EN instead");
			}
			int spaceCount = 0;
			int punctCount = 0;
			bool wasNonSpace = false;
			foreach (char c in p)
			{
				if (wasNonSpace && Char.IsWhiteSpace(c)) spaceCount++;
				else wasNonSpace = true;
				if (Char.IsPunctuation(c)) punctCount++;
				if (baseAlphabet.IndexOf(c) < 0 && ValidAlphabets.punctuationsForTermsPhraseTableFiltering.IndexOf(c) < 0 )//&& ValidAlphabets.EN.IndexOf(c) < 0)
				{
					return false;
				}
			}
			if (spaceCount > 0 || punctCount > 1)
			{
				return false;
			}
			return true;
		}

		private static Dictionary<string, bool> ReadStopWords(string srcStopWordListFile)
		{
			Dictionary<string, bool> res = new Dictionary<string, bool>();
			StreamReader sr = new StreamReader(srcStopWordListFile, Encoding.UTF8);
			while (!sr.EndOfStream)
			{
				string line = sr.ReadLine().Trim();
				if (!res.ContainsKey(line.ToLower())) res.Add(line.ToLower(), true);
			}
			sr.Close();
			return res;
		}
	}
}
