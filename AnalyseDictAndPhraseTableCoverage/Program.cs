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
using AddTermFeatureToPhraseTable;

namespace AnalyseDictAndPhraseTableCoverage
{
    class MainClass
    {
		/// <summary>
		/// This program is used to calculate OOV statistics of aligned terms (e.g., the scores reported in the RANLP 2013 paper).
		/// NOTE - the input is currently not in the right format (as I have changed the output of MPAligner).
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
        public static void Main (string[] args)
        {
            string inputFile = "";
            string dictFile = "";
            string phraseTableFile = "";
            bool swapAlignments = false;
            string outputFile = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-i" && i + 1 < args.Length)
                {
                    inputFile = args[i + 1];
                }
                else if (args[i] == "-d" && i + 1 < args.Length)
                {
                    dictFile = args[i + 1];
                }
                else if (args[i] == "-p" && i + 1 < args.Length)
                {
                    phraseTableFile = args[i + 1];
                }
                else if (args[i] == "-s")
                {
                    swapAlignments = true;
                }
                else if (args[i] == "-o" && i+1 <args.Length)
                {
                    outputFile = args[i + 1];
                }
            }

            if (string.IsNullOrWhiteSpace(inputFile)||!File.Exists(inputFile))
            {
                Console.WriteLine("Input file missing or cannot be found!");
                PrintUsage();
            }

            List<AlignmentElement> alignments = ReadInputData(inputFile,swapAlignments);


            //PerformDictAnalysis(alignments, dictFile, outputFile);

            PerformPhraseTableAnalysis(alignments, phraseTableFile, outputFile);
        }

        static void PerformPhraseTableAnalysis (List<AlignmentElement> alignments, string phraseTableFile, string outputFile)
        {
            Console.WriteLine();
            Console.WriteLine("[PHRASE TABLE STATISTICS]");
            char[] sep = { '\t',' ' };
            Dictionary<string,Dictionary<string,bool>> srcToTrgAlign = GetDictFromList(alignments,true,false, false,true);
            Dictionary<string,Dictionary<string,bool>> srcToTrgLowAlign = GetDictFromList(alignments,true,true, false,true);
            Dictionary<string,Dictionary<string,bool>> trgToSrcAlign = GetDictFromList(alignments,false,false, false,true);
            Dictionary<string,Dictionary<string,bool>> TrgToSrcLowAlign = GetDictFromList(alignments,false,true, false,true);

            StreamReader sr = new StreamReader(phraseTableFile,Encoding.UTF8);
            int counter=0;
            while(!sr.EndOfStream)
            {
                counter++;
                if (counter%10000==0)
                {
                    Console.Write(".");
                    if (counter%500000==0)
                    {
                        Console.WriteLine(counter.ToString());
                    }
                }
                string line = sr.ReadLine().Trim();
                PhraseTableEntry pte = PhraseTableEntry.ParsePhraseTableLine(line);
                if (pte!=null)
                {
                    if (srcToTrgAlign.ContainsKey(pte.src) && srcToTrgAlign[pte.src].ContainsKey(pte.trg)) srcToTrgAlign[pte.src][pte.trg] = true;
                    if (srcToTrgLowAlign.ContainsKey(pte.src.ToLower()) && srcToTrgLowAlign[pte.src.ToLower()].ContainsKey(pte.trg.ToLower())) srcToTrgLowAlign[pte.src.ToLower()][pte.trg.ToLower()] = true;
                    if (trgToSrcAlign.ContainsKey(pte.trg) && trgToSrcAlign[pte.trg].ContainsKey(pte.src)) trgToSrcAlign[pte.trg][pte.src] = true;
                    if (TrgToSrcLowAlign.ContainsKey(pte.trg.ToLower()) && TrgToSrcLowAlign[pte.trg.ToLower()].ContainsKey(pte.src.ToLower())) TrgToSrcLowAlign[pte.trg.ToLower()][pte.src.ToLower()] = true;
                }
            }
            sr.Close();
            
            Console.WriteLine("Statistics for case sensitive SRC-to-TRG alignment:");            
            Console.WriteLine();
            DrawStatistics(srcToTrgAlign, outputFile, "phrase.src-to-trg");            
            Console.WriteLine();
            Console.WriteLine("Statistics for case insensitive SRC-to-TRG alignment:");            
            Console.WriteLine();
            DrawStatistics(srcToTrgLowAlign, outputFile, "phrase.src-to-trg.lowercase");            
            Console.WriteLine();
            Console.WriteLine("Statistics for case sensitive TRG-to-SRC alignment:");            
            Console.WriteLine();
            DrawStatistics(trgToSrcAlign, outputFile, "phrase.trg-to-src");            
            Console.WriteLine();
            Console.WriteLine("Statistics for case insensitive TRG-to-SRC alignment:");            
            Console.WriteLine();
            DrawStatistics(TrgToSrcLowAlign, outputFile, "phrase.trg-to-src.lowercase");            
            Console.WriteLine();            
            Console.WriteLine();
        }

