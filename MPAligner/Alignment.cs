//
//  Alignment.cs
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
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Amib.Threading;
using System.Collections;

namespace MPAligner
{
    public class Alignment
    {
        public Alignment ()
        {
        }

        //A dirty way to find a max string length for alignment normalization.
        private static int maxStrLen = 0;

        public static List<AlignmentInfoElement> AlignListPairs (MPAlignerConfiguration configuration, List<ProcessedTermEntry> srcTermList, List<ProcessedTermEntry> trgTermList, bool interlinguaDictUsed, bool interlinguaTranslitUsed, string srcLang, string trgLang, string srcFile, string trgFile, Dictionary<string, Dictionary<string, bool>> excDict, Dictionary<string, bool> srcStopWords, Dictionary<string, bool> trgStopWords)
        {
            if (configuration == null||configuration.langPairEntryDict==null||string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
            {
                return null;
            }
            
            string langKey = srcLang+"_"+trgLang;
            
            MPAlignerConfigurationLangPairEntry lpeConf = new MPAlignerConfigurationLangPairEntry();
            if (configuration.langPairEntryDict.ContainsKey(langKey))
            {
                lpeConf = configuration.langPairEntryDict[langKey];
            }
            else
            {
                lpeConf = new MPAlignerConfigurationLangPairEntry();
                lpeConf.srcLang = srcLang;
                lpeConf.trgLang = trgLang;
            }
            
            List<AlignmentInfoElement> res = new List<AlignmentInfoElement>();
            for(int i=0;i< srcTermList.Count;i++)
            {
                ProcessedTermEntry srcPte = srcTermList[i];
                ProcessedTermEntry trgPte = trgTermList[i];
                if (srcPte!=null && trgPte!=null)
                {
                    AlignmentInfoElement aie = new AlignmentInfoElement();
                    List<WordAlignmentElement> srcToTrg = new List<WordAlignmentElement>();
                    List<WordAlignmentElement> trgToSrc = new List<WordAlignmentElement>();
                    maxStrLen = 0;
                    
                    if (interlinguaDictUsed && interlinguaTranslitUsed)
                    {
                        
                        ///Types:
                        /// 0 - dictionary,
                        /// 1 - simple translit,
                        /// 2 - target or source,
                        /// 3 - translit
                        
                        //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLATION
                        AlignStringProbabEntryListLists (lpeConf, srcPte.translationList, trgPte.translationList, srcToTrg, trgToSrc, 0, 0);
                        
                        //Translation is in EN language; SOURCE TRANSLATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 0, 1);
                        
                        //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLITERATION
                        AlignStringProbabEntryListLists (lpeConf, srcPte.translationList, trgPte.transliterationList, srcToTrg, trgToSrc, 0, 3);
                        
                        //Translation is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLATION
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.translationList, srcToTrg, trgToSrc, 1, 0);
                        
                        //Translation is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLATION
                        AlignStringProbabEntryListLists (lpeConf, srcPte.transliterationList, trgPte.translationList, srcToTrg, trgToSrc, 3, 0);
                        
                        //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 3, 1);
                        
                        //Transliteration is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLITERATION
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.transliterationList, srcToTrg, trgToSrc, 1, 3);
                        
                        //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLITERATION
                        AlignStringProbabEntryListLists (lpeConf, srcPte.transliterationList, trgPte.transliterationList, srcToTrg, trgToSrc, 3, 3);
                        
                        //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                        
                    }
                    else if (interlinguaTranslitUsed)
                    {
                        //Translation is in target language; SOURCE TRANSLATION vs TARGET
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 0, 2);
                        
                        //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 3, 1);
                        
                        //Transliteration is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLITERATION
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.transliterationList, srcToTrg, trgToSrc, 1, 3);
                        
                        //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLITERATION
                        AlignStringProbabEntryListLists (lpeConf, srcPte.transliterationList, trgPte.transliterationList, srcToTrg, trgToSrc, 3, 2);
                        
                        //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                        
                        //Translation is in target language; SOURCE vs TARGET TRANSLATION 
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.translationList, srcToTrg, trgToSrc, 2, 0);
                    }
                    else if (interlinguaDictUsed)
                    {
                        //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLATION
                        AlignStringProbabEntryListLists (lpeConf, srcPte.translationList, trgPte.translationList, srcToTrg, trgToSrc, 0, 0);
                        
                        //Translation is in EN language; SOURCE TRANSLATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 0, 1);
                        
                        //Translation is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLATION
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.translationList, srcToTrg, trgToSrc, 1, 0);
                        
