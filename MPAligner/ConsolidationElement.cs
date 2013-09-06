//
//  ConsolidationElement.cs
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
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace MPAligner
{
	public class ConsolidationElement
	{
		public ConsolidationElement ()
		{
		}

		public string line;
		public double prob;

		public static void ConsolidateRefTabsep(string inputFile, string outputFile, double threshold)
		{
			NumberFormatInfo nfi = new NumberFormatInfo ();
			nfi.CurrencyDecimalSeparator = ".";
			nfi.NumberDecimalSeparator = ".";
			nfi.PercentDecimalSeparator = ".";
			StreamReader sr = new StreamReader (inputFile, Encoding.UTF8);
			StreamWriter sw = new StreamWriter (outputFile, false, new UTF8Encoding (false));
			sw.NewLine = "\n";
			Dictionary<string,Dictionary<string,int>> added = new Dictionary<string, Dictionary<string, int>> ();
			// SRC -> TRG -> ElemList
			Dictionary<string,Dictionary<string,List<ConsolidationElement>>> lemmaDict = new Dictionary<string, Dictionary<string, List<ConsolidationElement>>> ();
			char[] sep = {'\t'};
			while (!sr.EndOfStream) {
				string line = sr.ReadLine ();
				if (!string.IsNullOrWhiteSpace (line)) {
					string[] dataArr = line.Split (sep, StringSplitOptions.None);
					if (dataArr.Length == 20) {
						double prob = Convert.ToDouble (dataArr [6], nfi);
						if (prob >= threshold-0.1) {
							string srcKey = dataArr [1] + dataArr [7] + dataArr [8];
							string trgKey = dataArr [3] + dataArr [12] + dataArr [13];
							string srcLemmaKey = System.Net.WebUtility.HtmlDecode(dataArr [8]);
							string trgLemmaKey = System.Net.WebUtility.HtmlDecode(dataArr [13]);
							if (srcLemmaKey.Length<3||trgLemmaKey.Length<3) continue;
							if (!IsValidLemma(srcLemmaKey)||!IsValidLemma(trgLemmaKey)) continue;
							if (!added.ContainsKey (srcKey)) {
								added.Add (srcKey, new Dictionary<string, int> ());
							}
							if (!added [srcKey].ContainsKey (trgKey)) {
								added [srcKey].Add (trgKey, 1);
								//sw.WriteLine (line);
								if (!lemmaDict.ContainsKey (srcLemmaKey)) {
									lemmaDict.Add (srcLemmaKey, new Dictionary<string, List<ConsolidationElement>> ());
								}
								if (!lemmaDict [srcLemmaKey].ContainsKey (trgLemmaKey)) {
									lemmaDict [srcLemmaKey].Add (trgLemmaKey, new List<ConsolidationElement> ());
								}
								ConsolidationElement ce = new ConsolidationElement ();
								ce.line = line;
								ce.prob = prob;
								lemmaDict [srcLemmaKey] [trgLemmaKey].Add (ce);
							} else {
								added [srcKey] [trgKey]++;
							}
						}
					}
				}
			}

			List<string> srcLemmas = new List<string> (lemmaDict.Keys);
			srcLemmas.Sort ();
			foreach (string srcLemma in srcLemmas) {
				double max = Double.MinValue;
				Dictionary<string,double> avgProbDict = new Dictionary<string, double>();
				List<string> trgLemmas = new List<string>();
				foreach(string trgLemma in lemmaDict[srcLemma].Keys)
				{
					trgLemmas.Add(trgLemma);
					double sum=0;
					double count=0;
					foreach(ConsolidationElement ce in lemmaDict[srcLemma][trgLemma])
					{
						sum+=ce.prob;
						count++;
					}
					double score = 0;
					if (count>0)
					{
						score = sum/count;
						if (score>max) max = score;
					}
					avgProbDict.Add(trgLemma,score);
				}
				trgLemmas.Sort();
				double minThr = max - 0.05;
				foreach(string trgLemma in trgLemmas)
				{
					if (avgProbDict[trgLemma]>=minThr&&avgProbDict[trgLemma]>=threshold)
					{
						List<string> lines = new List<string>();
						foreach(ConsolidationElement ce in lemmaDict[srcLemma][trgLemma])
						{
							lines.Add(ce.line);
						}
						lines.Sort();
						foreach(string line in lines)
						{
							sw.WriteLine(System.Net.WebUtility.HtmlDecode(line));
						}
					}
				}
			}

			sw.Close ();
			sr.Close ();
		}

		static bool IsValidLemma (string lemma)
		{
			if (!string.IsNullOrWhiteSpace (lemma)) {
				foreach (char c in lemma)
				{
					if (Char.IsLetter(c)) continue;
					if (Char.IsDigit(c)) continue;
					if (Char.IsWhiteSpace(c)) continue;
					if (c=='-') continue;
					if (c=='‒') continue;
					if (c=='–') continue;
					if (c=='—') continue;
					if (c=='―') continue;
					if (c=='\'') continue;
					if (Char.IsControl(c)) return false;
					if (Char.IsPunctuation(c)) return false;
				}
				if (lemma.Length > 0 && (Char.IsControl (lemma [0]) || Char.IsPunctuation(lemma [0])||Char.IsControl (lemma [lemma.Length-1]) || Char.IsPunctuation(lemma [lemma.Length-1])))
					return false;
				return true;
			}
			return false;
		}
	}
}

