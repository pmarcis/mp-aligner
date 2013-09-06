//
//  AlignmentElement.cs
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

namespace AnalyseDictAndPhraseTableCoverage
{
    public class AlignmentElement
    {
        public AlignmentElement ()
        {
        }

        public string srcTerm;
        public string trgTerm;
        public string srcMsd;
        public string trgMsd;
        public string srcLemma;
        public string trgLemma;
        public double alignmentScore;
        public string srcFile;
        public string trgFile;
    }
}

