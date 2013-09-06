//
//  Main.cs
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
using System.Globalization;
using System.Collections.Generic;
using System.Security;

namespace ApplyThreshold
{
    class MainClass
    {
		/// <summary>
		/// This has been initially used to apply threshold to the MPAligner results.
		/// The consolidation part has been already integrated as a standard feature in MPAligner
		/// This code is obsolete and the simple threshold method IS NOT SUPPORTED anymore.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
        public static void Main (string[] args)
        {
            StreamReader sr = new StreamReader (args [0], Encoding.UTF8);
            StreamWriter sw = new StreamWriter (args [1], false, new UTF8Encoding (false));
            sw.NewLine = "\n";
            double threshold = 0;
            NumberFormatInfo nfi = new NumberFormatInfo ();
            nfi.CurrencyDecimalSeparator = ".";
            nfi.NumberDecimalSeparator = ".";
            nfi.PercentDecimalSeparator = ".";
            threshold = Convert.ToDouble (args [2], nfi);
            bool consolidateOutputs = false;
            if (args.Length >= 4) {
                if (args [3] == "1") {
                    consolidateOutputs = true;
                }
            }
            if (!consolidateOutputs) {
                SimplyApplyThreshold (sr, sw, threshold, nfi);
            } else {
                ConsolidateOutputs (sr,sw,threshold,nfi);
            }
        }

        static void ConsolidateOutputs (StreamReader sr, StreamWriter sw, double threshold, NumberFormatInfo nfi)
        {
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
                return true;
            }
            return false;
        }

        static void SimplyApplyThreshold (StreamReader sr, StreamWriter sw, double threshold, NumberFormatInfo nfi, int concWordCount = 3)
        {
            Dictionary<string,Dictionary<string,bool>> added = new Dictionary<string, Dictionary<string, bool>> ();
            char[] sep =  {
                '\t'
            };
            int counter = 0;
            while (!sr.EndOfStream) {
                counter++;
                if (counter % 1000 == 0) {
                    Console.Write (".");
                    if (counter % 50000 ==0)
                    {
                        Console.WriteLine (" - "+counter.ToString());
                    }
                }
                string line = sr.ReadLine ();
                line = line.Trim ();
                if (!string.IsNullOrWhiteSpace (line)) {
                    string[] dataArr = line.Split (sep, StringSplitOptions.None);
                    if (dataArr.Length >= 9) {
                        double prob = Convert.ToDouble (dataArr [2], nfi);
                        if (prob >= threshold) {
                            string srcKey = dataArr [0] + dataArr [3] + dataArr [4];
                            string trgKey = dataArr [1] + dataArr [5] + dataArr [6];
                            if (!added.ContainsKey (srcKey)) {
                                added.Add (srcKey, new Dictionary<string, bool> ());
                            }
                            if (!added [srcKey].ContainsKey (trgKey)) {
                                added [srcKey].Add (trgKey, true);
                                string srcConcordance = FindConcordance(dataArr [0],dataArr [7], concWordCount);
                                string trgConcordance = FindConcordance(dataArr [1],dataArr [8], concWordCount);
                                sw.WriteLine (System.Net.WebUtility.HtmlDecode(line)+"\t"+srcConcordance.Replace('\t',' ')+"\t"+trgConcordance.Replace('\t',' '));
                            }
                        }
                    }
                }
            }
            sw.Close ();
            sr.Close ();
        }

        static string FindConcordance(string term, string file, int wordCount)
        {
            string text = GetPlaintextFromTaggedFile(file);
            int idx = GetValidConcordanceIdx(text, term);
            if (idx>=0)
            {
                int leftIdx = GetConcordanceIdx(text, wordCount,idx-1,-1);
                int rightIdx = GetConcordanceIdx(text, wordCount,idx+term.Length,1);
                return text.Substring(leftIdx,rightIdx-leftIdx+1).Trim();
            }
            else
            {
                return "";
            }
        }

        static int GetConcordanceIdx (string text, int wordCount, int idx, int direction)
        {
            int currIdx = idx;
            bool withinOther = false;
            int wordsCounted = 0;
            while (currIdx>=0 && currIdx<text.Length)
            {
                if (text[currIdx]=='\n'||text[currIdx]=='\r')
                {
                    break;
                }
                if (Char.IsWhiteSpace(text[currIdx]))
                {
                    if (withinOther)
                    {
                        wordsCounted++;
                    }
                    withinOther = false;
                }
                else
                {
                    withinOther = true;
                }
                if (wordsCounted>=wordCount)
                {
                    break;
                }
                currIdx+=direction;
            }
            return currIdx-direction;
        }

        static int GetValidConcordanceIdx (string text, string term, int idxIn = 0)
        {
            int idx = text.IndexOf(term,idxIn,StringComparison.CurrentCultureIgnoreCase);
            if (idx>=0)
            {
                if (idx>0 && !IsValidSeparator(text, idx-1, -1))
                {
                    return GetValidConcordanceIdx (text,term,idx+1);
                }
                if (idx+term.Length<text.Length && !IsValidSeparator(text, idx+term.Length, 1))
                {
                    return GetValidConcordanceIdx (text,term,idx+1);
                }
                return idx;
            }
            else
            {
                return -1;
            }
        }

        static bool IsValidSeparator(string text, int idx, int direction)
        {
            int changingIdx = idx;
            while(changingIdx>=0 && changingIdx<text.Length)
            {
                if (Char.IsWhiteSpace(text[changingIdx])) return true;
                if (Char.IsLetterOrDigit(text[changingIdx])) return false;
                //if (Char.IsPunctuation(text[changingIdx])) //The punctuations are ignored as some may be used to stick words together.
                changingIdx+=direction;
            }
            return true;
        }

        static string GetPlaintextFromTaggedFile(string file)
        {
            StreamReader sr = new StreamReader(file,Encoding.UTF8);
            string text = File.ReadAllText(file,Encoding.UTF8);
            //Read the text and process all MUC-7 tags. All other tags will remain untouched!
            StringBuilder sb = new StringBuilder(5000);
            for (int i=0;i<text.Length;i++)
            {
                if (text[i] == '<')
                {
                    //Try to find the first of all three MUC-7 NE categories.
                    int findEnemex = text.IndexOf("<TENAME", i);
                    if (findEnemex == i)
                    {
                        //If the ENAMEX tag starts in the current position...
                        string textToAdd = RemoveFirstEntity("TENAME", text, ref i);
                        sb.Append(textToAdd);
                    }
                    else
                    {
                        sb.Append(text[i].ToString());
                    }
                }
                else
                {
                    sb.Append(text[i].ToString());
                }
            }
            return sb.ToString();
        }

        static string RemoveFirstEntity(string p, string text, ref int i)
        {
            try
            {
                //Find the first closing character (the end of the open tag).
                int closingIdx = text.IndexOf('>', i);
                //Find the closing tag.
                int closingTagIdx = text.IndexOf("</" + p + ">", i);
                //Get the entity attribute values - original markup.
                string entity = text.Substring(i, closingIdx - i + 1).Remove(0,p.Length + 1);
                entity = entity.Substring(0, entity.Length - 1);
                //Extract the NE text.
                string textToAdd = text.Substring(closingIdx + 1, closingTagIdx - closingIdx - 1);
                //Set the index to the position of the '>' character of the NE closing tag.
                i = closingTagIdx + p.Length + 2;
                return textToAdd;
            }
            catch
            {
                Console.WriteLine("Mark-up error at index " +i.ToString());
            }
            return "";
        }
    }
}