        static void PerformDictAnalysis (List<AlignmentElement> alignments, string dictFile, string outputFile)
        {
            Console.WriteLine();
            Console.WriteLine("[DICTIONARY STATISTICS]");
            char[] sep = { '\t',' ' };
            Dictionary<string,Dictionary<string,bool>> srcToTrgAlign = GetDictFromList(alignments,true,false, true, false);
            Dictionary<string,Dictionary<string,bool>> srcToTrgLowAlign = GetDictFromList(alignments,true,true, true, false);
            Dictionary<string,Dictionary<string,bool>> srcToTrgLowLemmaAlign = GetDictFromList(alignments,true,true, true, false, true);
            Dictionary<string,Dictionary<string,bool>> trgToSrcAlign = GetDictFromList(alignments,false,false, true, false);
            Dictionary<string,Dictionary<string,bool>> TrgToSrcLowAlign = GetDictFromList(alignments,false,true, true, false);

            StreamReader sr = new StreamReader(dictFile,Encoding.UTF8);
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                string[] data = line.Split(sep,StringSplitOptions.None);
                if(data.Length==3)
                {
                    if (srcToTrgAlign.ContainsKey(data[0]) && srcToTrgAlign[data[0]].ContainsKey(data[1]))
                    {
                        srcToTrgAlign[data[0]][data[1]] = true;
                    }
                    if (srcToTrgLowAlign.ContainsKey(data[0].ToLower()) && srcToTrgLowAlign[data[0].ToLower()].ContainsKey(data[1].ToLower()))
                    {
                        srcToTrgLowAlign[data[0].ToLower()][data[1].ToLower()] = true;
                    }
                    if (srcToTrgLowLemmaAlign.ContainsKey(data[0].ToLower()) && srcToTrgLowLemmaAlign[data[0].ToLower()].ContainsKey(data[1].ToLower()))
                    {
                        srcToTrgLowLemmaAlign[data[0].ToLower()][data[1].ToLower()] = true;
                    }
                    if (trgToSrcAlign.ContainsKey(data[1]) && trgToSrcAlign[data[1]].ContainsKey(data[0])) trgToSrcAlign[data[1]][data[0]] = true;
                    if (TrgToSrcLowAlign.ContainsKey(data[1].ToLower()) && TrgToSrcLowAlign[data[1].ToLower()].ContainsKey(data[0].ToLower())) TrgToSrcLowAlign[data[1].ToLower()][data[0].ToLower()] = true;
                }
            }
            sr.Close();

