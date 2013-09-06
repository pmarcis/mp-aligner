//
//  MPAlignerConfigurationDictEntry.cs
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
using System.Globalization;
using System.Xml.Serialization;

namespace MPAligner
{
    /// <summary>
    /// MPAligner configuration dictionary entry.
    /// </summary>
    [Serializable]
    public class MPAlignerConfigurationDictEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MPAligner.MPAlignerConfigurationDictEntry"/> class.
        /// </summary>
        public MPAlignerConfigurationDictEntry ()
        {
            _srcLang = null;
            _trgLang = null;
            path = null;
            stem = false;
            maxVariants = 3;
            variantThreshold = 0.3;
            separators = " \t";
            filterDictionary = true;
            _nfi = new NumberFormatInfo();
            decimalSeparator = ".";
            _nfi.CurrencyDecimalSeparator = ".";
            _nfi.NumberDecimalSeparator = ".";
            _nfi.PercentDecimalSeparator = ".";
            encodingCodePage = 65001;
            use = true;
            dictBf = new BezierFunction(0.4,0.8,0,1);
        }
        
        [XmlElement("dictBf")]
        public BezierFunction dictBf;
        
        [XmlAttribute("use")]
        public bool use;
        
        /// <summary>
        /// Whether (true) or not (false) to use (lightweight!!!) stemming of the dictionary entries.
        /// </summary>
        [XmlAttribute("stem")]
        public bool stem;        
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
        /// The dictionary path.
        /// </summary>
        [XmlElement("path")]
        public string path;
        /// <summary>
        /// The maxium number of translation variants per entry to keep.
        /// </summary>
        [XmlAttribute("max")]
        public int maxVariants;
        /// <summary>
        /// The translation variant threshold.
        /// </summary>
        [XmlAttribute("thr")]
        public double variantThreshold;
        /// <summary>
        /// The dictionary entry separators.
        /// </summary>
        [XmlAttribute("sep")]
        public string sepForXml {
            get {
                return separators.Replace("\t","[TAB]").Replace("\r","[CR]").Replace("\n","[LF]");
            }
            set {
                separators=value.Replace("[TAB]","\t").Replace("[CR]","\r").Replace("[LF]","\n");
            }
        }
        
        [XmlIgnore]
        public string separators;


        /// <summary>
        /// Whether (true) or not (false) to flter the dictionary.
        /// </summary>
        [XmlAttribute("filter")]
        public bool filterDictionary;
        /// <summary>
        /// Gets or sets the decimal separator string (default is a point).
        /// </summary>
        /// <value>
        /// The decimal separator string.
        /// </value>
        [XmlAttribute("decSep")]
        public string decimalSeparator {
            get {
                return _decimalSeparator;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _decimalSeparator = value;
                    _nfi.CurrencyDecimalSeparator = value;
                    _nfi.NumberDecimalSeparator = value;
                    _nfi.PercentDecimalSeparator = value;
                }
            }
        }
        /// <summary>
        /// The _decimal separator string.
        /// </summary>
        [XmlIgnore]
        private string _decimalSeparator;
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// For UTF-8 use 65001.
        /// </value>
        [XmlAttribute("enc")]
        public int encodingCodePage {
            get {
                return encoding.CodePage;
            }
            set {
                encoding = Encoding.GetEncoding(value);

            }
        }
        [XmlIgnore]
        public Encoding encoding;
        /// <summary>
        /// The Number format info.
        /// </summary>
        [XmlIgnore]
        public NumberFormatInfo numberFormatInfo
        {
            get{
                return _nfi;
            }
        }
        
        /// <summary>
        /// The number format info - automatically generated for XML.
        /// </summary>
        [XmlIgnore]
        private NumberFormatInfo _nfi;
    }
}
