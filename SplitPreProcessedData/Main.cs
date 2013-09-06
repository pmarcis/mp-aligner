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
using MPAligner;
using MPFramework;
using System.IO;

namespace SplitPreProcessedData
{
    class MainClass
    {
		/// <summary>
		/// This program is used only for testing (it has no other purpose).
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
        public static void Main (string[] args)
        {
            PreprocessedTermData ptd = PreprocessedTermData.ReadFromFile(args[0]);
            bool isInterlinguaDict = ptd.interlinguaDictUsed;
            bool isInterlinguaTranslit = ptd.interlinguaTranslitUsed;
            string outDir = args[1].EndsWith(Path.DirectorySeparatorChar.ToString())?args[1]:args[1]+Path.DirectorySeparatorChar.ToString();
            
            //string fileOne = outDir+"eurovoc_preprocessed_"+(isInterlinguaDict?"id_":"sd_")+(isInterlinguaTranslit?"it_":"st_")+ptd.srcLang+(isInterlinguaDict&&isInterlinguaTranslit?"_en":"_"+ptd.trgLang)+".xml";
            //string fileTwo = outDir+"eurovoc_preprocessed_"+(isInterlinguaDict?"id_":"sd_")+(isInterlinguaTranslit?"it_":"st_")+ptd.trgLang+(isInterlinguaDict&&isInterlinguaTranslit?"_en":"_"+ptd.srcLang)+".xml";
            string fileOne = outDir+"eurovoc_preprocessed_"+ptd.srcLang+(isInterlinguaDict&&isInterlinguaTranslit?"_en":"_"+ptd.trgLang)+".xml";
            string fileTwo = outDir+"eurovoc_preprocessed_"+ptd.trgLang+(isInterlinguaDict&&isInterlinguaTranslit?"_en":"_"+ptd.srcLang)+".xml";

            string outStrOne = MPFramework.MPFrameworkFunctions.SerializeObjectInstance<ProcessedTermEntry[]>(ptd.srcTerms);
            string outStrTwo = MPFramework.MPFrameworkFunctions.SerializeObjectInstance<ProcessedTermEntry[]>(ptd.trgTerms);
            
            if(!File.Exists(fileOne))
                File.WriteAllText(fileOne,outStrOne);
            if(!File.Exists(fileTwo))
                File.WriteAllText(fileTwo,outStrTwo);
            
                    /*foreach(ProcessedTermEntry pte in ptd.srcTerms)
                    {
                        if(!srcTermList.ContainsKey(pte.lowercaceForm))
                        {
                            srcTermList.Add(pte.lowercaceForm,pte);
                        }
                    }
                    foreach(ProcessedTermEntry pte in ptd.trgTerms)
                    {
                        if(!trgTermList.ContainsKey(pte.lowercaceForm))
                        {
                            trgTermList.Add(pte.lowercaceForm,pte);
                        }
                    }
                    srcLang = ptd.srcLang;
                    trgLang = ptd.trgLang;
                    interlinguaDictUsed = ptd.interlinguaDictUsed;
                    interlinguaTranslitUsed = ptd.interlinguaTranslitUsed;*/
                    
            
            /*if (!string.IsNullOrWhiteSpace(preProcessedTermOutputFile))
                    {
                        List<ProcessedTermEntry> srcTerms = new List<ProcessedTermEntry>(srcTermList.Values);
                        List<ProcessedTermEntry> trgTerms = new List<ProcessedTermEntry>(trgTermList.Values);
                        PreprocessedTermData ptd = new PreprocessedTermData();
                        ptd.interlinguaDictUsed = interlinguaDictUsed;
                        ptd.interlinguaTranslitUsed = interlinguaTranslitUsed;
                        ptd.srcTerms = srcTerms.ToArray();
                        ptd.trgTerms = trgTerms.ToArray();
                        ptd.srcLang = srcLang;
                        ptd.trgLang = trgLang;
                        
                        string outStr = MPFramework.MPFrameworkFunctions.SerializeObjectInstance<PreprocessedTermData>(ptd);
                        File.WriteAllText(preProcessedTermOutputFile,outStr);
                    }*/
        }
    }
}
