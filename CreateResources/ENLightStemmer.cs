﻿//
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
    public class ENLightStemmer : Stemmer
    {
        Regex p1 = new Regex("^(is|was|oss|css)$");

        Regex p8 = new Regex("(s\'s)$");
        Regex p9 = new Regex("(\'s|s\')$");
        Regex p10 = new Regex("(s)$");

        public string StemString(string s)
        {
            string lowerStr = s.ToLower();
            if (s.Length < 2) return s;
            if (p1.IsMatch(lowerStr))
            {
                return s;
            }
            if (p8.IsMatch(lowerStr) && s.Length >= 5)
            {
                return s.Substring(0, s.Length - 3);
            }
            if (p9.IsMatch(lowerStr) && s.Length >= 4)
            {
                return s.Substring(0, s.Length - 2);
            }
            if (p10.IsMatch(lowerStr) && s.Length >= 3)
            {
                return s.Substring(0, s.Length - 1);
            }
            return s;
        }
    }
}
