//
//  TermTaggedFileParser.cs
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
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace MPAligner
{
    public class TermTaggedFileParser
    {
        /// <summary>
        /// Parses a term tagged file - reads terms and returns a list of strings (terms).
        /// </summary>
        /// <returns>
        /// The terms in a <c>Dictionary</c>.
        /// </returns>
        /// <param name='file'>
        /// Input file.
        /// </param>
        /// <param name='enc'>
        /// Encoding to use when reading the input file.
		/// </param>
		/// <param name='concLen'>
		/// The length of concordances that should be retained for each term.
		/// </param>
		public static Dictionary<string,SimpleTermEntry> ParseTermTaggedFile(string file, Encoding enc, int concLen)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencyDecimalSeparator = ".";
            nfi.NumberDecimalSeparator = ".";
            nfi.PercentDecimalSeparator = ".";
            Dictionary<string, SimpleTermEntry> res = new Dictionary<string, SimpleTermEntry>();
            StreamReader sr = new StreamReader(file, enc);
            //Here we treat every line as a separate source of information - a separate paragraph.
            //Although there may be cases where sentences are broken between lines, such cases are because of corrupt handling of data (wrong formatting of data) in the first place.
            //Here we do not want to deal with problems caused by misuse of text, therefore we treat every line as a separate paragraph (as it should be done!).
            //Also ... in the event of annotations spanning between lines, we will ignore such annotations!
            //Also ... we do not deal here with overlapping annotations. If there will be such annotations, we will 
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                Dictionary<string,SimpleTermEntry> tempDict = ParseTermsInString(line, nfi, concLen);
                foreach(KeyValuePair<string,SimpleTermEntry> kvp in tempDict)
                {
                    if (!res.ContainsKey(kvp.Key))
                    {
                        res.Add(kvp.Key,kvp.Value);
                    }
                    else
                    {
                        res[kvp.Key].count+=kvp.Value.count;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Parses a string and returns a list of found terms.
        /// Terms should not be overlapping or nested!
        /// </summary>
        /// <returns>
        /// The terms in a <c>Dictionary</c>.
        /// </returns>
        /// <param name='text'>
        /// Input text.
		/// </param>
		/// <param name='concLen'>
		/// The length of the concordance that has to be retrieved for each term.
		/// </param>
		public static Dictionary<string, SimpleTermEntry> ParseTermsInString (string text, NumberFormatInfo nfi, int concLen)
        {
            string tempText = text;
			string plain = GetPlaintextFromTaggedString (text);
            Dictionary<string,SimpleTermEntry> res = new Dictionary<string, SimpleTermEntry>();
            //List<string> partialText = new List<string> ();
            while (true) {
                int idx = tempText.IndexOf ("<TENAME");
                int endIdx = tempText.IndexOf ("</TENAME");
				if (idx >= 0 && endIdx > idx) {
					int clIdx = tempText.IndexOf ('>', idx);
					if (clIdx >= 0 && clIdx < endIdx) {

						string attrStr = tempText.Substring (idx + 7, clIdx - idx - 7);
						string termStr = tempText.Substring (clIdx + 1, endIdx - clIdx - 1);

						SimpleTermEntry ste = ReadAttributesFromString (attrStr, termStr, nfi);
						if (ste != null) {
							if (!res.ContainsKey (ste.term.ToLower ())) {
								string conc = FindConcordance (ste.term, plain, concLen);
								ste.conc = conc;
								res.Add (ste.term.ToLower (), ste);
							} else {
								string conc = FindConcordance (ste.term, plain, concLen);
								if (string.IsNullOrWhiteSpace(ste.conc)||conc.Length > ste.conc.Length)
									ste.conc = conc;
								res [ste.term.ToLower ()].count ++;
							}
						}
					}
					tempText = tempText.Substring (endIdx + 1);
				} else if (endIdx < idx && endIdx>=0) {
					int clIdx = tempText.IndexOf ('>', Math.Max (idx, endIdx));
					tempText = tempText.Substring (endIdx + 1);
				} else {
					break;
				}
            }
            /*
            while (true) {
                int idx = tempText.IndexOf ("<TENAME");
                int endIdx = tempText.IndexOf ("</TENAME");
                if (idx >= 0 && endIdx > idx) {
                    if (idx > 0 && partialText.Count > 0) {
                        string addableContent = tempText.Substring (0, idx);
                        for (int i=0; i<partialText.Count; i++) {
                            partialText [i] += addableContent;
                        }
                    }
                    partialText.Add ("");

                    int clIdx = tempText.IndexOf ('>', idx);
                    tempText = tempText.Substring (clIdx + 1);
                } else if (endIdx >= 0) {
                    if (endIdx > 0 && partialText.Count > 0) {
                        string addableContent = tempText.Substring (0, endIdx);
                        for (int i=0; i<partialText.Count; i++) {
                            partialText [i] += addableContent;
                        }
                    }
                    int clIdx = tempText.IndexOf ('>', endIdx);
                    if (clIdx>=0)
                    {
                        tempText = tempText.Substring (clIdx + 1);
                    }
                    else
                    {
                        tempText = tempText.Substring (endIdx + 8);
                    }
                    if (partialText.Count>0)
                    {
                        if (!res.ContainsKey(partialText[partialText.Count-1]))
                        {
                            res.Add(partialText[partialText.Count-1],1);
                        }
                        else
                        {
                            res[partialText[partialText.Count-1]]++;
                        }
                        partialText.RemoveAt(partialText.Count-1);
                    }
                } else {
                    break;
                }
            }*/
            return res;
        }

        static SimpleTermEntry ReadAttributesFromString (string attrStr, string termStr, NumberFormatInfo nfi)
        {
            SimpleTermEntry ste = new SimpleTermEntry ();
            ste.term = termStr.Trim();
            ste.count = 1;
            string tempStr = attrStr;
            while (true) {
                int sIdx = tempStr.IndexOf("SCORE");
                int lIdx = tempStr.IndexOf("LEMMA");
				int mIdx = tempStr.IndexOf("MSD");
				int nIdx = tempStr.IndexOf("NORM");
				int nmIdx = tempStr.IndexOf("NORMMSD");
				if (sIdx>=0 && (sIdx<lIdx || lIdx<0 ) && (sIdx<mIdx || mIdx<0) && (sIdx<nIdx || nIdx<0) && (sIdx<nmIdx || nmIdx<0))
                {
                    try{
                        ste.prob = Convert.ToDouble(GetAttrStr(tempStr, sIdx+5).Trim(),nfi);
                        tempStr = tempStr.Substring(sIdx+5);
                    }
                    catch
                    {
                        tempStr = tempStr.Substring(sIdx+5);
                    }
                }
				else if (mIdx>=0 && (mIdx<lIdx || lIdx<0 ) && (mIdx<sIdx || sIdx<0) && (mIdx<nIdx || nIdx<0) && (mIdx<nmIdx || nmIdx<0))
                {
                    ste.msdSeq = System.Net.WebUtility.HtmlDecode(GetAttrStr(tempStr, mIdx+3));
                    tempStr = tempStr.Substring(mIdx+3);
                }
				else if (lIdx>=0 && (lIdx<sIdx || sIdx<0 ) && (lIdx<mIdx || mIdx<0) && (lIdx<nIdx || nIdx<0) && (lIdx<nmIdx || nmIdx<0))
                {
                    ste.lemmaSeq = System.Net.WebUtility.HtmlDecode(GetAttrStr(tempStr, lIdx+5));
                    tempStr = tempStr.Substring(lIdx+5);
				}
				else if (nIdx>=0 && (nIdx<sIdx || sIdx<0 ) && (nIdx<mIdx || mIdx<0) && (nIdx<lIdx || lIdx<0) && (nIdx<nmIdx || nmIdx<0))
				{
					ste.normSeq = System.Net.WebUtility.HtmlDecode(GetAttrStr(tempStr, nIdx+4));
					tempStr = tempStr.Substring(nIdx+4);
				}
				else if (nmIdx>=0 && (nmIdx<sIdx || sIdx<0 ) && (nmIdx<mIdx || mIdx<0) && (nmIdx<=nIdx || nIdx<0) && (nmIdx<lIdx || lIdx<0))
				{
					ste.normMsdSeq = System.Net.WebUtility.HtmlDecode(GetAttrStr(tempStr, nmIdx+6));
					tempStr = tempStr.Substring(nmIdx+6);
				}
				else
                {
                    break;
                }
            }

            if (ste.lemmaSeq.Contains("_")&&!ste.term.Contains("_"))
            {
                ste.lemmaSeq = ste.lemmaSeq.Replace("_"," ");
            }

            return ste;
        }

        static string GetAttrStr (string tempStr, int idx)
        {
            int q1Idx = tempStr.IndexOf ('\"', idx);
            if (q1Idx >= 0) {
                int q2Idx = tempStr.IndexOf('\"', q1Idx+1);
                if (q2Idx>0)
                {
                    return tempStr.Substring(q1Idx+1,q2Idx-q1Idx-1);
                }
                else
                {
                    return "";
                }
            } else {
                return "";
            }
        }

		static string FindConcordance(string term, string text, int wordCount)
        {
            //string text = GetPlaintextFromTaggedFile(file);
            int idx = GetValidConcordanceIdx(text, term);
            if (idx>=0)
            {
                int leftIdx = GetConcordanceIdx(text, wordCount,idx-1,-1);
                int rightIdx = GetConcordanceIdx(text, wordCount,idx+term.Length,1);
                return text.Substring(leftIdx,rightIdx-leftIdx+1).Trim();
            }
            else
            {
                return "";
            }
        }

        static int GetConcordanceIdx (string text, int wordCount, int idx, int direction)
        {
            int currIdx = idx;
            bool withinOther = false;
            int wordsCounted = 0;
            while (currIdx>=0 && currIdx<text.Length)
            {
                if (text[currIdx]=='\n'||text[currIdx]=='\r')
                {
                    break;
                }
                if (Char.IsWhiteSpace(text[currIdx]))
                {
                    if (withinOther)
                    {
                        wordsCounted++;
                    }
                    withinOther = false;
                }
                else
                {
                    withinOther = true;
                }
                if (wordsCounted>=wordCount)
                {
                    break;
                }
                currIdx+=direction;
            }
            return currIdx-direction;
        }

        static int GetValidConcordanceIdx (string text, string term, int idxIn = 0)
        {
            int idx = text.IndexOf(term,idxIn,StringComparison.CurrentCultureIgnoreCase);
            if (idx>=0)
            {
                if (idx>0 && !IsValidSeparator(text, idx-1, -1))
                {
                    return GetValidConcordanceIdx (text,term,idx+1);
                }
                if (idx+term.Length<text.Length && !IsValidSeparator(text, idx+term.Length, 1))
                {
                    return GetValidConcordanceIdx (text,term,idx+1);
                }
                return idx;
            }
            else
            {
                return -1;
            }
        }

        static bool IsValidSeparator(string text, int idx, int direction)
        {
            int changingIdx = idx;
            while(changingIdx>=0 && changingIdx<text.Length)
            {
                if (Char.IsWhiteSpace(text[changingIdx])) return true;
                if (Char.IsLetterOrDigit(text[changingIdx])) return false;
                //if (Char.IsPunctuation(text[changingIdx])) //The punctuations are ignored as some may be used to stick words together.
                changingIdx+=direction;
            }
            return true;
        }

		static string GetPlaintextFromTaggedString(string text)
        {
			if (string.IsNullOrWhiteSpace (text))
			{
				return "";
			}
            //Read the text and process all MUC-7 tags. All other tags will remain untouched!
            StringBuilder sb = new StringBuilder(5000);
            for (int i=0;i<text.Length;i++)
            {
                if (text[i] == '<')
                {
                    //Try to find the first of all three MUC-7 NE categories.
                    int findEnemex = text.IndexOf("<TENAME", i);
                    if (findEnemex == i)
                    {
                        //If the ENAMEX tag starts in the current position...
                        string textToAdd = RemoveFirstEntity("TENAME", text, ref i);
                        sb.Append(textToAdd);
                    }
                    else
                    {
                        sb.Append(text[i].ToString());
                    }
                }
                else
                {
                    sb.Append(text[i].ToString());
                }
            }
            return sb.ToString();
        }

        static string RemoveFirstEntity(string p, string text, ref int i)
        {
            try
            {
                //Find the first closing character (the end of the open tag).
                int closingIdx = text.IndexOf('>', i);
                //Find the closing tag.
                int closingTagIdx = text.IndexOf("</" + p + ">", i);
                //Get the entity attribute values - original markup.
                string entity = text.Substring(i, closingIdx - i + 1).Remove(0,p.Length + 1);
                entity = entity.Substring(0, entity.Length - 1);
                //Extract the NE text.
                string textToAdd = text.Substring(closingIdx + 1, closingTagIdx - closingIdx - 1);
                //Set the index to the position of the '>' character of the NE closing tag.
                i = closingTagIdx + p.Length + 2;
                return textToAdd;
            }
            catch
            {
                Console.WriteLine("Mark-up error at index " +i.ToString());
            }
            return "";
        }
    }
}
