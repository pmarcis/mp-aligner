//
//  Program.cs
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

namespace CreateResources
{
    public class SimpleCharacterTransliteration
    {
        static Dictionary<string, string> translDict = new Dictionary<string, string>();
        public static string Transliterate(string s)
        {
            if (translDict.ContainsKey(s)) return translDict[s];
            StringBuilder outString = new StringBuilder();
            foreach(char c in s)
            {
                if ("АаAaĀāΆάΑαÁáÀàÂâǍǎĂăÃãẢảȦȧẠạÄäÅåḀḁĄąᶏȺⱥȀȁẤấẦầẪẫẨẩẬậẮắẰằẴẵẲẳẶặǺǻǠǡǞǟȀȁȂȃⱭɑᴀɐɒＡａ".IndexOf(c) >= 0)
                {
                    outString.Append("a");
                }
                else if ("BbΒβБбḂḃḄḅḆḇɃƀƁɓƂƃ ᵬᶀʙＢｂȸ".IndexOf(c) >= 0)
                {
                    outString.Append("b");
                }
                else if ("CcČčЦцĆćĈĉĊċCcÇçḈḉȻȼƇƈɕᴄＣｃ".IndexOf(c) >= 0)
                {
                    outString.Append("c");
                }
                else if ("DdΔδДдĎďḊḋḐḑḌḍḒḓḎḏĐđDdƉɖƊɗƋƌᵭᶁᶑȡᴅＤｄȸ".IndexOf(c) >= 0)
                {
                    outString.Append("d");
                }
                else if ("EeĒēΕεЕеЭэΈέÉéÈèÊêḘḙĚěĔĕẼẽḚḛẺẻĖėËëȨȩĘęᶒɆɇȄȅẾếỀềỄễỂểḜḝḖḗḔḕȆȇẸẹỆệᴇＥｅ".IndexOf(c) >= 0)
                {
                    outString.Append("e");
                }
                else if ("FfΦφФфḞḟƑƒᵮᶂＦｆ".IndexOf(c) >= 0)
                {
                    outString.Append("f");
                }
                else if ("GgΓγГгǦǧǴǵĞğĜĝĠġĢģḠḡǤǥƓɠᶃɢＧｇŊŋ".IndexOf(c) >= 0)
                {
                    outString.Append("g");
                }
                else if ("HhΗηΉήĤĥȞȟḦḧḢḣḨḩḤḥḪḫHẖĦħⱧⱨɦʰʜＨｈ".IndexOf(c) >= 0)
                {
                    outString.Append("h");
                }
                else if ("IiĪīΊίΙιИиЫыÍíÌìĬĭÎîǏǐÏïḮḯĨĩĮįỈỉȈȉȊȋỊịḬḭƗɨᵻᶖİiIıɪＩｉIi".IndexOf(c) >= 0)
                {
                    outString.Append("i");
                }
                else if ("JjЙйĴĵɈɉJǰʝɟʄᴊＪｊJj".IndexOf(c) >= 0)
                {
                    outString.Append("j");
                }
                else if ("KkΚκКкḰḱǨǩĶķḲḳḴḵƘƙⱩⱪᶄᶄᴋＫｋ".IndexOf(c) >= 0)
                {
                    outString.Append("k");
                }
                else if ("LlĻļΛλЛлĹĺĽľḶḷḸḹḼḽḺḻŁłĿŀȽƚⱠⱡⱢɫɬᶅɭȴʟＬｌ".IndexOf(c) >= 0)
                {
                    outString.Append("l");
                }
                else if ("MmΜμМмḾḿṀṁṂṃᵯᶆɱᴍＭｍ".IndexOf(c) >= 0)
                {
                    outString.Append("m");
                }
                else if ("NnŅņΝνНнŃńǸǹŇňÑñṄṅṆṇṊṋṈṉNnƝɲȠƞᵰᶇɳȵɴＮｎŊŋ".IndexOf(c) >= 0)
                {
                    outString.Append("n");
                }
                else if ("OoΟοΌόОоÓóÒòŎŏÔôỐốỒồỖỗỔổǑǒÖöȪȫŐőÕõṌṍṎṏȬȭȮȯȰȱØøǾǿǪǫǬǭŌōṒṓṐṑỎỏȌȍȎȏƠơỚớỜờỠỡỞởỢợỌọỘộƟɵᴏＯｏ".IndexOf(c) >= 0)
                {
                    outString.Append("o");
                }
                else if ("PpΠπПпṔṕṖṗⱣᵽƤƥPpᵱᶈᴘＰｐȹ".IndexOf(c) >= 0)
                {
                    outString.Append("p");
                }
                else if ("QqɊɋʠＱｑȹ".IndexOf(c) >= 0)
                {
                    outString.Append("q");
                }
                else if ("RrρΡРрŔŕŘřṘṙŖŗȐȑȒȓṚṛṜṝṞṟɌɍⱤɽᵲᶉɼɾᵳʀＲｒ".IndexOf(c) >= 0)
                {
                    outString.Append("r");
                }
                //TODO: I put the Russian Жж and Шш with s, but I am not sure whether that is correct
                else if ("SsŠšςσΣŚСсЖжШшśṤṥŜŝṦṧṠṡẛŞşṢṣṨṩȘșSsᵴᶊʂȿＳｓ".IndexOf(c) >= 0)
                {
                    outString.Append("s");
                }
                else if ("TtΤτТтŤťṪṫŢţṬṭȚțṰṱṮṯŦŧȾⱦƬƭƮʈTẗᵵƫȶᶙᴛＴｔ".IndexOf(c) >= 0)
                {
                    outString.Append("t");
                }
                else if ("UuŪūУуЮюÚúÙùŬŭÛûǓǔŮůÜüǗǘǛǜǙǚǕǖŰűŨũṸṹŲųṺṻỦủȔȕȖȗƯưỨứỪừỮữỬửỰựỤụṲṳṶṷṴṵɄʉᵾᶙᴜＵｕ".IndexOf(c) >= 0)
                {
                    outString.Append("u");
                }
                else if ("VvВвṼṽṾṿƲʋᶌᶌⱱⱴᴠＶｖ".IndexOf(c) >= 0)
                {
                    outString.Append("v");
                }
                else if ("WwΩωΏώẂẃẀẁŴŵẄẅẆẇẈẉẘẘⱲⱳᴡＷｗ".IndexOf(c) >= 0)
                {
                    outString.Append("w");
                }
                else if ("XxΧχХхẌẍẊẋᶍＸｘ".IndexOf(c) >= 0)
                {
                    outString.Append("x");
                }
                //TODO: I put the Russian Ёё with y, but I am not sure whether that is correct
                else if ("YyΥυЁёΎύÝýỲỳŶŷẙŸÿỸỹẎẏȲȳỶỷỴỵɎɏƳƴʏＹｙ".IndexOf(c) >= 0)
                {
                    outString.Append("y");
                }
                else if ("ZzŽžζΖЗзŹźẐẑŻżẒẓẔẕƵƶȤȥⱫⱬᵶᶎʐʑɀᴢＺｚ".IndexOf(c) >= 0)
                {
                    outString.Append("z");
                }
                else if ("ÆæᴁᴭᵆǼǽǢǣᴂᴂᴔÆæᴁᴭᵆǼǽǢǣ".IndexOf(c) >= 0)
                {
                    outString.Append("ae");
                }
                else if ("θΘ".IndexOf(c) >= 0)
                {
                    outString.Append("th");
                }
                else if ("Яя".IndexOf(c) >= 0)
                {
                    outString.Append("ya");
                }
                else if ("Чч".IndexOf(c) >= 0)
                {
                    outString.Append("ch");
                }
                else if ("Щщ".IndexOf(c) >= 0)
                {
                    outString.Append("shc");
                }
                else if ("ЪъЬь".IndexOf(c) >= 0)
                {
                    //Do not append anything for these characters...
                    //These are Russian softening and hardening symbols that would disappear in English...
                }
                else if ("ψΨ".IndexOf(c) >= 0)
                {
                    outString.Append("ps");
                }
                else if ("ξΞ".IndexOf(c) >= 0)
                {
                    outString.Append("ks");
                }
                else if ("Œœ".IndexOf(c) >= 0)
                {
                    outString.Append("oe");
                }
                else if ("ᵫ".IndexOf(c) >= 0)
                {
                    outString.Append("ue");
                }
                else if ("ﬁ".IndexOf(c) >= 0)
                {
                    outString.Append("fi");
                }
                else if ("ﬂ".IndexOf(c) >= 0)
                {
                    outString.Append("fl");
                }
                else if ("ǱǲǳǄǅǆ".IndexOf(c) >= 0)
                {
                    outString.Append("dz");
                }
                else if ("Ĳĳ".IndexOf(c) >= 0)
                {
                    outString.Append("ij");
                }
                else if ("Ǉǈǉ".IndexOf(c) >= 0)
                {
                    outString.Append("lj");
                }
                else if ("Ǌǋǌ".IndexOf(c) >= 0)
                {
                    outString.Append("nj");
                }
                else if ("ᴔ".IndexOf(c) >= 0)
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
