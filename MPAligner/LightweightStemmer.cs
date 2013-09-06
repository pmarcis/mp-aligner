//
//  LightweightStemmer.cs
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
    public class LightweightStemmer
    {
        public LightweightStemmer ()
        {
        }

        /// <summary>
        /// Stem the specified phrase and language.
        /// </summary>
        /// <param name='phrase'>
        /// Phrase.
        /// </param>
        /// <param name='language'>
        /// Language.
        /// </param>
        public static string Stem(string phrase, string language)
        {
            //TODO: Implement stemming.
            return phrase;
        }
    }
}

