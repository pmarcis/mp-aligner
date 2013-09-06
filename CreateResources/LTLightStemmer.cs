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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreateResources
{
    public class LTLightStemmer : Stemmer
    {
        Regex p8 = new Regex("(ąįį|ais|ąja|ąją|ąjį|ama|amą|ame|ami|amo|ams|amu|amų|ant|asi|ate|aus|čia|čią|čiu|čių|eis|ėje|ėme|ems|ėms|ėse|ėsi|ęsi|ėte|iai|iam|ias|iąs|iat|iau|iem|ies|įjį|ijų|ima|imą|ime|imi|imo|ims|imu|imų|int|ioj|iom|ion|ios|isi|ite|iui|iuj|ium|iuo|ius|kim|kis|kit|mis|nie|oje|oji|ojo|oma|omą|ome|omi|omo|oms|omu|omų|ose|osi|ote|sai|sią|sim|sis|sit|siu|šią|šim|šis|šit|šiu|tai|tam|tas|tis|tos|tum|tus|tųs|tys|udu|uje|ųjį|ųjų|umi|ums|uos|usi|usį|vęs|vim|vyj|yje|ymą|yme|ymo|ymu|ymų|yse)$");
        Regex p9 = new Regex("(ai|am|an|as|ąs|at|au|ei|ėj|ėm|ėn|es|ės|ęs|ėt|ia|ią|ie|im|in|io|is|įs|it|iu|ių|ki|ms|oj|om|on|os|ot|si|sų|ši|ta|tą|ti|tį|to|ts|tu|tų|ui|uj|um|uo|us|ūs|ve|vi|vo|yj|ys)$");
        Regex p10 = new Regex("(a|ą|e|ė|ę|i|į|k|o|s|š|t|u|ų|y)$");

        public string StemString(string s)
        {
            if (s.Length <= 3) return s;
            if (p8.IsMatch(s) && s.Length >= 5)
            {
                return s.Substring(0, s.Length - 3);
            }
            if (p9.IsMatch(s) && s.Length >= 4)
            {
                return s.Substring(0, s.Length - 2);
            }
            if (p10.IsMatch(s) && s.Length >= 3)
            {
                return s.Substring(0, s.Length - 1);
            }
            return s;
        }
    }
}
