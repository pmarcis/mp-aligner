mp-aligner
==========

The MPAligner is a toolkit for cross-lingual term mapping in term tagged documents. The toolkit is specifically designed to address term mapping between European languages.

If you are using MPaligner, please cite the following paper:

    @inproceedings{Pinnis2013,
  
    address = {Hissar, Bulgaria},
	
    author = {Pinnis, Mārcis},
	
    booktitle = {Proceedings of the 9th International Conference on Recent Advances in Natural Language Processing (RANLP 2013)},
	
    title = {{Context Independent Term Mapper for European Languages}},
	
    year = {2013}
	
    }


This release of MPAligner is licensed under the following license: [Attribution-NonCommercial-ShareAlike 3.0 Unported](http://creativecommons.org/licenses/by-nc-sa/3.0/)

The repository is structured as follows:

* The source code of MPAligner is stored under [MPAligner](MPAligner).

* The source code for the tool that generates transliteration training data, dictionaries, and invalid alignment dictionaries is located under [CreateResources](CreateResources).

* The transliteration system (Moses SMT) training recipes are stored under [ScriptsForTransliterationTraining](ScriptsForTransliterationTraining).

* The compiled MPAligner version can be found under [CompiledVersion](CompiledVersion). The compiled version lacks a lot of linguistic resources (only resources for EN-LV and EN-DE have been uploaded) because of spacial constraints by GitHub. If you want to acquire resources for any other language pair (from the RANLP 2013 paper), please do not hesitate to ask!


Other tools, which are not that important, but serve a particular purpose in evaluation or testing (however, are not necessary if you want to just execute MPAligner) are:

* [AnalyseDictAndPhraseTableCoverage](AnalyseDictAndPhraseTableCoverage) - used to acquire out-of-vocabulary scores for the RANLP 2013 paper.

* [ApplyThreshold](ApplyThreshold) - OBSOLETE - used to apply a threshold and consolidate the alignment results. This code has been integrated in MPAligner. The project has been left here only for testing purposes.

* [ConsolidateEurovocResults](ConsolidateEurovocResults) - generates an Excel spreadsheet form results of the EuroVoc evaluation (for more details refer to the source code).

* [ReplaceStringInFile](ReplaceStringInFile) - a simple utility function.

* [SplitPreProcessedData](SplitPreProcessedData) - OBSOLETE - used to speed up EuroVoc evaluation.
