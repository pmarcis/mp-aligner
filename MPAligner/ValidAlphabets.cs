//
//  ValidAlphabets.cs
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
using System.Threading.Tasks;

namespace MPAligner
{
    public class ValidAlphabets
    {
        //Official EU:
        
        /// <summary>
        /// Bulgarian
        /// </summary>
        public static string BG = "АаАаБбВвГгДдЕеЖжЗзИиЙйКкЛлЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЪъьЮюЯя";
        
        /// <summary>
        /// Czech
        /// </summary>
        public static string CS = "AaÁáBbCcČčDdĎďEeÉéĚěFfGgHhCHchIiÍíJjKkLlMmNnŇňOoÓóPpRrŘřSsŠšTtŤťUuÚúŮůVvYyÝýZzŽžQqWwXx";
        
        /// <summary>
        /// Danish
        /// </summary>
        public static string DA = "ABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅabcdefghijklmnopqrstuvwxyzæøåéóǿáÉÓǾÁ";
        
        /// <summary>
        /// Dutch
        /// </summary>
        public static string NL = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzëïöäüËÏÖÄÜ";
        
        /// <summary>
        /// English
        /// </summary>
        public static string EN = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\'’";
        
        /// <summary>
        /// Estonian
        /// </summary>
        public static string ET = "ABCDEFGHIJKLMNOPQRSŠZŽTUVWÕÄÖÜXYabcdefghijklmnopqrsšzžtuvwõäöüxy";
        
        
        /// <summary>
        /// Finnish
        /// </summary>
        public static string FI = "ABCDEFGHIJKLMNOPQRSTUVXYZÅÄÖabcdefghijklmnopqrstuvxyzåäöwW";
        
        /// <summary>
        /// French
        /// </summary>
        public static string FR = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzéàèùâêîôûëïüÿçÉÀÈÙÂÊÎÔÛËÏÜŸÇ";
        
        /// <summary>
        /// German
        /// </summary>
        public static string DE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄäÖöÜüßſ";
        
        
        /// <summary>
        /// Greek
        /// </summary>
        public static string EL = "ΑαΒβΓγΔδΕεΖζΗηΘθΙιϊΪΐΚκΛλΜμΝνΞξΟοΠπΡρΣσςΤτΥυϋΫΦφΧχΨψΩωώΏάΆόΌöÖέΈίΊύΎήΉ";
        
        /// <summary>
        /// Hungarian
        /// </summary>
        public static string HU = "AÁBCDEÉFGHIÍJKLMNOÓÖŐPQRSTUÚÜŰVWXYZaábcdeéfghiíjklmnoóöőpqrstuúüűvwxyzõÕ";
        
        /// <summary>
        /// Irish
        /// </summary>
        public static string GA = "AaBbCcDdEeFfGgHhIiLlMmNnOoPpRrSsTtUuáéíóúÁÉÍÓÚjkqvwxyzJKQVWXYZ";
        
        /// <summary>
        /// Italian
        /// </summary>
        public static string IT = "AaBbCcDdEeFfGgHhIiLlMmNnOoPpQqRrSsTtUuVvZz";
        
        /// <summary>
        /// Latvian
        /// </summary>
        public static string LV = "AĀBCČDEĒFGĢHIĪJKĶLĻMNŅOPRSŠTUŪVZŽaābcčdeēfgģhiījkķlļmnņoprsštuūvzž";
        
        /// <summary>
        /// Lithuanian
        /// </summary>
        public static string LT = "AaĄąBbCcČčDdEeĘęĖėFfGgHhIiĮįYyJjKkLlMmNnOoPpRrSsŠšTtUuŲųŪūVvZzŽž";
        
        /// <summary>
        /// Maltese
        /// </summary>
        public static string MT = "AaBbĊċDdEeFfĠġGgGħgħHhĦħIiIeieJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxŻżZz";
        
        /// <summary>
        /// Polish
        /// </summary>
        public static string PL = "AaĄąBbCcĆćDdEeĘęFfGgHhIiJjKkLlŁłMmNnŃńOoÓóPpRrSsŚśTtUuWwYyZzŹźŻżqvxQVX";
        
        /// <summary>
        /// Portuguese
        /// </summary>
        public static string PT = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÁáÂâÃãÀàÇçÉéÊêÍíÓóÔôÕõÚú";
        
        /// <summary>
        /// Romanian
        /// </summary>
        public static string RO = "AaĂăÂâBbCcDdEeFfGgHhIiÎîJjKkLlMmNnOoPpQqRrSsȘșşŞTtȚțţŢUuVvWwXxYyZz";
        
        /// <summary>
        /// Slovak
        /// </summary>
        public static string SK = "AaÁáÄäásEeÉéIiÍíOoÓóÔôUuÚúYyÝýBbCcČčDdĎďFfGgHhJjKkLlĹĺĽľMmNnŇňPpQqRrŔŕSsŠšTtŤťVvWwXxZzŽž";
        