                        //Transliteration is in target language; SOURCE vs TARGET TRANSLITERATION 
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.transliterationList, srcToTrg, trgToSrc, 2, 3);
                        
                        //Transliteration is in target language; SOURCE TRANSLITERATION vs TARGET
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 3, 2);
                        
                        //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                        
                    }
                    else
                    {
                        //Translation is in target language; SOURCE TRANSLATION vs TARGET
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 0, 2);
                        
                        //Transliteration is in target language; SOURCE TRANSLITERATION vs TARGET
                        AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 3, 2);
                        
                        //Translation is in target language; SOURCE vs TARGET TRANSLATION 
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.translationList, srcToTrg, trgToSrc, 2, 0);
                        
                        //Transliteration is in target language; SOURCE vs TARGET TRANSLITERATION 
                        AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.transliterationList, srcToTrg, trgToSrc, 2, 3);
                        
                        //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                        AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                        
                    }
                    aie.srcToTrgAlignments = srcToTrg;
                    aie.trgToSrcAlignments = trgToSrc;
                    
                    aie.srcEntry = srcPte;
                    aie.trgEntry = trgPte;
                    
                    ConsolidateOverlaps(lpeConf,aie, excDict);
                    bool valid = CreateStrListsForEval(configuration,aie,srcStopWords,trgStopWords,false);
                    aie.alignmentScore = EvaluateAlignmentScore(lpeConf,aie);
                    //If you wish to debug the process, comment the lines below that clear the alignments...
                    aie.srcToTrgAlignments.Clear();
                    aie.trgToSrcAlignments.Clear();
                    aie.consolidatedAlignment.Clear();
                    aie.srcFile = srcFile;
                    aie.trgFile = trgFile;
                    res.Add(aie);
                }
            }
            return res;
        }
        
        public static List<AlignmentInfoElement> AlignPairsMultiThreaded(MPAlignerConfiguration configuration, Dictionary<string, ProcessedTermEntry> srcTerms, Dictionary<string, ProcessedTermEntry> trgTerms, bool interlinguaDictUsed, bool interlinguaTranslitUsed, string srcLang, string trgLang, string srcFile, string trgFile, Dictionary<string, Dictionary<string, bool>> excDict, Dictionary<string, bool> srcStopWords, Dictionary<string, bool> trgStopWords)
        {
            if (configuration == null||configuration.langPairEntryDict==null||string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
            {
                return null;
            }
			Log.Write ("Starting alignmet of "+ srcTerms.Count.ToString()+" "+srcLang+" and "+ trgTerms.Count.ToString()+" "+trgLang+" terms.",LogLevelType.LIMITED_OUTPUT);
            
            int threadCount = configuration.alignmentThreads;

            STPStartInfo stpStartInfo = new STPStartInfo();
            stpStartInfo.IdleTimeout = 100*1000;
            stpStartInfo.MaxWorkerThreads = 5*threadCount;
            stpStartInfo.MinWorkerThreads = threadCount;
            stpStartInfo.EnableLocalPerformanceCounters = true;

            SmartThreadPool smartThreadPool = new SmartThreadPool(stpStartInfo);

            string langKey = srcLang+"_"+trgLang;
            
            MPAlignerConfigurationLangPairEntry lpeConf = new MPAlignerConfigurationLangPairEntry();
            if (configuration.langPairEntryDict.ContainsKey(langKey))
            {
                lpeConf = configuration.langPairEntryDict[langKey];
            }
            else
            {
                lpeConf = new MPAlignerConfigurationLangPairEntry();
                lpeConf.srcLang = srcLang;
                lpeConf.trgLang = trgLang;
            }
            int counter = 0;
            //threadedAlignments = new List<AlignmentInfoElement>();
            
            List<AlignmentInfoElement> res = new List<AlignmentInfoElement>();
            Dictionary<string,Dictionary<string,bool>> alignedList = new Dictionary<string, Dictionary<string, bool>>();
            List<IWorkItemResult<AlignmentInfoElement>> wirList = new List<IWorkItemResult<AlignmentInfoElement>>(1000);
            _configuration = configuration;
            _interlinguaDictUsed=interlinguaDictUsed;
            _interlinguaTranslitUsed=interlinguaTranslitUsed;
            _srcFile=srcFile;
            _trgFile=trgFile;
            _excDict=excDict;
            _srcStopWords=srcStopWords;
            _trgStopWords=trgStopWords;
            _lpeConf=lpeConf;
            foreach(string srcTerm in srcTerms.Keys)
            {
                counter++;
                if (counter%50==0)
                {
                    Console.Write(".");
                    if (counter%1000==0)
                    {
                        Console.WriteLine(" - "+counter.ToString());
                    }
                }
                ProcessedTermEntry srcPte = srcTerms[srcTerm];
                foreach(string trgTerm in trgTerms.Keys)
                {
                    //List<Tuple<ProcessedTermEntry,ProcessedTermEntry>> unProcessed = new List<Tuple<ProcessedTermEntry, ProcessedTermEntry>>();
                    if (wirList.Count>=100000)
                    {
                        smartThreadPool.WaitForIdle();
                        for(int i=0;i<wirList.Count;i++)
                        {
                            if (wirList[i].IsCompleted && wirList[i].Exception==null)
                            {
                                AlignmentInfoElement aie = (AlignmentInfoElement)wirList[i].Result;
                                if (aie!=null && (!alignedList.ContainsKey(aie.alignedLowSrcStr)||!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)))
                                {
                                    res.Add(aie);
                                    if (!alignedList.ContainsKey(aie.alignedLowSrcStr)) alignedList.Add(aie.alignedLowSrcStr,new Dictionary<string, bool>());
                                    if (!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)) alignedList[aie.alignedLowSrcStr].Add(aie.alignedLowTrgStr,true);
                                }
                            }
                            else if (!wirList[i].IsCompleted)
                            {
                                int times = 100;
                                while(!wirList[i].IsCompleted && times>0)
                                {
                                    times--;
                                    System.Threading.Thread.Sleep(100);
                                }
                                if (wirList[i].IsCompleted && wirList[i].Exception==null)
                                {
                                    AlignmentInfoElement aie = (AlignmentInfoElement)wirList[i].Result;
                                    if (aie!=null && (!alignedList.ContainsKey(aie.alignedLowSrcStr)||!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)))
                                    {
                                        res.Add(aie);
                                        if (!alignedList.ContainsKey(aie.alignedLowSrcStr)) alignedList.Add(aie.alignedLowSrcStr,new Dictionary<string, bool>());
                                        if (!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)) alignedList[aie.alignedLowSrcStr].Add(aie.alignedLowTrgStr,true);
                                    }
                                }
                            }
                        }
                        wirList.Clear();
                    }
                    try
                    {
                        IWorkItemResult<AlignmentInfoElement> wir = smartThreadPool.QueueWorkItem(
                            new Amib.Threading.Func<ProcessedTermEntry, ProcessedTermEntry, AlignmentInfoElement>(AlignSingleTermPair), srcPte, trgTerms[trgTerm]);
                        if (wir!=null) wirList.Add(wir);
                    }
                    catch
                    {
                        Log.Write("Thread exception catched - cannot create a new thread within term alignment!", LogLevelType.WARNING);
                    }
                    //smartThreadPool
                    /*while(smartThreadPool.PerformanceCountersReader.WorkItemsQueued>=100)
                    {
                        System.Threading.Thread.Sleep(5);
                    }*/
                    
                    //AlignmentInfoElement aie = AlignSingleTermPair (configuration, trgTerms[trgTerm], interlinguaDictUsed, interlinguaTranslitUsed, srcFile, trgFile, excDict, srcStopWords, trgStopWords, lpeConf, srcPte);
                    //if (aie!=null)
                    //{
                        //res.Add(aie);
                    //}
                }
            }
            //Console.WriteLine();
            if (wirList.Count>0)
            {
                smartThreadPool.WaitForIdle();
                for(int i=0;i<wirList.Count;i++)
                {
                    if (wirList[i].IsCompleted && wirList[i].Exception==null)
                    {
                        AlignmentInfoElement aie = (AlignmentInfoElement)wirList[i].Result;
                        if (aie!=null && (!alignedList.ContainsKey(aie.alignedLowSrcStr)||!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)))
                        {
                            res.Add(aie);
                            if (!alignedList.ContainsKey(aie.alignedLowSrcStr)) alignedList.Add(aie.alignedLowSrcStr,new Dictionary<string, bool>());
                            if (!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)) alignedList[aie.alignedLowSrcStr].Add(aie.alignedLowTrgStr,true);
                        }
                    }
                    else if (!wirList[i].IsCompleted)
                    {
                        int times = 100;
                        while(!wirList[i].IsCompleted && times>0)
                        {
                            times--;
                            System.Threading.Thread.Sleep(100);
                        }
                        if (wirList[i].IsCompleted && wirList[i].Exception==null)
                        {
                            AlignmentInfoElement aie = (AlignmentInfoElement)wirList[i].Result;
                            if (aie!=null && (!alignedList.ContainsKey(aie.alignedLowSrcStr)||!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)))
                            {
                                res.Add(aie);
                                if (!alignedList.ContainsKey(aie.alignedLowSrcStr)) alignedList.Add(aie.alignedLowSrcStr,new Dictionary<string, bool>());
                                if (!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)) alignedList[aie.alignedLowSrcStr].Add(aie.alignedLowTrgStr,true);
                            }
                        }
                    }
                }
                wirList.Clear();
            }
            try{
                smartThreadPool.Shutdown(true,100);
                smartThreadPool.Dispose();
                smartThreadPool = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
                try
                {
                    smartThreadPool.Shutdown(true,100);
                    smartThreadPool.Dispose();
                    smartThreadPool = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
			Log.Write ("Alignmet finished - "+ res.Count.ToString()+" term pairs aligned over the alignment threshold " +lpeConf.finalAlignmentThr.ToString()+".\n",LogLevelType.LIMITED_OUTPUT);
            return res;
        }
        
        //private static List<AlignmentInfoElement> threadedAlignments;
        private static Dictionary<string, Dictionary<string, bool>> _excDict;
        private static MPAlignerConfiguration _configuration;
        private static bool _interlinguaDictUsed;
        private static bool _interlinguaTranslitUsed;
        private static string _srcFile;
        private static string _trgFile;
        private static Dictionary<string, bool> _srcStopWords;
        private static Dictionary<string, bool> _trgStopWords;
        private static MPAlignerConfigurationLangPairEntry _lpeConf;
        
        public static AlignmentInfoElement AlignSingleTermPair (ProcessedTermEntry srcPte, ProcessedTermEntry trgPte)
        {
            if (srcPte!=null && trgPte!=null)
            {
                AlignmentInfoElement aie = new AlignmentInfoElement();
                List<WordAlignmentElement> srcToTrg = new List<WordAlignmentElement>();
                List<WordAlignmentElement> trgToSrc = new List<WordAlignmentElement>();
                maxStrLen = 0;
                
                if (_interlinguaDictUsed && _interlinguaTranslitUsed)
                {
                    
                    ///Types:
                    /// 0 - dictionary,
                    /// 1 - simple translit,
                    /// 2 - target,
                    /// 3 - translit
            
                    //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLATION
                    AlignStringProbabEntryListLists (_lpeConf, srcPte.translationList, trgPte.translationList, srcToTrg, trgToSrc, 0, 0);
                    
                    //Translation is in EN language; SOURCE TRANSLATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.translationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 0, 1);
            
                    //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLITERATION
                    AlignStringProbabEntryListLists (_lpeConf, srcPte.translationList, trgPte.transliterationList, srcToTrg, trgToSrc, 0, 3);
            
                    //Translation is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLATION
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.simpleTransliteration, trgPte.translationList, srcToTrg, trgToSrc, 1, 0);
                    
                    //Translation is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLATION
                    AlignStringProbabEntryListLists (_lpeConf, srcPte.transliterationList, trgPte.translationList, srcToTrg, trgToSrc, 3, 0);
            
                    //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.transliterationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 3, 1);
            
                    //Transliteration is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLITERATION
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.simpleTransliteration, trgPte.transliterationList, srcToTrg, trgToSrc, 1, 3);
                    
                    //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLITERATION
                    AlignStringProbabEntryListLists (_lpeConf, srcPte.transliterationList, trgPte.transliterationList, srcToTrg, trgToSrc, 3, 3);
            
                    //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringLists (_lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                    
                }
                else if (_interlinguaTranslitUsed)
                {
                    //Translation is in target language; SOURCE TRANSLATION vs TARGET
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.translationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 0, 2);
            
                    //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.transliterationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 3, 1);
            
                    //Transliteration is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLITERATION
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.simpleTransliteration, trgPte.transliterationList, srcToTrg, trgToSrc, 1, 3);
                    
                    //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLITERATION
                    AlignStringProbabEntryListLists (_lpeConf, srcPte.transliterationList, trgPte.transliterationList, srcToTrg, trgToSrc, 3, 2);
            
                    //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringLists (_lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);

                    //Translation is in target language; SOURCE vs TARGET TRANSLATION 
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.lowercaseWords, trgPte.translationList, srcToTrg, trgToSrc, 2, 0);
                }
                else if (_interlinguaDictUsed)
                {
                    //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLATION
                    AlignStringProbabEntryListLists (_lpeConf, srcPte.translationList, trgPte.translationList, srcToTrg, trgToSrc, 0, 0);
            
                    //Translation is in EN language; SOURCE TRANSLATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.translationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 0, 1);
            
                    //Translation is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLATION
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.simpleTransliteration, trgPte.translationList, srcToTrg, trgToSrc, 1, 0);
                    
                    //Transliteration is in target language; SOURCE TRANSLITERATION vs TARGET
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.transliterationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 3, 2);
            
                    //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringLists (_lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);

                    //Transliteration is in target language; SOURCE vs TARGET TRANSLITERATION 
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.lowercaseWords, trgPte.transliterationList, srcToTrg, trgToSrc, 2, 3);

                }
                else
                {
                    //Translation is in target language; SOURCE TRANSLATION vs TARGET
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.translationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 0, 2);
                    
                    //Transliteration is in target language; SOURCE TRANSLITERATION vs TARGET
                    AlignStringProbabEntryListToStringList (_lpeConf, srcPte.transliterationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 3, 2);
                    
                    //Translation is in target language; SOURCE vs TARGET TRANSLATION 
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.lowercaseWords, trgPte.translationList, srcToTrg, trgToSrc, 2, 0);
                    
                    //Transliteration is in target language; SOURCE vs TARGET TRANSLITERATION 
                    AlignStringListToStringProbabEntryList (_lpeConf, srcPte.lowercaseWords, trgPte.transliterationList, srcToTrg, trgToSrc, 2, 3);

                    //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                    AlignStringLists (_lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                                                
                }
                aie.srcToTrgAlignments = srcToTrg;
                aie.trgToSrcAlignments = trgToSrc;
                
                aie.srcEntry = srcPte;
                aie.trgEntry = trgPte;
                
                ConsolidateOverlaps(_lpeConf,aie, _excDict);
                if(CreateStrListsForEval(_configuration,aie,_srcStopWords,_trgStopWords))
                {
                    aie.alignmentScore = EvaluateAlignmentScore(_lpeConf,aie);
                    if (aie.alignmentScore>=_lpeConf.finalAlignmentThr)
                    {
                        //If you wish to debug the process, comment the lines below that clear the alignments...
                        aie.srcToTrgAlignments.Clear();
                        aie.trgToSrcAlignments.Clear();
                        aie.consolidatedAlignment.Clear();
                        aie.srcFile = _srcFile;
                        aie.trgFile = _trgFile;
                        return aie;
                    }
                }
            }
            return null;
        }
        
        public static List<AlignmentInfoElement> AlignPairs(MPAlignerConfiguration configuration, Dictionary<string, ProcessedTermEntry> srcTerms, Dictionary<string, ProcessedTermEntry> trgTerms, bool interlinguaDictUsed, bool interlinguaTranslitUsed, string srcLang, string trgLang, string srcFile, string trgFile, Dictionary<string, Dictionary<string, bool>> excDict, Dictionary<string, bool> srcStopWords, Dictionary<string, bool> trgStopWords)
        {
            if (configuration == null||configuration.langPairEntryDict==null||string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
            {
                return null;
            }
			Log.Write ("Starting alignmet of "+ srcTerms.Count.ToString()+" "+srcLang+" and "+ trgTerms.Count.ToString()+" "+trgLang+" terms.",LogLevelType.LIMITED_OUTPUT);

            string langKey = srcLang+"_"+trgLang;
            
            MPAlignerConfigurationLangPairEntry lpeConf = new MPAlignerConfigurationLangPairEntry();
            if (configuration.langPairEntryDict.ContainsKey(langKey))
            {
                lpeConf = configuration.langPairEntryDict[langKey];
            }
            else
            {
                lpeConf = new MPAlignerConfigurationLangPairEntry();
                lpeConf.srcLang = srcLang;
                lpeConf.trgLang = trgLang;
            }
            int counter = 0;

            Dictionary<string,Dictionary<string,bool>> alignedList = new Dictionary<string, Dictionary<string, bool>>();
            List<AlignmentInfoElement> res = new List<AlignmentInfoElement>();
            foreach(string srcTerm in srcTerms.Keys)
            {
                counter++;
                if (counter%50==0)
                {
                    Console.Write(".");
                    if (counter%1000==0)
                    {
                        Console.WriteLine(" - "+counter.ToString());
                    }
                }
                ProcessedTermEntry srcPte = srcTerms[srcTerm];
                foreach(string trgTerm in trgTerms.Keys)
                {
                    ProcessedTermEntry trgPte = trgTerms[trgTerm];
                    if (srcPte!=null && trgPte!=null)
                    {
                        AlignmentInfoElement aie = new AlignmentInfoElement();
                        List<WordAlignmentElement> srcToTrg = new List<WordAlignmentElement>();
                        List<WordAlignmentElement> trgToSrc = new List<WordAlignmentElement>();
                        maxStrLen = 0;
                        
                        if (interlinguaDictUsed && interlinguaTranslitUsed)
                        {
                            
                            ///Types:
                            /// 0 - dictionary,
                            /// 1 - simple translit,
                            /// 2 - target,
                            /// 3 - translit

                            //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLATION
                            AlignStringProbabEntryListLists (lpeConf, srcPte.translationList, trgPte.translationList, srcToTrg, trgToSrc, 0, 0);
                            
                            //Translation is in EN language; SOURCE TRANSLATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 0, 1);

                            //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLITERATION
                            AlignStringProbabEntryListLists (lpeConf, srcPte.translationList, trgPte.transliterationList, srcToTrg, trgToSrc, 0, 3);

                            //Translation is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLATION
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.translationList, srcToTrg, trgToSrc, 1, 0);
                            
                            //Translation is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLATION
                            AlignStringProbabEntryListLists (lpeConf, srcPte.transliterationList, trgPte.translationList, srcToTrg, trgToSrc, 3, 0);

                            //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 3, 1);

                            //Transliteration is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLITERATION
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.transliterationList, srcToTrg, trgToSrc, 1, 3);
                            
                            //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLITERATION
                            AlignStringProbabEntryListLists (lpeConf, srcPte.transliterationList, trgPte.transliterationList, srcToTrg, trgToSrc, 3, 3);

                            //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                            
                        }
                        else if (interlinguaTranslitUsed)
                        {
                            //Translation is in target language; SOURCE TRANSLATION vs TARGET
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 0, 2);

                            //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 3, 1);

                            //Transliteration is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLITERATION
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.transliterationList, srcToTrg, trgToSrc, 1, 3);
                            
                            //Transliteration is in EN language; SOURCE TRANSLITERATION vs TARGET TRANSLITERATION
                            AlignStringProbabEntryListLists (lpeConf, srcPte.transliterationList, trgPte.transliterationList, srcToTrg, trgToSrc, 3, 2);

                            //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                        
                            //Translation is in target language; SOURCE vs TARGET TRANSLATION 
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.translationList, srcToTrg, trgToSrc, 2, 0);
                        }
                        else if (interlinguaDictUsed)
                        {
                            //Translation is in EN language; SOURCE TRANSLATION vs TARGET TRANSLATION
                            AlignStringProbabEntryListLists (lpeConf, srcPte.translationList, trgPte.translationList, srcToTrg, trgToSrc, 0, 0);

                            //Translation is in EN language; SOURCE TRANSLATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 0, 1);

                            //Translation is in EN language; SOURCE SIMPLE TRANSLITERATION vs TARGET TRANSLATION
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.simpleTransliteration, trgPte.translationList, srcToTrg, trgToSrc, 1, 0);
                            
                            //Transliteration is in target language; SOURCE TRANSLITERATION vs TARGET
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 3, 2);

                            //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);

                            //Transliteration is in target language; SOURCE vs TARGET TRANSLITERATION 
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.transliterationList, srcToTrg, trgToSrc, 2, 3);

                        }
                        else
                        {
                            //Translation is in target language; SOURCE TRANSLATION vs TARGET
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.translationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 0, 2);
                            
                            //Transliteration is in target language; SOURCE TRANSLITERATION vs TARGET
                            AlignStringProbabEntryListToStringList (lpeConf, srcPte.transliterationList, trgPte.lowercaseWords, srcToTrg, trgToSrc, 3, 2);
                            
                            //Translation is in target language; SOURCE vs TARGET TRANSLATION 
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.translationList, srcToTrg, trgToSrc, 2, 0);
                            
                            //Transliteration is in target language; SOURCE vs TARGET TRANSLITERATION 
                            AlignStringListToStringProbabEntryList (lpeConf, srcPte.lowercaseWords, trgPte.transliterationList, srcToTrg, trgToSrc, 2, 3);

                            //Simple translit of both is in EN; SOURCE SIMPLE TRANSLITERATION vs TARGET SIMPLE TRANSLITERATION
                            AlignStringLists (lpeConf, srcPte.simpleTransliteration, trgPte.simpleTransliteration, srcToTrg, trgToSrc, 1, 1);
                                                        
                        }
                        aie.srcToTrgAlignments = srcToTrg;
                        aie.trgToSrcAlignments = trgToSrc;
                        aie.srcToTrgAlignments.Sort(
                            delegate(WordAlignmentElement w1, WordAlignmentElement w2)
                            {
                                double avgW1Overlap = (w1.fromOverlap+w1.toOverlap)/2;
                                double avgW2Overlap = (w2.fromOverlap+w2.toOverlap)/2;
                                // Descending sort of toOverlap's if the 
                                if (avgW1Overlap!=avgW2Overlap)
                                {
                                    return avgW2Overlap.CompareTo(avgW1Overlap);
                                }
                                if (w2.fromLen == w1.fromLen)
                                {
                                    if (w2.toOverlap==w1.toOverlap)
                                    {
                                        return w1.fromId.CompareTo(w2.fromId);
                                    }
                                    return w2.toOverlap.CompareTo(w1.toOverlap);                                    
                                }
                                return w2.fromLen.CompareTo(w1.fromLen);
                            }
                        );
                        aie.trgToSrcAlignments.Sort(
                            delegate(WordAlignmentElement w1, WordAlignmentElement w2)
                            {
                                double avgW1Overlap = (w1.fromOverlap+w1.toOverlap)/2;
                                double avgW2Overlap = (w2.fromOverlap+w2.toOverlap)/2;
                                // Descending sort of toOverlap's if the 
                                if (avgW1Overlap!=avgW2Overlap)
                                {
                                    return avgW2Overlap.CompareTo(avgW1Overlap);
                                }
                                // Descending sort of toOverlap's if the 
                                if (w2.toLen == w1.toLen)
                                {
                                    if (w2.fromOverlap==w1.fromOverlap)
                                    {
                                        return w1.toId.CompareTo(w2.toId);
                                    }
                                    return w2.fromOverlap.CompareTo(w1.fromOverlap);                                    
                                }
                                return w2.toLen.CompareTo(w1.toLen);
                            }
                        );
                        
                        aie.srcEntry = srcPte;
                        aie.trgEntry = trgPte;
                        
                        ConsolidateOverlaps(lpeConf,aie, excDict);
                        if(CreateStrListsForEval(configuration,aie,srcStopWords,trgStopWords))
                        {
                            if (!alignedList.ContainsKey(aie.alignedLowSrcStr)||!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr))
                            {
                                if (!alignedList.ContainsKey(aie.alignedLowSrcStr)) alignedList.Add(aie.alignedLowSrcStr,new Dictionary<string, bool>());
                                if (!alignedList[aie.alignedLowSrcStr].ContainsKey(aie.alignedLowTrgStr)) alignedList[aie.alignedLowSrcStr].Add(aie.alignedLowTrgStr,true);
                                
                                aie.alignmentScore = EvaluateAlignmentScore(lpeConf,aie);
                                if (aie.alignmentScore>=lpeConf.finalAlignmentThr)
                                {
                                    //If you wish to debug the process, comment the lines below that clear the alignments...
                                    aie.srcToTrgAlignments.Clear();
                                    aie.trgToSrcAlignments.Clear();
                                    aie.consolidatedAlignment.Clear();
                                    aie.srcFile = srcFile;
                                    aie.trgFile = trgFile;
                                    res.Add(aie);
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(" - "+counter.ToString());
			Log.Write ("Alignmet finished - "+ res.Count.ToString()+" term pairs aligned over the alignment threshold " +lpeConf.finalAlignmentThr.ToString()+".\n",LogLevelType.LIMITED_OUTPUT);
            return res;
        }
        
        
        public static double EvaluateAlignmentScore(MPAlignerConfigurationLangPairEntry lpeConf,AlignmentInfoElement aie)
        {
            if (aie==null||string.IsNullOrWhiteSpace(aie.srcStrForAlignment)||string.IsNullOrWhiteSpace(aie.trgStrForAlignment))
            {
                return 0;
            }
            double maxLen = Math.Max(aie.srcStrForAlignment.Length,aie.trgStrForAlignment.Length);
            double res = (maxLen-LevenshteinDistance.Compute(aie.srcStrForAlignment,aie.trgStrForAlignment))/maxLen;
            res*=(aie.srcMultiplier+aie.trgMultiplier)/2;
            return res;
        }
        
        public static double EvaluateLevenshteinSimilarity(string src,string trg)
        {
            if (string.IsNullOrWhiteSpace(src)||string.IsNullOrWhiteSpace(trg))
            {
                return 0;
            }
            double maxLen = Math.Max(src.Length,trg.Length);
            return (maxLen-LevenshteinDistance.Compute(src,trg))/maxLen;
        }
        
        public static bool CreateStrListsForEval (MPAlignerConfiguration conf, AlignmentInfoElement aie, Dictionary<string,bool> srcStopWords, Dictionary<string,bool> trgStopWords, bool stripListsOnError = true)
        {
            if (aie == null || aie.consolidatedAlignment == null || aie.srcEntry == null || aie.trgEntry == null) {
                return false;
            }
            aie.minSrcId = Int32.MaxValue;
            aie.minTrgId = Int32.MaxValue;
            aie.maxSrcId = Int32.MinValue;
            aie.maxTrgId = Int32.MinValue;
            
            double srcMultiplier = 1;
            double trgMultiplier = 1;
            
            int prevFromId = -1;
            int prevToId = -1;

            int minSrcNonStop = Int32.MaxValue;
            int maxSrcNonStop = Int32.MinValue;
            int minTrgNonStop = Int32.MaxValue;
            int maxTrgNonStop = Int32.MinValue;
            
            //aie.finalAlignment = new List<StringComparisonElement>();
            
            StringBuilder src = new StringBuilder ();
            StringBuilder trg = new StringBuilder ();
            //StringBuilder srcL = new StringBuilder ();
            //StringBuilder trgL = new StringBuilder ();
            
            Dictionary<int,bool> srcIds = new Dictionary<int, bool> ();
            Dictionary<int,bool> trgIds = new Dictionary<int, bool> ();
            
            bool onlyStopSrc = true;
            bool onlyStopTrg = true;
            /*if (!conf.allowTrimmedAlignments)
            {
                aie.minSrcId = 0;
                aie.maxSrcId = aie.srcEntry.lowercaseWords.Count-1;
                aie.minTrgId = 0;
                aie.maxTrgId = aie.trgEntry.lowercaseWords.Count-1;
            }*/
            
            foreach (WordAlignmentElement wae in aie.consolidatedAlignment) {
                double srcLen = aie.srcEntry.lowercaseWords [wae.fromId].Length;
                double trgLen = aie.trgEntry.lowercaseWords [wae.toId].Length;
                //if (conf.allowTrimmedAlignments)
                //{
                if (wae.fromId < aie.minSrcId) {
                    aie.minSrcId = wae.fromId;
                }
                if (wae.fromId < minSrcNonStop && !srcStopWords.ContainsKey (aie.srcEntry.lowercaseWords [wae.fromId])) {
                    minSrcNonStop = wae.fromId;
                }
                if (wae.fromId > aie.maxSrcId) {
                    aie.maxSrcId = wae.fromId;
                }
                if (wae.fromId > maxSrcNonStop && !srcStopWords.ContainsKey (aie.srcEntry.lowercaseWords [wae.fromId])) {
                    maxSrcNonStop = wae.fromId;
                }
                if (wae.toId < aie.minTrgId) {
                    aie.minTrgId = wae.toId;
                }
                if (wae.toId < minTrgNonStop && !trgStopWords.ContainsKey (aie.trgEntry.lowercaseWords [wae.toId])) {
                    minTrgNonStop = wae.toId;
                }
                if (wae.toId > aie.maxTrgId) {
                    aie.maxTrgId = wae.toId;
                }
                if (wae.toId > maxTrgNonStop && !trgStopWords.ContainsKey (aie.trgEntry.lowercaseWords [wae.toId])) {
                    maxTrgNonStop = wae.toId;
                }
                //}
                //TODO: For the future - there is a limitation that you cannot evaluate alignments where for one token you have acquired overlaps in different languages (f.e., term is in [EN][LV] aligned segments, but the final string can only be in EN - it cannot be split in two parts!).
                if (!conf.allowTrimmedAlignments
                    ||(minSrcNonStop<=wae.fromId && maxSrcNonStop>=wae.fromId
                           && minTrgNonStop<=wae.toId && maxTrgNonStop>=wae.toId))
                {
                    if (wae.fromId == prevFromId) {
                        if (wae.toId != prevToId) {
                            prevToId = wae.toId;
                            //trg.Append(" ");
                            string trgStr = GetCorrectString (aie.trgEntry, wae.toId, wae.toType, wae.toTypeId);
                            if (!trgStopWords.ContainsKey (aie.trgEntry.lowercaseWords [wae.toId]))
                                onlyStopTrg = false;
                            trg.Append (trgStr);
                            trgMultiplier *= (((aie.trgEntry.len - trgLen) / aie.trgEntry.len) + (trgLen / aie.trgEntry.len) * GetProbab (aie.trgEntry, wae.toId, wae.toType, wae.toTypeId));
                        }
                    } else if (wae.toId == prevToId) {
                        if (wae.fromId != prevFromId) {
                            prevFromId = wae.fromId;
                            //src.Append(" ");
                            string srcStr = GetCorrectString (aie.srcEntry, wae.fromId, wae.fromType, wae.fromTypeId);
                            if (!srcStopWords.ContainsKey (aie.srcEntry.lowercaseWords [wae.fromId]))
                                onlyStopSrc = false;
                            src.Append (srcStr);
                            srcMultiplier *= (((aie.srcEntry.len - srcLen) / aie.srcEntry.len) + (srcLen / aie.srcEntry.len) * GetProbab (aie.srcEntry, wae.fromId, wae.fromType, wae.fromTypeId));
                        }
                    } else {
                        prevToId = wae.toId;
                        prevFromId = wae.fromId;
                        string srcStr = GetCorrectString (aie.srcEntry, wae.fromId, wae.fromType, wae.fromTypeId);
                        if (!srcStopWords.ContainsKey (aie.srcEntry.lowercaseWords [wae.fromId]))
                            onlyStopSrc = false;
                        src.Append (srcStr);
                        string trgStr = GetCorrectString (aie.trgEntry, wae.toId, wae.toType, wae.toTypeId);
                        if (!trgStopWords.ContainsKey (aie.trgEntry.lowercaseWords [wae.toId]))
                            onlyStopTrg = false;
                        trg.Append (trgStr);
                        srcMultiplier *= (((aie.srcEntry.len - srcLen) / aie.srcEntry.len) + (srcLen / aie.srcEntry.len) * GetProbab (aie.srcEntry, wae.fromId, wae.fromType, wae.fromTypeId));
                        trgMultiplier *= (((aie.trgEntry.len - trgLen) / aie.trgEntry.len) + (trgLen / aie.trgEntry.len) * GetProbab (aie.trgEntry, wae.toId, wae.toType, wae.toTypeId));
                    }
                    if (!srcIds.ContainsKey (wae.fromId))
                        srcIds.Add (wae.fromId, true);
                    if (!trgIds.ContainsKey (wae.toId))
                        trgIds.Add (wae.toId, true);
                }
            }
            
            //Try to find words in the middle of the current alignment that have not been aligned.
            //If such are found, penalise the source and target alignments by the length of the wrong alignment.
            for (int i=0; i<aie.srcEntry.lowercaseWords.Count; i++) {
                if (i >= aie.minSrcId && i <= aie.maxSrcId && !srcIds.ContainsKey (i)) {
                    string str = aie.srcEntry.lowercaseWords [i];
                    if (!srcStopWords.ContainsKey (str))
                        onlyStopSrc = false;
                    src.Append (str);
                    trg.Append (new String (' ', str.Length));
                }
            }
            
            for (int i=0; i<aie.trgEntry.lowercaseWords.Count; i++) {
                if (i >= aie.minTrgId && i <= aie.maxTrgId && !trgIds.ContainsKey (i)) {
                    string str = aie.trgEntry.lowercaseWords [i];
                    if (!trgStopWords.ContainsKey (str))
                        onlyStopTrg = false;
                    trg.Append (str);
                    src.Append (new String (' ', str.Length));
                }
            }
            
            if (src.Length > 0) {
                aie.srcStrForAlignment = src.ToString ();
                aie.trgStrForAlignment = trg.ToString ();
            }
            aie.srcMultiplier = srcMultiplier;
            aie.trgMultiplier = trgMultiplier;
            bool wasBad = false;
            if (!conf.allowTrimmedAlignments) {
                if (aie.minSrcId > 0 || aie.minTrgId > 0 || aie.maxSrcId + 1 < aie.srcEntry.lowercaseWords.Count || aie.maxTrgId + 1 < aie.trgEntry.lowercaseWords.Count) {
                    wasBad = true;
                }
                aie.minSrcId = 0;
                aie.minTrgId = 0;
                aie.maxSrcId = aie.srcEntry.lowercaseWords.Count - 1;
                aie.maxTrgId = aie.trgEntry.lowercaseWords.Count - 1;
            } else {
                if (minSrcNonStop >= 0 && minSrcNonStop < aie.srcEntry.lowercaseWords.Count
                    && maxSrcNonStop >= 0 && maxSrcNonStop < aie.srcEntry.lowercaseWords.Count
                    && minTrgNonStop >= 0 && minTrgNonStop < aie.trgEntry.lowercaseWords.Count
                    && maxTrgNonStop >= 0 && maxTrgNonStop < aie.trgEntry.lowercaseWords.Count) {
                    aie.minSrcId = minSrcNonStop;
                    aie.minTrgId = minTrgNonStop;
                    aie.maxSrcId = maxSrcNonStop;
                    aie.maxTrgId = maxTrgNonStop;
                }
                else if (stripListsOnError)
                {
                    aie.srcStrForAlignment = "";
                    aie.trgStrForAlignment = "";
                    aie.alignedLowSrcStr = "";
                    aie.alignedLowTrgStr = "";
                    return false;
                }
            }

            //Just to be on the safe side - check whether the ID's are in index boundaries:
            if (aie.minSrcId < 0 || aie.minTrgId < 0 || aie.maxSrcId + 1 > aie.srcEntry.lowercaseWords.Count || aie.maxTrgId + 1 > aie.trgEntry.lowercaseWords.Count) {
                aie.minSrcId = 0;
                aie.minTrgId = 0;
                aie.maxSrcId = aie.srcEntry.lowercaseWords.Count - 1;
                aie.maxTrgId = aie.trgEntry.lowercaseWords.Count - 1;
                if (stripListsOnError)
                {
                    aie.srcStrForAlignment = "";
                    aie.trgStrForAlignment = "";
                    aie.alignedLowSrcStr = "";
                    aie.alignedLowTrgStr = "";
                }
                return false;
            }
            
            if (wasBad) {
                if (stripListsOnError)
                {
                    aie.srcStrForAlignment = "";
                    aie.trgStrForAlignment = "";
                    aie.alignedLowSrcStr = "";
                    aie.alignedLowTrgStr = "";
                }
                return false;
            }

            if (onlyStopSrc || onlyStopTrg) {
                if (stripListsOnError)
                {
                    aie.srcStrForAlignment = "";
                    aie.trgStrForAlignment = "";
                    aie.alignedLowSrcStr = "";
                    aie.alignedLowTrgStr = "";
                }
                return false;
            }

            aie.alignedLowSrcStr = AlignmentInfoElement.GetStrFromEntry(aie.srcEntry.lowercaseWords,aie.minSrcId,aie.maxSrcId);
            aie.alignedLowTrgStr = AlignmentInfoElement.GetStrFromEntry(aie.trgEntry.lowercaseWords,aie.minTrgId,aie.maxTrgId);
            return true;
        }

        public static string GetCorrectString (ProcessedTermEntry pte,int id, short type, int typeId)
        {
            ///Types:
            /// 0 - dictionary,
            /// 1 - simple translit,
            /// 2 - target,
            /// 3 - translit
            if (type==0)
            {
                return pte.translationList[id][typeId].str;
            }
            else if (type == 1)
            {
                return pte.simpleTransliteration[id];
            }
            else if (type == 2)
            {
                return pte.lowercaseWords[id];
            }
            else if (type == 3)
            {
                return pte.transliterationList[id][typeId].str;
            }
            return null;
        }
        
        public static double GetProbab (ProcessedTermEntry pte,int id, short type, int typeId)
        {
            ///Types:
            /// 0 - dictionary,
            /// 1 - simple translit,
            /// 2 - target,
            /// 3 - translit
            if (type==0)
            {
                return pte.translationList[id][typeId].probab;
            }
            else if (type == 1)
            {
                return 1;
            }
            else if (type == 2)
            {
                return 1;
            }
            else if (type == 3)
            {
                return pte.transliterationList[id][typeId].probab;
            }
            return 1;
        }
        
        public static bool CheckLevenshteinThreshold(MPAlignerConfigurationLangPairEntry lpeConf, double similarity, short fromType, short toType)
        {
            ///Types:
            /// 0 - dictionary,
            /// 1 - simple translit,
            /// 2 - target,
            /// 3 - translit
            if (lpeConf!=null)
            {
                if (fromType == 0)
                {
                    if (toType == 0)
                    {
                        return similarity>=lpeConf.lDictEntryOverlapThr;
                    }
                    else if (toType == 1)
                    {
                        return similarity>=lpeConf.lDictToSTranslitOverlapThr;
                    }
                    else if (toType == 2)
                    {
                        return similarity>=lpeConf.lDictToWordOverlapThr;
                    }
                    else if (toType == 3)
                    {
                        return similarity>=lpeConf.lDictToTranslitOverlapThr;
                    }
                }
                else if (fromType == 1)
                {
                    if (toType == 1)
                    {
                        return similarity>=lpeConf.lSTranslitOverlapThr;
                    }
                    else if (toType == 0)
                    {
                        return similarity>=lpeConf.lDictToSTranslitOverlapThr;
                    }
                    else if (toType == 2)
                    {
                        return similarity>=lpeConf.lSTranslitToWordOverlapThr;
                    }
                    else if (toType == 3)
                    {
                        return similarity>=lpeConf.lTranslitToSTranslitOverlapThr;
                    }
                }
                else if (fromType == 3)
                {
                    if (toType == 3)
                    {
                        return similarity>=lpeConf.lTranslitEntryOverlapThr;
                    }
                    else if (toType == 1)
                    {
                        return similarity>=lpeConf.lTranslitToSTranslitOverlapThr;
                    }
                    else if (toType == 2)
                    {
                        return similarity>=lpeConf.lTranslitToWordOverlapThr;
                    }
                    else if (toType == 0)
                    {
                        return similarity>=lpeConf.lDictToTranslitOverlapThr;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("The language pair configuration entry is empty!");
            }
            return false; //Such a combination should not be possible at all...
        }
        
        public static bool CheckOverlapThreshold(MPAlignerConfigurationLangPairEntry lpeConf, double overlap, short fromType, short toType)
        {
            ///Types:
            /// 0 - dictionary,
            /// 1 - simple translit,
            /// 2 - target,
            /// 3 - translit
            if (lpeConf!=null)
            {
                if (fromType == 0)
                {
                    if (toType == 0)
                    {
                        return overlap>=lpeConf.dictEntryOverlapThr;
                    }
                    else if (toType == 1)
                    {
                        return overlap>=lpeConf.dictToSTranslitOverlapThr;
                    }
                    else if (toType == 2)
                    {
                        return overlap>=lpeConf.dictToWordOverlapThr;
                    }
                    else if (toType == 3)
                    {
                        return overlap>=lpeConf.dictToTranslitOverlapThr;
                    }
                }
                else if (fromType == 1)
                {
                    if (toType == 1)
                    {
                        return overlap>=lpeConf.sTranslitOverlapThr;
                    }
                    else if (toType == 0)
                    {
                        return overlap>=lpeConf.dictToSTranslitOverlapThr;
                    }
                    else if (toType == 2)
                    {
                        return overlap>=lpeConf.sTranslitToWordOverlapThr;
                    }
                    else if (toType == 3)
                    {
                        return overlap>=lpeConf.translitToSTranslitOverlapThr;
                    }
                }
                else if (fromType == 3)
                {
                    if (toType == 3)
                    {
                        return overlap>=lpeConf.translitEntryOverlapThr;
                    }
                    else if (toType == 1)
                    {
                        return overlap>=lpeConf.translitToSTranslitOverlapThr;
                    }
                    else if (toType == 2)
                    {
                        return overlap>=lpeConf.translitToWordOverlapThr;
                    }
                    else if (toType == 0)
                    {
                        return overlap>=lpeConf.dictToTranslitOverlapThr;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("The language pair configuration entry is empty!");
            }
            return false; //Such a combination should not be possible at all...
        }
        
        //private static List<WordAlignmentElement> backupList;

        static void AlignStrings (MPAlignerConfigurationLangPairEntry lpeConf, List<WordAlignmentElement> srcToTrg, List<WordAlignmentElement> trgToSrc, short fromType, short toType,  int srcId, int srcTypeId, int trgId, int trgTypeId, string srcStr, string trgStr)// Dictionary<string, Dictionary<string, bool>> excDict)
        {
            //if (excDict==null||!excDict.ContainsKey(srcStr)||!excDict[srcStr].ContainsKey(trgStr))
            //{
                //TODO: There is a bug in here - the invalid alignment dictionary is incorrectly applied! That is, the validation should have been between lowercased tokens rather than translation equivalents!
                int srcIdStart = -1;
                int srcIdEnd = -1;
                int trgIdStart = -1;
                int trgIdEnd = -1;
                string matching = LongestCommonSubstring.Get(srcStr, trgStr,out srcIdStart, out srcIdEnd, out trgIdStart, out trgIdEnd);
                
                double lenDiff = matching.Length;
                lenDiff/=Math.Min(srcStr.Length, trgStr.Length);
                if (matching!=null && CheckOverlapThreshold(lpeConf, lenDiff, fromType, toType)
                && (!lpeConf.enforce1stChar || (Check1stCharOverlapValidity(srcIdStart,srcIdEnd,trgIdStart,trgIdEnd) && (!lpeConf.enforce2ndChar || Check2ndCharOverlapValidity(srcIdStart,srcIdEnd,trgIdStart,trgIdEnd)))))
                {
                    ///Types:
                    /// 0 - dictionary,
                    /// 1 - simple translit,
                    /// 2 - target,
                    /// 3 - translit
                    double srcOverlap = ((double)(srcIdEnd-srcIdStart+1))/((double)srcStr.Length);
                    double trgOverlap = ((double)(trgIdEnd-trgIdStart+1))/((double)trgStr.Length);
                    WordAlignmentElement wae = CreateWordAlignmentElement(trgStr, trgIdStart, trgIdEnd, 1, srcId, trgId, fromType, toType, srcTypeId, trgTypeId, srcOverlap,srcStr.Length, srcIdStart, trgIdStart);
                    srcToTrg.Add(wae);
                    wae = CreateWordAlignmentElement(srcStr, srcIdStart, srcIdEnd, -1, srcId, trgId, fromType, toType, srcTypeId, trgTypeId, trgOverlap,trgStr.Length, srcIdStart, trgIdStart);
                    trgToSrc.Add(wae);
                }
                else if (matching!=null)
                {
                    double similarity = EvaluateLevenshteinSimilarity(srcStr,trgStr);
                    //We limit the levenshtein similarity results so that the first two characters have to match!
                    //This is done to get rid of a lot of pre-fix mismatching mistakes...
                    if (CheckLevenshteinThreshold(lpeConf, similarity, fromType, toType)&&srcStr.Length>2&&trgStr.Length>2&&srcStr[0]==trgStr[0]&&srcStr[1]==trgStr[1])
                    {

                        double srcOverlap = similarity;
                        double trgOverlap = similarity;
                        WordAlignmentElement wae = CreateWordAlignmentElement(trgStr, 0, trgStr.Length-1, 1, srcId, trgId, fromType, toType, srcTypeId, trgTypeId, srcOverlap,srcStr.Length, 0, 0, false);
                        wae.isLevenshtein = true;
                        srcToTrg.Add(wae);
                        wae = CreateWordAlignmentElement(srcStr, 0, srcStr.Length-1, -1, srcId, trgId, fromType, toType, srcTypeId, trgTypeId, trgOverlap,trgStr.Length, 0, 0,false);
                        wae.isLevenshtein = true;
                        trgToSrc.Add(wae);
                    }
                }
            //}
        }

        static bool Check1stCharOverlapValidity (int srcIdStart, int srcIdEnd, int trgIdStart, int trgIdEnd)
        {
            if (srcIdStart==1||trgIdStart==1)
            {
                return false;
            }
            return true;
        }

        static bool Check2ndCharOverlapValidity (int srcIdStart, int srcIdEnd, int trgIdStart, int trgIdEnd)
        {
            if (srcIdStart==2||trgIdStart==2)
            {
                return false;
            }
            return true;
        }
        
        public static void AlignStringProbabEntryListToStringList (MPAlignerConfigurationLangPairEntry lpeConf, List<List<StringProbabEntry>> srcList, List<string> trgList, List<WordAlignmentElement> srcToTrg, List<WordAlignmentElement> trgToSrc, short fromType, short toType)//, Dictionary<string, Dictionary<string, bool>> excDict)
        {
            if (srcToTrg == null) {
                srcToTrg = new List<WordAlignmentElement> ();
            }
            if (trgToSrc == null) {
                trgToSrc = new List<WordAlignmentElement>();
            }
            for (int srcId=0; srcId<srcList.Count;srcId++)
            {
                for (int srcTypeId=0;srcTypeId<srcList[srcId].Count;srcTypeId++)
                {
                    for (int trgId=0; trgId<trgList.Count;trgId++)
                    {
                        string srcStr = srcList[srcId][srcTypeId].str;
                        string trgStr = trgList[trgId];
                        int trgTypeId = -1;
                        AlignStrings (lpeConf, srcToTrg, trgToSrc, fromType, toType, srcId, srcTypeId, trgId, trgTypeId, srcStr, trgStr);//, excDict);
                    }
                }
            }
        }
        
        
        
        public static void AlignStringLists (MPAlignerConfigurationLangPairEntry lpeConf, List<string> srcList, List<string> trgList, List<WordAlignmentElement> srcToTrg, List<WordAlignmentElement> trgToSrc, short fromType, short toType)//, Dictionary<string, Dictionary<string, bool>> excDict)
        {
            if (srcToTrg == null) {
                srcToTrg = new List<WordAlignmentElement> ();
            }
            if (trgToSrc == null) {
                trgToSrc = new List<WordAlignmentElement>();
            }
            for (int srcId=0; srcId<srcList.Count;srcId++)
            {
                for (int trgId=0; trgId<trgList.Count;trgId++)
                {
                    string srcStr = srcList[srcId];
                    string trgStr = trgList[trgId];
                    int srcTypeId = -1;
                    int trgTypeId = -1;
                    AlignStrings (lpeConf, srcToTrg, trgToSrc, fromType, toType, srcId, srcTypeId, trgId, trgTypeId, srcStr, trgStr);//, excDict);
                }
            }

        }
        
        public static void AlignStringListToStringProbabEntryList (MPAlignerConfigurationLangPairEntry lpeConf, List<string> srcList, List<List<StringProbabEntry>> trgList, List<WordAlignmentElement> srcToTrg, List<WordAlignmentElement> trgToSrc, short fromType, short toType)//, Dictionary<string, Dictionary<string, bool>> excDict)
        {
            if (srcToTrg == null) {
                srcToTrg = new List<WordAlignmentElement> ();
            }
            if (trgToSrc == null) {
                trgToSrc = new List<WordAlignmentElement>();
            }
            
            for (int srcId=0; srcId<srcList.Count;srcId++)
            {
                for (int trgId=0; trgId<trgList.Count;trgId++)
                {
                    for (int trgTypeId =0;trgTypeId<trgList[trgId].Count;trgTypeId++)
                    {
                        
                        string srcStr = srcList[srcId];
                        string trgStr = trgList[trgId][trgTypeId].str;
                        int srcTypeId = -1;
                        AlignStrings (lpeConf, srcToTrg, trgToSrc, fromType, toType, srcId, srcTypeId, trgId, trgTypeId, srcStr, trgStr);//, excDict);
                    }
                }
            }
        }
        
        public static void AlignStringProbabEntryListLists (MPAlignerConfigurationLangPairEntry lpeConf, List<List<StringProbabEntry>> srcList, List<List<StringProbabEntry>> trgList, List<WordAlignmentElement> srcToTrg, List<WordAlignmentElement> trgToSrc, short fromType, short toType)//, Dictionary<string, Dictionary<string, bool>> excDict)
        {
            if (srcToTrg == null) {
                srcToTrg = new List<WordAlignmentElement> ();
            }
            if (trgToSrc == null) {
                trgToSrc = new List<WordAlignmentElement>();
            }
            for (int srcId=0; srcId<srcList.Count;srcId++)
            {
                for (int srcTypeId=0;srcTypeId<srcList[srcId].Count;srcTypeId++)
                {
                    for (int trgId=0; trgId<trgList.Count;trgId++)
                    {
                        for (int trgTypeId =0;trgTypeId<trgList[trgId].Count;trgTypeId++)
                        {
                            string srcStr = srcList[srcId][srcTypeId].str;
                            string trgStr = trgList[trgId][trgTypeId].str;
                            AlignStrings (lpeConf, srcToTrg, trgToSrc, fromType, toType, srcId, srcTypeId, trgId, trgTypeId, srcStr, trgStr);//, excDict);
                        }
                    }
                }
            }

        }
        
        public static WordAlignmentElement CreateWordAlignmentElement(string str, int startId, int endId, int direction, int fromId, int toId, short fromType, short toType, int fromTypeId, int toTypeId, double fromOverlap, int fromLen, int fromIndex, int toIndex, bool changeOverlap = true)
        {
            if (str.Length>maxStrLen) maxStrLen = str.Length;
            if (fromLen>maxStrLen) maxStrLen = str.Length;
            WordAlignmentElement wae = new WordAlignmentElement();
            wae.alignmentMap = new bool[str.Length];// Array.CreateInstance(typeof(bool),str.Length);
            int overlap=0;
            for (int i=0;i<str.Length;i++)
            {
                if (i<startId||i>endId)
                {
                    wae.alignmentMap[i]=false;
                }
                else
                {
                    wae.alignmentMap[i]=true;
                    overlap++;
                }
            }
            if (direction==1)
            {
                if (changeOverlap) wae.toOverlap = overlap/str.Length;
                else wae.toOverlap = fromOverlap;
                wae.fromOverlap = fromOverlap;
                wae.fromLen = fromLen;
                wae.toLen = str.Length;
            }
            else
            {
                wae.toOverlap = fromOverlap;
                if (changeOverlap) wae.fromOverlap = overlap/str.Length;
                else wae.fromOverlap = fromOverlap;
                wae.fromLen = str.Length;
                wae.toLen = fromLen;
            }
            wae.fromStartIndex = fromIndex;
            wae.toStartIndex = toIndex;
            wae.direction = direction;
            wae.fromId = fromId;
            wae.toId = toId;
            wae.fromType = fromType;
            wae.toType = toType;
            wae.fromTypeId = fromTypeId;
            wae.toTypeId = toTypeId;            
            return wae;
        }
        

        
        public static void ConsolidateOverlaps(MPAlignerConfigurationLangPairEntry lpeConf,AlignmentInfoElement aie, Dictionary<string, Dictionary<string, bool>> excDict)
        {
            if (lpeConf==null||aie==null||aie.srcEntry==null||aie.trgEntry==null)
            {
                return;
            }
            if (aie.srcToTrgAlignments==null || aie.trgToSrcAlignments==null)
            {
                return;
            }
            //Options:
            //Iterate through srcToTrg and find top 1 overlap for each source ID (do not worry about repetitive word overlaps at this point!)
            Dictionary<int, WordAlignmentElement> maxSrcToTrgOverlaps = new Dictionary<int, WordAlignmentElement>();
            Dictionary<int, bool[]> trgStrOverlapDict = new Dictionary<int, bool[]>();
            
            foreach(WordAlignmentElement wae in aie.srcToTrgAlignments)
            {
                if (excDict==null||!excDict.ContainsKey(aie.srcEntry.lowercaseWords[wae.fromId])||!excDict[aie.srcEntry.lowercaseWords[wae.fromId]].ContainsKey(aie.trgEntry.lowercaseWords[wae.toId]))
                {
                    if (wae.fromOverlap<lpeConf.minShortFragmentOverlap && wae.fromLen<lpeConf.minShortFragmentLen||
                        wae.fromLen<lpeConf.minShortFragmentLen && wae.toLen>lpeConf.maxShortFragmentTargetLen)
                    {
                        //In the case if there is a "short" fragment and the target length is "big", the alignment won't be applied (this is to limit false alignments!).
                    }
                    else if (Math.Max(wae.fromOverlap,wae.toOverlap)>=lpeConf.minSrcOrTrgOverlap) // this
                    {
                        if (!maxSrcToTrgOverlaps.ContainsKey(wae.fromId))
                        {
                            if (trgStrOverlapDict.ContainsKey(wae.toId) && !IsOverlapConflict(lpeConf, trgStrOverlapDict[wae.toId], wae.alignmentMap, null))
                            {
                                trgStrOverlapDict[wae.toId] = AdjustAlignmentMap(lpeConf,trgStrOverlapDict[wae.toId],wae.alignmentMap, null);
                                maxSrcToTrgOverlaps.Add(wae.fromId, wae);
                            }
                            else if (!trgStrOverlapDict.ContainsKey(wae.toId))
                            {
                                trgStrOverlapDict.Add(wae.toId,new bool[maxStrLen]); //This way we ensure that small alignments do not cause big problems!
                                trgStrOverlapDict[wae.toId] = AdjustAlignmentMap(lpeConf,trgStrOverlapDict[wae.toId],wae.alignmentMap, null);
                                maxSrcToTrgOverlaps.Add(wae.fromId, wae);
                                //This is when for a target ID there is no alignment map already specified (should not happen, but just in case...
                            }
                        }
                        else if (wae.fromOverlap>maxSrcToTrgOverlaps[wae.fromId].fromOverlap)
                        {
                            if (wae.toId == maxSrcToTrgOverlaps[wae.fromId].toId)
                            {
                                if (trgStrOverlapDict.ContainsKey(wae.toId) && !IsOverlapConflict(lpeConf, trgStrOverlapDict[wae.toId], wae.alignmentMap, maxSrcToTrgOverlaps[wae.fromId].alignmentMap))
                                {
                                    trgStrOverlapDict[wae.toId] = AdjustAlignmentMap(lpeConf,trgStrOverlapDict[wae.toId],wae.alignmentMap, maxSrcToTrgOverlaps[wae.fromId].alignmentMap);
                                    maxSrcToTrgOverlaps[wae.fromId] = wae;
                                }
                                else if (!trgStrOverlapDict.ContainsKey(wae.toId))
                                {
                                    trgStrOverlapDict.Add(wae.toId,new bool[maxStrLen]);
                                    trgStrOverlapDict[wae.toId] = AdjustAlignmentMap(lpeConf,trgStrOverlapDict[wae.toId],wae.alignmentMap, null);
                                    maxSrcToTrgOverlaps[wae.fromId] = wae;
                                    //This is when for a target ID there is no alignment map already specified (should not happen, but just in case...
                                }
                            }
                            else
                            {
                                //If the target ID changes then we have to remove alignment from the target map. As we do only one pass through the alignments, some important updates can be lost in this way if the list is not sorted.
                                if (trgStrOverlapDict.ContainsKey(wae.toId) && !IsOverlapConflict(lpeConf, trgStrOverlapDict[wae.toId], wae.alignmentMap, null))
                                {
                                    //At first, remove alignment from the previous target.
                                    if (trgStrOverlapDict.ContainsKey(maxSrcToTrgOverlaps[wae.fromId].toId)) trgStrOverlapDict[maxSrcToTrgOverlaps[wae.fromId].toId] = RemoveFromAlignmentMap (lpeConf,trgStrOverlapDict[maxSrcToTrgOverlaps[wae.fromId].toId], maxSrcToTrgOverlaps[wae.fromId].alignmentMap);
                                    //Now adjust the new target alignment.
                                    trgStrOverlapDict[wae.toId] = AdjustAlignmentMap(lpeConf,trgStrOverlapDict[wae.toId],wae.alignmentMap, null);
                                    maxSrcToTrgOverlaps[wae.fromId] = wae;
                                }
                                else if (!trgStrOverlapDict.ContainsKey(wae.toId))
                                {
                                    //At first, remove alignment from the previous target.
                                    if (trgStrOverlapDict.ContainsKey(maxSrcToTrgOverlaps[wae.fromId].toId)) trgStrOverlapDict[maxSrcToTrgOverlaps[wae.fromId].toId] = RemoveFromAlignmentMap (lpeConf,trgStrOverlapDict[maxSrcToTrgOverlaps[wae.fromId].toId], maxSrcToTrgOverlaps[wae.fromId].alignmentMap);
                                    //Now create the new target alignment.
                                    trgStrOverlapDict.Add(wae.toId,new bool[maxStrLen]);
                                    trgStrOverlapDict[wae.toId] = AdjustAlignmentMap(lpeConf,trgStrOverlapDict[wae.toId],wae.alignmentMap, null);
                                    maxSrcToTrgOverlaps[wae.fromId] = wae;
                                    //This is when for a target ID there is no alignment map already specified (should not happen, but just in case...
                                }
                                //This is when for a source ID the target ID changes.
                            }
                        }
                    }
                }
            }
            
            
            Dictionary<int, WordAlignmentElement> maxTrgToSrcOverlaps = new Dictionary<int, WordAlignmentElement>();
            Dictionary<int, bool[]> srcStrOverlapDict = new Dictionary<int, bool[]>();
            
            foreach(WordAlignmentElement wae in aie.trgToSrcAlignments)
            {
                if (excDict==null||!excDict.ContainsKey(aie.srcEntry.lowercaseWords[wae.fromId])||!excDict[aie.srcEntry.lowercaseWords[wae.fromId]].ContainsKey(aie.trgEntry.lowercaseWords[wae.toId]))
                {
                    if (wae.toOverlap<lpeConf.minShortFragmentOverlap && wae.fromLen<lpeConf.minShortFragmentLen||
                        wae.toLen<lpeConf.minShortFragmentLen && wae.fromLen>lpeConf.maxShortFragmentTargetLen)
                    {
                        //In the case if there is a "short" fragment and the target length is "big", the alignment won't be applied (this is to limit false alignments!).
                    }
                    else if (Math.Max(wae.fromOverlap,wae.toOverlap)>=lpeConf.minSrcOrTrgOverlap)
                    {
                        if (!maxTrgToSrcOverlaps.ContainsKey(wae.toId))
                        {
                            if (srcStrOverlapDict.ContainsKey(wae.fromId) && !IsOverlapConflict(lpeConf, srcStrOverlapDict[wae.fromId], wae.alignmentMap, null))
                            {
                                srcStrOverlapDict[wae.fromId] = AdjustAlignmentMap(lpeConf,srcStrOverlapDict[wae.fromId],wae.alignmentMap, null);
                                maxTrgToSrcOverlaps.Add(wae.toId, wae);
                            }
                            else if (!srcStrOverlapDict.ContainsKey(wae.fromId))
                            {
                                srcStrOverlapDict.Add(wae.fromId,new bool[maxStrLen]); //This way we ensure that small alignments do not cause big problems!
                                srcStrOverlapDict[wae.toId] = AdjustAlignmentMap(lpeConf,srcStrOverlapDict[wae.fromId],wae.alignmentMap, null);
                                maxTrgToSrcOverlaps.Add(wae.toId, wae);
                                //This is when for a target ID there is no alignment map already specified (should not happen, but just in case...
                            }
                        }
                        else if (wae.toOverlap>maxTrgToSrcOverlaps[wae.toId].toOverlap)
                        {
                            if (wae.fromId == maxTrgToSrcOverlaps[wae.toId].fromId)
                            {
                                if (srcStrOverlapDict.ContainsKey(wae.fromId) && !IsOverlapConflict(lpeConf, srcStrOverlapDict[wae.fromId], wae.alignmentMap, maxTrgToSrcOverlaps[wae.toId].alignmentMap))
                                {
                                    srcStrOverlapDict[wae.fromId] = AdjustAlignmentMap(lpeConf,srcStrOverlapDict[wae.fromId],wae.alignmentMap, maxTrgToSrcOverlaps[wae.toId].alignmentMap);
                                    maxTrgToSrcOverlaps[wae.toId] = wae;
                                }
                                else if (!srcStrOverlapDict.ContainsKey(wae.fromId))
                                {
                                    srcStrOverlapDict.Add(wae.fromId,new bool[maxStrLen]);
                                    srcStrOverlapDict[wae.fromId] = AdjustAlignmentMap(lpeConf,srcStrOverlapDict[wae.fromId],wae.alignmentMap, null);
                                    maxTrgToSrcOverlaps[wae.toId] = wae;
                                    //This is when for a target ID there is no alignment map already specified (should not happen, but just in case...
                                }
                            }
                            else
                            {
                                //If the target ID changes then we have to remove alignment from the target map. As we do only one pass through the alignments, some important updates can be lost in this way if the list is not sorted.
                                if (srcStrOverlapDict.ContainsKey(wae.fromId) && !IsOverlapConflict(lpeConf, srcStrOverlapDict[wae.fromId], wae.alignmentMap, null))
                                {
                                    //At first, remove alignment from the previous target.
                                    if (srcStrOverlapDict.ContainsKey(maxTrgToSrcOverlaps[wae.toId].fromId)) srcStrOverlapDict[maxTrgToSrcOverlaps[wae.toId].fromId] = RemoveFromAlignmentMap (lpeConf,srcStrOverlapDict[maxTrgToSrcOverlaps[wae.toId].fromId], maxTrgToSrcOverlaps[wae.toId].alignmentMap);
                                    //Now adjust the new target alignment.
                                    srcStrOverlapDict[wae.fromId] = AdjustAlignmentMap(lpeConf,srcStrOverlapDict[wae.fromId],wae.alignmentMap, null);
                                    maxTrgToSrcOverlaps[wae.toId] = wae;
                                }
                                else if (!srcStrOverlapDict.ContainsKey(wae.fromId))
                                {
                                    //At first, remove alignment from the previous target.
                                    if (srcStrOverlapDict.ContainsKey(maxTrgToSrcOverlaps[wae.toId].fromId)) srcStrOverlapDict[maxTrgToSrcOverlaps[wae.toId].fromId] = RemoveFromAlignmentMap (lpeConf,srcStrOverlapDict[maxTrgToSrcOverlaps[wae.toId].fromId], maxTrgToSrcOverlaps[wae.toId].alignmentMap);
                                    //Now create the new target alignment.
                                    srcStrOverlapDict.Add(wae.fromId,new bool[maxStrLen]);
                                    srcStrOverlapDict[wae.fromId] = AdjustAlignmentMap(lpeConf,srcStrOverlapDict[wae.fromId],wae.alignmentMap, null);
                                    maxTrgToSrcOverlaps[wae.toId] = wae;
                                    //This is when for a target ID there is no alignment map already specified (should not happen, but just in case...
                                }
                                //This is when for a source ID the target ID changes.
                            }
                        }
                    }
                }
            }
            
            //At this stage we have one alignment from SRC to TRG and one from TRG to SRC. We need to consolidate both and also trim the beginning and the end...
            aie.consolidatedAlignment = ConsolidateAlignments(aie.srcEntry.lowercaseWords.Count, aie.trgEntry.lowercaseWords.Count, maxSrcToTrgOverlaps,maxTrgToSrcOverlaps);
            
            
        }
        
        public static List<WordAlignmentElement> ConsolidateAlignments (int srcCount, int trgCount, Dictionary<int, WordAlignmentElement> maxSrcToTrgOverlaps, Dictionary<int, WordAlignmentElement> maxTrgToSrcOverlaps)
        {
            if (srcCount==0 || trgCount == 0 || maxSrcToTrgOverlaps == null || maxTrgToSrcOverlaps == null)
            {
                return null;
            }
            List<WordAlignmentElement> res = new List<WordAlignmentElement>();
            Dictionary<int, bool> isSrcAligned = new Dictionary<int, bool>();
            Dictionary<int, bool> isTrgAligned = new Dictionary<int, bool>();
            for(int i=0;i<srcCount;i++)
            {
                if (maxSrcToTrgOverlaps.ContainsKey(i))
                {
                    if(maxSrcToTrgOverlaps[i].fromOverlap>=maxSrcToTrgOverlaps[i].toOverlap)
                    {
                        res.Add(maxSrcToTrgOverlaps[i]);
                        isSrcAligned.Add(i,true);
                        if (!isTrgAligned.ContainsKey(maxSrcToTrgOverlaps[i].toId)) isTrgAligned.Add(maxSrcToTrgOverlaps[i].toId,true);
                    }
                    else if (maxSrcToTrgOverlaps[i].fromOverlap<maxSrcToTrgOverlaps[i].toOverlap && !maxTrgToSrcOverlaps.ContainsKey(maxSrcToTrgOverlaps[i].toId))
                    {
                        res.Add(maxSrcToTrgOverlaps[i]);
                        isSrcAligned.Add(i,true);
                        if (!isTrgAligned.ContainsKey(maxSrcToTrgOverlaps[i].toId)) isTrgAligned.Add(maxSrcToTrgOverlaps[i].toId,true);
                    }
                }
            }
            for(int i=0;i<trgCount;i++)
            {
                if (!isTrgAligned.ContainsKey(i) && maxTrgToSrcOverlaps.ContainsKey(i))
                {
                    if(!isSrcAligned.ContainsKey(maxTrgToSrcOverlaps[i].fromId) && maxTrgToSrcOverlaps[i].toOverlap>=maxTrgToSrcOverlaps[i].fromOverlap)
                    {
                        res.Add(maxTrgToSrcOverlaps[i]);
                    }
                }
            }
            res.Sort();
            return res;
        }
        
        
        public static bool[] RemoveFromAlignmentMap (MPAlignerConfigurationLangPairEntry lpeConf,bool[] mapOne, bool[] mapTwo)
        {
            
            if (mapOne==null||mapTwo==null)
            {
                throw new ArgumentNullException("Either mapOne or mapTwo are empty (null)!");
            }
            bool[] res = mapOne;
            
            int l1 = mapOne.Length;
            int l2 = mapTwo.Length;
            
            for (int i=0;i<mapOne.Length;i++)
            {
                int adjM2Index = (i*l2)/l1;
                if (mapTwo[adjM2Index])
                {
                    res[i]=false;
                }
            }
            return res;
        }
        
        public static bool[] AdjustAlignmentMap (MPAlignerConfigurationLangPairEntry lpeConf,bool[] mapOne, bool[] mapTwo, bool[] formerMap)
        {
            
            if (mapOne==null||mapTwo==null)
            {
                throw new ArgumentNullException("Either mapOne or mapTwo are empty (null)!");
            }
            bool[] res = mapOne;
            
            int l1 = mapOne.Length;
            int l2 = mapTwo.Length;
            int l3 = formerMap!=null?formerMap.Length:-1;
            
            for (int i=0;i<mapOne.Length;i++)
            {
                int adjM2Index = (i*l2)/l1;
                int adjM3Index = (i*l3)/l1;
                if (mapTwo[adjM2Index])
                {
                    res[i]=true;
                }
                else if (formerMap!=null && formerMap[adjM3Index]==true)
                {
                    res[i]=false; //If we changed the alignments, we need to remove alignments from the former alignment map.
                }
            }
            return res;
        }
        
        /// <summary>
        /// Determines whether there is an overlap conflict between mapOne and mapTwo. For valid overlaps the validMap is used.
        /// This is an experimental implementation!
        /// TODO: Analyse effect of the variations in length to the colnflict detection!
        /// </summary>
        /// <returns>
        /// <c>true</c> if there is an overlap conflict between mapOne and mapTwo; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='lpeConf'>
        /// The language pair configuration entry.
        /// </param>
        /// <param name='mapOne'>
        /// Map one - boolean array.
        /// </param>
        /// <param name='mapTwo'>
        /// Map two - boolean array.
        /// </param>
        /// <param name='validMap'>
        /// Valid alignment map - boolean array.
        /// </param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
        /// </exception>
        public static bool IsOverlapConflict (MPAlignerConfigurationLangPairEntry lpeConf,bool[] mapOne, bool[] mapTwo, bool[] validMap)
        {
            int overlaps = 0;
            if (mapOne==null||mapTwo==null)
            {
                throw new ArgumentNullException("Either mapOne, mapTwo or validMap are empty (null)!");
            }
            int l1 = mapOne.Length;
            int l2 = mapTwo.Length;
            int l3 = validMap!=null?validMap.Length:-1;
            for (int i=0;i<mapOne.Length;i++)
            {
                int adjM2Index = (i*l2)/l1;
                int adjM3Index = (i*l3)/l1;
                if (mapOne[i]==mapTwo[adjM2Index]&& mapTwo[adjM2Index] && (validMap ==null || !validMap[adjM3Index]))
                {
                    overlaps++;
                    if (overlaps>lpeConf.maxOverlapCharsInCompounds)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

