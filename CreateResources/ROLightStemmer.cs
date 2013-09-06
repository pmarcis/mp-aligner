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
    public class ROLightStemmer : Stemmer
    {
        Regex p8 = new Regex("(aţi|eam|eţi|iţi|ele|eai|eau|ând|ile|ind|ule|uşi|ate|ură|uţi|lor|ite|ată|ute|use|ută|aşi|ită|ise|iră|işi|ase|ară|uri|âţi|sei|eze|eat|ezi|ese|uia|uţe|esc|ata|uţă|uşe|ste|ora|oi||iul|ita|ica|iam|iţe|iţa|eli|aua)$");
        Regex p9 = new Regex("(ea|ii|at|em|au|ai|it|ut|ul|am|le|im|ei|ăm|ui|or|se|ie|oi|ia|ez|ât|st|şi|să|ua|uş|eo)$");
        Regex p10 = new Regex("(i|e|a|ă|u|o|s|î|t|m|l)$");

        public string StemString(string s)
        {
            if (s.Length <= 3) return s;
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
