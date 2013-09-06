//
//  SimpleTermEntry.cs
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

namespace MPAligner
{
    public class SimpleTermEntry
    {
        public SimpleTermEntry (string termStr, string msdStr, string lemmaStr, string normStr, string normMsdStr, string concordance, double probability, int freq)
        {
            term = termStr;
            msdSeq = msdStr;
            lemmaSeq = lemmaStr;
			normSeq = normStr;
			normMsdSeq = normMsdStr;
			conc = concordance;
            prob = probability;
            count = freq;
        }

        public SimpleTermEntry ()
        {
            term = null;
            msdSeq = null;
            lemmaSeq = null;
			normSeq = null;
			normMsdSeq = null;
			conc = null;
            prob = 0;
            count=0;
        }

        public string term;
        public string msdSeq;
        public string lemmaSeq;
		public string normSeq;
		public string normMsdSeq;
		public string conc;
        public double prob;
        public int count;
    }
}

