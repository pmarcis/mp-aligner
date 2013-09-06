//
//  ExceptionDictionaryParser.cs
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

namespace MPAligner
{
    public class ExceptionDictionaryParser
    {
        public ExceptionDictionaryParser ()
        {
        }

        public static Dictionary<string, Dictionary<string, bool>> ParseExceptionDictionary (MPAlignerConfigurationExceptionEntry cee)
        {
            string fileName = cee.path;
            Encoding enc = cee.encoding;
            char[] sep = {'\t', ' '};
            Dictionary<string, Dictionary<string, bool>> res = new Dictionary<string, Dictionary<string, bool>> ();
            StreamReader sr = new StreamReader (fileName, enc);
            //Read the dictionary file to the end.
            while (!sr.EndOfStream) {
                string line = sr.ReadLine ().Trim ();
                string[] data = line.Split (sep, StringSplitOptions.RemoveEmptyEntries);
                //Apply valid entry filtering.
                if (data.Length >= 2) { //We ignore lines that do not have two constituents - source word/phrase, target word/phrase.

                    string src = data[0].ToLower();
                    string trg = data[1].ToLower();
                    //Apply stemming if required.
                    if (cee.stem) {
                        src = LightweightStemmer.Stem (src, cee.srcLang);
                        trg = LightweightStemmer.Stem (trg, cee.trgLang);
                    }
                    //Add the entry to the Dictionary<string, double>.
                    if (!res.ContainsKey (src))
                        res.Add (src, new Dictionary<string, bool> ());
                    if (!res [src].ContainsKey (trg))
                        res [src].Add (trg, true);
                }
            }
            return res;
        }
    }
}

