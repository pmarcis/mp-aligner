//
//  MPAlignerConfigurationLangPairEntry.cs
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
using System.Xml.Serialization;

namespace MPAligner
{
    /// <summary>
    /// MP Aligner configuration lang pair entry.
    /// I apologize (:P) to everyone who thiks that there are too many configuration parameters here.
    /// But ... I have set the defaults for LV-EN and they work pretty well for language pairs between languages, for which writing is based on the Latin alphabet.
    /// </summary>
    [Serializable]
    public class MPAlignerConfigurationLangPairEntry
    {
        public MPAlignerConfigurationLangPairEntry ()
        {
            dictEntryOverlapThr = 0.75; // ID=0
            dictToWordOverlapThr = 0.75; // ID=1
            dictToSTranslitOverlapThr = 0.75; // ID=2
            dictToTranslitOverlapThr = 0.75; // ID=3
            translitEntryOverlapThr = 0.75; // ID=4
            translitToWordOverlapThr = 0.75; // ID=5
            translitToSTranslitOverlapThr = 0.75; // ID=6
            sTranslitOverlapThr = 0.75; // ID=7
            sTranslitToWordOverlapThr = 0.75; // ID=8
            
            
            lDictEntryOverlapThr = 0.75; // ID=0
            lDictToWordOverlapThr = 0.75; // ID=1
            lDictToSTranslitOverlapThr = 0.75; // ID=2
            lDictToTranslitOverlapThr = 0.75; // ID=3
            lTranslitEntryOverlapThr = 0.75; // ID=4
            lTranslitToWordOverlapThr = 0.75; // ID=5
            lTranslitToSTranslitOverlapThr = 0.75; // ID=6
            lSTranslitOverlapThr = 0.75; // ID=7
            lSTranslitToWordOverlapThr = 0.75; // ID=8
            
            srcLang = null;
            trgLang = null;
            maxOverlapCharsInCompounds = 1;

            minSrcOrTrgOverlap = 0.7;
            
            minShortFragmentOverlap = 0.7;
            minShortFragmentLen = 4;
            maxShortFragmentTargetLen = 6;
            finalAlignmentThr = 0.6;
            printThr = 0.6;
            enforce1stChar = true;
            enforce2ndChar = true;
        }
        
        [XmlAttribute("enforce1stChar")]
        public bool enforce1stChar = true;
        [XmlAttribute("enforce2ndChar")]
        public bool enforce2ndChar = true;

        [XmlAttribute("printThr")]
        public double printThr = 0.1;
        
        [XmlAttribute("sLang")]
        public string srcLang;
        [XmlAttribute("tLang")]
        public string trgLang;
        [XmlAttribute("dThr")]
        public double dictEntryOverlapThr = 0.75; // ID=0
        [XmlAttribute("dToWThr")]
        public double dictToWordOverlapThr = 0.75; // ID=1
        [XmlAttribute("dToSThr")]
        public double dictToSTranslitOverlapThr = 0.75; // ID=2
        [XmlAttribute("dToTThr")]
        public double dictToTranslitOverlapThr = 0.75; // ID=3
        [XmlAttribute("tThr")]
        public double translitEntryOverlapThr = 0.75; // ID=4
        [XmlAttribute("tToWThr")]
        public double translitToWordOverlapThr = 0.75; // ID=5
        [XmlAttribute("tToSThr")]
        public double translitToSTranslitOverlapThr = 0.75; // ID=6
        [XmlAttribute("sThr")]
        public double sTranslitOverlapThr = 0.75; // ID=7
        [XmlAttribute("sToWThr")]
        public double sTranslitToWordOverlapThr = 0.75; // ID=8
        
        [XmlAttribute("lDThr")]
        public double lDictEntryOverlapThr = 0.75; // ID=0
        [XmlAttribute("lDToWThr")]
        public double lDictToWordOverlapThr = 0.75; // ID=1
        [XmlAttribute("lDToSThr")]
        public double lDictToSTranslitOverlapThr = 0.75; // ID=2
        [XmlAttribute("lDToTThr")]
        public double lDictToTranslitOverlapThr = 0.75; // ID=3
        [XmlAttribute("lTThr")]
        public double lTranslitEntryOverlapThr = 0.75; // ID=4
        [XmlAttribute("lTToWThr")]
        public double lTranslitToWordOverlapThr = 0.75; // ID=5
        [XmlAttribute("lTToSThr")]
        public double lTranslitToSTranslitOverlapThr = 0.75; // ID=6
        [XmlAttribute("lSThr")]
        public double lSTranslitOverlapThr = 0.75; // ID=7
        [XmlAttribute("lSToWThr")]
        public double lSTranslitToWordOverlapThr = 0.75; // ID=8
        
        [XmlAttribute("alignThr")]
        public double finalAlignmentThr;

        /// <summary>
        /// The minimum source or target overlap - for every word alignment element.
        /// </summary>
        [XmlAttribute("minSrcToTrgOverlap")]
        public double minSrcOrTrgOverlap = 0.7;
        
        [XmlAttribute("maxCompCharOverlap")]
        public int maxOverlapCharsInCompounds;
        [XmlAttribute("minCompPartLen")]
        public int minAllowedCompundPartLen;
        
        [XmlAttribute("minShortFragmOverlap")]
        public double minShortFragmentOverlap;
        [XmlAttribute("shortFragmLen")]
        public double minShortFragmentLen;
        [XmlAttribute("maxShortFragmTargLen")]
        public double maxShortFragmentTargetLen;
    }
}

