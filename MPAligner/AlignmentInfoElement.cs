//
//  AlignmentInfoElement.cs
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
using System.Xml.Serialization;
using MPFramework;

namespace MPAligner
{
    [Serializable]
    public class AlignmentInfoElement :IComparable<AlignmentInfoElement>
    {
        public AlignmentInfoElement ()
        {
            srcToTrgAlignments = new List<WordAlignmentElement>();
        }

        public ProcessedTermEntry srcEntry;
        public ProcessedTermEntry trgEntry;
        
        [XmlIgnore]
        public List<WordAlignmentElement> srcToTrgAlignments;
        [XmlIgnore]
        public List<WordAlignmentElement> trgToSrcAlignments;

        public List<WordAlignmentElement> consolidatedAlignment;
        //public List<StringComparisonElement> finalAlignment;
        
        [XmlAttribute("minSrcId")]
        public int minSrcId;
        [XmlAttribute("minTrgId")]
        public int minTrgId;
        [XmlAttribute("maxSrcId")]
        public int maxSrcId;
        [XmlAttribute("maxTrgId")]
        public int maxTrgId;
        
        public string srcStrForAlignment;
        public string trgStrForAlignment;

        public string alignedLowSrcStr;
        public string alignedLowTrgStr;
        
        public string srcFile;
        public string trgFile;
        
        [XmlAttribute("alingScore")]
        public double alignmentScore;
        
        
        [XmlAttribute("srcMult")]
        public double srcMultiplier;        
        [XmlAttribute("trgMult")]
        public double trgMultiplier;
        
        public static void PrintList(string outputFormat, string outputFile, List<AlignmentInfoElement> list, bool printTopSrc, MPAlignerConfigurationLangPairEntry lpeConf = null, string srcLang="", string trgLang="", string collectionId="", string domain="")
        {
            MPAlignerConfigurationLangPairEntry lpeConfig = lpeConf!=null?lpeConf:new MPAlignerConfigurationLangPairEntry();
            if (list == null) return;
            Encoding utf8WithoutBom = new UTF8Encoding(false);
            //string outFile = outputFile+".xml";
            if (outputFormat.ToLower() == "xml")
            {
                list.Sort();
                List<AlignmentInfoElement> newList = new List<AlignmentInfoElement>();
                string previousSrc = null;
                foreach(AlignmentInfoElement aie in list)
                {
                    string currSrc = GetStrFromEntry(aie.srcEntry.surfaceFormWords, aie.minSrcId, aie.maxSrcId,aie.srcEntry.lemmaSeq).Trim();
                    string currTrg = GetStrFromEntry(aie.trgEntry.surfaceFormWords, aie.minTrgId, aie.maxTrgId,aie.trgEntry.lemmaSeq).Trim();
                    if (string.IsNullOrWhiteSpace(currTrg)||string.IsNullOrWhiteSpace(currSrc)) continue;
                    if ((!printTopSrc || previousSrc!=currSrc.ToLower()) && aie.alignmentScore>=lpeConfig.printThr)
                    {
                        newList.Add(aie);
                        previousSrc = currSrc.ToLower();
                    }
                }
                string outStr = MPFramework.MPFrameworkFunctions.SerializeObjectInstance<List<AlignmentInfoElement>>(newList);
                File.WriteAllText(outputFile,outStr,utf8WithoutBom);
            }
            else
			{
				StreamWriter sw = new StreamWriter(outputFile,false, utf8WithoutBom);
				PrintTabsep (sw, outputFormat, list, printTopSrc, lpeConfig, srcLang, trgLang, collectionId, domain);
				sw.Close();
            }
        }

