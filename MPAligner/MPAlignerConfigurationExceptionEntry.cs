//
//  MPAlignerConfigurationExceptionEntry.cs
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
using System.Xml.Serialization;

namespace MPAligner
{
    [Serializable]
    public class MPAlignerConfigurationExceptionEntry
    {
        public MPAlignerConfigurationExceptionEntry ()
        {
            use = true;
            srcLang = null;
            trgLang = null;
            path = null;
            separators = "\t ";
            encodingCodePage = 65001;
            stem = false;
        }
        
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
    }
}

