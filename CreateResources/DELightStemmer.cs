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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreateResources
{
    public class DELightStemmer : Stemmer
    {
        Regex p6 = new Regex("(stens)$");
        Regex p7 = new Regex("(erem|eren|erer|eres|stem|sten|ster|stes|test)$");
        Regex p8 = new Regex("(den|end|ens|ere|ern|est|ien|nen|nes|sen|ses|ste|ten|tet)$");
        Regex p9 = new Regex("(em|en|er|es|et|im|ns|se|st|ta|te)$");
        Regex p10 = new Regex("(a|e|n|t|s)$");

        public string StemString(string s)
        {
            string lowerStr = s.ToLower();
            if (s.Length < 2) return s;
            if (p6.IsMatch(lowerStr) && s.Length >= 7)
            {
                return s.Substring(0, s.Length - 5);
            }
            else if (p7.IsMatch(lowerStr) && s.Length >= 6)
            {
                return s.Substring(0, s.Length - 4);
            }
            else if (p8.IsMatch(lowerStr) && s.Length >= 5)
            {
                return s.Substring(0, s.Length - 3);
            }
            else if (p9.IsMatch(lowerStr) && s.Length >= 4)
            {
                return s.Substring(0, s.Length - 2);
            }
            else if (p10.IsMatch(lowerStr) && s.Length >= 3)
            {
                return s.Substring(0, s.Length - 1);
            }
            return s;
        }
    }
}