            Console.WriteLine("Statistics for case sensitive SRC-to-TRG alignment:");            
            Console.WriteLine();
            DrawStatistics(srcToTrgAlign, outputFile, "dict.src-to-trg");            
            Console.WriteLine();
            Console.WriteLine("Statistics for case insensitive SRC-to-TRG alignment:");            
            Console.WriteLine();
            DrawStatistics(srcToTrgLowAlign, outputFile, "dict.src-to-trg.lowercase");            
            Console.WriteLine();
            Console.WriteLine("Statistics for case insensitive SRC-to-TRG lemma alignment:");            
            Console.WriteLine();
            DrawStatistics(srcToTrgLowLemmaAlign, outputFile, "dict.src-to-trg.lemma.lowercase");            
            Console.WriteLine();
            Console.WriteLine("Statistics for case sensitive TRG-to-SRC alignment:");            
            Console.WriteLine();
            DrawStatistics(trgToSrcAlign, outputFile, "dict.trg-to-src");            
            Console.WriteLine();
            Console.WriteLine("Statistics for case insensitive TRG-to-SRC alignment:");            
            Console.WriteLine();
            DrawStatistics(TrgToSrcLowAlign, outputFile, "dict.trg-to-src.lowercase");            
            Console.WriteLine();            
            Console.WriteLine();

        }

        static void DrawStatistics (Dictionary<string, Dictionary<string, bool>> alignments, string outputFile, string suffix)
        {
            int totalPairs=0;
            int sourcePaired=0;
            int sourceUnpaired=0;
            int paired=0;


            if (string.IsNullOrWhiteSpace(outputFile))
            {
                foreach(string src in alignments.Keys)
                {
                    bool wasSrcPaired = false;
                    foreach(string trg in alignments[src].Keys)
                    {
                        totalPairs++;
                        if (alignments[src][trg])
                        {
                            paired++;
                            wasSrcPaired = true;
                        }
                    }
                    if (wasSrcPaired) sourcePaired++;
                    else sourceUnpaired++;
                }
            }
            else
            {
                UTF8Encoding enc = new UTF8Encoding(false);
                StreamWriter swFound = new StreamWriter(outputFile+"."+suffix+".found.txt",false, enc);
                StreamWriter swNotFound = new StreamWriter(outputFile+"."+suffix+".not-found.txt",false, enc);
                foreach(string src in alignments.Keys)
                {
                    bool wasSrcPaired = false;
                    foreach(string trg in alignments[src].Keys)
                    {
                        totalPairs++;
                        if (alignments[src][trg])
                        {
                            paired++;
                            wasSrcPaired = true;
                            swFound.WriteLine(src+"\t"+trg);
                        }
                        else
                        {
                            swNotFound.WriteLine(src+"\t"+trg);
                        }
                    }
                    if (wasSrcPaired) sourcePaired++;
                    else sourceUnpaired++;
                }
                swFound.Close();
                swNotFound.Close();
            }
            Console.WriteLine("Total entries:\t"+totalPairs.ToString());
            Console.WriteLine("Total found:\t"+paired.ToString());
            Console.WriteLine("Total not found:\t"+(totalPairs-paired).ToString());
            Console.WriteLine("Total FIRST (src or trg) found:\t"+(sourcePaired).ToString());
            Console.WriteLine("Total FIRST (src or trg) not found:\t"+(sourceUnpaired).ToString());
        }

        static void PrintUsage()
        {
            Console.WriteLine("AnalyseDictAndPhraseTableCoverage.exe -i [Input file] -d [Dictionary file] -p [Phrase table file] (-s - only if source and target should be swapped in input data)");
        }

        static Dictionary<string,Dictionary<string,bool>> GetDictFromList(List<AlignmentElement> alignments, bool srcToTrg, bool lowerCased, bool onlyUnigrams, bool onlyMultigrams, bool onlyLemmas = false)
        {
            Dictionary<string,Dictionary<string,bool>> res = new Dictionary<string, Dictionary<string, bool>>();
            //int counter = 0;
            //int counter2 = 0;
            foreach(AlignmentElement ae in alignments)
            {
                //counter++;
                if ((onlyUnigrams && DoesNotContainSpaces(ae.srcTerm) && DoesNotContainSpaces(ae.trgTerm))
                    || (onlyMultigrams && (!DoesNotContainSpaces(ae.srcTerm) || !DoesNotContainSpaces(ae.trgTerm)))
                    || (!onlyMultigrams && !onlyUnigrams))
                {
                    //counter2++;
                    string srcStr = lowerCased?ae.srcTerm.ToLower():ae.srcTerm;
                    string trgStr = lowerCased?ae.trgTerm.ToLower():ae.trgTerm;
                    if (onlyLemmas)
                    {
                        if (string.IsNullOrWhiteSpace(ae.srcLemma)|| string.IsNullOrWhiteSpace(ae.trgLemma)) continue;
                        srcStr = lowerCased?ae.srcLemma.ToLower():ae.srcLemma;
                        trgStr = lowerCased?ae.trgLemma.ToLower():ae.trgLemma;
                    }
                    if (srcToTrg)
                    {
                        if (!res.ContainsKey(srcStr)) res.Add(srcStr,new Dictionary<string, bool>());
                        if (!res[srcStr].ContainsKey(trgStr)) res[srcStr].Add(trgStr,false);
                    }
                    else
                    {
                        if (!res.ContainsKey(trgStr)) res.Add(trgStr,new Dictionary<string, bool>());
                        if (!res[trgStr].ContainsKey(srcStr)) res[trgStr].Add(srcStr,false);
                    }
                }
            }
            //Console.WriteLine(counter.ToString()+" "+counter2);
            return res;
        }

        static bool DoesNotContainSpaces (string srcTerm)
        {
            for (int i=0;i<srcTerm.Length;i++)
            {
                if (Char.IsWhiteSpace(srcTerm[i])) return false;
            }
            return true;
        }

        static List<AlignmentElement> ReadInputData(string inputFile, bool swapAlignments = false)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencyDecimalSeparator = ".";
            nfi.NumberDecimalSeparator = ".";
            nfi.PercentDecimalSeparator = ".";
            List<AlignmentElement> alignments = new List<AlignmentElement>();
            StreamReader sr = new StreamReader(inputFile,Encoding.UTF8);
            char[] sep = { '\t' };
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                string[] data = line.Split(sep,StringSplitOptions.None);
                if (data.Length>=3)//Only MPAligner ref_tabsep output data is supported!
                {
                    AlignmentElement ae = new AlignmentElement();
                    if (!swapAlignments)
                    {
                        ae.srcTerm = data[0];
                        ae.trgTerm = data[1];
                        ae.alignmentScore = Convert.ToDouble(data[2],nfi);
                        if (data.Length>=9)
                        {
                            ae.srcMsd = data[3];
                            ae.srcLemma = data[4];
                            ae.trgMsd = data[5];
                            ae.trgLemma = data[6];
                            ae.srcFile = data[7];
                            ae.trgFile = data[8];
                        }
                    }
                    else
                    {
                        ae.srcTerm = data[1];
                        ae.trgTerm = data[0];
                        ae.alignmentScore = Convert.ToDouble(data[2],nfi);
                        if (data.Length>=9)
                        {
                            ae.srcMsd = data[5];
                            ae.srcLemma = data[6];
                            ae.trgMsd = data[3];
                            ae.trgLemma = data[4];
                            ae.srcFile = data[8];
                            ae.trgFile = data[7];
                        }
                    }
                    alignments.Add(ae);
                }
            }
            sr.Close();
            return alignments;
        }
    }
}
