//
//  StopwordListParser.cs
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
    public class StopwordListParser
    {
        public StopwordListParser ()
        {
        }

        public static Dictionary<string, bool> ParseStopwordList (MPAlignerConfigurationStopWordListEntry cswle)
        {
            string fileName = cswle.path;
            Encoding enc = cswle.encoding;
            Dictionary<string, bool> res = new Dictionary<string, bool> ();
            StreamReader sr = new StreamReader (fileName, enc);
            //Read the dictionary file to the end.
            while (!sr.EndOfStream) {
                string line = sr.ReadLine ().Trim ();
                if (cswle.stem) {
                    line = LightweightStemmer.Stem (line, cswle.lang);
                }
                if (!res.ContainsKey (line))
                    res.Add (line, true);
            }
            return res;
        }
    }
}

