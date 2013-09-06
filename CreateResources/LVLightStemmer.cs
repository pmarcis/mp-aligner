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

namespace CreateResources
{
    public class LVLightStemmer:Stemmer
    {
        Regex p1 = new Regex("^(ārā|cik|kad|maz|pus|rīt|sen|šad|šur|tur|žēl|" +
            "kur|jau|tad|vēl|tik|pie|pēc|gar|par|pār|bez|aiz|zem|dēļ|lai|vai|arī|gan|" +
    "bet|jeb|būt|esi|būs|kas|kam|kur)$");

        Regex p8 = new Regex("(iem|ais|ies|iet)$");
        Regex p9 = new Regex("(am|as|ai|ām|ās|os|ie|es|em|ēm|ēs|ij|īm|is|īs|us|um|im|āt|at|it)$");
        Regex p10 = new Regex("(a|ā|e|ē|i|ī|m|o|s|š|t|u|ū)$");

        public string StemString(string s)
        {
            if (s.Length < 2) return s;
            if (p1.IsMatch(s))
            {
                return s;
            }
            if (p8.IsMatch(s) && s.Length >= 5)
            {
                return s.Substring(0, s.Length - 3);
            }
            if (p9.IsMatch(s) && s.Length >= 4)
            {
                return s.Substring(0, s.Length - 2);
            }
            if (p10.IsMatch(s) && s.Length >= 3)
            {
                return s.Substring(0, s.Length - 1);
            }
            return s;
        }
    }
}
