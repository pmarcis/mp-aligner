//
//  ProbabilisticDictionaryParser.cs
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
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MPAligner
{
    /// <summary>
    /// Probabilistic dictionary parser.
    /// Provides methods for dictionary parsing.
    /// </summary>
    public class ProbabilisticDictionaryParser
    {
        /// <summary>
        /// Parses a probabilistic dictionary.
        /// </summary>
        /// <returns>
        /// The dictionary in the following <c>Dictionary</c> format: "Source->Target->Probability".
        /// </returns>
        /// <param name='cde'>
        /// MPAligner Configuration Dictionary Entry describing how to parse the dictionary.
        /// </param>
        public static Dictionary<string, Dictionary<string, double>> ParseDictionary (MPAlignerConfigurationDictEntry cde)
        {
            string fileName = cde.path;
            Encoding enc = cde.encoding;
            char[] sep = cde.separators.ToCharArray ();
            bool filterDictionary = cde.filterDictionary;
            NumberFormatInfo nfi = cde.numberFormatInfo;

            Dictionary<string, Dictionary<string, double>> res = new Dictionary<string, Dictionary<string, double>> ();
            StreamReader sr = new StreamReader (fileName, enc);
            //Read the dictionary file to the end.
            while (!sr.EndOfStream) {
                string line = sr.ReadLine ().Trim ();
                string[] data = line.Split (sep, StringSplitOptions.RemoveEmptyEntries);
                //Apply valid entry filtering.
                if (data.Length == 3 && (!filterDictionary || (IsValidPhrase (data [0]) && IsValidPhrase (data [1])))) { //We ignore lines that do not have three constituents - source word/phrase, target word/phrase, probability.
                    try {
                        double prob = Convert.ToDouble (data [2], nfi);
                        //Apply threshold filtering.
                        if (prob >= cde.variantThreshold) {
                            prob = cde.dictBf.Get(prob);
                            string src = data[0];
                            src = src.ToLower();
                            string trg = data[1];
                            trg = trg.ToLower();
                            //Apply stemming if required.
                            if (cde.stem)
                            {
                                src = LightweightStemmer.Stem(src,cde.srcLang);
                                trg = LightweightStemmer.Stem(trg,cde.trgLang);
                            }
                            //Add the entry to the Dictionary<string, double>.
                            if (!res.ContainsKey (src))
                                res.Add (src, new Dictionary<string, double> ());
                            if (!res [src].ContainsKey (trg))
                                res [src].Add (trg, prob);
                        }
                    } catch{
                    }
                }
            }
            //Apply maximum translation hypothesis filtering.
            //FilterTopEquivalents (cde, res);
            sr.Close();
            return res;
        }

        /// <summary>
        /// Filters top translation equivalents.
        /// </summary>
        /// <param name='cde'>
        /// Configuration dictionary entry.
        /// </param>
        /// <param name='res'>
        /// The in/out dictionary to filter.
        /// </param>
        public static void FilterTopEquivalents (MPAlignerConfigurationDictEntry cde, Dictionary<string, Dictionary<string, double>> res)
        {
            if (cde.maxVariants > 0 && res!=null) {
                foreach (string src in res.Keys)
                {
                    //Filter top pairs only if more than allowed pairs present.
                    if (res[src].Count>cde.maxVariants)
                    {
                        List<KeyValuePair<string, double>> list = new List<KeyValuePair<string, double>>();
                        //As Mono does not support Dictionary to List functionality, that is done manually (thereby sorting the pairs.
                        foreach(string trg in res[src].Keys)
                        {
                            double prob = res[src][trg];
                            int id = -1;
                            for(int i=0;i<list.Count;i++)
                            {
                                if (list[i].Value<prob)
                                {
                                    id = i;
                                    break;
                                }
                            }
                            if (id>-1)
                            {
                                list.Insert(id,new KeyValuePair<string, double>(trg,prob));
                            }
                            else
                            {
                                list.Add(new KeyValuePair<string, double>(trg,prob));
                            }
                        }
                        //Now we put back the top entries to the result dictionary.
                        res[src].Clear();
                        foreach(KeyValuePair<string,double>tElem in list)
                        {
                            res[src].Add(tElem.Key,tElem.Value);
                            if (res[src].Count>=cde.maxVariants)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a phrase is a valid textual phrase (a potential term phrase).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the phrase is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='phrase'>
        /// Phrase
        /// </param>
        private static bool IsValidPhrase(string phrase)
        {
            foreach (char c in phrase)
            {
                //TODO: Rethink validation of phrases ...
                //The filtering is rude - all symbols ($%#^) are treated as invalid characters.
                if (Char.IsControl(c) || (Char.IsPunctuation(c) && c != '-') || (Char.IsSeparator(c) && c != ' ') || (Char.IsSymbol(c) && c != '&'))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
