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
using System.Threading.Tasks;

namespace CreateResources
{
    public class RudeStemmer : Stemmer
    {

        public string StemString(string s)
        {
            if (s.Length < 4) return s;
            if (s.Length == 4) return s.Substring(0, 3);
            if (s.Length == 5) return s.Substring(0, 3);
            if (s.Length == 6) return s.Substring(0, 4);
            return s.Substring(0, s.Length - 3);
        }
    }
}