        /// <summary>
        /// Slovene - may be incomplete!
        /// </summary>
        public static string SL = "AaBbCcČčDdEeFfGgHhIiJjKkLlMmNnOoPpRrSsŠšTtUuVvZzŽžĆĐQWXYÄËÖÜćđqwxyäëöüòêéóèíôáÒÊÉÓÈÍÔÁ";
        
        /// <summary>
        /// Spanish
        /// </summary>
        public static string ES = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyzáéíóúüÁÉÍÓÚÜ";
        
        /// <summary>
        /// Swedish
        /// </summary>
        public static string SV = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäöéÉ";
        
        //Semi-Official languages:
        
        /// <summary>
        /// Catalan
        /// </summary>
        public static string CA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÀÉÈÍÏÓÒÚÜÇàéèíïóòúüç";
        
        /// <summary>
        /// Galician
        /// </summary>
        public static string GL = "AaBbCcDdEeFfGgHhIiLlMmNnÑñOoPpQqRrSsTtUuVvXxZzJKWYjkwy";
        
        /// <summary>
        /// Basque
        /// </summary>
        public static string EU = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyzçÇ";
        
        //Scottish
        //public static string GD = ""; //Same as Gaelic
        
        /// <summary>
        /// Scottish Gaelic; Gaelic
        /// </summary>
        public static string GD = "abcdefghilmnoprstuàèìòùABCDEFGHILMNOPRSTUÀÈÌÒÙ";
        
        /// <summary>
        /// Welsh
        /// </summary>
        public static string CY = "KVXZJkvxzjABCDEFGHILMNOPRSTUWYabcdefghilmnoprstuwy";
        
        //Other:
        
        /// <summary>
        /// Croatian
        /// </summary>
        public static string HR = "ABCČĆDĐEFGHIJKLMNOPRSŠTUVZŽabcčćdđefghijklmnoprsštuvzž";
        
        /// <summary>
        /// Russian
        /// </summary>
        public static string RU = "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя";
        
        /// <summary>
        /// Arabic - not implemented.
        /// </summary>
        //public static string AR = "";
        
        /// <summary>
        /// Turkish
        /// </summary>
        public static string TR = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZabcçdefgğhıijklmnoöprsştuüvyzÂÎÛâîû";
        
        /// <summary>
        /// Urdu
        /// </summary>
        public static string UR = "ابپتٹثجچحخدڈذرڑزژسشصضطظعغفقکگلمنوہﮩﮨھءیے";
        
        /// <summary>
        /// Chinese - not implemented.
        /// </summary>
        //public static string ZH = "";
        
        /// <summary>
        /// Hindi
        /// </summary>
        public static string HI = "कखगघङहचछजझञयशटठडढणरषतथदधनलसपफबभमवअपआपाइपिईपीउपुऊपूऋपृॠṝपॄऌपॢॡपॣएपेऐपैओपोऔपौ";
        
        
        
        public static string punctuationsForTerms = "%$#@&*!?.,_ -‒–—―";
        public static string punctuationsForTermsPhraseTableFiltering = "-‒–—― ";
        
        public static string GetAlphabet(string language)
        {
            string baseAlphabet = "";
            switch (language.ToUpper())
            {
                case "BG":
                    baseAlphabet = ValidAlphabets.BG;
                    break;
                case "CS":
                    baseAlphabet = ValidAlphabets.CS;
                    break;
                case "CA":
                    baseAlphabet = ValidAlphabets.CA;
                    break;
                case "CY":
                    baseAlphabet = ValidAlphabets.CY;
                    break;
                case "DA":
                    baseAlphabet = ValidAlphabets.DA;
                    break;
                case "DE":
                    baseAlphabet = ValidAlphabets.DE;
                    break;
                case "EL":
                    baseAlphabet = ValidAlphabets.EL;
                    break;
                case "EN":
                    baseAlphabet = ValidAlphabets.EN;
                    break;
                case "ES":
                    baseAlphabet = ValidAlphabets.ES;
                    break;
                case "ET":
                    baseAlphabet = ValidAlphabets.ET;
                    break;
                case "EU":
                    baseAlphabet = ValidAlphabets.EU;
                    break;
                case "FI":
                    baseAlphabet = ValidAlphabets.FI;
                    break;
                case "FR":
                    baseAlphabet = ValidAlphabets.FR;
                    break;
                case "GA":
                    baseAlphabet = ValidAlphabets.GA;
                    break;
                case "GD":
                    baseAlphabet = ValidAlphabets.GD;
                    break;
                case "GL":
                    baseAlphabet = ValidAlphabets.GL;
                    break;
                case "HR":
                    baseAlphabet = ValidAlphabets.HR;
                    break;
                case "HU":
                    baseAlphabet = ValidAlphabets.HU;
                    break;
                case "HI":
                    baseAlphabet = ValidAlphabets.HI;
                    break;
                case "IT":
                    baseAlphabet = ValidAlphabets.IT;
                    break;
                case "LT":
                    baseAlphabet = ValidAlphabets.LT;
                    break;
                case "LV":
                    baseAlphabet = ValidAlphabets.LV;
                    break;
                case "MT":
                    baseAlphabet = ValidAlphabets.MT;
                    break;
                case "NL":
                    baseAlphabet = ValidAlphabets.NL;
                    break;
                case "PL":
                    baseAlphabet = ValidAlphabets.PL;
                    break;
                case "PT":
                    baseAlphabet = ValidAlphabets.PT;
                    break;
                case "RO":
                    baseAlphabet = ValidAlphabets.RO;
                    break;
                case "RU":
                    baseAlphabet = ValidAlphabets.RU;
                    break;
                case "SK":
                    baseAlphabet = ValidAlphabets.SK;
                    break;
                case "SL":
                    baseAlphabet = ValidAlphabets.SL;
                    break;
                case "SV":
                    baseAlphabet = ValidAlphabets.SV;
                    break;
                case "TR":
                    baseAlphabet = ValidAlphabets.TR;
                    break;
                case "UR":
                    baseAlphabet = ValidAlphabets.UR;
                    break;
            }
            return baseAlphabet;
        }
        
