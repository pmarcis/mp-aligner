//
//  ProcessedTermEntry.cs
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
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using MPFramework;
using Amib.Threading;

namespace MPAligner
{
    public class ProcessedTermEntry
    {
        public ProcessedTermEntry()
        {
            len=0;
            surfaceForm = "";
            lowercaceForm = "";
			concordance = "";
			normSeq = new List<string> ();
			normMsdSeq = new List<string> ();
            translationList = new List<List<StringProbabEntry>>();
            transliterationList = new List<List<StringProbabEntry>>();
            lowercaseWords = new List<string>();
            simpleTransliteration = new List<string>();
            msdSeq = new List<string>();
            lemmaSeq = new List<string>();
        }
        
        private static Regex whitespaceRegex = new Regex("\\s+");
        
        public string surfaceForm;
        public string lowercaceForm;
		public string concordance;
        public int len;
        public List<string> lemmaSeq;
		public List<string> msdSeq;
		public List<string> normSeq;
        public List<string> normMsdSeq;
        public List<string> lowercaseWords;
        public List<string> surfaceFormWords;
        public List<string> simpleTransliteration;
        public List<List<StringProbabEntry>> transliterationList;
        public List<List<StringProbabEntry>> translationList;
        public static Dictionary<string,Dictionary<string,List<StringProbabEntry>>> translitTemp = new Dictionary<string, Dictionary<string, List<StringProbabEntry>>>();

        public static List<ProcessedTermEntry> ProcessTermsList (List<string> terms, Dictionary<string, Dictionary<string, double>> srcToTrgDict, string lang, MPAlignerConfigurationTranslEntry translitEntry, string mosesPath, string tempFilePath, int threadCount = 1 , bool stemWords = false)
        {
            List<ProcessedTermEntry> res = new List<ProcessedTermEntry>(1000);
            Dictionary<string, int> lowercasedWordDict = new Dictionary<string, int>(1000);
            if (terms!=null)
            {
                string langKey = translitEntry!=null?((translitEntry.srcLang!=null?translitEntry.srcLang:"")+"_"+(translitEntry.trgLang!=null?translitEntry.trgLang:"")):lang;
                if (!translitTemp.ContainsKey(langKey)) translitTemp.Add(langKey,new Dictionary<string, List<StringProbabEntry>>());
                foreach(string surfaceForm in terms)
                {
                    string lowerCase = surfaceForm.ToLower();
                    ProcessedTermEntry pte = new ProcessedTermEntry();
                    pte.surfaceForm = surfaceForm;
                    pte.lowercaceForm = lowerCase;
                    pte.surfaceFormWords = new List<string>(whitespaceRegex.Split(surfaceForm));
                    string[] lowerCaseWordArr = whitespaceRegex.Split(lowerCase);
                    pte.lowercaseWords.InsertRange(0,lowerCaseWordArr);
                    
                    foreach(string word in lowerCaseWordArr)
                    {
                        pte.len+=word.Length;
                        if (!lowercasedWordDict.ContainsKey(word) && !translitTemp[langKey].ContainsKey(word))
                        {
                            lowercasedWordDict.Add(word,0);
                        }
                        string stem = null;
                        if (stemWords)
                        {
                            stem = LightweightStemmer.Stem(word,lang);
                        }
                        //if (lang !="en")
                        //{
                        pte.simpleTransliteration.Add(SimpleCharacterTransliteration.Transliterate(word));
                        //}
                        //else
                        //{
                        //    pte.simpleTransliteration = pte.lowercaseWords;
                        //}
                        if (srcToTrgDict!=null)
                        {
                            List<StringProbabEntry> currList = new List<StringProbabEntry>();
                            if (stemWords)
                            {
                                if (srcToTrgDict.ContainsKey(stem))
                                {
                                    foreach(string trgStem in srcToTrgDict[stem].Keys)
                                    {
                                        StringProbabEntry spe = new StringProbabEntry();
                                        spe.str=trgStem;
                                        spe.probab = srcToTrgDict[stem][trgStem];
                                        currList.Add(spe);
                                    }
                                }
                            }
                            else
                            {
                                if (srcToTrgDict.ContainsKey(word))
                                {
                                    foreach(string trgWord in srcToTrgDict[word].Keys)
                                    {
                                        StringProbabEntry spe = new StringProbabEntry();
                                        spe.str=trgWord;
                                        spe.probab = srcToTrgDict[word][trgWord];
                                        currList.Add(spe);
                                    }
                                }
                            }
                            pte.translationList.Add(currList);
                        }
                    }
                    res.Add(pte);
                }
                Dictionary<string, List<StringProbabEntry>> translitDict = new Dictionary<string, List<StringProbabEntry>>();
                //if (threadCount<2)
                //{
                    translitDict = GetTransliterations(lowercasedWordDict,translitEntry, mosesPath, tempFilePath, threadCount);
                    //This is not nice, however necessary due to the multi-threaded execution - the temp list is not updated in the single-thread scenario
                //    Dictionary<string, List<StringProbabEntry>> tmp = new Dictionary<string, List<StringProbabEntry>>();
                //    CopyTranslits(translitDict,tmp, translitEntry);
                //}
                //else
                //{
                //    translitDict = GetTransliterationsMultiThreaded(lowercasedWordDict,translitEntry, mosesPath, tempFilePath, threadCount);
                //}
                for(int i=0; i<res.Count;i++)
                {
                    foreach(string word in res[i].lowercaseWords)
                    {
                        if (translitDict.ContainsKey(word))
                        {
                            res[i].transliterationList.Add(translitDict[word]);
                        }
                        else if (translitTemp.ContainsKey(langKey)&&translitTemp[langKey].ContainsKey(word))
                        {
                            res[i].transliterationList.Add(translitTemp[langKey][word]);
                        }
                        else
                        {
                            res[i].transliterationList.Add(new List<StringProbabEntry>());
                        }
                    }
                }
                //We add a simple data amount threshold in order not to overflow the memory ...
                if (translitTemp[langKey].Count>=50000)
                {
                    translitTemp[langKey].Clear();
                    GC.Collect();
                }
            }
            return res;
        }

