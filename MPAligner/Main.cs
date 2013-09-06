//
//  Main.cs
//
//  Author:
//       Marcis Pinnis <marcis.pinnis@gmail.com>
//
//  Copyright (c) 2013 Marcis Pinnis
//
//  This program can be freely used only for scientific and educational purposes.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
//


namespace MPAligner
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Text;
	using System.Globalization;

    class MainClass
    {
	
		public static void PrintUsage()
		{
			Console.WriteLine("MPAligner is a designed to perform term mapping accross European Languages in a context-independent fashion.\nIf you use MPAligner or any of the resources provided with MPAligner for research purposes please cite the following paper:");
			Console.WriteLine("  @inproceedings{Pinnis2013,");
			Console.WriteLine("  address = {Hissar, Bulgaria},");
			Console.WriteLine("  author = {Pinnis, Mârcis},");
			Console.WriteLine("  booktitle = {Proceedings of the 9th International Conference on Recent Advances in Natural Language Processing (RANLP 2013)},");
			Console.WriteLine("  title = {{Context Independent Term Mapper for European Languages}},");
			Console.WriteLine("  year = {2013}");
			Console.WriteLine("  }");
			Console.WriteLine();
			Console.WriteLine("Usage: mono --runtime=v4.0 ./MPAligner.exe [ARGS]");
			Console.WriteLine("  The [ARGS] are as follows:");
			Console.WriteLine("    -m [Method] - the processing method:");
			Console.WriteLine("      config             - for creation of an example configuration file");
			Console.WriteLine("      taggedfilepairs    - for term-tagged file pair list processing");
			Console.WriteLine("      singletaggedpair   - for a single term-tagged file pair processing");
			Console.WriteLine("      singletermpairlist - for processing of a single parallel term pair list - can be used to filter term pairs of other term aligners");
			Console.WriteLine("      eurovoceval - for processing and evaluation of MPAligner on EuroVoc data");
			Console.WriteLine("    -c [Configuration File] - mandatory configuration file");
			Console.WriteLine("    -if [Input Format]   - the input data format:");
			Console.WriteLine("      tagged_plaintext   - plaintext where terms are annotated with <TENAME> tags");
			Console.WriteLine("                           (for more details refer to the Tilde's Wrapper System for CollTerm)");
			Console.WriteLine("      term_list          - plaintext term lists; one term per line");
			Console.WriteLine("      preprocessed_terms - pre-processed term list file in XML format (only for testing!)");
			Console.WriteLine("    -i [Input File]      - the input file (when 1 input file is required)");
			Console.WriteLine("    -si [Source Input File] - the source input file (when 2 input files are required)");
			Console.WriteLine("    -ti [Target Input File] - the target input file (when 2 input files are required)");
			Console.WriteLine("    -sl [Source language]   - the source language - lowercase 2 character ISO 639-1 code");
			Console.WriteLine("    -tl [Target language]   - the target language - lowercase 2 character ISO 639-1 code");
			Console.WriteLine("    -o [Output File]   - the output file for mapped terms");
			Console.WriteLine("    -of [Output Format]   - the output format:");
			Console.WriteLine("      xml        - XML (serialised AlignmentInfoElement list");
			Console.WriteLine("      tabsep     - TSV (limited alignment information)");
			Console.WriteLine("      ref_tabsep - TSV, default (full alignment information)");
			Console.WriteLine("    -pto [Pre-processed Term Output File] - the pre-processed term output file - may be used when single document pair processing is performed to write the pre-processed intermediate results in an XML file (used for testing)");
			Console.WriteLine("    -ttf [Temporary Transliteration File] - optional and if not given a default output file will be defined");
			Console.WriteLine("    -ss [Source Text File for Skipping] - the file name of a source text file till which to skip processing (used in an AND logic together with -st)");
			Console.WriteLine("    -st [Target Text File for Skipping] - the file name of a target text file till which to skip  (used in an AND logic together with -ss)");
			Console.WriteLine("    -d_id [Domain ID] - the domain identifier for output (required for the project TaaS)");
			Console.WriteLine("    -c_id [Collection ID] - the collection identifier for output (required for the project TaaS)");
			Console.WriteLine ();
			Console.WriteLine ("Examples of execution commands are as follows:");
			Console.WriteLine ("  In order to create an example configuration file:");
			Console.WriteLine ("    mono --runtime=v4.0 ./MPAligner.exe -m config -o MPAlignerConfig.xml");
			Console.WriteLine ("  In order to map terms using a term-tagged document pair list file:");
			Console.WriteLine ("    mono --runtime=v4.0 ./MPAligner.exe -m taggedfilepairs -c ./MPAlignerConfig.xml -if tagged_plaintext -i ./documentAlignmentFile.txt -sl lv -tl en -o ./lv-en-term-pairs.txt -d_id 32 -c_id http://taas-project.eu/MP_Aligner/IT_News/V20.08.2013");
			Console.WriteLine ("  In order to map terms using one term-tagged document pair:");
			Console.WriteLine ("    mono --runtime=v4.0 ./MPAligner.exe -m singletaggedpair -c ./MPAlignerConfig.xml -if term_list -si ./srcTerms.txt -ti ./trgTerms.txt -sl lv -tl en -o ./res.xml -d_id 32 -c_id http://taas-project.eu/MP_Aligner/IT_News/V20.08.2013 -of xml");
		}
        public static void Main (string[] args)
        {
            string configFile = null;
            string method = null;
            string inputFile = null;
            string inputFormat = "tagged_plaintext";//Allowed values: tagged_plaintext, preprocessed_terms, term_list
            string srcInputFile = null;
            string trgInputFile = null;
            string srcLang = null;
            string trgLang = null;
            string outputFile = null;
			string consolidatedOutputFile = null;
			string outputFormat = "";//"tabsep";//Allowed values: ref_tabsep, tabsep, xml
            string preProcessedTermOutputFile = null;//"/home/marcis/Dropbox/MonoProjects/MPAligner/MPAligner/bin/Debug/testTermData.xml";//null;
            string tempTranslitFile = null;
			bool consolidateResults = false;
			double consolidationThreshold = 0;
            //bool logPrepData = false;
			string domainId = "";
			string collectionId = "";
            //The skipping parameters are just for debugging. Use them only manually!
            string skipSrc = "";
            string skipTrg = "";
            MPAlignerConfiguration configuration = null; 
			//Read all configuration parameters from the command line.
			for (int i=0; i<args.Length; i++) {
				if ((args [i] == "-c" || args [i] == "--configuration") && args.Length > i + 1) {
					configFile = args [i + 1];
					configuration = new MPAlignerConfiguration ();
					configuration.Load (configFile);
				} else if ((args [i] == "-m" || args [i] == "--method") && args.Length > i + 1) {
					method = args [i + 1];
				} else if ((args [i] == "-i" || args [i] == "--input-file") && args.Length > i + 1) {
					inputFile = args [i + 1];
					//} else if (args [i] == "-lp" || args [i] == "--log-pre-processed") {
					//	logPrepData = true;
				} else if ((args [i] == "-if" || args [i] == "--input-format") && args.Length > i + 1) {
					inputFormat = args [i + 1];
				} else if ((args [i] == "-si" || args [i] == "--source-input") && args.Length > i + 1) {
					srcInputFile = args [i + 1];
				} else if ((args [i] == "-ti" || args [i] == "--target-input") && args.Length > i + 1) {
					trgInputFile = args [i + 1];
				} else if ((args [i] == "-sl" || args [i] == "--source-language") && args.Length > i + 1) {
					srcLang = MPFramework.MPFrameworkFunctions.GetValidLangString (args [i + 1]);
				} else if ((args [i] == "-tl" || args [i] == "--target-language") && args.Length > i + 1) {
					trgLang = MPFramework.MPFrameworkFunctions.GetValidLangString (args [i + 1]);
				} else if ((args [i] == "-o" || args [i] == "--output-file") && args.Length > i + 1) {
					outputFile = args [i + 1];
				} else if ((args [i] == "-of" || args [i] == "--output-format") && args.Length > i + 1) {
					outputFormat = args [i + 1];
				} else if ((args [i] == "-pto" || args [i] == "--pre-processed-term-output-file") && args.Length > i + 1) {
					preProcessedTermOutputFile = args [i + 1];
				} else if ((args [i] == "-ttf" || args [i] == "--temp-translit-file") && args.Length > i + 1) {
					tempTranslitFile = args [i + 1];
				} else if ((args [i] == "-ss" || args [i] == "--skip-source-file") && args.Length > i + 1) {
					skipSrc = args [i + 1];
				} else if ((args [i] == "-st" || args [i] == "--skip-target-file") && args.Length > i + 1) {
					skipTrg = args [i + 1];
				} else if ((args [i] == "-d_id" || args [i] == "--domain-id") && args.Length > i + 1) {
					domainId = args [i + 1];
				} else if ((args [i] == "-c_id" || args [i] == "--collection-id") && args.Length > i + 1) {
					collectionId = args [i + 1];
				} else if ((args [i] == "-ct" || args [i] == "--consolidation-threshold") && args.Length > i + 1) {
					//Consolidation works only if the ref_tabsep output format is specified!
					NumberFormatInfo nfi = new NumberFormatInfo ();
					nfi.CurrencyDecimalSeparator = ".";
					nfi.NumberDecimalSeparator = ".";
					nfi.PercentDecimalSeparator = ".";
					consolidationThreshold = Convert.ToDouble (args [i + 1], nfi);
					consolidateResults = true;
				}
			}
			//Break if a method is not defined.
            if (string.IsNullOrWhiteSpace (method)) {
                Log.Write ("Method not specified!",LogLevelType.ERROR,configuration);
                PrintUsage ();
                return;
            }
			//Write a configuration file to the output file if the config method is specified.
            if (method.ToLower () == "config") {
                if (string.IsNullOrWhiteSpace (outputFile)) {
                    Log.Write("Output file not specified!",LogLevelType.ERROR,configuration);
                    PrintUsage ();
                    return;
                }
                MPAlignerConfiguration conf = new MPAlignerConfiguration ();
                MPAlignerConfigurationDictEntry cde = new MPAlignerConfigurationDictEntry ();
                cde.srcLang = "lv";
                cde.trgLang = "en";
                cde.path = "/home/marcis/TILDE/RESOURCES/DICT/lv_en_noisy";
                conf.dictConfEntryDict.Add ("lv_en", cde);
                cde = new MPAlignerConfigurationDictEntry ();
                cde.srcLang = "lt";
                cde.trgLang = "en";
                cde.path = "/home/marcis/TILDE/RESOURCES/DICT/lt_en";
                conf.dictConfEntryDict.Add ("lt_en", cde);
                MPAlignerConfigurationTranslEntry cte = new MPAlignerConfigurationTranslEntry ();
                cte.mosesIniPath = "/home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/LV-EN/lv-en-binarised-model.moses.ini";
                cte.srcLang = "lv";
                cte.trgLang = "en";
                conf.translConfEntryDict.Add ("lv_en", cte);
                cte = new MPAlignerConfigurationTranslEntry ();
                cte.mosesIniPath = "/home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/LV-EN/lt-en-binarised-model.moses.ini";
                cte.srcLang = "lt";
                cte.trgLang = "en";
                conf.translConfEntryDict.Add ("lt_en", cte);
                MPAlignerConfigurationLangPairEntry lpe = new MPAlignerConfigurationLangPairEntry ();
                lpe.srcLang = "lv";
                lpe.trgLang = "en";
                conf.langPairEntryDict.Add ("lv_en", lpe);
                lpe = new MPAlignerConfigurationLangPairEntry ();
                lpe.srcLang = "lt";
                lpe.trgLang = "en";
                conf.langPairEntryDict.Add ("lt_en", lpe);
                MPAlignerConfigurationExceptionEntry cee = new MPAlignerConfigurationExceptionEntry ();
                cee.srcLang = "lv";
                cee.trgLang = "en";
                cee.path = "/home/marcis/TILDE/RESOURCES/EXC_DICT/lv_en_exc";
                conf.excDictEntryDict.Add ("lv_en", cee);
                cee = new MPAlignerConfigurationExceptionEntry ();
                cee.srcLang = "lt";
                cee.trgLang = "en";
                cee.path = "/home/marcis/TILDE/RESOURCES/EXC_DICT/lt_en_exc";
                conf.excDictEntryDict.Add ("lt_en", cee);
                MPAlignerConfigurationStopWordListEntry cswle = new MPAlignerConfigurationStopWordListEntry ();
                cswle.lang = "lv";
                cswle.path = "/home/marcis/TILDE/RESOURCES/STOP_WORD/lv_stop";
                conf.stopWordListEntryDict.Add ("lv", cswle);
                cswle = new MPAlignerConfigurationStopWordListEntry ();
                cswle.lang = "lt";
                cswle.path = "/home/marcis/TILDE/RESOURCES/STOP_WORD/lt_stop";
                conf.stopWordListEntryDict.Add ("lt", cswle);
                cswle = new MPAlignerConfigurationStopWordListEntry ();
                cswle.lang = "en";
                cswle.path = "/home/marcis/TILDE/RESOURCES/STOP_WORD/en_stop";
                conf.stopWordListEntryDict.Add ("en", cswle);
                conf.Save (outputFile);
                return;
            }

			//Try reading the default configuration if none is passed, but if the default configuration can not be found, break.
            if (string.IsNullOrWhiteSpace (configFile) && File.Exists ("MPAlignerConfig.xml")) {
                configuration = new MPAlignerConfiguration ();
                configuration.Load (configFile);
            } else if (string.IsNullOrWhiteSpace (configFile)) {
                Log.Write("Configuration file missing in application directory and a substitution runtime configuration file is not specified!",LogLevelType.ERROR,configuration);
                PrintUsage ();
                return;
            }

			//In the case if an output format is not defined in the command line, read it from the configuration file.
			if (string.IsNullOrWhiteSpace (outputFormat))
				outputFormat = configuration.outputFormat;

			//In the case if the configuration does not specify an output format, use the default output format.
			if (string.IsNullOrWhiteSpace (outputFormat)) {
				outputFormat = "ref_tabsep";
			}
            
            Log.confLogLevel = configuration.logLevel;

            if (string.IsNullOrWhiteSpace (tempTranslitFile)) {
                tempTranslitFile = outputFile+".tmp";
            }

            Log.Write ("configFile: "+(configFile!=null?configFile:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("method: "+(method!=null?method:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("inputFile: "+(inputFile!=null?inputFile:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("inputFormat: "+(inputFormat!=null?inputFormat:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("srcInputFile: "+(srcInputFile!=null?srcInputFile:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("trgInputFile: "+(trgInputFile!=null?trgInputFile:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("srcLang: "+(srcLang!=null?srcLang:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("trgLang: "+(trgLang!=null?trgLang:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("outputFile: "+(outputFile!=null?outputFile:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("outputFormat: "+(outputFormat!=null?outputFormat:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("preProcessedTermOutputFile: "+(preProcessedTermOutputFile!=null?preProcessedTermOutputFile:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("tempTranslitFile: "+(tempTranslitFile!=null?tempTranslitFile:""),LogLevelType.LIMITED_OUTPUT,configuration);
			Log.Write ("consolidation threshold: "+(consolidateResults?consolidationThreshold.ToString():""),LogLevelType.LIMITED_OUTPUT,configuration);

			if (outputFormat == "ref_tabsep" && consolidateResults) {
				consolidatedOutputFile = outputFile;
				outputFile += ".raw";
			}

			//For document pair-based alignment.
            if (method.ToLower () == "taggedfilepairs") {
                char[] sep = {'\t'};
                if (string.IsNullOrWhiteSpace(inputFile)||!File.Exists(inputFile))
                {
                    Log.Write("Input file list file not specified or cannot be found!",LogLevelType.ERROR,configuration);
                    PrintUsage();
                    return;
                }
                if (string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
                {
                    Log.Write("Source and/or target languages not specified!",LogLevelType.ERROR,configuration);
                    PrintUsage();
                    return;
                }
				//Read the alignment thresholds and other language pair specific numerical/single-value parameters.
				MPAlignerConfigurationLangPairEntry lpeConf = ReadLangPairConfig (srcLang, trgLang, configuration);
				//The size of the cache may affect the performance of the alignment!
				Dictionary<string, ProcessedTermEntry> srcTermCache = new Dictionary<string, ProcessedTermEntry>();
                Dictionary<string, ProcessedTermEntry> trgTermCache = new Dictionary<string, ProcessedTermEntry>();

                bool interlinguaDictUsed = false;
                bool interlinguaTranslitUsed = false;
                
				//Define dictionaries for pre-processing.
                Dictionary<string, Dictionary<string, double>> srcDict = null;
                Dictionary<string, Dictionary<string, double>> trgDict = null;
                Dictionary<string, Dictionary<string, double>> srcToTrgDict = null;
                Dictionary<string, Dictionary<string, double>> trgToSrcDict = null;

				//Define transliteration configurations for pre-processing.
                MPAlignerConfigurationTranslEntry srcTranslitConf = null;
                MPAlignerConfigurationTranslEntry trgTranslitConf = null;
                MPAlignerConfigurationTranslEntry srcToTrgTranslitConf = null;
                MPAlignerConfigurationTranslEntry trgToSrcTranslitConf = null;

				//Read dictionaries and transliterations.
                interlinguaDictUsed = ReadDictionaries(configuration,srcLang,trgLang, out srcDict, out trgDict, out srcToTrgDict, out trgToSrcDict);
                interlinguaTranslitUsed = GetTranslitConfig(configuration,srcLang,trgLang,out srcTranslitConf,out trgTranslitConf,out srcToTrgTranslitConf, out trgToSrcTranslitConf);
                
				//Define the alignments (the variable holding alignment results)
                Dictionary<string,Dictionary<string, AlignmentInfoElement>> alignments = new Dictionary<string, Dictionary<string, AlignmentInfoElement>>();
                
				//Define and read exception dictionaries.
				Dictionary<string, Dictionary<string, bool>> excDict = null;
                ReadExceptionDictionary(configuration,srcLang, trgLang,out excDict);
                
				//Define and read stopword lists.
                Dictionary<string,bool> srcStopWords = null;
                ReadStopwordList(configuration,srcLang,out srcStopWords);
                Dictionary<string,bool> trgStopWords = null;
                ReadStopwordList(configuration,trgLang,out trgStopWords);
                
                StreamReader sr = new StreamReader(inputFile,Encoding.UTF8);
                int pairCounter = 0;
				bool skip = !string.IsNullOrWhiteSpace(skipSrc)&&!string.IsNullOrWhiteSpace(skipTrg)?true:false;

				//Read input document alignment file and process file pairs.
                while(!sr.EndOfStream)
                {
                    pairCounter++;
                    string line = sr.ReadLine().Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] arr = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length<2)
                    {
                        continue; //If the alignment line does not contain at least two entries, the document alignment is not valid.
                    }
                    string srcFile = arr[0];
                    string trgFile = arr[1];
                    if (!File.Exists(srcFile))
                    {
                        Log.Write("Input file \""+srcFile+"\" cannot be found!",LogLevelType.WARNING,configuration);
                        continue;
                    }
                    if (!File.Exists(trgFile))
                    {
                        Log.Write("Input file \""+trgFile+"\" cannot be found!",LogLevelType.WARNING,configuration);
                        continue;
                    }
                    string srcFileName = Path.GetFileName(srcFile);
                    string trgFileName = Path.GetFileName(trgFile);

                    //The skipping condition is for debugging - if the system crashes due to insufficient memory...
                    if (skip)
                    {
                        if (srcFileName==skipSrc&&trgFileName == skipTrg)
                        {
                            skip = false;
                        }
                        else
                        {
                            Log.Write("Skipping file pair "+srcFileName+" and " + trgFileName+".",LogLevelType.WARNING,configuration);
                            continue;
                        }
                    }

					Log.Write("Processing file pair "+srcFileName+" and " + trgFileName+".",LogLevelType.LIMITED_OUTPUT,configuration);
                    
					//Define term entry data variables (used for sotring terms in pre-pre-processed and pre-processed states).
                    Dictionary<string,SimpleTermEntry> srcInitialList = new Dictionary<string, SimpleTermEntry>();
                    Dictionary<string,SimpleTermEntry> trgInitialList = new Dictionary<string, SimpleTermEntry>();
                    Dictionary<string,SimpleTermEntry> srcInitialTempList = new Dictionary<string, SimpleTermEntry>();
                    Dictionary<string,SimpleTermEntry> trgInitialTempList = new Dictionary<string, SimpleTermEntry>();
                    
                    Dictionary<string, ProcessedTermEntry> srcTermList = new Dictionary<string, ProcessedTermEntry>();
                    Dictionary<string, ProcessedTermEntry> trgTermList = new Dictionary<string, ProcessedTermEntry>();
                    Dictionary<string, ProcessedTermEntry> srcTermTempList = new Dictionary<string, ProcessedTermEntry>();
                    Dictionary<string, ProcessedTermEntry> trgTermTempList = new Dictionary<string, ProcessedTermEntry>();
                    
					//Two input formats are currently supported - term-tagged plaintext files and term list (one term per line) files.
                    if (inputFormat=="tagged_plaintext")
                    {
						//Read terms from the term-tagged documents.
                        srcInitialTempList = TermTaggedFileParser.ParseTermTaggedFile(srcFile,Encoding.UTF8, configuration.concLen);
                        trgInitialTempList = TermTaggedFileParser.ParseTermTaggedFile(trgFile,Encoding.UTF8, configuration.concLen);
                    }
                    else
                    {
						//Read terms from the term list files.
                        srcInitialTempList = ListFileParser.Parse(srcFile,Encoding.UTF8);
                        trgInitialTempList = ListFileParser.Parse(trgFile,Encoding.UTF8);
                    }
                    
                    //Search for already pre-processed source terms in the cache.
                    foreach(string term in srcInitialTempList.Keys)
                    {
                        string lower = term.ToLower();
                        if (srcTermCache.ContainsKey(lower))
                        {
                            if (!srcTermList.ContainsKey(lower)) srcTermList.Add(lower, srcTermCache[lower]);
                        }
                        else
                        {
                            srcInitialList.Add(term, srcInitialTempList[term]);
                        }
                    }
                    
                    //Search for already pre-processed target terms in the cache.
                    foreach(string term in trgInitialTempList.Keys)
                    {
                        string lower = term.ToLower();
                        if (trgTermCache.ContainsKey(lower))
                        {
                            if (!trgTermList.ContainsKey(lower)) trgTermList.Add(lower, trgTermCache[lower]);
                        }
                        else
                        {
                            trgInitialList.Add(term, trgInitialTempList[term]);
                        }
                    }
                    
                    //Now pre-process terms that have not been pre-processed again.
                    if (srcDict!=null||trgDict!=null)
                    {
                        if (srcTranslitConf!=null && trgTranslitConf!=null)
                        {
                            srcTermTempList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermTempList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                        else
                        {
                            srcTermTempList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermTempList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                    }
                    else
                    {
                        if (srcTranslitConf!=null && trgTranslitConf!=null)
                        {
                            srcTermTempList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcToTrgDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermTempList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgToSrcDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                        else
                        {
                            srcTermTempList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcToTrgDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermTempList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgToSrcDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                    }
                    
                    //Update the pre-processed term list for alignment.
                    foreach(string srcTerm in srcTermTempList.Keys)
                    {
                        if (!srcTermList.ContainsKey(srcTerm)) srcTermList.Add(srcTerm,srcTermTempList[srcTerm]);
                        if (!srcTermCache.ContainsKey(srcTerm)) srcTermCache.Add(srcTerm, srcTermTempList[srcTerm]);
                    }
                    
                    foreach(string trgTerm in trgTermTempList.Keys)
                    {
                        if (!trgTermList.ContainsKey(trgTerm)) trgTermList.Add(trgTerm,trgTermTempList[trgTerm]);
                        if (!trgTermCache.ContainsKey(trgTerm)) trgTermCache.Add(trgTerm, trgTermTempList[trgTerm]);
                    }
                    
                    //Execute alignment for one file pair.
                    List<AlignmentInfoElement> alignment = new List<AlignmentInfoElement>();
					//The execution may be multi-threaded or single-threaded. The multi-threaded execution may be instable. Therefore, be careful when using multi-threading.
                    if (configuration.useMultiThreadedExecution)
                    {
                        alignment = Alignment.AlignPairsMultiThreaded(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcFile, trgFile, excDict, srcStopWords, trgStopWords);
                    }
                    else
                    {
                        alignment = Alignment.AlignPairs(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcFile, trgFile, excDict, srcStopWords, trgStopWords);
                    }
                    if (alignment!=null)
                    {
                        foreach(AlignmentInfoElement aie in alignment)
                        {
                            if (!alignments.ContainsKey(aie.srcEntry.lowercaceForm))
                            {
                                alignments.Add(aie.srcEntry.lowercaceForm, new Dictionary<string, AlignmentInfoElement>());
                            }
                            if (!alignments[aie.srcEntry.lowercaceForm].ContainsKey(aie.trgEntry.lowercaceForm))
                            {
                                alignments[aie.srcEntry.lowercaceForm].Add(aie.trgEntry.lowercaceForm, aie);
                            }
                        }
                    }
					//If pre-processed term cache is full, empty it (this maybe can be imrpoved with the help of some sort of a flowing cache (always circulating).
                    if (srcTermCache.Count>50000)
                    {
                        srcTermCache.Clear();
                        srcTermCache = new Dictionary<string, ProcessedTermEntry>();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    if (trgTermCache.Count>50000)
                    {
                        trgTermCache.Clear();
                        trgTermCache = new Dictionary<string, ProcessedTermEntry>();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
					//After each 50 pairs, print rsults.
                    if (pairCounter%50==0||alignments.Count>50000)
                    {
						Log.Write("Printing intermediate results after "+pairCounter.ToString()+" file pairs",LogLevelType.LIMITED_OUTPUT,configuration);
                        List<AlignmentInfoElement> resAlignment = new List<AlignmentInfoElement>();
                        foreach(string src in alignments.Keys)
                        {
                            foreach(string trg in alignments[src].Keys)
                            {
                                resAlignment.Add(alignments[src][trg]);
                            }
                        }
                        AlignmentInfoElement.AppendList(outputFormat,outputFile,resAlignment,lpeConf,srcLang,trgLang,collectionId,domainId);
                        alignments.Clear();
                        alignments = new Dictionary<string, Dictionary<string, AlignmentInfoElement>>();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                sr.Close();
                //If there are alignments left, write them to the output file.
				if (!string.IsNullOrWhiteSpace(outputFile))
                {
					Log.Write("Printing final results after "+pairCounter.ToString()+" file pairs",LogLevelType.LIMITED_OUTPUT,configuration);
                    List<AlignmentInfoElement> resAlignment = new List<AlignmentInfoElement>();
                    foreach(string src in alignments.Keys)
                    {
                        foreach(string trg in alignments[src].Keys)
                        {
                            resAlignment.Add(alignments[src][trg]);
                        }
                    }
					AlignmentInfoElement.AppendList(outputFormat,outputFile,resAlignment,lpeConf,srcLang,trgLang,collectionId,domainId);
                }
            }
            else if (method.ToLower () == "singletaggedpair") //TODO: REFACTOR (the file pair list processing could be handled (wisely) through a single file pair processing method!!!
            {
                //Define the instances of source and target processed term lists.
                Dictionary<string, ProcessedTermEntry> srcTermList = new Dictionary<string, ProcessedTermEntry>();
                Dictionary<string, ProcessedTermEntry> trgTermList = new Dictionary<string, ProcessedTermEntry>();
                bool interlinguaDictUsed = false;
                bool interlinguaTranslitUsed = false;
                
                if (inputFormat=="preprocessed_terms")
                {
                    if (string.IsNullOrWhiteSpace(inputFile)||!File.Exists(inputFile))
                    {
                        Log.Write("Pre-processed term input file not specified or cannot be found!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
                    {
                        Log.Write("Source and/or target languages not specified!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    PreprocessedTermData ptd = PreprocessedTermData.ReadFromFile(inputFile);
                    foreach(ProcessedTermEntry pte in ptd.srcTerms)
                    {
                        if(!srcTermList.ContainsKey(pte.lowercaceForm))
                        {
                            srcTermList.Add(pte.lowercaceForm,pte);
                        }
                    }
                    foreach(ProcessedTermEntry pte in ptd.trgTerms)
                    {
                        if(!trgTermList.ContainsKey(pte.lowercaceForm))
                        {
                            trgTermList.Add(pte.lowercaceForm,pte);
                        }
                    }
                    srcLang = ptd.srcLang;
                    trgLang = ptd.trgLang;
                    interlinguaDictUsed = ptd.interlinguaDictUsed;
                    interlinguaTranslitUsed = ptd.interlinguaTranslitUsed;
                    
                    Dictionary<string, Dictionary<string, bool>> excDict = null;
                    ReadExceptionDictionary(configuration,srcLang, trgLang,out excDict);
                    
                    Dictionary<string,bool> srcStopWords = null;
                    ReadStopwordList(configuration,srcLang,out srcStopWords);
                    
                    Dictionary<string,bool> trgStopWords = null;
                    ReadStopwordList(configuration,trgLang,out trgStopWords);
                    
                    if (!string.IsNullOrWhiteSpace(outputFile))
                    {
                        List<AlignmentInfoElement> alignment = new List<AlignmentInfoElement>();
                        if (configuration.useMultiThreadedExecution)
                        {
                            alignment = Alignment.AlignPairsMultiThreaded(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
                        }
                        else
                        {
                            alignment = Alignment.AlignPairs(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
                        }
						AlignmentInfoElement.PrintList(outputFormat,outputFile,alignment, configuration.printTopTrgForSrc,null,srcLang,trgLang,collectionId,domainId);
                    }
                }
                else if (inputFormat=="term_list"||inputFormat=="tagged_plaintext")
                {
                    if (string.IsNullOrWhiteSpace(srcInputFile)||!File.Exists(srcInputFile)||string.IsNullOrWhiteSpace(trgInputFile)||!File.Exists(trgInputFile))
                    {
                        Log.Write("Source and/or target files not specified or cannot be found!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
                    {
                        Log.Write("Source and/or target languages not specified!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    
                    Dictionary<string,SimpleTermEntry> srcInitialList = new Dictionary<string, SimpleTermEntry>();
                    Dictionary<string,SimpleTermEntry> trgInitialList = new Dictionary<string, SimpleTermEntry>();
                    
                    if (inputFormat=="tagged_plaintext")
                    {
                        srcInitialList = TermTaggedFileParser.ParseTermTaggedFile(srcInputFile,Encoding.UTF8, configuration.concLen);
                        trgInitialList = TermTaggedFileParser.ParseTermTaggedFile(trgInputFile,Encoding.UTF8, configuration.concLen);
                    }
                    else
                    {
                        srcInitialList = ListFileParser.Parse(srcInputFile,Encoding.UTF8);
                        trgInitialList = ListFileParser.Parse(trgInputFile,Encoding.UTF8);
                    }
					Log.Write ("Unprocessed source terms: "+srcInitialList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
					Log.Write ("Unprocessed target terms: "+trgInitialList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
                    Dictionary<string, Dictionary<string, double>> srcDict = null;
                    Dictionary<string, Dictionary<string, double>> trgDict = null;
                    Dictionary<string, Dictionary<string, double>> srcToTrgDict = null;
                    Dictionary<string, Dictionary<string, double>> trgToSrcDict = null;
    
                    MPAlignerConfigurationTranslEntry srcTranslitConf = null;
                    MPAlignerConfigurationTranslEntry trgTranslitConf = null;
                    MPAlignerConfigurationTranslEntry srcToTrgTranslitConf = null;
                    MPAlignerConfigurationTranslEntry trgToSrcTranslitConf = null;
    
                    interlinguaDictUsed = ReadDictionaries(configuration,srcLang,trgLang, out srcDict, out trgDict, out srcToTrgDict, out trgToSrcDict);
                    interlinguaTranslitUsed = GetTranslitConfig(configuration,srcLang,trgLang,out srcTranslitConf,out trgTranslitConf,out srcToTrgTranslitConf, out trgToSrcTranslitConf);
                    
                    if (srcDict!=null||trgDict!=null)
                    {
                        if (srcTranslitConf!=null && trgTranslitConf!=null)
                        {
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                        else
                        {
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                    }
                    else
                    {
                        if (srcTranslitConf!=null && trgTranslitConf!=null)
                        {
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcToTrgDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgToSrcDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                        else
                        {
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcToTrgDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgToSrcDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                    }
					Log.Write ("Pre-processed source terms: "+srcTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
					Log.Write ("Pre-processed target terms: "+trgTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);

                    ///If pre-processed terms should be saved for future use an output format is created.
					/// This functionality is not available for the file pair list-based processing.
                    if (!string.IsNullOrWhiteSpace(preProcessedTermOutputFile))
                    {
                        List<ProcessedTermEntry> srcTerms = new List<ProcessedTermEntry>(srcTermList.Values);
                        List<ProcessedTermEntry> trgTerms = new List<ProcessedTermEntry>(trgTermList.Values);
                        PreprocessedTermData ptd = new PreprocessedTermData();
                        ptd.interlinguaDictUsed = interlinguaDictUsed;
                        ptd.interlinguaTranslitUsed = interlinguaTranslitUsed;
                        ptd.srcTerms = srcTerms.ToArray();
                        ptd.trgTerms = trgTerms.ToArray();
                        ptd.srcLang = srcLang;
                        ptd.trgLang = trgLang;
                        string outStr = MPFramework.MPFrameworkFunctions.SerializeObjectInstance<PreprocessedTermData>(ptd);
                        File.WriteAllText(preProcessedTermOutputFile,outStr);
                    }
                    
                    Dictionary<string, Dictionary<string, bool>> excDict = null;
                    ReadExceptionDictionary(configuration,srcLang, trgLang,out excDict);
                    
                    Dictionary<string,bool> srcStopWords = null;
                    ReadStopwordList(configuration,srcLang,out srcStopWords);
                    
                    Dictionary<string,bool> trgStopWords = null;
                    ReadStopwordList(configuration,trgLang,out trgStopWords);
                    
                    if (!string.IsNullOrWhiteSpace(outputFile))
                    {
                        List<AlignmentInfoElement> alignment = new List<AlignmentInfoElement>();
                        if (configuration.useMultiThreadedExecution)
                        {
                            alignment = Alignment.AlignPairsMultiThreaded(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
                        }
                        else
                        {
                            alignment = Alignment.AlignPairs(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
                        }
						AlignmentInfoElement.PrintList(outputFormat,outputFile,alignment, configuration.printTopTrgForSrc,null,srcLang,trgLang,collectionId,domainId);
                    }
                }
                else
                {
                    Log.Write ("Input format UNKNOWN or UNDEFINED.",LogLevelType.ERROR,configuration);
                    return;
                }
            }
            else if (method.ToLower () == "singletermpairlist") //Use this method only if filtering of term pairs or some sort of evaluation is necessary!
            {
                //Define the instances of source and target processed term lists.
                List<ProcessedTermEntry> srcTermList = new List<ProcessedTermEntry>();
                List<ProcessedTermEntry> trgTermList = new List<ProcessedTermEntry>();
                bool interlinguaDictUsed = false;
                bool interlinguaTranslitUsed = false;
                if (inputFormat=="preprocessed_terms")
                {
                    if (string.IsNullOrWhiteSpace(inputFile)||!File.Exists(inputFile))
                    {
                        Log.Write("Pre-processed term input file not specified or cannot be found!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
                    {
                        Log.Write("Source and/or target languages not specified!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    PreprocessedTermData ptd = PreprocessedTermData.ReadFromFile(inputFile);
                    srcTermList.AddRange(ptd.srcTerms);
                    trgTermList.AddRange(ptd.trgTerms);
					Log.Write ("Pre-processed source terms: "+srcTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
					Log.Write ("Pre-processed target terms: "+trgTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);

                    srcLang = ptd.srcLang;
                    trgLang = ptd.trgLang;
                    interlinguaDictUsed = ptd.interlinguaDictUsed;
                    interlinguaTranslitUsed = ptd.interlinguaTranslitUsed;
                    
                    Dictionary<string, Dictionary<string, bool>> excDict = null;
                    ReadExceptionDictionary(configuration,srcLang, trgLang,out excDict);
					Log.Write ("Exception dictionary entries: "+excDict.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);

                    Dictionary<string,bool> srcStopWords = null;
                    ReadStopwordList(configuration,srcLang,out srcStopWords);
					Log.Write ("Source language stopwords: "+srcStopWords.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);

                    Dictionary<string,bool> trgStopWords = null;
                    ReadStopwordList(configuration,trgLang,out trgStopWords);
					Log.Write ("Target language stopwords: "+trgStopWords.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);

                    if (!string.IsNullOrWhiteSpace(outputFile))
                    {
                        List<AlignmentInfoElement> alignment = Alignment.AlignListPairs(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
						Log.Write ("Alignment elements after alignment: "+alignment.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
						AlignmentInfoElement.PrintList(outputFormat,outputFile,alignment, configuration.printTopTrgForSrc,null,srcLang,trgLang,collectionId,domainId);
                    }
                }
                else 
                {

                    if (string.IsNullOrWhiteSpace(srcInputFile)||!File.Exists(srcInputFile)||string.IsNullOrWhiteSpace(trgInputFile)||!File.Exists(trgInputFile))
                    {
                        Log.Write("Source and/or target files not specified or cannot be found!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
                    {
                        Log.Write("Source and/or target languages not specified!",LogLevelType.ERROR,configuration);
                        PrintUsage();
                        return;
                    }
                    
                    List<string> srcInitialList = new List<string>();
                    List<string> trgInitialList = new List<string>();
                    srcInitialList = ListFileParser.ParseList(srcInputFile,Encoding.UTF8);
                    trgInitialList = ListFileParser.ParseList(trgInputFile,Encoding.UTF8);
                    if (srcInitialList.Count!=trgInitialList.Count)
                    {
                        Log.Write("Source and target term lists are with different lengths",LogLevelType.ERROR,configuration);
                        throw new ArgumentException("Source and target term lists are with different lengths");
                    }
					Log.Write ("Unprocessed source terms: "+srcInitialList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
					Log.Write ("Unprocessed target terms: "+trgInitialList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
                    Dictionary<string, Dictionary<string, double>> srcDict = null;
                    Dictionary<string, Dictionary<string, double>> trgDict = null;
                    Dictionary<string, Dictionary<string, double>> srcToTrgDict = null;
                    Dictionary<string, Dictionary<string, double>> trgToSrcDict = null;
                    
                    MPAlignerConfigurationTranslEntry srcTranslitConf = null;
                    MPAlignerConfigurationTranslEntry trgTranslitConf = null;
                    MPAlignerConfigurationTranslEntry srcToTrgTranslitConf = null;
                    MPAlignerConfigurationTranslEntry trgToSrcTranslitConf = null;
                    
                    interlinguaDictUsed = ReadDictionaries(configuration,srcLang,trgLang, out srcDict, out trgDict, out srcToTrgDict, out trgToSrcDict);
                    interlinguaTranslitUsed = GetTranslitConfig(configuration,srcLang,trgLang,out srcTranslitConf,out trgTranslitConf,out srcToTrgTranslitConf, out trgToSrcTranslitConf);
                    
                    if (srcDict!=null||trgDict!=null)
                    {
                        if (srcTranslitConf!=null && trgTranslitConf!=null)
                        {
                            srcTermList = ProcessedTermEntry.ProcessTermsList(srcInitialList,srcDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTermsList(trgInitialList,trgDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                        else
                        {
                            srcTermList = ProcessedTermEntry.ProcessTermsList(srcInitialList,srcDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTermsList(trgInitialList,trgDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                    }
                    else
                    {
                        if (srcTranslitConf!=null && trgTranslitConf!=null)
                        {
                            srcTermList = ProcessedTermEntry.ProcessTermsList(srcInitialList,srcToTrgDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTermsList(trgInitialList,trgToSrcDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                        else
                        {
                            srcTermList = ProcessedTermEntry.ProcessTermsList(srcInitialList,srcToTrgDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                            trgTermList = ProcessedTermEntry.ProcessTermsList(trgInitialList,trgToSrcDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        }
                    }
					Log.Write ("Pre-processed source terms: "+srcTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
					Log.Write ("Pre-processed target terms: "+trgTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
                    
                    ///If pre-processed terms should be saved for future use an output format is created.
                    if (!string.IsNullOrWhiteSpace(preProcessedTermOutputFile))
                    {
                        PreprocessedTermData ptd = new PreprocessedTermData();
                        ptd.interlinguaDictUsed = interlinguaDictUsed;
                        ptd.interlinguaTranslitUsed = interlinguaTranslitUsed;
                        ptd.srcTerms = srcTermList.ToArray();
                        ptd.trgTerms = trgTermList.ToArray();
                        ptd.srcLang = srcLang;
                        ptd.trgLang = trgLang;
                        
                        string outStr = MPFramework.MPFrameworkFunctions.SerializeObjectInstance<PreprocessedTermData>(ptd);
                        File.WriteAllText(preProcessedTermOutputFile,outStr);
                    }
                    
                    Dictionary<string, Dictionary<string, bool>> excDict = null;
                    ReadExceptionDictionary(configuration,srcLang, trgLang,out excDict);
                    
                    Dictionary<string,bool> srcStopWords = null;
                    ReadStopwordList(configuration,srcLang,out srcStopWords);
                    
                    Dictionary<string,bool> trgStopWords = null;
                    ReadStopwordList(configuration,trgLang,out trgStopWords);
                    
                    if (!string.IsNullOrWhiteSpace(outputFile))
                    {
                        List<AlignmentInfoElement> alignment = Alignment.AlignListPairs(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
						AlignmentInfoElement.PrintList(outputFormat,outputFile,alignment, configuration.printTopTrgForSrc,null,srcLang,trgLang,collectionId,domainId);
                    }
                }
            }
            else if (method.ToLower () == "eurovoceval")
            {
                if (string.IsNullOrWhiteSpace(inputFile)||!File.Exists(inputFile))
                {
                    Log.Write("Eurovoc input file not specified or cannot be found!",LogLevelType.ERROR,configuration);
                    PrintUsage();
                    return;
                }
                if (string.IsNullOrWhiteSpace(srcLang)||string.IsNullOrWhiteSpace(trgLang))
                {
                    Log.Write("Source or target language not specified!",LogLevelType.ERROR,configuration);
                    PrintUsage();
                    return;
                }
                
                configuration.allowTrimmedAlignments = false;
                //configuration.useMultiThreadedExecution = false;
                configuration.printTopTrgForSrc = true;

                
                string logFile = outputFile+".res.log";
                StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8);
                
                Dictionary<string,List<string>> eurovocDict = ReadEurovocDict(inputFile);;
                //List<string> langList = GetLangsFromConf(configuration);
                
                
                //for(int i = 0;i<langList.Count;i++)
                //{
                    //for(int j = 0;j<langList.Count;j++)
                    //{
                        //if (i==j) continue;
                        //srcLang = langList[i];
                        //trgLang = langList[j];
				Log.Write("Processing pair "+srcLang+"_"+trgLang,LogLevelType.LIMITED_OUTPUT,configuration);
                if (Char.IsDigit(outputFile[outputFile.Length-1])) outputFile = outputFile.Substring(0,outputFile.Length-1);
                string alignmentOutputFile = outputFile+"."+srcLang+"_"+trgLang+".align.txt";
                if (File.Exists(alignmentOutputFile))
                {
					Log.Write("Pair "+srcLang+"_"+trgLang+" already processed! Evaluating...",LogLevelType.LIMITED_OUTPUT,configuration);
                    List<StringComparisonElement> terms = new List<StringComparisonElement>();
                    StreamReader sr = new StreamReader(alignmentOutputFile,Encoding.UTF8);
                    char[] sep = {'\t'};
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.CurrencyDecimalSeparator=".";
                    nfi.NumberDecimalSeparator=".";
                    nfi.PercentDecimalSeparator=".";
                    while(!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().Trim();
                        string[] arr = line.Split(sep,StringSplitOptions.None);
                        if (arr.Length>=3)
                        {
                            StringComparisonElement sce = new StringComparisonElement();
                            sce.src = arr[0];
                            sce.trg = arr[1];
                            sce.similarity = Convert.ToDouble(arr[2],nfi);
                            terms.Add(sce);
                        }
                    }
                    sr.Close();
                    terms.Sort();

                    List<double> scores = new List<double>();
                    double tmp = 0;
                    while (tmp<=1)
                    {
                        scores.Add(tmp);
                        tmp+=0.01;
                    }
                    List<double> correct = new List<double>();
                    for(int t=0;t<scores.Count;t++)
                    {
                        correct.Add(0);
                    }
                    List<double> total = new List<double>();
                    for(int t=0;t<scores.Count;t++)
                    {
                        total.Add(0);
                    }

                    int totalForRec = 0;
                    Dictionary<string,Dictionary<string,bool>> goldList = new Dictionary<string, Dictionary<string, bool>>();
                    for (int s = 0;s<eurovocDict[srcLang].Count;s++)
                    {
                        if (!eurovocDict[srcLang][s].Contains("(under translation)")&&!eurovocDict[trgLang][s].Contains("(under translation)"))
                        {
                            totalForRec++;
                            if (!goldList.ContainsKey(eurovocDict[srcLang][s].ToLower())) goldList.Add(eurovocDict[srcLang][s].ToLower(), new Dictionary<string,bool>());
                            if (!goldList[eurovocDict[srcLang][s].ToLower()].ContainsKey(eurovocDict[trgLang][s].ToLower())) goldList[eurovocDict[srcLang][s].ToLower()].Add(eurovocDict[trgLang][s].ToLower(),true);
                        }
                    }
                    
                    string previousSrc = null;
                    foreach(StringComparisonElement sce in terms)
                    {
                        string currSrc = sce.src;
                        if (previousSrc!=currSrc.ToLower())
                        {
                            string src = sce.src.ToLower();
                            string trg = sce.trg.ToLower();
                            double alignScore = sce.similarity;
                            bool corr = false;
                            if (goldList.ContainsKey(src)&&goldList[src].ContainsKey(trg)) corr = true;
                            for (int s =0;s<scores.Count;s++)
                            {
                                if (scores[s]<=alignScore)
                                {
                                    if (corr) correct[s]++;
                                    total[s]++;
                                }
                            }
                            previousSrc = currSrc.ToLower();
                        }
                    }
                    
                    for(int s=0;s<scores.Count;s++)
                    {
                        double corr = correct[s];
                        double tot = total[s];
                        double totCorr = totalForRec;
                        double prec = corr/tot*100;
                        double rec = corr/totCorr*100;
                        double f1 = prec*rec*2/(prec+rec);
                        sw.WriteLine(srcLang+"\t"+trgLang+"\t"+scores[s].ToString()+"\t"+corr.ToString()+"\t"+tot.ToString()+"\t"+totCorr.ToString()+"\t"+prec.ToString()+"\t"+rec.ToString()+"\t"+f1.ToString());
                    }
                    sw.Flush();
                    //}
                    //}
                    sw.Close();
                    //continue;
                    return;
                }
                string preprocessedOutputFile = outputFile+"."+srcLang+"_"+trgLang+".prep.txt";
                Dictionary<string,SimpleTermEntry> srcInitialList = StringListToDict(eurovocDict[srcLang]);
                Dictionary<string,SimpleTermEntry> trgInitialList = StringListToDict(eurovocDict[trgLang]);
                
				Log.Write ("Unprocessed source terms: "+srcInitialList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
				Log.Write ("Unprocessed target terms: "+trgInitialList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
                Dictionary<string, Dictionary<string, double>> srcDict = null;
                Dictionary<string, Dictionary<string, double>> trgDict = null;
                Dictionary<string, Dictionary<string, double>> srcToTrgDict = null;
                Dictionary<string, Dictionary<string, double>> trgToSrcDict = null;

                MPAlignerConfigurationTranslEntry srcTranslitConf = null;
                MPAlignerConfigurationTranslEntry trgTranslitConf = null;
                MPAlignerConfigurationTranslEntry srcToTrgTranslitConf = null;
                MPAlignerConfigurationTranslEntry trgToSrcTranslitConf = null;

                bool interlinguaDictUsed = ReadDictionaries(configuration,srcLang,trgLang, out srcDict, out trgDict, out srcToTrgDict, out trgToSrcDict);
                bool interlinguaTranslitUsed = GetTranslitConfig(configuration,srcLang,trgLang,out srcTranslitConf,out trgTranslitConf,out srcToTrgTranslitConf, out trgToSrcTranslitConf);
                
                Dictionary<string,ProcessedTermEntry> srcTermList = new Dictionary<string,ProcessedTermEntry>();
                Dictionary<string,ProcessedTermEntry> trgTermList = new Dictionary<string,ProcessedTermEntry>();

                if (File.Exists(preprocessedOutputFile))
                {
                    Log.Write("Preprocessed term data found! Reading pre-processed data to save time!", LogLevelType.WARNING,configuration);
                    PreprocessedTermData ptd1 = PreprocessedTermData.ReadFromFile(preprocessedOutputFile);
                    interlinguaDictUsed = ptd1.interlinguaDictUsed;
                    interlinguaTranslitUsed = ptd1.interlinguaTranslitUsed;
                    foreach(ProcessedTermEntry pte in ptd1.srcTerms)
                    {
                        if (!srcTermList.ContainsKey(pte.lowercaceForm))
                        {
                            srcTermList.Add(pte.lowercaceForm,pte);
                        }
                    }
                    foreach(ProcessedTermEntry pte in ptd1.trgTerms)
                    {
                        if (!trgTermList.ContainsKey(pte.lowercaceForm))
                        {
                            trgTermList.Add(pte.lowercaceForm,pte);
                        }
                    }
                }
                else if (interlinguaDictUsed&&interlinguaTranslitUsed)
                {
                    string dir = Path.GetDirectoryName(preprocessedOutputFile);
                    if (!dir.EndsWith(Path.DirectorySeparatorChar.ToString())) dir+=Path.DirectorySeparatorChar.ToString();
                    string prepSrcToTrgFile = dir+"eurovoc_preprocessed_"+srcLang+"_en.xml";
                    string prepTrgToSrcFile = dir+"eurovoc_preprocessed_"+trgLang+"_en.xml";
                    if (File.Exists(prepSrcToTrgFile))
                    {
						Log.Write ("Reading processed term list: eurovoc_preprocessed_"+srcLang+"_en.xml",LogLevelType.LIMITED_OUTPUT,configuration);
                        srcTermList = ProcessedTermEntry.ReadFromFile(prepSrcToTrgFile);
                    }
                    if (File.Exists(prepTrgToSrcFile))
                    {
						Log.Write ("Reading processed term list: eurovoc_preprocessed_"+trgLang+"_en.xml",LogLevelType.LIMITED_OUTPUT,configuration);
                        trgTermList = ProcessedTermEntry.ReadFromFile(prepTrgToSrcFile);
                    }
                }
                else if (!interlinguaDictUsed&&!interlinguaTranslitUsed)
                {
                    string dir = Path.GetDirectoryName(preprocessedOutputFile);
                    if (!dir.EndsWith(Path.DirectorySeparatorChar.ToString())) dir+=Path.DirectorySeparatorChar.ToString();
                    string prepSrcToTrgFile = dir+"eurovoc_preprocessed_"+srcLang+"_"+trgLang+".xml";
                    string prepTrgToSrcFile = dir+"eurovoc_preprocessed_"+trgLang+"_"+srcLang+".xml";
                    if (File.Exists(prepSrcToTrgFile))
                    {
						Log.Write ("Reading processed term list: eurovoc_preprocessed_"+srcLang+"_"+trgLang+".xml",LogLevelType.LIMITED_OUTPUT,configuration);
                        srcTermList = ProcessedTermEntry.ReadFromFile(prepSrcToTrgFile);
                    }
                    if (File.Exists(prepTrgToSrcFile))
                    {
						Log.Write ("Reading processed term list: eurovoc_preprocessed_"+trgLang+"_"+srcLang+".xml",LogLevelType.LIMITED_OUTPUT,configuration);
                        trgTermList = ProcessedTermEntry.ReadFromFile(prepTrgToSrcFile);
                    }
                }
                
                if (srcDict!=null||trgDict!=null)
                {
                    if (srcTranslitConf!=null && trgTranslitConf!=null)
                    {
                        if (srcTermList.Count<1)
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        if (trgTermList.Count<1)
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                    }
                    else
                    {
                        if (srcTermList.Count<1)
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        if (trgTermList.Count<1)
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                    }
                }
                else
                {
                    if (srcTranslitConf!=null && trgTranslitConf!=null)
                    {
                        if (srcTermList.Count<1)
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcToTrgDict,srcLang,srcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        if (trgTermList.Count<1)
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgToSrcDict,trgLang,trgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                    }
                    else
                    {
                        if (srcTermList.Count<1)
                            srcTermList = ProcessedTermEntry.ProcessTerms(srcInitialList,srcToTrgDict,srcLang,srcToTrgTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                        if (trgTermList.Count<1)
                            trgTermList = ProcessedTermEntry.ProcessTerms(trgInitialList,trgToSrcDict,trgLang,trgToSrcTranslitConf, configuration.mosesPath, tempTranslitFile,configuration.alignmentThreads);
                    }
                }
				Log.Write ("Pre-processed source terms: "+srcTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
				Log.Write ("Pre-processed target terms: "+trgTermList.Count.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);

                ///If pre-processed terms should be saved for future use an output format is created.
                
                List<ProcessedTermEntry> srcTerms = new List<ProcessedTermEntry>(srcTermList.Values);
                List<ProcessedTermEntry> trgTerms = new List<ProcessedTermEntry>(trgTermList.Values);
                PreprocessedTermData ptd = new PreprocessedTermData();
                ptd.interlinguaDictUsed = interlinguaDictUsed;
                ptd.interlinguaTranslitUsed = interlinguaTranslitUsed;
                ptd.srcTerms = srcTerms.ToArray();
                ptd.trgTerms = trgTerms.ToArray();
                ptd.srcLang = srcLang;
                ptd.trgLang = trgLang;
                
                string outStr = MPFramework.MPFrameworkFunctions.SerializeObjectInstance<PreprocessedTermData>(ptd);
                File.WriteAllText(preprocessedOutputFile,outStr);
                
                Dictionary<string, Dictionary<string, bool>> excDict = null;
                ReadExceptionDictionary(configuration,srcLang, trgLang,out excDict);
                
                Dictionary<string,bool> srcStopWords = null;
                ReadStopwordList(configuration,srcLang,out srcStopWords);
                
                Dictionary<string,bool> trgStopWords = null;
                ReadStopwordList(configuration,trgLang,out trgStopWords);

                //Need to pre-set the alignment thresholds, otherwise these will be overriden by defaults.
                MPAlignerConfigurationLangPairEntry lpeConf = ReadLangPairConfig (srcLang, trgLang, configuration);
                
                List<AlignmentInfoElement> alignment = new List<AlignmentInfoElement>();
                if (configuration.useMultiThreadedExecution)
                {
                    alignment = Alignment.AlignPairsMultiThreaded(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
                }
                else
                {
                    alignment = Alignment.AlignPairs(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
                }
                //Multi-threaded execution is not stable at the moment...
                //List<AlignmentInfoElement> alignment = Alignment.AlignPairsMultiThreaded(configuration,srcTermList,trgTermList,interlinguaDictUsed,interlinguaTranslitUsed,srcLang,trgLang, srcInputFile, trgInputFile, excDict, srcStopWords, trgStopWords);
				AlignmentInfoElement.PrintList(outputFormat, alignmentOutputFile, alignment, configuration.printTopTrgForSrc, lpeConf,srcLang,trgLang,collectionId,domainId);
                {
                    List<double> scores = new List<double>();
                    double tmp = 0;
                    while (tmp<=1)
                    {
                        scores.Add(tmp);
                        tmp+=0.01;
                    }
                    List<double> correct = new List<double>();
                    for(int t=0;t<scores.Count;t++)
                    {
                        correct.Add(0);
                    }
                    List<double> total = new List<double>();
                    for(int t=0;t<scores.Count;t++)
                    {
                        total.Add(0);
                    }
                    
                    int totalForRec = 0;
                    Dictionary<string,Dictionary<string,bool>> goldList = new Dictionary<string, Dictionary<string, bool>>();
                    for (int s = 0;s<eurovocDict[srcLang].Count;s++)
                    {
                            if (!eurovocDict[srcLang][s].ToLower().Contains("(under translation)")&&!eurovocDict[trgLang][s].ToLower().Contains("(under translation)"))
                        {
                            totalForRec++;
                                if (!goldList.ContainsKey(eurovocDict[srcLang][s].ToLower())) goldList.Add(eurovocDict[srcLang][s].ToLower(), new Dictionary<string,bool>());
                                if (!goldList[eurovocDict[srcLang][s].ToLower()].ContainsKey(eurovocDict[trgLang][s].ToLower())) goldList[eurovocDict[srcLang][s].ToLower()].Add(eurovocDict[trgLang][s].ToLower(),true);
                        }
                    }
                    
                    string previousSrc = null;
                    alignment.Sort();
                    foreach(AlignmentInfoElement aie in alignment)
                    {
                        string currSrc = AlignmentInfoElement.GetStrFromEntry(aie.srcEntry.surfaceFormWords, aie.minSrcId, aie.maxSrcId);
                        if (previousSrc!=currSrc.ToLower())
                        {
                            string src = aie.srcEntry.surfaceForm.ToLower();
                            string trg = aie.trgEntry.surfaceForm.ToLower();
                            double alignScore = aie.alignmentScore;
                            bool corr = false;
                            if (goldList.ContainsKey(src)&&goldList[src].ContainsKey(trg)) corr = true;
                            for (int s =0;s<scores.Count;s++)
                            {
                                if (scores[s]<=alignScore)
                                {
                                    if (corr) correct[s]++;
                                    total[s]++;
                                }
                            }
                            previousSrc = currSrc.ToLower();
                        }
                    }
                    
                    for(int s=0;s<scores.Count;s++)
                    {
                        double corr = correct[s];
                        double tot = total[s];
                        double totCorr = totalForRec;
                        double prec = corr/tot*100;
                        double rec = corr/totCorr*100;
                        double f1 = prec*rec*2/(prec+rec);
                        sw.WriteLine(srcLang+"\t"+trgLang+"\t"+scores[s].ToString()+"\t"+corr.ToString()+"\t"+tot.ToString()+"\t"+totCorr.ToString()+"\t"+prec.ToString()+"\t"+rec.ToString()+"\t"+f1.ToString());
                    }
                    sw.Flush();
                    //}
                    //}
                    sw.Close();
                }
            }
            if (File.Exists(tempTranslitFile)) File.Delete(tempTranslitFile);
			if (consolidateResults) {
				Log.Write ("Consolidating aligned term pairs with a threshold of: "+consolidationThreshold.ToString(),LogLevelType.LIMITED_OUTPUT,configuration);
				//In the case if -ct (consolidation threshold) was defined and the output format has been ref_tabsep, the consolidation of results is perfomed.
				ConsolidationElement.ConsolidateRefTabsep(outputFile, consolidatedOutputFile,consolidationThreshold);
			}
		}

		/// <summary>
		/// Reads the language pair specific configuration - term alignment thresholds.
		/// </summary>
		/// <returns>The language pair configuration.</returns>
		/// <param name="srcLang">Source language.</param>
		/// <param name="trgLang">Target language.</param>
		/// <param name="configuration">Configuration.</param>
		static MPAlignerConfigurationLangPairEntry ReadLangPairConfig (string srcLang, string trgLang, MPAlignerConfiguration configuration)
		{
			string langKey = srcLang + "_" + trgLang;
			MPAlignerConfigurationLangPairEntry lpeConf = new MPAlignerConfigurationLangPairEntry ();
			if (configuration.langPairEntryDict.ContainsKey (langKey)) {
				lpeConf = configuration.langPairEntryDict [langKey];
			}
			else {
				lpeConf.srcLang = srcLang;
				lpeConf.trgLang = trgLang;
				lpeConf.finalAlignmentThr = 0.6;
				lpeConf.printThr = 0.6;//A default value of 0.6 is usually the lowest value that is still reasonable for the cognate-based overlaps, therefore, wethe default to 0.6. However, for different applications the threshold could be raised even higher.
				configuration.langPairEntryDict.Add (langKey, lpeConf);
			}
			return lpeConf;
		}

        public static Dictionary<string, SimpleTermEntry> StringListToDict (List<string> list)
        {
            Dictionary<string, SimpleTermEntry> res = new Dictionary<string, SimpleTermEntry>();
            foreach(string word in list)
            {
				if (!res.ContainsKey (word.ToLower ()))
					res.Add (word.ToLower (), new SimpleTermEntry (word, "", "", "", "", "", 1, 1));
                else res[word].count++;
            }
            return res;
        }

        public static List<string> GetLangsFromConf (MPAlignerConfiguration configuration)
        {
            List<string> res = new List<string>();
            foreach(string lang in configuration.stopWordListEntryDict.Keys)
            {
                res.Add(lang);
            }
            return res;
        }

        public static Dictionary<string, List<string>> ReadEurovocDict (string inputFile)
        {
			Log.Write ("Reading EuroVoc dictionary from file "+inputFile+".",LogLevelType.LIMITED_OUTPUT);
            Dictionary<string,List<string>> res = new Dictionary<string, List<string>>();
            Dictionary<int,string> indexDict = new Dictionary<int, string>();
            char[] sep = {'\t'};
            StreamReader sr = new StreamReader(inputFile, Encoding.UTF8);
            int count = -1;
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] arr = line.Split(sep, StringSplitOptions.None);
                    if (indexDict.Count>0)
                    {
                        if (arr.Length==count)
                        {
                            for(int i=0;i<arr.Length;i++)
                            {
                                res[indexDict[i]].Add(arr[i]);
                            }
                        }
                    }
                    else
                    {
                        count=arr.Length;
                        for(int i=0;i<arr.Length;i++)
                        {
                            string langId = arr[i].ToLower();
                            if (!res.ContainsKey(langId)) res.Add(langId, new List<string>());
                            indexDict.Add(i,langId);
                        }
                    }
                }
            }
            sr.Close();
			Log.Write ("EuroVoc dictionary for "+res.Count.ToString()+" languages read.",LogLevelType.LIMITED_OUTPUT);
            
            return res;
        }

        public static void ReadExceptionDictionary (MPAlignerConfiguration configuration, string srcLang, string trgLang, out Dictionary<string, Dictionary<string, bool>> srcToTrgExcDict)
        {
            srcToTrgExcDict = new Dictionary<string, Dictionary<string, bool>> ();
            string langKey = srcLang + "_" + trgLang;
			Log.Write ("Searching for an exception dictionary for the laguage pair "+langKey+".",LogLevelType.LIMITED_OUTPUT,configuration);
            if (configuration.excDictEntryDict.ContainsKey (langKey)&&configuration.excDictEntryDict[langKey].use) {
                try
                {
                    srcToTrgExcDict = ExceptionDictionaryParser.ParseExceptionDictionary(configuration.excDictEntryDict[langKey]);
					Log.Write("Exception dictionary for the laguage pair "+langKey+" loaded: " + configuration.excDictEntryDict[langKey].path, LogLevelType.LIMITED_OUTPUT,configuration);
                }
                catch{
                    Log.Write ("The exception dictionary for the laguage pair "+langKey+" was not found or is corrupted.",LogLevelType.WARNING,configuration);
                }
            }
            else
            {
                Log.Write ("The exception dictionary for the laguage pair "+langKey+" was not found or is disabled.",LogLevelType.WARNING,configuration);
            }
            return;
        }

        public static void ReadStopwordList (MPAlignerConfiguration configuration, string lang, out Dictionary<string, bool> stopwordDict)
        {
			Log.Write ("Searching for a stopword list for laguage "+lang+".",LogLevelType.LIMITED_OUTPUT,configuration);
            stopwordDict = new Dictionary<string, bool> ();
            if (configuration.stopWordListEntryDict.ContainsKey (lang) && configuration.stopWordListEntryDict[lang].use) {
                try{
                    stopwordDict = StopwordListParser.ParseStopwordList(configuration.stopWordListEntryDict[lang]);
					Log.Write("Stopword list for language "+lang+" loaded: " + configuration.stopWordListEntryDict[lang].path, LogLevelType.LIMITED_OUTPUT,configuration);
                }
                catch{
                    Log.Write ("Stopword list for laguage "+lang+" was not found or is corrupted.",LogLevelType.WARNING,configuration);
                }
            }
            else
            {
                Log.Write ("Stopword list for laguage "+lang+" was not found or is disabled.",LogLevelType.WARNING,configuration);
            }
            return;
        }

        static Dictionary<string, Dictionary<string, double>> GetInverseDictionary (Dictionary<string, Dictionary<string, double>> dict, MPAlignerConfigurationDictEntry cde)
        {
            if (dict == null)
                return null;
            Dictionary<string, Dictionary<string, double>> res = new Dictionary<string, Dictionary<string, double>> ();
            foreach (string src in dict.Keys) {
                foreach(string trg in dict[src].Keys)
                {
                    if (!res.ContainsKey(trg)) res.Add(trg,new Dictionary<string, double>());
                    if (!res[trg].ContainsKey(src)) res[trg].Add(src,dict[src][trg]);
                }
            }
            return res;
        }
        
        public static bool ReadDictionaries(MPAlignerConfiguration configuration, string srcLang, string trgLang,out Dictionary<string, Dictionary<string, double>> srcDict, out Dictionary<string, Dictionary<string, double>> trgDict, out Dictionary<string, Dictionary<string, double>> srcToTrgDict, out Dictionary<string, Dictionary<string, double>> trgToSrcDict)
        {
			Log.Write ("Searching for and reading dictionaries.",LogLevelType.LIMITED_OUTPUT,configuration);
            srcDict = null;
            trgDict = null;
            srcToTrgDict = null;
            trgToSrcDict = null;
            string srcLangKey = srcLang+"_en";
            string trgLangKey = trgLang+"_en";
            string langKey = srcLang+"_"+trgLang;
            string langKey2 = trgLang+"_"+srcLang;
            //Read dictionaries. If reading fails, log a warning and continue.
            //At first we check if the EN interlingua should be used.
            if (configuration.forceEnDictInterlingua && configuration.dictConfEntryDict.ContainsKey(srcLangKey)&&configuration.dictConfEntryDict.ContainsKey(trgLangKey))
            {
                if (configuration.dictConfEntryDict[srcLangKey].use && configuration.dictConfEntryDict[trgLangKey].use)
                {
                    try{
                        srcDict = ProbabilisticDictionaryParser.ParseDictionary(configuration.dictConfEntryDict[srcLangKey]);
                        trgDict = ProbabilisticDictionaryParser.ParseDictionary(configuration.dictConfEntryDict[trgLangKey]);
                        ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[srcLangKey],srcDict);
                        ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[trgLangKey],trgDict);
						Log.Write (srcLangKey+" dictionary with "+srcDict.Count.ToString()+" "+srcLang+" entries loaded: "+configuration.dictConfEntryDict[srcLangKey].path, LogLevelType.LIMITED_OUTPUT,configuration);
						Log.Write (trgLangKey+" dictionary with "+trgDict.Count.ToString()+" "+trgLang+" entries loaded: "+configuration.dictConfEntryDict[trgLangKey].path, LogLevelType.LIMITED_OUTPUT,configuration);
                        return true;//Interlingua dictionary used.
                    }
                    catch{
                        srcDict = null;
                        trgDict = null;
                        Log.Write ("Cannot force EN interlingua dictionary usage for the pair "+langKey+" as one of the interlingua dictionaries may be missing or corrupt!",LogLevelType.WARNING,configuration);
                        Log.Write ("Will try fallback to direct dictionary without the EN interlingua.",LogLevelType.WARNING,configuration);
                    }
                }
                else
                {
                    Log.Write ("Cannot force EN interlingua dictionary usage for the pair "+langKey+" as at least one of the interlingua dictionaries is disabled!",LogLevelType.WARNING,configuration);
                    Log.Write ("Will try fallback to direct dictionary without the EN interlingua.",LogLevelType.WARNING,configuration);
                }
            }
            //If EN interlingua should not be used or one of the interlingua dictionaries is missing, try loading the direct dictionary.
            if (configuration.dictConfEntryDict.ContainsKey(langKey)&&configuration.dictConfEntryDict[langKey].use)
            {
                srcDict = null;
                trgDict = null;
                try{
                    srcToTrgDict = ProbabilisticDictionaryParser.ParseDictionary(configuration.dictConfEntryDict[langKey]);
                    ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[langKey],srcToTrgDict);
                    if (configuration.dictConfEntryDict.ContainsKey(langKey2)&&configuration.dictConfEntryDict[langKey2].use)
                    {
                        try
                        {
                            trgToSrcDict = ProbabilisticDictionaryParser.ParseDictionary(configuration.dictConfEntryDict[langKey2]);
                            ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[langKey2],trgToSrcDict);
							Log.Write (langKey + " dictionary with "+srcToTrgDict.Count.ToString()+" "+srcLang+" entries loaded: "+ configuration.dictConfEntryDict[langKey].path,LogLevelType.LIMITED_OUTPUT,configuration);
							Log.Write (langKey2 + " dictionary with "+trgToSrcDict.Count.ToString()+" "+trgLang+" entries loaded: "+ configuration.dictConfEntryDict[langKey2].path,LogLevelType.LIMITED_OUTPUT,configuration);
                            return false;
                        }
                        catch
                        {
                            Log.Write ("Cannot read the dictionary for the pair "+langKey2+"! The dictionary may be missing or corrupt! The "+langKey+" dictionary will be inverted.",LogLevelType.WARNING,configuration);
                            trgToSrcDict = GetInverseDictionary(srcToTrgDict, configuration.dictConfEntryDict[langKey]);
                            ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[langKey],trgToSrcDict);
							Log.Write (langKey + " dictionary with "+srcToTrgDict.Count.ToString()+" "+srcLang+" entries loaded: "+ configuration.dictConfEntryDict[langKey].path,LogLevelType.LIMITED_OUTPUT,configuration);
							Log.Write (langKey2 + " dictionary with "+trgToSrcDict.Count.ToString()+" "+trgLang+" entries loaded (inverse of): "+ configuration.dictConfEntryDict[langKey].path,LogLevelType.LIMITED_OUTPUT,configuration);
                            return false;
                        }
                    }
                    else
                    {
                        Log.Write ("For the pair "+langKey2+" the inverted "+langKey+" dictionary will be used.",LogLevelType.WARNING,configuration);
                        trgToSrcDict = GetInverseDictionary(srcToTrgDict, configuration.dictConfEntryDict[langKey]);
                        ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[langKey],trgToSrcDict);
						Log.Write (langKey + " dictionary with "+srcToTrgDict.Count.ToString()+" "+srcLang+" entries loaded: "+ configuration.dictConfEntryDict[langKey].path,LogLevelType.LIMITED_OUTPUT,configuration);
						Log.Write (langKey2 + " dictionary with "+trgToSrcDict.Count.ToString()+" "+trgLang+" entries loaded (inverse of): "+ configuration.dictConfEntryDict[langKey].path,LogLevelType.LIMITED_OUTPUT,configuration);
                        return false;
                    }
                }
                catch{ //If a dictionary for a language pair is not given (nor is interlingua usage specified, the system will not use a dictionary at all).
                    srcToTrgDict = null;
                    trgToSrcDict = null;
                    Log.Write ("Cannot read the dictionary for the pair "+langKey+"! The dictionary may be missing or corrupt! The system will try to fall back to the inverse dictionary!",LogLevelType.WARNING,configuration);
                }
            }
            if (configuration.dictConfEntryDict.ContainsKey(langKey2)&&configuration.dictConfEntryDict[langKey2].use)
            {
                Log.Write ("Direct dictionary for "+langKey+" missing or disabled. The "+langKey2+" will be used instead.",LogLevelType.WARNING,configuration);
                srcDict = null;
                trgDict = null;
                try
                {
                    trgToSrcDict = ProbabilisticDictionaryParser.ParseDictionary(configuration.dictConfEntryDict[langKey2]);
                    srcToTrgDict = GetInverseDictionary(trgToSrcDict, configuration.dictConfEntryDict[langKey2]);
                    ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[langKey2],srcToTrgDict);
                    ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[langKey2],trgToSrcDict);
					Log.Write (langKey + " dictionary with "+srcToTrgDict.Count.ToString()+" "+srcLang+" entries loaded: " + configuration.dictConfEntryDict[langKey2].path, LogLevelType.LIMITED_OUTPUT,configuration);
					Log.Write (langKey2 + " dictionary with "+trgToSrcDict.Count.ToString()+" "+trgLang+" entries loaded (inverse of): " + configuration.dictConfEntryDict[langKey2].path, LogLevelType.LIMITED_OUTPUT,configuration);
                    return false;
                }
                catch
                {
                    Log.Write ("Cannot read the dictionary for the pair "+langKey2+"! The dictionary may be disabled, missing or corrupt. The system will try to fall back to interlingua dictionaries!",LogLevelType.WARNING,configuration);
                }
            }
            
            bool usingInterlingua = false;
            if (configuration.dictConfEntryDict.ContainsKey(srcLangKey)&&configuration.dictConfEntryDict[srcLangKey].use)
            {
                try{
                    srcDict = ProbabilisticDictionaryParser.ParseDictionary(configuration.dictConfEntryDict[srcLangKey]);
                    ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[srcLangKey],srcDict);
					Log.Write (srcLangKey+" dictionary with "+srcDict.Count.ToString()+" "+srcLang+" entries loaded: " + configuration.dictConfEntryDict[srcLangKey].path, LogLevelType.LIMITED_OUTPUT,configuration);
                    //Interlingua dictionary used.
                    usingInterlingua=true;
                }
                catch{
                    srcDict = null;
                    Log.Write ("Source-to-EN dictionary is missing or corrupt!",LogLevelType.WARNING,configuration);
                }
            }
            
            if (configuration.dictConfEntryDict.ContainsKey(trgLangKey)&&configuration.dictConfEntryDict[trgLangKey].use)
            {
                try{
                    trgDict = ProbabilisticDictionaryParser.ParseDictionary(configuration.dictConfEntryDict[trgLangKey]);
                    ProbabilisticDictionaryParser.FilterTopEquivalents(configuration.dictConfEntryDict[trgLangKey],trgDict);
					Log.Write (trgLangKey+" dictionary with "+trgDict.Count.ToString()+" "+trgLang+" entries loaded: " + configuration.dictConfEntryDict[trgLangKey].path, LogLevelType.LIMITED_OUTPUT,configuration);
                    //Interlingua dictionary used.
                    usingInterlingua=true;
                }
                catch{
                    trgDict = null;
                    Log.Write ("Target-to-EN dictionary is missing or corrupt!",LogLevelType.WARNING,configuration);
                }
            }
            
            if (usingInterlingua)
            {
                return true;
            }
            
            else //If the direct dictionary does not exist, log a warning and continue.
            {
                Log.Write ("At least one of the EN interlingua dictionaries is missing!",LogLevelType.WARNING,configuration);
            }
            Log.Write ("Dictionaries for the pair "+langKey+" were not found or loaded. The system will be executed without a dictionary!",LogLevelType.WARNING,configuration);
            srcDict = null;
            trgDict = null;
            srcToTrgDict = null;
            trgToSrcDict = null;
            return false;//Interlingua dictionary not used.
        }
        
        public static bool GetTranslitConfig(MPAlignerConfiguration configuration, string srcLang, string trgLang, out MPAlignerConfigurationTranslEntry srcTranslitConf, out MPAlignerConfigurationTranslEntry trgTranslitConf, out MPAlignerConfigurationTranslEntry srcToTrgTranslitConf, out MPAlignerConfigurationTranslEntry trgToSrcTranslitConf)
        {
			Log.Write ("Searching for transliteration configurations.",LogLevelType.LIMITED_OUTPUT,configuration);
            srcTranslitConf = null;
            trgTranslitConf = null;
            srcToTrgTranslitConf = null;
            trgToSrcTranslitConf = null;
            string srcLangKey = srcLang+"_en";
            string trgLangKey = trgLang+"_en";
            string langKey = srcLang+"_"+trgLang;
            string langKey2 = trgLang+"_"+srcLang;
            
            //Define transliteration directions and whether or not to use EN as interlingua.
            if (configuration.forceEnTranslitInterlingua && configuration.translConfEntryDict.ContainsKey(srcLangKey)&&configuration.translConfEntryDict.ContainsKey(trgLangKey) && configuration.translConfEntryDict[srcLangKey].use && configuration.translConfEntryDict[trgLangKey].use)
            {
                srcTranslitConf= configuration.translConfEntryDict[srcLangKey];
                trgTranslitConf= configuration.translConfEntryDict[trgLangKey];
				Log.Write ("EN interlingua transliteration loaded for language "+srcLang+": "+ configuration.translConfEntryDict[srcLangKey].mosesIniPath, LogLevelType.LIMITED_OUTPUT,configuration);
				Log.Write ("EN interlingua transliteration loaded for language "+trgLang+": "+ configuration.translConfEntryDict[trgLangKey].mosesIniPath, LogLevelType.LIMITED_OUTPUT,configuration);
                return true;
            }else if (configuration.forceEnTranslitInterlingua){
                Log.Write ("Cannot force EN interlingua transliteration for the pair "+langKey+" as at least one of the interlingua transliteration configurations is disabled or missing!",LogLevelType.WARNING,configuration);
                Log.Write ("Will try falling back to direct transliteration without the EN interlingua.",LogLevelType.WARNING,configuration);
            }
            //If EN interlingua should not be used or one of the interlingua transliteration configurations is missing, try the direct transliteration.
            bool foundAtLeastOne = false;
            if (configuration.translConfEntryDict.ContainsKey(langKey) && configuration.translConfEntryDict[langKey].use)
            {
                srcToTrgTranslitConf = configuration.translConfEntryDict[langKey];
				Log.Write ("Transliteration loaded for language "+srcLang+" into language "+trgLang+": "+ configuration.translConfEntryDict[langKey].mosesIniPath, LogLevelType.LIMITED_OUTPUT,configuration);
                foundAtLeastOne = true;
            }
            else //If the direct dictionary does not exist, log a warning and continue.
            {
                Log.Write ("Direct transliteration for the pair "+langKey+" was not found.",LogLevelType.WARNING,configuration);
            }
            
            //If EN interlingua should not be used or one of the interlingua transliteration configurations is missing, try the direct transliteration also in a reverse direction.
            if (configuration.translConfEntryDict.ContainsKey(langKey2) && configuration.translConfEntryDict[langKey2].use)
            {
                trgToSrcTranslitConf = configuration.translConfEntryDict[langKey2];
				Log.Write ("Transliteration loaded for language "+trgLang+" into language "+srcLang+": "+ configuration.translConfEntryDict[langKey2].mosesIniPath, LogLevelType.LIMITED_OUTPUT,configuration);
                foundAtLeastOne = true;
            }
            else //If the direct dictionary does not exist, log a warning and continue.
            {
                Log.Write ("Direct transliteration for the pair "+langKey2+" was not found.",LogLevelType.WARNING,configuration);
            }
            
            if (!foundAtLeastOne)
            {
                Log.Write ("Direct transliteration for the pairs "+langKey+" nor "+langKey2+" were not found. Will try falling back to interlingua transliteration.",LogLevelType.WARNING,configuration);
                bool interlinguaTranslitLoaded = false;
                if (configuration.translConfEntryDict.ContainsKey(srcLangKey) && configuration.translConfEntryDict[srcLangKey].use)
                {
                    srcTranslitConf= configuration.translConfEntryDict[srcLangKey];
                    interlinguaTranslitLoaded = true;
					Log.Write ("EN interlingua transliteration loaded for language "+srcLang+": "+ configuration.translConfEntryDict[srcLangKey].mosesIniPath,LogLevelType.LIMITED_OUTPUT,configuration);
                }
                if (configuration.translConfEntryDict.ContainsKey(trgLangKey) && configuration.translConfEntryDict[trgLangKey].use)
                {
                    trgTranslitConf= configuration.translConfEntryDict[trgLangKey];
                    interlinguaTranslitLoaded = true;
					Log.Write ("EN interlingua transliteration loaded for language "+trgLang+": "+ configuration.translConfEntryDict[trgLangKey].mosesIniPath,LogLevelType.LIMITED_OUTPUT,configuration);
                }
                
                if (interlinguaTranslitLoaded)
                {
                    return true;
                }else if (configuration.forceEnTranslitInterlingua){
                    Log.Write ("Cannot force EN interlingua transliteration for the pair "+langKey+" as at least one of the interlingua transliteration configurations is disabled or missing!",LogLevelType.WARNING,configuration);
                    Log.Write ("The system will be executed without transliteration.",LogLevelType.WARNING,configuration);
                }
            }
            return false;
        }
    }
}