        public static bool IsValidPhrase(string phrase, string language)
        {
            string baseAlphabet = "";
            switch (language.ToUpper())
            {
                case "BG":
                    baseAlphabet = ValidAlphabets.BG;
                    break;
                case "CS":
                    baseAlphabet = ValidAlphabets.CS;
                    break;
                case "CA":
                    baseAlphabet = ValidAlphabets.CA;
                    break;
                case "CY":
                    baseAlphabet = ValidAlphabets.CY;
                    break;
                case "DA":
                    baseAlphabet = ValidAlphabets.DA;
                    break;
                case "DE":
                    baseAlphabet = ValidAlphabets.DE;
                    break;
                case "EL":
                    baseAlphabet = ValidAlphabets.EL;
                    break;
                case "EN":
                    baseAlphabet = ValidAlphabets.EN;
                    break;
                case "ES":
                    baseAlphabet = ValidAlphabets.ES;
                    break;
                case "ET":
                    baseAlphabet = ValidAlphabets.ET;
                    break;
                case "EU":
                    baseAlphabet = ValidAlphabets.EU;
                    break;
                case "FI":
                    baseAlphabet = ValidAlphabets.FI;
                    break;
                case "FR":
                    baseAlphabet = ValidAlphabets.FR;
                    break;
                case "GA":
                    baseAlphabet = ValidAlphabets.GA;
                    break;
                case "GD":
                    baseAlphabet = ValidAlphabets.GD;
                    break;
                case "GL":
                    baseAlphabet = ValidAlphabets.GL;
                    break;
                case "HR":
                    baseAlphabet = ValidAlphabets.HR;
                    break;
                case "HU":
                    baseAlphabet = ValidAlphabets.HU;
                    break;
                case "HI":
                    baseAlphabet = ValidAlphabets.HI;
                    break;
                case "IT":
                    baseAlphabet = ValidAlphabets.IT;
                    break;
                case "LT":
                    baseAlphabet = ValidAlphabets.LT;
                    break;
                case "LV":
                    baseAlphabet = ValidAlphabets.LV;
                    break;
                case "MT":
                    baseAlphabet = ValidAlphabets.MT;
                    break;
                case "NL":
                    baseAlphabet = ValidAlphabets.NL;
                    break;
                case "PL":
                    baseAlphabet = ValidAlphabets.PL;
                    break;
                case "PT":
                    baseAlphabet = ValidAlphabets.PT;
                    break;
                case "RO":
                    baseAlphabet = ValidAlphabets.RO;
                    break;
                case "RU":
                    baseAlphabet = ValidAlphabets.RU;
                    break;
                case "SK":
                    baseAlphabet = ValidAlphabets.SK;
                    break;
                case "SL":
                    baseAlphabet = ValidAlphabets.SL;
                    break;
                case "SV":
                    baseAlphabet = ValidAlphabets.SV;
                    break;
                case "TR":
                    baseAlphabet = ValidAlphabets.TR;
                    break;
                case "UR":
                    baseAlphabet = ValidAlphabets.UR;
                    break;
            }
            if (string.IsNullOrWhiteSpace(baseAlphabet))
            {
                Console.Error.WriteLine("Phrase validation not supported for the language " + language);
                throw new ArgumentException("Phrase validation not supported for the language " + language);
                //Console.Error.WriteLine("Using default EN instead");
            }
            int spaceCount = 0;
            int punctCount = 0;
            bool wasNonSpace = false;
            foreach (char c in phrase)
            {
                if (wasNonSpace && Char.IsWhiteSpace(c)) spaceCount++;
                else wasNonSpace = true;
                if (Char.IsPunctuation(c)) punctCount++;
                if (baseAlphabet.IndexOf(c) < 0 && ValidAlphabets.punctuationsForTermsPhraseTableFiltering.IndexOf(c) < 0)//&& ValidAlphabets.EN.IndexOf(c) < 0)
                {
                    return false;
                }
            }
            if (spaceCount > 0 || punctCount > 1)
            {
                return false;
            }
            return true;
        }
    }
}
