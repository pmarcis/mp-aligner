//
//  LevenshteinDistance.cs
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

namespace MPAligner
{
    /// <summary>
    /// Algorithm downloaded from: http://webreflection.blogspot.com/2009/02/levenshtein-algorithm-revisited-25.html
	/// 
	/// The code within the LevenshteinDistance class belongs to the programmer of the WebReflection article.
	/// 
    /// License - MIT.
    /// Contains approximate string matching
    /// </summary>
    static class LevenshteinDistance
    {
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string a, string b)
        {
            // Levenshtein Algorithm Revisited - WebReflection
            if (a == b)
                return 0;
            if (a.Length == 0 || b.Length == 0)
                return a.Length == 0 ? b.Length : a.Length;
            int len1 = a.Length + 1,
                len2 = b.Length + 1,
                I = 0,
                i = 0,
                c, j, J;
            int[,] d = new int[len1, len2];
            while (i < len2)
                d[0, i] = i++;
            i = 0;
            while (++i < len1)
            {
                J = j = 0;
                c = a[I];
                d[i, 0] = i;
                while (++j < len2)
                {
                    d[i, j] = Math.Min(Math.Min(d[I, j] + 1, d[i, J] + 1), d[I, J] + (c == b[J] ? 0 : 1));
                    ++J;
                };
                ++I;
            };
            return d[len1 - 1, len2 - 1];
        }
    }
}