        public static Dictionary<string, ProcessedTermEntry> ProcessTerms(Dictionary<string, SimpleTermEntry> terms, Dictionary<string,Dictionary<string,double>> srcToTrgDict, string lang, MPAlignerConfigurationTranslEntry translitEntry, string mosesPath, string tempFilePath, int threadCount = 1 , bool stemWords = false)
        {
            Dictionary<string, ProcessedTermEntry> res = new Dictionary<string, ProcessedTermEntry>(1000);
            Dictionary<string, int> lowercasedWordDict = new Dictionary<string, int>(1000);
            if (terms!=null)
            {
				Log.Write ("Starting pre-processing of "+terms.Count.ToString()+" "+ lang +" terms.",LogLevelType.LIMITED_OUTPUT);
                string langKey = translitEntry!=null?((translitEntry.srcLang!=null?translitEntry.srcLang:"")+"_"+(translitEntry.trgLang!=null?translitEntry.trgLang:"")):lang;
                if (!translitTemp.ContainsKey(langKey)) translitTemp.Add(langKey,new Dictionary<string, List<StringProbabEntry>>());
                foreach(string lowerCase in terms.Keys)
                {
                    string surfaceForm = terms[lowerCase].term;
                    if (!res.ContainsKey(lowerCase)) //TODO: Nothing to do, but be aware that here we allow only the first capitalization of a surface form ... we will ignore other capitalizations.
                    {
                        ProcessedTermEntry pte = new ProcessedTermEntry();
                        pte.surfaceForm = surfaceForm;
						pte.concordance = !string.IsNullOrWhiteSpace (terms [lowerCase].conc) ? terms [lowerCase].conc : "";
						pte.normMsdSeq = !string.IsNullOrWhiteSpace (terms [lowerCase].normMsdSeq) ? new List<string> (whitespaceRegex.Split (terms [lowerCase].normMsdSeq)) : new List<string> ();
						pte.normSeq = !string.IsNullOrWhiteSpace (terms [lowerCase].normSeq) ? new List<string>(whitespaceRegex.Split(terms[lowerCase].normSeq)):new List<string>();
                        pte.lowercaceForm = lowerCase;
						pte.surfaceFormWords = !string.IsNullOrWhiteSpace (surfaceForm) ? new List<string> (whitespaceRegex.Split (surfaceForm)) : new List<string> ();
						string[] lowerCaseWordArr = !string.IsNullOrWhiteSpace (lowerCase) ? whitespaceRegex.Split (lowerCase) : null;
                        if (lowerCaseWordArr!=null)
						{
							pte.lowercaseWords.InsertRange(0,lowerCaseWordArr);
						}
						if (!string.IsNullOrWhiteSpace(terms[lowerCase].lemmaSeq))
                        {
                            pte.lemmaSeq = new List<string>(whitespaceRegex.Split(terms[lowerCase].lemmaSeq));
                        }
                        else
                        {
                            pte.lemmaSeq = new List<string>();
                            for (int i=0;i<pte.lowercaseWords.Count;i++){pte.lemmaSeq.Add("");}
                        }
                        if (!string.IsNullOrWhiteSpace(terms[lowerCase].msdSeq))
                        {
                            pte.msdSeq = new List<string>(whitespaceRegex.Split(terms[lowerCase].msdSeq));
                        }
                        else
                        {
                            pte.msdSeq = new List<string>();
                            for (int i=0;i<pte.lowercaseWords.Count;i++){pte.msdSeq.Add("");}
                        }
                        foreach(string word in lowerCaseWordArr)
                        {
                            pte.len+=word.Length;
                            if (!lowercasedWordDict.ContainsKey(word) && !translitTemp[langKey].ContainsKey(word))
                            {
                                lowercasedWordDict.Add(word,0);
                            }
                            string stem = null;
                            if (stemWords)
                            {
                                stem = LightweightStemmer.Stem(word,lang);
                            }
                            //if (lang !="en")
                            //{
                                pte.simpleTransliteration.Add(SimpleCharacterTransliteration.Transliterate(word));
                            //}
                            //else
                            //{
                            //    pte.simpleTransliteration = pte.lowercaseWords;
                            //}
                            if (srcToTrgDict!=null)
                            {
                                List<StringProbabEntry> currList = new List<StringProbabEntry>();
                                if (stemWords)
                                {
                                    if (srcToTrgDict.ContainsKey(stem))
                                    {
                                        foreach(string trgStem in srcToTrgDict[stem].Keys)
                                        {
                                            StringProbabEntry spe = new StringProbabEntry();
                                            spe.str=trgStem;
                                            spe.probab = srcToTrgDict[stem][trgStem];
                                            currList.Add(spe);
                                        }
                                    }
                                }
                                else
                                {
                                    if (srcToTrgDict.ContainsKey(word))
                                    {
                                        foreach(string trgWord in srcToTrgDict[word].Keys)
                                        {
                                            StringProbabEntry spe = new StringProbabEntry();
                                            spe.str=trgWord;
                                            spe.probab = srcToTrgDict[word][trgWord];
                                            currList.Add(spe);
                                        }
                                    }
                                }
                                pte.translationList.Add(currList);
                            }
                            
                        }
                        res.Add(lowerCase,pte);
                    }
                }
                Dictionary<string, List<StringProbabEntry>> translitDict = new Dictionary<string, List<StringProbabEntry>>();
                //if (threadCount<2)
                //{
                    translitDict = GetTransliterations(lowercasedWordDict,translitEntry, mosesPath, tempFilePath,threadCount);
                    //Dictionary<string, List<StringProbabEntry>> tmp = new Dictionary<string, List<StringProbabEntry>>();
                    //CopyTranslits(translitDict,tmp, translitEntry);
                //}
                //else
                //{
                //    translitDict = GetTransliterationsMultiThreaded(lowercasedWordDict,translitEntry, mosesPath, tempFilePath, threadCount);
                //}
                foreach (string lowerCase in res.Keys)
                {
                    foreach(string word in res[lowerCase].lowercaseWords)
                    {
                        if (translitDict.ContainsKey(word))
                        {
                            res[lowerCase].transliterationList.Add(translitDict[word]);
                        }
                        else if (translitTemp.ContainsKey(langKey)&&translitTemp[langKey].ContainsKey(word))
                        {
                            res[lowerCase].transliterationList.Add(translitTemp[langKey][word]);
                        }
                        else
                        {
                            res[lowerCase].transliterationList.Add(new List<StringProbabEntry>());
                        }
                    }
                }
                //We add a simple data amount threshold in order not to overflow the memory ...
                if (translitTemp[langKey].Count>=25000)
                {
                    translitTemp[langKey].Clear();
                    GC.Collect();
                }
            }
            return res;
        }

