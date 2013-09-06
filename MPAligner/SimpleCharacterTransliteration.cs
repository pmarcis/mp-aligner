//
//  SimpleCharacterTransliteration.cs
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

namespace MPAligner
{
    /// <summary>
    /// Simple character transliteration provides means for transliteration of words using character substitutions.
    /// Supported are latin (for a wide range of languages), greek and cyrillic characters.
    /// For other languages not supported I suggest building a transliteration system instead (as the character based method will probably not work very good for all languages...).
    /// Transliteration is performed to English characters.
    /// </summary>
    public class SimpleCharacterTransliteration
    {
        static Dictionary<string, string> translDict = new Dictionary<string, string>();
        public static string Transliterate(string s)
        {
            if (translDict.ContainsKey(s)) return translDict[s];
            StringBuilder outString = new StringBuilder();
            foreach(char c in s)
            {
                if ("АаAaĀāΆάΑαÁáÀàÂâǍǎĂăÃãẢảȦȧẠạÄäÅåḀḁĄąᶏȺⱥȀȁẤấẦầẪẫẨẩẬậẮắẰằẴẵẲẳẶặǺǻǠǡǞǟȀȁȂȃⱭɑᴀɐɒＡａАа".IndexOf(c)>=0)
                {
                    outString.Append("a");
                }
                else if ("BbΒβБбḂḃḄḅḆḇɃƀƁɓƂƃ ᵬᶀʙＢｂȸБб".IndexOf(c)>=0)
                {
                    outString.Append("b");
                }
                //TODO: C is probably one of the most ambiguous characters. In English ts is the same as c, but not in many other languages, therefore, this is a place where errors could appear...
                else if ("CcČčЦцĆćĈĉĊċCcÇçḈḉȻȼƇƈɕᴄＣｃЦц".IndexOf(c)>=0)
                {
                    outString.Append("c");
                }
                else if ("DdΔδДдĎďḊḋḐḑḌḍḒḓḎḏĐđDdƉɖƊɗƋƌᵭᶁᶑȡᴅＤｄȸДд".IndexOf(c)>=0)
                {
                    outString.Append("d");
                }
                else if ("EeĒēΕεЕеЭэΈέÉéÈèÊêḘḙĚěĔĕẼẽḚḛẺẻĖėËëȨȩĘęᶒɆɇȄȅẾếỀềỄễỂểḜḝḖḗḔḕȆȇẸẹỆệᴇＥｅЕе".IndexOf(c)>=0)
                {
                    outString.Append("e");
                }
                else if ("FfΦφФфḞḟƑƒᵮᶂＦｆФф".IndexOf(c)>=0)
                {
                    outString.Append("f");
                }
                else if ("GgΓγГгǦǧǴǵĞğĜĝĠġĢģḠḡǤǥƓɠᶃɢＧｇŊŋГг".IndexOf(c)>=0)
                {
                    outString.Append("g");
                }
                else if ("HhΗηΉήĤĥȞȟḦḧḢḣḨḩḤḥḪḫHẖĦħⱧⱨɦʰʜＨｈХх".IndexOf(c)>=0)
                {
                    outString.Append("h");
                }
                else if ("IiĪīΊίΙιИиЫыÍíÌìĬĭÎîǏǐÏïḮḯĨĩĮįỈỉȈȉȊȋỊịḬḭƗɨᵻᶖİiIıɪＩｉIiИи".IndexOf(c)>=0)
                {
                    outString.Append("i");
                }
                else if ("JjЙйĴĵɈɉJǰʝɟʄᴊＪｊJjЈј".IndexOf(c)>=0)
                {
                    outString.Append("j");
                }
                else if ("KkΚκКкḰḱǨǩĶķḲḳḴḵƘƙⱩⱪᶄᶄᴋＫｋКк".IndexOf(c)>=0)
                {
                    outString.Append("k");
                }
                else if ("LlĻļΛλЛлĹĺĽľḶḷḸḹḼḽḺḻŁłĿŀȽƚⱠⱡⱢɫɬᶅɭȴʟＬｌЛл".IndexOf(c)>=0)
                {
                    outString.Append("l");
                }
                else if ("MmΜμМмḾḿṀṁṂṃᵯᶆɱᴍＭｍМм".IndexOf(c)>=0)
                {
                    outString.Append("m");
                }
                else if ("NnŅņΝνНнŃńǸǹŇňÑñṄṅṆṇṊṋṈṉNnƝɲȠƞᵰᶇɳȵɴＮｎŊŋНн".IndexOf(c)>=0)
                {
                    outString.Append("n");
                }
                else if ("OoΟοΌόОоÓóÒòŎŏÔôỐốỒồỖỗỔổǑǒÖöȪȫŐőÕõṌṍṎṏȬȭȮȯȰȱØøǾǿǪǫǬǭŌōṒṓṐṑỎỏȌȍȎȏƠơỚớỜờỠỡỞởỢợỌọỘộƟɵᴏＯｏОо".IndexOf(c)>=0)
                {
                    outString.Append("o");
                }
                else if ("PpΠπПпṔṕṖṗⱣᵽƤƥPpᵱᶈᴘＰｐȹПп".IndexOf(c)>=0)
                {
                    outString.Append("p");
                }
                else if ("QqɊɋʠＱｑȹ".IndexOf(c)>=0)
                {
                    outString.Append("q");
                }
                else if ("RrρΡРрŔŕŘřṘṙŖŗȐȑȒȓṚṛṜṝṞṟɌɍⱤɽᵲᶉɼɾᵳʀＲｒРр".IndexOf(c)>=0)
                {
                    outString.Append("r");
                }
                else if ("SsςσΣŚСсśṤṥŜŝṦṧṠṡẛŞşṢṣṨṩȘșSsᵴᶊʂȿＳｓСс".IndexOf(c)>=0)
                {
                    outString.Append("s");
                }
                else if ("TtΤτТтŤťṪṫŢţṬṭȚțṰṱṮṯŦŧȾⱦƬƭƮʈTẗᵵƫȶᶙᴛＴｔТт".IndexOf(c)>=0)
                {
                    outString.Append("t");
                }
                else if ("UuŪūУуЮюÚúÙùŬŭÛûǓǔŮůÜüǗǘǛǜǙǚǕǖŰűŨũṸṹŲųṺṻỦủȔȕȖȗƯưỨứỪừỮữỬửỰựỤụṲṳṶṷṴṵɄʉᵾᶙᴜＵｕ".IndexOf(c)>=0)
                {
                    outString.Append("u");
                }
                else if ("VvВвṼṽṾṿƲʋᶌᶌⱱⱴᴠＶｖВв".IndexOf(c)>=0)
                {
                    outString.Append("v");
                }
                else if ("WwΩωΏώẂẃẀẁŴŵẄẅẆẇẈẉẘẘⱲⱳᴡＷｗ".IndexOf(c)>=0)
                {
                    outString.Append("w");
                }
                else if ("XxΧχХхẌẍẊẋᶍＸｘ".IndexOf(c)>=0)
                {
                    outString.Append("x");
                }
                //TODO: I put the Russian Ёё with y, but I am not sure whether that is correct
                else if ("YyΥυЁёΎύÝýỲỳŶŷẙŸÿỸỹẎẏȲȳỶỷỴỵɎɏƳƴʏＹｙУу".IndexOf(c)>=0)
                {
                    outString.Append("y");
                }
                else if ("ZzŽžζΖЗзŹźẐẑŻżẒẓẔẕƵƶȤȥⱫⱬᵶᶎʐʑɀᴢＺｚЗз".IndexOf(c) >= 0)
                {
                    outString.Append("z");
                }
                else if ("ŠšШшШш".IndexOf(c)>=0)
                {
                    outString.Append("sh");
                }
                else if ("ЖжЖж".IndexOf(c)>=0)
                {
                    outString.Append("zh");
                }
                else if ("ÆæᴁᴭᵆǼǽǢǣᴂᴂᴔÆæᴁᴭᵆǼǽǢǣ".IndexOf(c)>=0)
                {
                    outString.Append("ae");
                }
                else if ("θΘ".IndexOf(c)>=0)
                {
                    outString.Append("th");
                }
                else if ("Яя".IndexOf(c)>=0)
                {
                    outString.Append("ya");
                }
                //TODO: I put the Serbian Ћћ with ch, but I am not sure whether that is correct
                else if ("ЧчЋћЧч".IndexOf(c)>=0)
                {
                    outString.Append("ch");
                }
                else if ("Щщ".IndexOf(c)>=0)
                {
                    outString.Append("shc");
                }
                else if ("ЪъЬь".IndexOf(c)>=0)
                {
                    //Do not append anything for these characters...
                    //These are Russian softening and hardening symbols that would disappear in English...
                }
                else if ("ψΨ".IndexOf(c)>=0)
                {
                    outString.Append("ps");
                }
                else if ("ξΞ".IndexOf(c)>=0)
                {
                    outString.Append("ks");
                }
                else if ("Œœ".IndexOf(c)>=0)
                {
                    outString.Append("oe");
                }
                else if ("ᵫ".IndexOf(c)>=0)
                {
                    outString.Append("ue");
                }
                else if ("ﬁ".IndexOf(c)>=0)
                {
                    outString.Append("fi");
                }
                else if ("ﬂ".IndexOf(c)>=0)
                {
                    outString.Append("fl");
                }
                else if ("ǱǲǳǄǅǆЂђЏџ".IndexOf(c)>=0)
                {
                    outString.Append("dz");
                }
                else if ("Ĳĳ".IndexOf(c)>=0)
                {
                    outString.Append("ij");
                }
                else if ("ǇǈǉЉљ".IndexOf(c)>=0)
                {
                    outString.Append("lj");
                }
                else if ("ǊǋǌЊњ".IndexOf(c)>=0)
                {
                    outString.Append("nj");
                }
                else if ("ᴔ".IndexOf(c)>=0)
                {
                    outString.Append("eo");
                }
                else
                {
                    outString.Append(c);
                }
            }
            translDict.Add(s, outString.ToString());
            return outString.ToString();
        }

    }
}
