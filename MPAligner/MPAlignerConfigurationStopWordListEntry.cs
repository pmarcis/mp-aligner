//
//  MPAlignerConfigurationStopWordListEntry.cs
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
using System.Text;

namespace MPAligner
{
    public class MPAlignerConfigurationStopWordListEntry
    {
        public MPAlignerConfigurationStopWordListEntry ()
        {
            use = true;
            lang = null;
            path = null;
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
        /// The language.
        /// </summary>
        [XmlAttribute("lang")]
        public string lang {
            get {
                return _lang;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _lang = MPFramework.MPFrameworkFunctions.GetValidLangString(value);
                }
            }
        }
        
        [XmlIgnore]
        private string _lang;

        /// <summary>
        /// The dictionary path.
        /// </summary>
        [XmlElement("path")]
        public string path;
        
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

