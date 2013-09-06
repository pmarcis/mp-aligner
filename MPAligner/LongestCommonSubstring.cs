//
//  LongestCommonSubstring.cs
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
    public class LongestCommonSubstring
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MPAligner.LongestCommonSubstring"/> class.
        /// </summary>
        public LongestCommonSubstring ()
        {
        }

        /// <summary>
        /// Get the longest common substring in two strings.
        /// The algorithm has been partially taken from: http://en.wikibooks.org/wiki/Algorithm_implementation/Strings/Longest_common_substring .
        /// </summary>
        /// <param name='str1'>
        /// String one.
        /// </param>
        /// <param name='str2'>
        /// String two.
        /// </param>
        /// <param name='str1start'>
        /// Start index in string one (output parameter).
        /// </param>
        /// <param name='str1end'>
        /// End index in string one (output parameter).
        /// </param>
        /// <param name='str2start'>
        /// Start index in string two (output parameter).
        /// </param>
        /// <param name='str2end'>
        /// End index in string two (output parameter).
        /// </param>
        public static string Get(string str1, string str2, out int str1start, out int str1end, out int str2start, out int str2end)
        {
            str1start = -1;
            str2start = -1;
            str1end = -1;
            str2end = -1;
            if (String.IsNullOrEmpty(str1) || String.IsNullOrEmpty(str2))
            {
                return "";
            }
            int[,] num = new int[str1.Length, str2.Length];
            int maxlen = 0;
            int lastSubsBegin = 0;
            StringBuilder sequenceBuilder = new StringBuilder();

            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] != str2[j])
                        num[i, j] = 0;
                    else
                    {
                        if ((i == 0) || (j == 0))
                        {
                            num[i, j] = 1;
                        }
                        else
                        {
                            num[i, j] = 1 + num[i - 1, j - 1];
                        }
                        if (num[i, j] > maxlen)
                        {
                            str1start = i-maxlen;
                            str2start = j-maxlen;
                            str1end = i;
                            str2end = j;
                            maxlen = num[i, j];
                            int thisSubsBegin = i - num[i, j] + 1;
                            if (lastSubsBegin == thisSubsBegin)
                            {//if the current LCS is the same as the last time this block ran
                                sequenceBuilder.Append(str1[i]);
                            }
                            else //this block resets the string builder if a different LCS is found
                            {
                                lastSubsBegin = thisSubsBegin;
                                sequenceBuilder.Length = 0; //clear it
                                sequenceBuilder.Append(str1.Substring(lastSubsBegin, (i + 1) - lastSubsBegin));
                                
                            }
                        }
                    }
                }
            }
            //return maxlen;
            return sequenceBuilder.ToString();
        }
    }
}

