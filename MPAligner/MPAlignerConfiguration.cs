//
//  MPAlignerConfiguration.cs
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
using System.IO;
using System.Threading.Tasks;
using MPFramework;
using System.Xml;
using System.Xml.Serialization;

namespace MPAligner
{
    /// <summary>
    /// MPAligner configuration.
    /// </summary>
    [Serializable]
    public class MPAlignerConfiguration
    {
        /// <summary>
        /// The output format.
        /// Allowed values are: tabsep, ref_tabsep, xml
        /// </summary>
        [XmlAttribute("outputFormat")]
        public string outputFormat {
            get {
                return _outputFormat;
            }
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("[MPAligner] ERROR: Output format empty!");
                }
                if (value.ToUpper() == "TABSEP")
                {
                    _outputFormat = "tabsep";
                }
                else if (value.ToUpper() == "XML")
                {
                    _outputFormat = "xml";
				}
				else if (value.ToUpper() == "REF_TABSEP")
				{
					_outputFormat = "ref_tabsep";
				}
				else
                {
                    throw new ArgumentException("[MPAligner] ERROR: Output format not supported (supported values are: tabsep, ref_tabsep, and xml)!");
                }
            }
        }
        
        [XmlAttribute("threads")]
        public int alignmentThreads = 12;
        
        [XmlAttribute("threadedExec")]
        public bool useMultiThreadedExecution = false; //As the multithreaded execution is not yet stable, it is advised to use the single thread execution...
        
        [XmlAttribute("trimmedAlignments")]
        public bool allowTrimmedAlignments = false;

        [XmlAttribute("keepTrackOfFiles")]
        public bool keepTrackOfFiles = true;

        [XmlAttribute("printTopTrgForSrc")]
        public bool printTopTrgForSrc = false;

        /// <summary>
        /// The logging level.
        /// NONE - no logging.
        /// OUTPUT - just system output.
        /// WARNING - system output + warnings.
        /// ERROR - system output + warnings + errors (default).
        /// </summary>
        [XmlAttribute("logLevel")]
        public LogLevelType logLevel = LogLevelType.ERROR;

		/// <summary>
		/// The length of the concordance string of terms to be extracted from term-tagged documents.
		/// </summary>
		[XmlAttribute("concLen")]
		public int concLen = 3;

        [XmlAttribute("forceEnTranslitInterlingua")]
        public bool forceEnTranslitInterlingua = true;
        [XmlAttribute("forceEnDictInterlingua")]
        public bool forceEnDictInterlingua = true;
        [XmlIgnore]
        private string _outputFormat = "ref_tabsep";
        /// <summary>
        /// The moses path (full executable path).
        /// </summary>
        [XmlElement("mosesPath")]
        public string mosesPath = "";
        /// <summary>
        /// Gets or sets the transliteration configuration entries.
        /// </summary>
        /// <value>
        /// The transliteration configuration entries.
        /// The value is dynamically generated from the respective <c>Dictionary</c>.
        /// Purpose of the list is the XML serialisation and deserialisation.
        /// Therefore, it is advised not to use the list during runtime.
        /// </value>
        //[XmlElement("translitEntry", typeof(MPAlignerConfigurationTranslEntry))]
        [XmlArray]
        [XmlArrayItem(typeof(MPAlignerConfigurationTranslEntry))]
        public MPAlignerConfigurationTranslEntry[] translConfEntries {
            get {
                if (translConfEntryDict==null) return null;
                List<MPAlignerConfigurationTranslEntry> res = new List<MPAlignerConfigurationTranslEntry>();
                foreach(MPAlignerConfigurationTranslEntry cte in translConfEntryDict.Values)
                {
                    res.Add(cte);
                }
                return res.ToArray();
            }
            set {
                translConfEntryDict = new Dictionary<string, MPAlignerConfigurationTranslEntry> ();
                if (value != null) {
                    foreach (MPAlignerConfigurationTranslEntry cte in value) {
                        if (!string.IsNullOrWhiteSpace (cte.srcLang) && !string.IsNullOrWhiteSpace (cte.trgLang)) {
                            string key = cte.srcLang + "_" + cte.trgLang; 
                            if (!translConfEntryDict.ContainsKey (key)) {
                                translConfEntryDict.Add (key, cte);
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// The transliteration configuration entry <c>Dictionary</c>.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string,MPAlignerConfigurationTranslEntry> translConfEntryDict; 
        /// <summary>
        /// Gets or sets the dictionary configuration entries.
        /// </summary>
        /// <value>
        /// The dictionary configuration entries.
        /// The value is dynamically generated from the respective <c>Dictionary</c>.
        /// Purpose of the list is the XML serialisation and deserialisation.
        /// Therefore, it is advised not to use the list during runtime.
        /// </value>
        //[XmlElement("dictEntry", typeof(MPAlignerConfigurationDictEntry))]
        [XmlArray]
        [XmlArrayItem(typeof(MPAlignerConfigurationDictEntry))]
        public MPAlignerConfigurationDictEntry[] dictConfEntries {
            get {
                if (dictConfEntryDict==null) return null;
                List<MPAlignerConfigurationDictEntry> res = new List<MPAlignerConfigurationDictEntry>();
                foreach(MPAlignerConfigurationDictEntry cde in dictConfEntryDict.Values)
                {
                    res.Add(cde);
                }
                return res.ToArray();
            }
            set {
                dictConfEntryDict = new Dictionary<string, MPAlignerConfigurationDictEntry>();
                if (value != null) {
                    foreach (MPAlignerConfigurationDictEntry cde in value) {
                        if (!string.IsNullOrWhiteSpace (cde.srcLang) && !string.IsNullOrWhiteSpace (cde.trgLang)) {
                            string key = cde.srcLang + "_" + cde.trgLang; 
                            if (!dictConfEntryDict.ContainsKey (key)) {
                                dictConfEntryDict.Add (key, cde);
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// The dictionary configuration entry <c>Dictionary</c>.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string,MPAlignerConfigurationDictEntry> dictConfEntryDict; 
        
        [XmlArray]
        [XmlArrayItem(typeof(MPAlignerConfigurationLangPairEntry))]
        public MPAlignerConfigurationLangPairEntry[] langPairConfEntries {
            get {
                if (langPairEntryDict==null) return null;
                List<MPAlignerConfigurationLangPairEntry> res = new List<MPAlignerConfigurationLangPairEntry>();
                foreach(MPAlignerConfigurationLangPairEntry cte in langPairEntryDict.Values)
                {
                    res.Add(cte);
                }
                return res.ToArray();
            }
            set {
                langPairEntryDict = new Dictionary<string, MPAlignerConfigurationLangPairEntry> ();
                if (value != null) {
                    foreach (MPAlignerConfigurationLangPairEntry cte in value) {
                        if (!string.IsNullOrWhiteSpace (cte.srcLang) && !string.IsNullOrWhiteSpace (cte.trgLang)) {
                            string key = cte.srcLang + "_" + cte.trgLang; 
                            if (!langPairEntryDict.ContainsKey (key)) {
                                langPairEntryDict.Add (key, cte);
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// The language pair configuration entry <c>Dictionary</c>.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string,MPAlignerConfigurationLangPairEntry> langPairEntryDict; 

        [XmlArray]
        [XmlArrayItem(typeof(MPAlignerConfigurationExceptionEntry))]
        public MPAlignerConfigurationExceptionEntry[] excDictConfEntries {
            get {
                if (excDictEntryDict==null) return null;
                List<MPAlignerConfigurationExceptionEntry> res = new List<MPAlignerConfigurationExceptionEntry>();
                foreach(MPAlignerConfigurationExceptionEntry cee in excDictEntryDict.Values)
                {
                    res.Add(cee);
                }
                return res.ToArray();
            }
            set {
                excDictEntryDict = new Dictionary<string, MPAlignerConfigurationExceptionEntry> ();
                if (value != null) {
                    foreach (MPAlignerConfigurationExceptionEntry cee in value) {
                        if (!string.IsNullOrWhiteSpace (cee.srcLang) && !string.IsNullOrWhiteSpace (cee.trgLang)) {
                            string key = cee.srcLang + "_" + cee.trgLang; 
                            if (!excDictEntryDict.ContainsKey (key)) {
                                excDictEntryDict.Add (key, cee);
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// The language pair configuration entry <c>Dictionary</c>.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string,MPAlignerConfigurationExceptionEntry> excDictEntryDict; 

        
        [XmlArray]
        [XmlArrayItem(typeof(MPAlignerConfigurationStopWordListEntry))]
        public MPAlignerConfigurationStopWordListEntry[] stopWordListConfEntries {
            get {
                if (stopWordListEntryDict==null) return null;
                List<MPAlignerConfigurationStopWordListEntry> res = new List<MPAlignerConfigurationStopWordListEntry>();
                foreach(MPAlignerConfigurationStopWordListEntry cee in stopWordListEntryDict.Values)
                {
                    res.Add(cee);
                }
                return res.ToArray();
            }
            set {
                stopWordListEntryDict = new Dictionary<string, MPAlignerConfigurationStopWordListEntry> ();
                if (value != null) {
                    foreach (MPAlignerConfigurationStopWordListEntry cee in value) {
                        if (!string.IsNullOrWhiteSpace (cee.lang)) {
                            string key = cee.lang; 
                            if (!stopWordListEntryDict.ContainsKey (key)) {
                                stopWordListEntryDict.Add (key, cee);
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// The language pair configuration entry <c>Dictionary</c>.
        /// </summary>
        [XmlIgnore]
        public Dictionary<string,MPAlignerConfigurationStopWordListEntry> stopWordListEntryDict; 

        /// <summary>
        /// Initializes a new instance of the <see cref="MPAligner.MPAlignerConfiguration"/> class.
        /// </summary>
        public MPAlignerConfiguration()
        {
            mosesPath = "/home/marcis/LU/moses/mosesdecoder/bin/moses";
            dictConfEntryDict = new Dictionary<string, MPAlignerConfigurationDictEntry>();
            translConfEntryDict = new Dictionary<string, MPAlignerConfigurationTranslEntry>();
            langPairEntryDict = new Dictionary<string, MPAlignerConfigurationLangPairEntry>();
            excDictEntryDict = new Dictionary<string, MPAlignerConfigurationExceptionEntry>();
            stopWordListEntryDict = new Dictionary<string, MPAlignerConfigurationStopWordListEntry>();
            allowTrimmedAlignments = false;
            outputFormat="ref_tabsep";
            keepTrackOfFiles = true;
            logLevel = LogLevelType.ERROR;
            forceEnDictInterlingua = false;
            forceEnTranslitInterlingua = false;
            alignmentThreads = 12;
            useMultiThreadedExecution = false;
            printTopTrgForSrc = false;
			concLen = 3;
        }
        
        /// <summary>
        /// Saves the configuration to a specified <c>outputFile</c>.
        /// </summary>
        /// <param name='outputFile'>
        /// Output file.
        /// </param>
        public void Save(string outputFile)
        {
            string outputStr = MPFrameworkFunctions.SerializeObjectInstance<MPAlignerConfiguration>(this);
            File.WriteAllText(outputFile,outputStr,Encoding.UTF8);
        }

        /// <summary>
        /// Loads the configuration from a specified <c>inputFile</c>.
        /// </summary>
        /// <param name='inputFile'>
        /// Input file.
        /// </param>
        public void Load(string inputFile)
        {
            string inputStr = File.ReadAllText(inputFile,Encoding.UTF8);
            MPAlignerConfiguration conf = MPFrameworkFunctions.DeserializeString<MPAlignerConfiguration>(inputStr);
            
            dictConfEntryDict = conf.dictConfEntryDict;
            mosesPath = conf.mosesPath;
            translConfEntryDict = conf.translConfEntryDict;
            keepTrackOfFiles = conf.keepTrackOfFiles;
            logLevel = conf.logLevel;
            forceEnDictInterlingua = conf.forceEnDictInterlingua;
            forceEnTranslitInterlingua = conf.forceEnTranslitInterlingua;
            outputFormat = conf.outputFormat;
            excDictEntryDict = conf.excDictEntryDict;
            allowTrimmedAlignments = conf.allowTrimmedAlignments;
            stopWordListEntryDict = conf.stopWordListEntryDict;
            langPairEntryDict = conf.langPairEntryDict;
            alignmentThreads = conf.alignmentThreads;
            useMultiThreadedExecution = conf.useMultiThreadedExecution;
            printTopTrgForSrc = conf.printTopTrgForSrc;
			concLen = conf.concLen;
        }
    }
}
