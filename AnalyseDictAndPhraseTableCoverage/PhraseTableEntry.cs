//
//  PhraseTableEntry.cs
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
using System.Globalization;
using System.Text;

namespace AddTermFeatureToPhraseTable
{
    public class PhraseTableEntry : IComparable<PhraseTableEntry>
    {
        public string src;
        public string trg;
        public List<double> prob;
        public string fourthStr;
        public string fifthStr;
        public static int _compKey;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compKey">1 for "src", 2 for "trg".</param>
        public PhraseTableEntry(int compKey = 1)
        {
            _compKey = compKey;
            fourthStr = "";
            fifthStr = "";
        }
        
        public int CompareTo(PhraseTableEntry other)
        {
            if (_compKey == 1)
            {
                int srcCmp = src.CompareTo(other.src);
                if (srcCmp == 0)
                {
                    return trg.CompareTo(other.trg);
                }
                else
                {
                    return srcCmp;
                } 
            }
            else if (_compKey == 2)
            {
                int trgCmp = trg.CompareTo(other.trg);
                if (trgCmp == 0)
                {
                    return src.CompareTo(other.src);
                }
                else
                {
                    return trgCmp;
                } 
            }
            else
            {
                throw new ArgumentException("PhraseTableEntry comparison ID can be only 1 for \"src\" or 2 for \"trg\"");
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(src))
            {
                sb.Append(src);
            }
            sb.Append(" ||| ");
            if (!string.IsNullOrWhiteSpace(trg))
            {
                sb.Append(trg);
                sb.Append(" |||");
            }
            else
            {
                sb.Append("|||");
            }
            foreach (double p in prob)
            {
                sb.Append(" " + p.ToString("G"));//0.0####e+00"));
            }
            sb.Append(" ||| ");
            if (!string.IsNullOrWhiteSpace(fourthStr))
            {
                sb.Append(fourthStr);
                sb.Append(" ||| ");
            }
            else
            {
                sb.Append("||| ");
            }
            if (!string.IsNullOrWhiteSpace(fifthStr))
            {
                sb.Append(fifthStr);
            }
            return sb.ToString();
        }
        
        static string[] sep = { "|||" };
        static string[] sep2 = { " " };
        
        public static PhraseTableEntry ParsePhraseTableLine(string line)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencyDecimalSeparator = ".";
            nfi.NumberDecimalSeparator = ".";
            nfi.PercentDecimalSeparator = ".";
            PhraseTableEntry pte = new PhraseTableEntry(1);
            if (!string.IsNullOrWhiteSpace(line))
            {
                string[] arr = line.Split(sep, StringSplitOptions.None);
                if (arr.Length == 5)
                {
                    pte.src = arr[0].Trim();
                    pte.trg = arr[1].Trim();
                    string[] arr2 = arr[2].Split(sep2, StringSplitOptions.RemoveEmptyEntries);
                    pte.prob = new List<double>();
                    foreach (string str in arr2)
                    {
                        pte.prob.Add(Convert.ToDouble(str, nfi));
                    }
                    pte.fourthStr = arr[3].Trim();
                    pte.fifthStr = arr[4].Trim();
                }
            }
            return pte;
        }
    }
}
