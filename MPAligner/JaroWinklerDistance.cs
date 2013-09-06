//
//  JaroWinklerDistance.cs
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
using System.Text;
using System.Collections.Generic;

namespace MPAligner
{
    /// <summary>
    /// Jaro winkler distance.
    /// The code has been adapted from: http://isolvable.blogspot.com/2011/05/jaro-winkler-fast-fuzzy-linkage.html
    /// </summary>
    public class JaroWinklerDistance
    {
        public JaroWinklerDistance ()
        {

        }
        /// <summary>
        /// This region contains code related to Jaro Winkler string distance algorithm. 
        /// </summary>
        private const double defaultMismatchScore = 0.0;
        private const double defaultMatchScore = 1.0;
        /// <summary>
        /// gets the similarity of the two strings using Jaro distance.
        /// </summary>
        /// <param name="firstWord"></param>
        /// <param name="secondWord"></param>
        /// <returns>a value between 0-1 of the similarity</returns>
        /// 
        public static  double StringDistance(string firstWord, string secondWord) {
            if (firstWord != null && secondWord != null) 
            {
                if (firstWord == secondWord)
                {
                    return defaultMatchScore;
                }
                else
                {
                    //get half the length of the string rounded up - (this is the distance used for acceptable transpositions)
                    int halflen = Math.Min(firstWord.Length, secondWord.Length) / 2 + 1;
                    //get common characters
                    StringBuilder common1 = GetCommonCharacters(firstWord, secondWord, halflen);
                    int commonMatches = common1.Length;
                    //check for zero in common
                    if (commonMatches == 0)
                    {
                        return defaultMismatchScore;
                    }
                    StringBuilder common2 = GetCommonCharacters(secondWord, firstWord, halflen);
                    //check for same length common strings returning 0.0f is not the same
                    if (commonMatches != common2.Length)
                    {
                        return defaultMismatchScore;
                    }
                    //get the number of transpositions
                    int transpositions = 0;
                    for (int i = 0; i < commonMatches; i++)
                    {
                        if (common1[i] != common2[i])
                        {
                            transpositions++;
                        }
                    }
                    int j = 0;
                    j += 1;
                    //calculate jaro metric
                    transpositions /= 2;
                    double tmp1;
                    tmp1 = commonMatches / (3.0 * firstWord.Length) + commonMatches / (3.0 * secondWord.Length) + (commonMatches - transpositions) / (3.0 * commonMatches);
                    return tmp1;
                }
            }
            return defaultMismatchScore;
        }
        /// <summary>
        /// returns a string buffer of characters from string1 within string2 if they are of a given
        /// distance seperation from the position in string1.
        /// </summary>
        /// <param name="firstWord">string one</param>
        /// <param name="secondWord">string two</param>
        /// <param name="distanceSep">separation distance</param>
        /// <returns>a string buffer of characters from string1 within string2 if they are of a given
        /// distance seperation from the position in string1</returns>
        private static StringBuilder GetCommonCharacters(string firstWord, string secondWord, int distanceSep) {
            if ((firstWord != null) && (secondWord != null)) {
                StringBuilder returnCommons = new StringBuilder(20);
                StringBuilder copy = new StringBuilder(secondWord);
                int firstLen = firstWord.Length;
                int secondLen = secondWord.Length;
                for (int i = 0; i < firstLen; i++) {
                    char ch = firstWord[i];
                    bool foundIt = false;
                    for (int j = Math.Max(0, i - distanceSep);
                         !foundIt && j < Math.Min(i + distanceSep, secondLen);
                         j++) {
                        if (copy[j] == ch) {
                            foundIt = true;
                            returnCommons.Append(ch);
                            copy[j] = '#';
                        }
                    }
                }
                return returnCommons;
            }
            return null;
        }
    }
}

