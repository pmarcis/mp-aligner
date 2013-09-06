//
//  PreprocessedTermData.cs
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
using System.Xml;
using System.IO;
using MPFramework;
using System.Text;

namespace MPAligner
{
    public class PreprocessedTermData
    {
        public PreprocessedTermData ()
        {
        }
        
        public ProcessedTermEntry[] srcTerms;
        public ProcessedTermEntry[] trgTerms;
        public bool interlinguaDictUsed;
        public bool interlinguaTranslitUsed;
        
        public string srcLang;
        public string trgLang;
        
        public static PreprocessedTermData ReadFromFile (string inputFile)
        {
            string inputStr = File.ReadAllText(inputFile,Encoding.UTF8);
            return MPFrameworkFunctions.DeserializeString<PreprocessedTermData>(inputStr);
        }
    }
}

