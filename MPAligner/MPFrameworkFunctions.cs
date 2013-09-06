//
//  MPFrameworkFunctions.cs
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

////////////////////////////////////////////////////////////////////////////////////////////////////
/// \file   CsvToSqlTransformer\MPFrameworkFunctions.cs
///
/// \brief  Implements the framework functions class. 
////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace MPFramework
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// \class  MPFrameworkFunctions
    ///
    /// \brief  Generic and utility functions written by Mārcis Pinnis. 
    ///
    /// \author Mārcis Pinnis
    /// \date   2010.04.17.
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class MPFrameworkFunctions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MPFrameworkFunctions"/> class.
        /// </summary>
        public MPFrameworkFunctions()
        {
        }

        /// <summary>
        /// Generic deserialization function. Deserializes a string to a target type object.
        /// </summary>
        /// <typeparam name="T">The target type, to which a given string will be deserialized.</typeparam>
        /// <param name="stringToDeserialize">The string that will be deserialized.</param>
        /// <returns>An instance of the target type, which is the deserialized string.</returns>
        public static T DeserializeString<T>( string stringToDeserialize )
        {
            XmlSerializer ser = new XmlSerializer( typeof( T ) );
            //System.IO.MemoryStream mem = new System.IO.MemoryStream();
            StringReader sr = new StringReader( stringToDeserialize );
            T deserializationResult = ( T ) ser.Deserialize( sr );
            sr.Close();
            return deserializationResult;
        }

        /// <summary>
        /// Serializes the object instance.
        /// </summary>
        /// <typeparam name="T">The source type, which instance will be serialized.</typeparam>
        /// <param name="objectInstance">The object instance.</param>
        /// <returns>A serialized string of the given object instance.</returns>
        public static string SerializeObjectInstance<T>( T objectInstance )
        {
            XmlSerializer ser = new XmlSerializer( typeof( T ) );
            //System.IO.MemoryStream mem = new System.IO.MemoryStream();
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            ser.Serialize( sw , objectInstance );
            sw.Close();
            return sb.ToString();
        }

        /// <summary>
        /// Gets a valid two lowercase character language code from a full English name or three char code.
        /// 2 char codes are simply passed thrue.
        /// </summary>
        /// <returns>
        /// The 2 char language code.
        /// </returns>
        /// <param name='lang'>
        /// Language code or full name in English
        /// </param>
        public static string GetValidLangString (string lang)
        {
            if (string.IsNullOrWhiteSpace (lang)) {
                throw new ArgumentNullException ("lang", "The language parameter for the GetValidLangString function is not allowed to be empty.");
            }
            string res = lang.ToLower ();
            switch (res) {
                case "abkhaz":
                case "ab":
                case "abk":
                    return "ab";
                case "afar":
                case "aa":
                case "aar":
                    return "aa";
                case "afrikaans":
                case "af":
                case "afr":
                    return "af";
                case "akan":
                case "ak":
                case "aka":
                    return "ak";
                case "albanian":
                case "sq":
                case "sqi":
                case "alb":
                    return "sq";
                case "amharic":
                case "am":
                case "amh":
                    return "am";
                case "arabic":
                case "ar":
                case "ara":
                    return "ar";
                case "aragonese":
                case "an":
                case "arg":
                    return "an";
                case "armenian":
                case "hy":
                case "hye":
                case "arm":
                    return "hy";
                case "assamese":
                case "as":
                case "asm":
                    return "as";
                case "avaric":
                case "av":
                case "ava":
                    return "av";
                case "avestan":
                case "ae":
                case "ave":
                    return "ae";
                case "aymara":
                case "ay":
                case "aym":
                    return "ay";
                case "azerbaijani":
                case "az":
                case "aze":
                    return "az";
                case "bambara":
                case "bm":
                case "bam":
                    return "bm";
                case "bashkir":
                case "ba":
                case "bak":
                    return "ba";
                case "basque":
                case "eu":
                case "eus":
                case "baq":
                    return "eu";
                case "belarusian":
                case "be":
                case "bel":
                    return "be";
                case "bengali":
                case "bn":
                case "ben":
                    return "bn";
                case "bihari":
                case "bh":
                case "bih":
                    return "bh";
                case "bislama":
                case "bi":
                case "bis":
                    return "bi";
                case "bosnian":
                case "bs":
                case "bos":
                    return "bs";
                case "breton":
                case "br":
                case "bre":
                    return "br";
                case "bulgarian":
                case "bg":
                case "bul":
                    return "bg";
                case "burmese":
                case "my":
                case "mya":
                case "bur":
                    return "my";
                case "catalan":
                case "valencian":
                case "ca":
                case "cat":
                    return "ca";
                case "chamorro":
                case "ch":
                case "cha":
                    return "ch";
                case "chechen":
                case "ce":
                case "che":
                    return "ce";
                case "chichewa":
                case "chewa":
                case "nyanja":
                case "ny":
                case "nya":
                    return "ny";
                case "chinese":
                case "zh":
                case "zho":
                case "chi":
                    return "zh";
                case "chuvash":
                case "cv":
                case "chv":
                    return "cv";
                case "cornish":
                case "kw":
                case "cor":
                    return "kw";
                case "corsican":
                case "co":
                case "cos":
                    return "co";
                case "cree":
                case "cr":
                case "cre":
                    return "cr";
                case "croatian":
                case "hr":
                case "hrv":
                    return "hr";
                case "czech":
                case "cs":
                case "ces":
                case "cze":
                    return "cs";
                case "danish":
                case "da":
                case "dan":
                    return "da";
                case "divehi":
                case "dhivehi":
                case "maldivian":
                case "dv":
                case "div":
                    return "dv";
                case "dutch":
                case "nl":
                case "nld":
                case "dut":
                    return "nl";
                case "dzongkha":
                case "dz":
                case "dzo":
                    return "dz";
                case "english":
                case "en":
                case "eng":
                    return "en";
                case "esperanto":
                case "eo":
                case "epo":
                    return "eo";
                case "estonian":
                case "et":
                case "est":
                    return "et";
                case "ewe":
                case "ee":
                    return "ee";
                case "faroese":
                case "fo":
                case "fao":
                    return "fo";
                case "fijian":
                case "fj":
                case "fij":
                    return "fj";
                case "finnish":
                case "fi":
                case "fin":
                    return "fi";
                case "french":
                case "fr":
                case "fra":
                case "fre":
                    return "fr";
                case "fula":
                case "fulah":
                case "pulaar":
                case "pular":
                case "ff":
                case "ful":
                    return "ff";
                case "galician":
                case "gl":
                case "glg":
                    return "gl";
                case "georgian":
                case "ka":
                case "kat":
                case "geo":
                    return "ka";
                case "german":
                case "de":
                case "deu":
                case "ger":
                    return "de";
                case "greek":
                case "el":
                case "ell":
                case "gre":
                    return "el";
                case "guaraní":
                case "gn":
                case "grn":
                    return "gn";
                case "gujarati":
                case "gu":
                case "guj":
                    return "gu";
                case "haitian":
                case "haitian creole":
                case "ht":
                case "hat":
                    return "ht";
                case "hausa":
                case "ha":
                case "hau":
                    return "ha";
                case "hebrew":
                case "he":
                case "heb":
                    return "he";
                case "herero":
                case "hz":
                case "her":
                    return "hz";
                case "hindi":
                case "hi":
                case "hin":
                    return "hi";
                case "hiri motu":
                case "ho":
                case "hmo":
                    return "ho";
                case "hungarian":
                case "hu":
                case "hun":
                    return "hu";
                case "interlingua":
                case "ia":
                case "ina":
                    return "ia";
                case "indonesian":
                case "id":
                case "ind":
                    return "id";
                case "interlingue":
                case "ie":
                case "ile":
                    return "ie";
                case "irish":
                case "ga":
                case "gle":
                    return "ga";
                case "igbo":
                case "ig":
                case "ibo":
                    return "ig";
                case "inupiaq":
                case "ik":
                case "ipk":
                    return "ik";
                case "ido":
                case "io":
                    return "io";
                case "icelandic":
                case "is":
                case "isl":
                case "ice":
                    return "is";
                case "italian":
                case "it":
                case "ita":
                    return "it";
                case "inuktitut":
                case "iu":
                case "iku":
                    return "iu";
                case "japanese":
                case "ja":
                case "jpn":
                    return "ja";
                case "javanese":
                case "jv":
                case "jav":
                    return "jv";
                case "kalaallisut":
                case "greenlandic":
                case "kl":
                case "kal":
                    return "kl";
                case "kannada":
                case "kn":
                case "kan":
                    return "kn";
                case "kanuri":
                case "kr":
                case "kau":
                    return "kr";
                case "kashmiri":
                case "ks":
                case "kas":
                    return "ks";
                case "kazakh":
                case "kk":
                case "kaz":
                    return "kk";
                case "khmer":
                case "km":
                case "khm":
                    return "km";
                case "kikuyu":
                case "gikuyu":
                case "ki":
                case "kik":
                    return "ki";
                case "kinyarwanda":
                case "rw":
                case "kin":
                    return "rw";
                case "kyrgyz":
                case "ky":
                case "kir":
                    return "ky";
                case "komi":
                case "kv":
                case "kom":
                    return "kv";
                case "kongo":
                case "kg":
                case "kon":
                    return "kg";
                case "korean":
                case "ko":
                case "kor":
                    return "ko";
                case "kurdish":
                case "ku":
                case "kur":
                    return "ku";
                case "kwanyama":
                case "kuanyama":
                case "kj":
                case "kua":
                    return "kj";
                case "latin":
                case "la":
                case "lat":
                    return "la";
                case "luxembourgish":
                case "letzeburgesch":
                case "lb":
                case "ltz":
                    return "lb";
                case "ganda":
                case "lg":
                case "lug":
                    return "lg";
                case "limburgish":
                case "limburgan":
                case "limburger":
                case "li":
                case "lim":
                    return "li";
                case "lingala":
                case "ln":
                case "lin":
                    return "ln";
                case "lao":
                case "lo":
                    return "lo";
                case "lithuanian":
                case "lt":
                case "lit":
                    return "lt";
                case "luba-katanga":
                case "lu":
                case "lub":
                    return "lu";
                case "latvian":
                case "lv":
                case "lav":
                    return "lv";
                case "manx":
                case "gv":
                case "glv":
                    return "gv";
                case "macedonian":
                case "mk":
                case "mkd":
                case "mac":
                    return "mk";
                case "malagasy":
                case "mg":
                case "mlg":
                    return "mg";
                case "malay":
                case "ms":
                case "msa":
                case "may":
                    return "ms";
                case "malayalam":
                case "ml":
                case "mal":
                    return "ml";
                case "maltese":
                case "mt":
                case "mlt":
                    return "mt";
                case "māori":
                case "mi":
                case "mri":
                case "mao":
                    return "mi";
                case "marathi":
                case "mr":
                case "mar":
                    return "mr";
                case "marshallese":
                case "mh":
                case "mah":
                    return "mh";
                case "mongolian":
                case "mn":
                case "mon":
                    return "mn";
                case "nauru":
                case "na":
                case "nau":
                    return "na";
                case "navajo":
                case "navaho":
                case "nv":
                case "nav":
                    return "nv";
                case "norwegian bokmål":
                case "nb":
                case "nob":
                    return "nb";
                case "north ndebele":
                case "nd":
                case "nde":
                    return "nd";
                case "nepali":
                case "ne":
                case "nep":
                    return "ne";
                case "ndonga":
                case "ng":
                case "ndo":
                    return "ng";
                case "norwegian nynorsk":
                case "nn":
                case "nno":
                    return "nn";
                case "norwegian":
                case "no":
                case "nor":
                    return "no";
                case "nuosu":
                case "ii":
                case "iii":
                    return "ii";
                case "south ndebele":
                case "nr":
                case "nbl":
                    return "nr";
                case "occitan":
                case "oc":
                case "oci":
                    return "oc";
                case "ojibwe":
                case "ojibwa":
                case "oj":
                case "oji":
                    return "oj";
                case "old church slavonic":
                case "church slavic":
                case "church slavonic":
                case "old bulgarian":
                case "old slavonic":
                case "cu":
                case "chu":
                    return "cu";
                case "oromo":
                case "om":
                case "orm":
                    return "om";
                case "oriya":
                case "or":
                case "ori":
                    return "or";
                case "ossetian":
                case "ossetic":
                case "os":
                case "oss":
                    return "os";
                case "panjabi":
                case "punjabi":
                case "pa":
                case "pan":
                    return "pa";
                case "pāli":
                case "pi":
                case "pli":
                    return "pi";
                case "persian":
                case "fa":
                case "fas":
                case "per":
                    return "fa";
                case "polish":
                case "pl":
                case "pol":
                    return "pl";
                case "pashto":
                case "pushto":
                case "ps":
                case "pus":
                    return "ps";
                case "portuguese":
                case "pt":
                case "por":
                    return "pt";
                case "quechua":
                case "qu":
                case "que":
                    return "qu";
                case "romansh":
                case "rm":
                case "roh":
                    return "rm";
                case "kirundi":
                case "rn":
                case "run":
                    return "rn";
                case "romanian":
                case "moldavian":
                case "moldovan":
                case "ro":
                case "ron":
                case "rum":
                    return "ro";
                case "russian":
                case "ru":
                case "rus":
                    return "ru";
                case "sanskrit":
                case "sa":
                case "san":
                    return "sa";
                case "sardinian":
                case "sc":
                case "srd":
                    return "sc";
                case "sindhi":
                case "sd":
                case "snd":
                    return "sd";
                case "northern sami":
                case "se":
                case "sme":
                    return "se";
                case "samoan":
                case "sm":
                case "smo":
                    return "sm";
                case "sango":
                case "sg":
                case "sag":
                    return "sg";
                case "serbian":
                case "sr":
                case "srp":
                    return "sr";
                case "scottish gaelic":
                case "gaelic":
                case "gd":
                case "gla":
                    return "gd";
                case "shona":
                case "sn":
                case "sna":
                    return "sn";
                case "sinhala":
                case "sinhalese":
                case "si":
                case "sin":
                    return "si";
                case "slovak":
                case "sk":
                case "slk":
                case "slo":
                    return "sk";
                case "slovene":
                case "sl":
                case "slv":
                    return "sl";
                case "somali":
                case "so":
                case "som":
                    return "so";
                case "southern sotho":
                case "st":
                case "sot":
                    return "st";
                case "spanish":
                case "castilian":
                case "es":
                case "spa":
                    return "es";
                case "sundanese":
                case "su":
                case "sun":
                    return "su";
                case "swahili":
                case "sw":
                case "swa":
                    return "sw";
                case "swati":
                case "ss":
                case "ssw":
                    return "ss";
                case "swedish":
                case "sv":
                case "swe":
                    return "sv";
                case "tamil":
                case "ta":
                case "tam":
                    return "ta";
                case "telugu":
                case "te":
                case "tel":
                    return "te";
                case "tajik":
                case "tg":
                case "tgk":
                    return "tg";
                case "thai":
                case "th":
                case "tha":
                    return "th";
                case "tigrinya":
                case "ti":
                case "tir":
                    return "ti";
                case "tibetan standard":
                case "tibetan":
                case "bo":
                case "bod":
                case "tib":
                    return "bo";
                case "turkmen":
                case "tk":
                case "tuk":
                    return "tk";
                case "tagalog":
                case "tl":
                case "tgl":
                    return "tl";
                case "tswana":
                case "tn":
                case "tsn":
                    return "tn";
                case "tonga":
                case "to":
                case "ton":
                    return "to";
                case "turkish":
                case "tr":
                case "tur":
                    return "tr";
                case "tsonga":
                case "ts":
                case "tso":
                    return "ts";
                case "tatar":
                case "tt":
                case "tat":
                    return "tt";
                case "twi":
                case "tw":
                    return "tw";
                case "tahitian":
                case "ty":
                case "tah":
                    return "ty";
                case "uighur":
                case "uyghur":
                case "ug":
                case "uig":
                    return "ug";
                case "ukrainian":
                case "uk":
                case "ukr":
                    return "uk";
                case "urdu":
                case "ur":
                case "urd":
                    return "ur";
                case "uzbek":
                case "uz":
                case "uzb":
                    return "uz";
                case "venda":
                case "ve":
                case "ven":
                    return "ve";
                case "vietnamese":
                case "vi":
                case "vie":
                    return "vi";
                case "volapük":
                case "vo":
                case "vol":
                    return "vo";
                case "walloon":
                case "wa":
                case "wln":
                    return "wa";
                case "welsh":
                case "cy":
                case "cym":
                case "wel":
                    return "cy";
                case "wolof":
                case "wo":
                case "wol":
                    return "wo";
                case "western frisian":
                case "fy":
                case "fry":
                    return "fy";
                case "xhosa":
                case "xh":
                case "xho":
                    return "xh";
                case "yiddish":
                case "yi":
                case "yid":
                    return "yi";
                case "yoruba":
                case "yo":
                case "yor":
                    return "yo";
                case "zhuang":
                case "chuang":
                case "za":
                case "zha":
                    return "za";
                case "zulu":
                case "zu":
                case "zul":
                    return "zu";
                default:
                    return res;
            }
        }
        public static bool IsAllLower(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            for(int i=0;i<word.Length;i++)
            {
                if (Char.IsUpper(word[i])&&Char.IsLetter(word[i]))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool IsAllUpper(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            for(int i=0;i<word.Length;i++)
            {
                if (Char.IsLower(word[i])&&Char.IsLetter(word[i]))
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool IsFirstUpper(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            if (Char.IsUpper(word[0]))
            {
                return true;
            }
            return false;
        }
    }
}
