//
//  StringComparisonElement.cs
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
    public class StringComparisonElement :IComparable<StringComparisonElement>
    {
        public StringComparisonElement ()
        {
            src=null;
            trg=null;
            similarity = 0;
        }
        
        public string src;
        public string trg;
        public double similarity;

        #region IComparable implementation
        int IComparable<StringComparisonElement>.CompareTo (StringComparisonElement other)
        {
            if (src==other.src)
            {
                return other.similarity.CompareTo(this.similarity); // From large to small.
            }
            return src.CompareTo(other.src); // From A to Z
        }
        #endregion
    }
}