		private static void PrintTabsep(StreamWriter sw, string outputFormat, List<AlignmentInfoElement> list, bool printTopSrc, MPAlignerConfigurationLangPairEntry lpeConf, string srcLang="", string trgLang="", string collectionId="", string domain="")
		{
			NumberFormatInfo nfi = new NumberFormatInfo();
			nfi.CurrencyDecimalSeparator = ".";
			nfi.NumberDecimalSeparator = ".";
			nfi.PercentDecimalSeparator = ".";
			bool fullFormat = outputFormat.ToLower()=="tabsep"?false:true;
			string prevSrc = null;
			list.Sort();
			foreach(AlignmentInfoElement aie in list)
			{
				string currSrc = GetStrFromEntry(aie.srcEntry.surfaceFormWords, aie.minSrcId, aie.maxSrcId,aie.srcEntry.lemmaSeq).Trim();
				string srcLemmas = GetStrFromEntry(aie.srcEntry.lemmaSeq, aie.minSrcId, aie.maxSrcId).Trim();
				string srcMsd = GetStrFromEntry(aie.srcEntry.msdSeq, aie.minSrcId, aie.maxSrcId).Trim();
				string srcNorm = GetStrFromEntry(aie.srcEntry.normSeq, aie.minSrcId, aie.maxSrcId).Trim();
				string srcNormMsd = GetStrFromEntry(aie.srcEntry.normMsdSeq, aie.minSrcId, aie.maxSrcId).Trim();
				if (string.IsNullOrWhiteSpace(currSrc)) continue;
				if ((!printTopSrc || prevSrc!=currSrc.ToLower()) && aie.alignmentScore>=lpeConf.printThr)
				{
					string currTrg = GetStrFromEntry(aie.trgEntry.surfaceFormWords, aie.minTrgId, aie.maxTrgId,aie.trgEntry.lemmaSeq).Trim();
					string trgLemmas = GetStrFromEntry(aie.trgEntry.lemmaSeq, aie.minTrgId, aie.maxTrgId).Trim();
					string trgMsd = GetStrFromEntry(aie.trgEntry.msdSeq, aie.minTrgId, aie.maxTrgId).Trim();
					string trgNorm = GetStrFromEntry(aie.trgEntry.normSeq, aie.minTrgId, aie.maxTrgId).Trim();
					string trgNormMsd = GetStrFromEntry(aie.trgEntry.normMsdSeq, aie.minTrgId, aie.maxTrgId).Trim();
					if (string.IsNullOrWhiteSpace(currTrg)) continue;
					if (fullFormat)
					{
						sw.Write (srcLang);
						sw.Write("\t");
					}
					sw.Write(currSrc);
					sw.Write("\t");
					if (fullFormat)
					{
						sw.Write (trgLang);
						sw.Write("\t");
					}
					sw.Write(currTrg);
					sw.Write("\t");
					if (fullFormat)
					{
						sw.Write (domain);
						sw.Write("\t");
						sw.Write (collectionId);
						sw.Write("\t");
					}
					sw.Write(aie.alignmentScore.ToString("0.000000",nfi));
					if (fullFormat)
					{
						sw.Write("\t");
						sw.Write(srcMsd);
						sw.Write("\t");
						sw.Write(srcLemmas);
						sw.Write("\t");
						sw.Write(srcNorm);
						sw.Write("\t");
						sw.Write(srcNormMsd);
						sw.Write("\t");
						sw.Write(aie.srcEntry.concordance);
						sw.Write("\t");
						sw.Write(trgMsd);
						sw.Write("\t");
						sw.Write(trgLemmas);
						sw.Write("\t");
						sw.Write(trgNorm);
						sw.Write("\t");
						sw.Write(trgNormMsd);
						sw.Write("\t");
						sw.Write(aie.trgEntry.concordance);
						sw.Write("\t");
						sw.Write(aie.srcFile);
						sw.Write("\t");
						sw.Write(aie.trgFile);
						sw.Write("\t");
					}
					sw.WriteLine();
					prevSrc = currSrc.ToLower();
				}
			}
		}

		public static void AppendList(string outputFormat, string outputFile, List<AlignmentInfoElement> list, MPAlignerConfigurationLangPairEntry lpeConf = null, string srcLang="", string trgLang="", string collectionId="", string domain="")
        {
            MPAlignerConfigurationLangPairEntry lpeConfig = lpeConf!=null?lpeConf:new MPAlignerConfigurationLangPairEntry();
            if (list == null) return;
            Encoding utf8WithoutBom = new UTF8Encoding(false);
            //string outFile = outputFile+".xml";
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencyDecimalSeparator = ".";
            nfi.NumberDecimalSeparator = ".";
            nfi.PercentDecimalSeparator = ".";
            StreamWriter sw = new StreamWriter(outputFile,true, utf8WithoutBom);
			PrintTabsep (sw, outputFormat, list, false, lpeConfig, srcLang, trgLang, collectionId, domain);
            sw.Close();
        }

        public static string GetStrFromEntry (List<string> strList, int minId, int maxId, List<string> lemmaList = null)
        {
            int min = 0;
            int max = 0;
            if (strList==null)
            {
                return "";
            }
            if (minId<0||maxId<0||maxId>=strList.Count)
            {
                min=0;
                max = strList.Count-1;
            }
            else
            {
                min = minId;
                max = maxId;
            }
            StringBuilder sb = new StringBuilder(100);
            for (int i=min;i<=max;i++)
            {
                if (i>=strList.Count) break;
                if (sb.Length>0)
                {
                    sb.Append(" ");
                }
                if (lemmaList==null||i+1>lemmaList.Count)
                {
                    sb.Append(strList[i]);
                }
                else
                {
                    if (MPFrameworkFunctions.IsAllLower(lemmaList[i]))
                    {
                        sb.Append(strList[i].ToLower());
                    }
                    else if (MPFrameworkFunctions.IsAllUpper(lemmaList[i]))
                    {
                        sb.Append(strList[i].ToUpper());
                    }
                    else if (MPFrameworkFunctions.IsFirstUpper(lemmaList[i]))
                    {
                        string firstCapital = Char.ToUpper(strList[i][0]).ToString()+strList[i].Substring(1).ToLower();
                        sb.Append(firstCapital);
                    }
                    else
                    {
                        sb.Append(strList[i]);
                    }
                }
            }
            return sb.ToString();
        }

        #region IComparable[AlignmentInfoElement] implementation
        public int CompareTo (AlignmentInfoElement other)
        {
            string thisStr = AlignmentInfoElement.GetStrFromEntry(this.srcEntry.surfaceFormWords, this.minSrcId, this.maxSrcId);
            string otherStr = AlignmentInfoElement.GetStrFromEntry(other.srcEntry.surfaceFormWords, other.minSrcId, other.maxSrcId);
            if (thisStr==otherStr)
            {
                return other.alignmentScore.CompareTo(this.alignmentScore); // From large to small.
            }
            return thisStr.CompareTo(otherStr); // From A to Z.
        }
        #endregion
    }
}

