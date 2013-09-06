//
//  MPAlignerConfigurationTranslEntry.cs
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
    /// MPAligner configuration transliteration entry.
    /// </summary>
    [Serializable]
    public class MPAlignerConfigurationTranslEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MPAligner.MPAlignerConfigurationTranslEntry"/> class.
        /// </summary>
        public MPAlignerConfigurationTranslEntry ()
        {
            _srcLang = null;
            _trgLang = null;
            mosesIniPath = null;
            nBest = 5;
            stem = false;
            use = true;
            threshold = 0.1;
            maxLenDiff = 0.7;
            translitBf = new BezierFunction(0.2,0.6,0,1);
        }
        
        [XmlElement]
        public BezierFunction translitBf;
        
        [XmlAttribute("use")]
        public bool use;
        
        [XmlAttribute("thr")]
        public double threshold;
        
        [XmlAttribute("maxLenDiff")]
        public double maxLenDiff;

        /// <summary>
        /// The source language.
        /// </summary>
        [XmlAttribute("sLang")]
        public string srcLang {
            get {
                return _srcLang;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _srcLang = MPFramework.MPFrameworkFunctions.GetValidLangString(value);
                }
            }
        }
        [XmlIgnore]
        private string _srcLang;
        /// <summary>
        /// The target language.
        /// </summary>
        [XmlAttribute("tLang")]
        public string trgLang {
            get {
                return _trgLang;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _trgLang = MPFramework.MPFrameworkFunctions.GetValidLangString(value);
                }
            }
        }
        [XmlIgnore]
        private string _trgLang;
        /// <summary>
        /// The moses ini file path (moses configuration file path).
        /// </summary>
        [XmlElement("mosesIniPath")]
        public string mosesIniPath;
        /// <summary>
        /// The number of n-best transliterations to extract.
        /// </summary>
        [XmlAttribute("nBest")]
        public int nBest;
        /// <summary>
        /// The use of (lightweight!!!) stemming for simple transliteration comparison.
        /// </summary>
        [XmlAttribute("stem")]
        public bool stem;
    }
}