        private static void CopyTranslits (Dictionary<string, List<StringProbabEntry>> fromDict, Dictionary<string, List<StringProbabEntry>> toDict, MPAlignerConfigurationTranslEntry translEntry)
        {
            string langKey = translEntry != null ? ((translEntry.srcLang != null ? translEntry.srcLang : "") + "_" + (translEntry.trgLang != null ? translEntry.trgLang : "")) : "";
            if (fromDict != null) {
                if (toDict == null)
                {
                    toDict = new Dictionary<string, List<StringProbabEntry>>();
                }
                foreach(string term in fromDict.Keys)
                {
                    if (!toDict.ContainsKey(term))
                    {
                        toDict.Add(term, fromDict[term]);
                        if (!translitTemp[langKey].ContainsKey(term)) translitTemp[langKey].Add(term, new List<StringProbabEntry>());
                        translitTemp[langKey][term]=fromDict[term];
                    }
                }
            }
        }
        
        
        public static Dictionary<string, List<StringProbabEntry>> GetTransliterations (Dictionary<string, int> lowerCasedTerms, MPAlignerConfigurationTranslEntry translEntry, string mosesPath, string tempFilePath, int threadCount)
        {    

            Dictionary<string, List<StringProbabEntry>> res = new Dictionary<string, List<StringProbabEntry>> ();
            if (translEntry == null || lowerCasedTerms == null || lowerCasedTerms.Count < 1 || string.IsNullOrWhiteSpace (mosesPath) || string.IsNullOrWhiteSpace (tempFilePath)) {
                return res;
            }
            string langKey = translEntry != null ? ((translEntry.srcLang != null ? translEntry.srcLang : "") + "_" + (translEntry.trgLang != null ? translEntry.trgLang : "")) : "";

			Log.Write ("Starting transliteration of " + lowerCasedTerms.Count.ToString () + " tokens.", LogLevelType.LIMITED_OUTPUT);
            int idx = 0;
            List<List<string>> lowerCasedTermDictList = new List<List<string>> (threadCount);
            for (int i=0; i<threadCount; i++) {
                lowerCasedTermDictList.Add (new List<string> ());
            }
            foreach (string term in lowerCasedTerms.Keys) {
                lowerCasedTermDictList [idx % threadCount].Add (term);
                idx++;
            }
            
            string directory = Path.GetDirectoryName (mosesPath);
            List<Process> processes = new List<Process> ();

            for (int i=0; i<lowerCasedTermDictList.Count; i++) {
                if (lowerCasedTermDictList [i].Count > 0) {
                    try {
                        string tmpFile = tempFilePath + i.ToString () + ".tmp";
                        WriteWordsForTransliteration (lowerCasedTermDictList [i], tmpFile);
                        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo (mosesPath);
                        myProcessStartInfo.UseShellExecute = false;
                        myProcessStartInfo.WorkingDirectory = directory;
                        myProcessStartInfo.FileName = mosesPath;
                        myProcessStartInfo.CreateNoWindow = true;
                        myProcessStartInfo.RedirectStandardOutput = true;
                        myProcessStartInfo.RedirectStandardError = true;

                        StringBuilder sb = new StringBuilder ();
                        sb.Append (" -f ");
                        sb.Append ("\"" + translEntry.mosesIniPath + "\" ");
                        sb.Append (" -i ");
                        sb.Append ("\"" + tmpFile + "\" ");
                        sb.Append (" -n-best-list ");
                        sb.Append ("\"" + tmpFile + ".n_best\" " + translEntry.nBest.ToString ());
                        myProcessStartInfo.Arguments = sb.ToString ();
                        
                        processes.Add (new Process ());
                        processes [processes.Count - 1].StartInfo = myProcessStartInfo;
                        bool started = processes [processes.Count - 1].Start ();
                        processes [processes.Count - 1].ErrorDataReceived += p_ErrorDataReceived;
                        processes [processes.Count - 1].OutputDataReceived += p_OutputDataReceived;
                        processes [processes.Count - 1].BeginOutputReadLine ();
                        processes [processes.Count - 1].BeginErrorReadLine ();

                    } catch {
                    }
                }
            }
            for (int i=0; i<processes.Count; i++) {
                processes [i].WaitForExit ();
                processes [i].Close ();
                processes [i].Dispose ();
            }
            processes.Clear ();

            for (int i=0; i<lowerCasedTermDictList.Count; i++) {
                if (lowerCasedTermDictList[i].Count > 0) {
                    string tmpFile = tempFilePath + i.ToString () + ".tmp";
                    if (File.Exists (tmpFile + ".n_best")) {
                    
                        NumberFormatInfo nfi = new NumberFormatInfo ();
                        nfi.CurrencyDecimalSeparator = ".";
                        nfi.NumberDecimalSeparator = ".";
                        nfi.PercentDecimalSeparator = ".";
                        Dictionary<string,Dictionary<string,bool>> existingTranslits = new Dictionary<string, Dictionary<string,bool>> ();
                    
                        StreamReader sr = new StreamReader (tmpFile + ".n_best", Encoding.UTF8);
                        string[] sep = {"|||"};
                        while (!sr.EndOfStream) {
                            string line = sr.ReadLine ();
                            string[] dataArr = line.Split (sep, StringSplitOptions.RemoveEmptyEntries);
                            if (dataArr.Length == 4) {
                                try {
                                    string idStr = dataArr [0];
                                    idStr = idStr.Trim ();
                                    int id = Convert.ToInt32 (idStr);
                                    string word = dataArr [1];
                                
                                    StringProbabEntry spe = new StringProbabEntry ();
                                    spe.str = word.Trim ().Replace (" ", "");
                                    string probabStr = dataArr [3];
                                    probabStr = probabStr.Trim ().Replace (',', '.');
                                    spe.probab = Math.Exp (Convert.ToDouble (probabStr, nfi));
                                    if (spe.probab>1) spe.probab = 1;
                                    if (id < lowerCasedTermDictList[i].Count) {
                                        string term = lowerCasedTermDictList[i][id];
                                        double min = Math.Min (spe.str.Length, term.Length);
                                        double max = Math.Max (spe.str.Length, term.Length);
                                        double lenDiff = min / max;
                                        //Log.Write(term+" "+word+" "+lenDiff.ToString()+" "+spe.probab.ToString(),LogLevelType.ERROR);
                                        if (lenDiff >= translEntry.maxLenDiff) {
                                            if (!existingTranslits.ContainsKey (term))
                                                existingTranslits.Add (term, new Dictionary<string,bool> ());
                                        
                                            if (!res.ContainsKey (term))
                                                res.Add (term, new List<StringProbabEntry> ());
                                            if (!translitTemp[langKey].ContainsKey(term)) translitTemp[langKey].Add(term, new List<StringProbabEntry>());
                                            if (!existingTranslits [term].ContainsKey (spe.str) && spe.probab >= translEntry.threshold) {
                                                spe.probab = translEntry.translitBf.Get (spe.probab);
                                                existingTranslits [term].Add (spe.str, true);
                                                res [term].Add (spe);
                                                translitTemp[langKey][term].Add(spe);
                                            }
                                        }
                                    }
                                } catch {
                                }
                            }
                        }
                    }
                    try {
                        File.Delete (tmpFile + ".n_best");
                        File.Delete (tmpFile);
                    } catch {
                    }
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return res;
        }

        static void p_OutputDataReceived (object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException ();
        }

        static void p_ErrorDataReceived (object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException ();
        }
        
        public static void WriteWordsForTransliteration(List<string> lowerCasedTerms, string outputFile)
        {
            if (string.IsNullOrWhiteSpace(outputFile)) return;
            Encoding outputEnc = new UTF8Encoding(false);
            StreamWriter sw = new StreamWriter(outputFile, false, outputEnc);
            sw.NewLine = "\n";
            foreach(string word in lowerCasedTerms)
            {
                foreach(char c in word)
                {
                    sw.Write(c);
                    sw.Write(" ");
                }
                sw.WriteLine();
            }
            sw.Close();
        }
        
        public static Dictionary<string, ProcessedTermEntry> ReadFromFile (string inputFile)
        {
            string inputStr = File.ReadAllText(inputFile,Encoding.UTF8);
            ProcessedTermEntry[] terms = MPFrameworkFunctions.DeserializeString<ProcessedTermEntry[]>(inputStr);
            Dictionary<string,ProcessedTermEntry> res = new Dictionary<string, ProcessedTermEntry>();
            foreach(ProcessedTermEntry pte in terms)
            {
                if (!res.ContainsKey(pte.lowercaceForm))
                {
                    res.Add(pte.lowercaceForm,pte);
                }
            }
            return res;
        }
        
    }
}
