using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace ConsolidateEurovocResults
{
    class MainClass
    {

        static Dictionary<string,bool> directSupport = new Dictionary<string, bool>();
        static Dictionary<string,bool> interlinguaSupport = new Dictionary<string, bool>();
        static Dictionary<string,bool> partialInterlinguaSupport = new Dictionary<string, bool>();
        //Dictionary<string,string> lackingSupport = new Dictionary<string, string>();

        static void FillDirectSupportDict ()
        {
            directSupport = new Dictionary<string, bool>();
            directSupport.Add("lv_en", true);
            directSupport.Add("lt_en", true);
            directSupport.Add("de_en", true);
            directSupport.Add("et_en", true);
            directSupport.Add("es_en", true);
            directSupport.Add("el_en", true);
            directSupport.Add("hr_en", true);
            directSupport.Add("ro_en", true);
            directSupport.Add("sl_en", true);
            directSupport.Add("lv_lt", true);
            directSupport.Add("lt_lv", true);
            directSupport.Add("en_lt", true);
            directSupport.Add("en_lv", true);
            directSupport.Add("en_hr", true);
            directSupport.Add("en_de", true);
            directSupport.Add("en_et", true);
            directSupport.Add("en_el", true);
            directSupport.Add("en_ro", true);
            directSupport.Add("en_sl", true);
            directSupport.Add("cs_en", true);
            directSupport.Add("da_en", true);
            directSupport.Add("fr_en", true);
            directSupport.Add("bg_en", true);
            directSupport.Add("it_en", true);
            directSupport.Add("hu_en", true);
            directSupport.Add("mt_en", true);
            directSupport.Add("nl_en", true);
            directSupport.Add("pl_en", true);
            directSupport.Add("pt_en", true);
            directSupport.Add("sk_en", true);
            directSupport.Add("fi_en", true);
            directSupport.Add("sv_en", true);
            directSupport.Add("en_es", true);
            directSupport.Add("en_cs", true);
            directSupport.Add("en_da", true);
            directSupport.Add("en_fr", true);
            directSupport.Add("en_bg", true);
            directSupport.Add("en_it", true);
            directSupport.Add("en_hu", true);
            directSupport.Add("en_mt", true);
            directSupport.Add("en_nl", true);
            directSupport.Add("en_pl", true);
            directSupport.Add("en_pt", true);
            directSupport.Add("en_sk", true);
            directSupport.Add("en_fi", true);
            directSupport.Add("en_sv", true);
        }
        static void FillInterlinguaSupportDict ()
        {
            interlinguaSupport = new Dictionary<string, bool>();
            string[] arr = {"lv","lt","et","de","ro","sl","el","hr","es","cs","da","fr","bg","it","hu","mt","nl","pl","pt","sk","fi","sv"};
            for (int i=0; i<arr.Length; i++) {
                for (int j=0; j<arr.Length; j++) {
                    if (i==j) continue;
                    interlinguaSupport.Add(arr[i]+"_"+arr[j],true);
                }
            }
        }
        
        static void FillPartialInterlinguaSupportDict ()
        {
            partialInterlinguaSupport = new Dictionary<string, bool>();
            string[] arr = {"bg","es","cs","da","fr","hu","it","mt","nl","pl","pt","sk","fi","sv","lv","lt","et","de","ro","sl","el","hr"};
            string[] arr2 = {"sr"};
            for (int i=0; i<arr.Length; i++) {
                for (int j=0; j<arr2.Length; j++) {
                    //if (i==j) continue;
                    partialInterlinguaSupport.Add(arr[i]+"_"+arr2[j],true);
                }
            }
            for (int i=0; i<arr2.Length; i++) {
                for (int j=0; j<arr.Length; j++) {
                    //if (i==j) continue;
                    partialInterlinguaSupport.Add(arr2[i]+"_"+arr[j],true);
                }
            }
        }

        public static void Main (string[] args)
        {
            FillDirectSupportDict();
            FillInterlinguaSupportDict();
            FillPartialInterlinguaSupportDict ();
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.CurrencyDecimalSeparator = ".";
            nfi.NumberDecimalSeparator = ".";
            nfi.PercentDecimalSeparator = ".";
            List<string> inputFiles = new List<string>();
            string outputFile = "";
            for (int i=0;i<args.Length;i++)
            {
                if (i+1==args.Length)
                {
                    outputFile = args[i];
                    break;
                }
                else
                {
                    inputFiles.Add(args[i]);
                }
            }
            List<string> langs = new List<string>();
            StreamWriter sw = new StreamWriter (outputFile, false, new UTF8Encoding (false));
            Dictionary<string,Dictionary<string,List<List<double>>>> scores = new Dictionary<string, Dictionary<string, List<List<double>>>>();
            char[] sep={'\t'};
            foreach(string inputFile in inputFiles)
            {
                StreamReader sr = new StreamReader (inputFile, Encoding.UTF8);
                while (!sr.EndOfStream) {
                    string line = sr.ReadLine().Trim().Trim(new char[]{'\uFEFF','\u200B'}).Replace(',','.');
                    string[] arr = line.Split(sep, StringSplitOptions.None);
                    if (arr.Length==9)
                    {
                        string srcLang = arr[0];
                        string trgLang = arr[1];
                        if (!langs.Contains(srcLang)) langs.Add(srcLang);
                        if (!langs.Contains(trgLang)) langs.Add(trgLang);
                        if (!scores.ContainsKey(srcLang)) scores.Add(srcLang,new Dictionary<string, List<List<double>>>());
                        if (!scores[srcLang].ContainsKey(trgLang)) scores[srcLang].Add(trgLang,new List<List<double>>());
                        List<double> list = new List<double>();
                        for (int i=2;i<arr.Length;i++)
                        {
                            double t = Convert.ToDouble(arr[i],nfi);
                            list.Add(t);
                        }
                        scores[srcLang][trgLang].Add(list);
                    }
                }
                sr.Close();
            }
            langs.Sort();
            //Comment out the next 3 lines if you have a different list (I wanted to have a custom order, therefore I hardcoded the list)...
            langs.Clear();
            //string[] langArr = {"de","el","en","et","hr","lt","lv","ro","sl","bg","es","cs","pl","sk","da","sv","fr","hu","mt","nl","pt","fi","sr"};
            string[] langArr = {"da","de","en","nl","sv","el","lt","lv","es","fr","it","pt","ro","bg","cs","hr","pl","sk","sl","sr","mt","et","fi","hu"};
            /*
             * Baltic: lt, lv
             * West Slavic: cs, pl, sk
             * South Slavic: bg, hr, sl, sr 
             * Slavic: bg, cs, hr, pl, sk, sl, sr 
             * North Germanic: da, sv
             * West Germanic: en, nl
             * East Germanic: de
             * Germanic: da, de, en, nl, sv
             * Italic: es, fr, it, pt, ro
             * Hellenic: el
             * Siculo-Arabic: mt (maltese)
             * Finno-Ugric: et, fi, hu
            */

            langs.AddRange(langArr);
            sw.WriteLine("<table>");
            sw.WriteLine("<tr><td style=\"text-align:center;font-weight:bold;\" colspan=\""+(langs.Count+1).ToString()+"\">MPAligner evaluation results on EUROVOC.</td></tr>");
            sw.WriteLine("<tr><td style=\"text-align:left;\" colspan=\""+(langs.Count+1).ToString()+"\">Each result cell for a source-to-target language alignment consists of: <span style=\"color:red;\">precision in red</span> / <span style=\"color:green;\">recall in green</span> / <span style=\"color:blue;\">F1-measure in blue</span>; the corresponding MPAligner alignment score threshold is in black.</td></tr>");
            sw.WriteLine("<tr><td style=\"text-align:left;\" colspan=\""+(langs.Count+1).ToString()+"\">Cell backgrounds denote the following:<br />"+
                         "&nbsp;&nbsp;&nbsp;&nbsp;Green - the source-to-target language terms were aligned using a direct dictionary and transliteration (source-to-target and target-to-source) support;<br />"+
                         "&nbsp;&nbsp;&nbsp;&nbsp;Red - the source-to-target language terms were aligned through interlingua (English) dictionary and transliteration (source-to-English and target-to-English) support;<br />"+
                         "&nbsp;&nbsp;&nbsp;&nbsp;Blue - the source-to-target language terms were aligned through interlingua (English) dictionary and transliteration for one language (source-to-English or target-to-English) support;<br />"+
                         "&nbsp;&nbsp;&nbsp;&nbsp;Light grey - the source-to-target language terms were aligned through simple character transliteration support.</td></tr>");
            sw.WriteLine("<tr><td style=\"text-align:left;\" colspan=\""+(langs.Count+1).ToString()+"\">The alignment with a top one target alignment for each source language term.</td></tr>");
            sw.WriteLine("<tr></tr>");
            sw.WriteLine("<tr><td style=\"text-align:center;\" colspan=\""+(langs.Count+1).ToString()+"\">Maximal F-measure results. Rows - source language; Columns - target language.</td></tr>");
            PrintTopFMeasureStatsTable(scores,langs,sw);
            sw.WriteLine("<tr></tr>");
            sw.WriteLine("<tr><td style=\"text-align:center;\" colspan=\""+(langs.Count+1).ToString()+"\">Results with precision above 90%. Rows - source language; Columns - target language.</td></tr>");
            PrintMinPrecisionStatsTable(scores,langs,sw,90);
            sw.WriteLine("<tr></tr>");
            sw.WriteLine("<tr><td style=\"text-align:center;\" colspan=\""+(langs.Count+1).ToString()+"\">Results with precision above 95%. Rows - source language; Columns - target language.</td></tr>");
            PrintMinPrecisionStatsTable(scores,langs,sw,95);
            sw.WriteLine("<tr></tr>");
            sw.WriteLine("<tr><td style=\"text-align:center;\" colspan=\""+(langs.Count+1).ToString()+"\">Maximal F-measure results. with EN as the source language.</td></tr>");
            PrintEnToOtherFMeasureResults(scores,langs,sw);
            /*sw.WriteLine("<tr></tr>");
            sw.WriteLine("<tr><td>Results with recall above 50%. Rows - source language; Columns - target language.</td></tr>");
            PrintMinRecallStatsTable(scores,langs,sw,50);
            sw.WriteLine("<tr></tr>");
            sw.WriteLine("<tr><td>Results with recall above 40%. Rows - source language; Columns - target language.</td></tr>");
            PrintMinRecallStatsTable(scores,langs,sw,40);
            sw.WriteLine("<tr></tr>");
            sw.WriteLine("<tr><td>Results with recall above 30%. Rows - source language; Columns - target language.</td></tr>");
            PrintMinRecallStatsTable(scores,langs,sw,30);*/
            sw.WriteLine("</table>");
            sw.Close();
            return;
        }

        static void PrintTopFMeasureStats (Dictionary<string, Dictionary<string, List<List<double>>>> scores, List<string> langs, StreamWriter sw)
        {
            StringBuilder sb = new StringBuilder (50000);
            foreach (string srcLang in langs) {
                sb.Append("\t");
                sb.Append(srcLang);
            }
            sb.AppendLine();
            foreach (string srcLang in langs) {
                StringBuilder sbLine = new StringBuilder(1000);
                sbLine.Append(srcLang);
                foreach(string trgLang in langs)
                {
                    sbLine.Append("\t");
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        double max = Double.MinValue;
                        int maxId = -1;
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][6]>max)
                            {
                                max = scores[srcLang][trgLang][i][6];
                                maxId = i;
                            }
                        }
                        if (maxId>=0)
                        {
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][0],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][4],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][5],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][6],1));
                        }
                    }
                }
                sb.AppendLine(sbLine.ToString());
            }
            sw.WriteLine(sb.ToString().Replace(".",","));
        }

        static string GetColor (double minFM, double maxFM, double score, int r, int g, int b)
        {
            double interval = maxFM - minFM;
            double x = score - minFM;
            double negAddition = x * 25 / interval;
            int addition = 0;//25 - Convert.ToInt32(negAddition);
            Color fmC = Color.FromArgb (r + addition, g + addition, b + addition);
            return String.Format ("#{0}{1}{2}{3}", String.Empty, fmC.R.ToString ("X2"), fmC.G.ToString ("X2"), fmC.B.ToString ("X2"));
        }

        static void PrintEnToOtherFMeasureResults(Dictionary<string, Dictionary<string, List<List<double>>>> scores, List<string> langs, StreamWriter sw)
        {
            StringBuilder sb = new StringBuilder (50000);
            sb.Append("<tr>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">Target language</td>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">P</td>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">R</td>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">F1</td>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">Thr</td>");
            sb.Append("</tr>");
            sb.AppendLine();
            string srcLang = "en";
            foreach(string trgLang in langs)
            {
                if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                {
                    double max = Double.MinValue;
                    int maxId = -1;
                    for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                    {
                        if (scores[srcLang][trgLang][i][6]>max)
                        {
                            max = scores[srcLang][trgLang][i][6];
                            maxId = i;
                        }
                    }
                    if (maxId>=0)
                    {
                        sb.Append("<tr>");
                        string fmColor = GetColor (0,1,scores[srcLang][trgLang][maxId][6],220,220,220);
                        //string fmColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],121,135,147);
                        //string pColor = GetColor (minP,maxP,scores[srcLang][trgLang][maxId][4],100,0,0);
                        //string rColor = GetColor (minR,maxR,scores[srcLang][trgLang][maxId][5],0,100,0);
                        //string pColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],152,128,114);
                        string pColor = GetColor (0,1,scores[srcLang][trgLang][maxId][6],185,185,185);
                        //string rColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],126,139,118);
                        string rColor = GetColor (0,1,scores[srcLang][trgLang][maxId][6],150,150,150);
                        if (directSupport.ContainsKey(srcLang+"_"+trgLang))
                        {
                            sb.Append("<td style=\"background-color:"+rColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                        }
                        else if (interlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                        {
                            sb.Append("<td style=\"background-color:"+pColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                        }
                        else if (partialInterlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                        {
                            sb.Append("<td style=\"background-color:"+fmColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                        }
                        else
                        {
                            //sbLine.Append("<td style=\"background-color:#EDEDED;border: thin solid black;text-align:center;vertical-align:middle;\">");
                            sb.Append("<td style=\"background-color:#FFFFFF;border: thin solid black;text-align:center;vertical-align:middle;\">");
                        }
                        sb.Append(trgLang);
                        sb.AppendLine("</td>");

                        sb.Append("<td style=\"background-color:#FFFFFF;border: thin solid black;vertical-align:middle;text-align:center;\">");
                        sb.Append(Math.Round(scores[srcLang][trgLang][maxId][4],1).ToString("0.0"));
                        sb.AppendLine("</td>");
                        sb.Append("<td style=\"background-color:#FFFFFF;border: thin solid black;vertical-align:middle;text-align:center;\">");
                        sb.Append(Math.Round(scores[srcLang][trgLang][maxId][5],1).ToString("0.0"));
                        sb.AppendLine("</td>");
                        sb.Append("<td style=\"background-color:#FFFFFF;border: thin solid black;vertical-align:middle;text-align:center;\">");
                        sb.Append(Math.Round(scores[srcLang][trgLang][maxId][6],1).ToString("0.0"));
                        sb.AppendLine("</td>");
                        sb.Append("<td style=\"background-color:#FFFFFF;border: thin solid black;vertical-align:middle;text-align:center;\">");
                        sb.Append(Math.Round(scores[srcLang][trgLang][maxId][0],2).ToString("0.0"));
                        sb.AppendLine("</td>");
                        sb.AppendLine("</tr>");
                    }
                    else
                    {
                        sb.AppendLine("<tr>");
                        sb.Append("<td style=\"border: thin solid black;text-align:center;vertical-align:middle;\">");
                        sb.Append("-");
                        sb.AppendLine("</td>");
                        sb.AppendLine("</tr>");
                        sw.WriteLine(sb.ToString().Replace(".",","));
                    }
                }
            }
            sw.WriteLine(sb.ToString().Replace(".",","));
        }

        static void PrintTopFMeasureStatsTable (Dictionary<string, Dictionary<string, List<List<double>>>> scores, List<string> langs, StreamWriter sw)
        {
            double minFM = 1;
            double maxFM = 0;
            double minP = 1;
            double maxP = 0;
            double minR = 1;
            double maxR = 0;
            foreach (string srcLang in langs) {
                foreach(string trgLang in langs)
                {
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][6]>maxFM) maxFM = scores[srcLang][trgLang][i][6];
                            if (scores[srcLang][trgLang][i][6]<minFM) minFM = scores[srcLang][trgLang][i][6];
                            if (scores[srcLang][trgLang][i][4]>maxP) maxP = scores[srcLang][trgLang][i][4];
                            if (scores[srcLang][trgLang][i][4]<minP) minP = scores[srcLang][trgLang][i][4];
                            if (scores[srcLang][trgLang][i][5]>maxR) maxR = scores[srcLang][trgLang][i][5];
                            if (scores[srcLang][trgLang][i][5]<minR) minR = scores[srcLang][trgLang][i][5];
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder (50000);
            sb.Append("<tr>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\"></td>");
            foreach (string srcLang in langs) {
                sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">");
                sb.Append(srcLang);
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            sb.AppendLine();
            foreach (string srcLang in langs) {
                StringBuilder sbLine = new StringBuilder(1000);
                sbLine.Append("<tr>");
                sbLine.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">");
                sbLine.Append(srcLang);
                sbLine.Append("</td>");
                foreach(string trgLang in langs)
                {
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        double max = Double.MinValue;
                        int maxId = -1;
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][6]>max)
                            {
                                max = scores[srcLang][trgLang][i][6];
                                maxId = i;
                            }
                        }
                        if (maxId>=0)
                        {
                            string fmColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],220,220,220);
                            //string fmColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],121,135,147);
                            //string pColor = GetColor (minP,maxP,scores[srcLang][trgLang][maxId][4],100,0,0);
                            //string rColor = GetColor (minR,maxR,scores[srcLang][trgLang][maxId][5],0,100,0);
                            //string pColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],152,128,114);
                            string pColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],185,185,185);
                            //string rColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],126,139,118);
                            string rColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],150,150,150);
                            if (directSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+rColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else if (interlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+pColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else if (partialInterlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+fmColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else
                            {
                                //sbLine.Append("<td style=\"background-color:#EDEDED;border: thin solid black;text-align:center;vertical-align:middle;\">");
                                sbLine.Append("<td style=\"background-color:#FFFFFF;border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }

                            //sbLine.Append("<span style=\"color:"+pColor+"\">");
                            sbLine.Append("<span style=\"color:red\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][4],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append(" ");
                            //sbLine.Append("<span style=\"color:"+rColor+"\">");
                            sbLine.Append("<span style=\"color:green\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][5],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append("<br />");
                            //sbLine.Append("<span style=\"color:"+fmColor+"\">");
                            sbLine.Append("<span style=\"color:blue\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][6],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append(" ");
                            sbLine.Append("<span style=\"font-size:8pt;\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][maxId][0],2).ToString("0.00"));
                            sbLine.Append("</span>");
                            sbLine.Append("</td>");
                        }
                        else
                        {
                            sbLine.Append("<td style=\"border: thin solid black;text-align:center;vertical-align:middle;\">");
                            sbLine.Append("-");
                            sbLine.Append("</td>");
                        }
                    }
                    else if (srcLang==trgLang)
                    {
                        sbLine.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;text-align:center;vertical-align:middle;\">");
                        sbLine.Append("-");
                        sbLine.Append("</td>");
                    }
                    else
                    {
                        sbLine.Append("<td style=\"border: thin solid black;text-align:center;vertical-align:middle;\">");
                        sbLine.Append("-");
                        sbLine.Append("</td>");
                    }
                }
                sbLine.Append("</tr>");
                sb.AppendLine(sbLine.ToString());
            }
            sw.WriteLine(sb.ToString().Replace(".",","));
        }

        static void PrintMinPrecisionStatsTable (Dictionary<string, Dictionary<string, List<List<double>>>> scores, List<string> langs, StreamWriter sw, double minThr)
        {
            double minFM = 1;
            double maxFM = 0;
            double minP = 1;
            double maxP = 0;
            double minR = 1;
            double maxR = 0;
            foreach (string srcLang in langs) {
                foreach(string trgLang in langs)
                {
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][4]>=minThr){
                                if (scores[srcLang][trgLang][i][6]>maxFM) maxFM = scores[srcLang][trgLang][i][6];
                                if (scores[srcLang][trgLang][i][6]<minFM) minFM = scores[srcLang][trgLang][i][6];
                                if (scores[srcLang][trgLang][i][4]>maxP) maxP = scores[srcLang][trgLang][i][4];
                                if (scores[srcLang][trgLang][i][4]<minP) minP = scores[srcLang][trgLang][i][4];
                                if (scores[srcLang][trgLang][i][5]>maxR) maxR = scores[srcLang][trgLang][i][5];
                                if (scores[srcLang][trgLang][i][5]<minR) minR = scores[srcLang][trgLang][i][5];
                            }
                        }
                    }
                }
            }
            StringBuilder sb = new StringBuilder (50000);
            sb.Append("<tr>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\"></td>");
            foreach (string srcLang in langs) {
                sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">");
                sb.Append(srcLang);
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            sb.AppendLine();
            foreach (string srcLang in langs) {
                StringBuilder sbLine = new StringBuilder(1000);
                sbLine.Append("<tr>");
                sbLine.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">");
                sbLine.Append(srcLang);
                sbLine.Append("</td>");
                foreach(string trgLang in langs)
                {
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        double min = Double.MaxValue;
                        int minId = -1;
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][4]<=min && scores[srcLang][trgLang][i][4]>=minThr)
                            {
                                min = scores[srcLang][trgLang][i][4];
                                minId = i;
                            }
                        }
                        if (minId>=0)
                        {
                            string fmColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][minId][6],220,220,220);
                            //string fmColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],121,135,147);
                            //string pColor = GetColor (minP,maxP,scores[srcLang][trgLang][maxId][4],100,0,0);
                            //string rColor = GetColor (minR,maxR,scores[srcLang][trgLang][maxId][5],0,100,0);
                            //string pColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],152,128,114);
                            string pColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][minId][6],185,185,185);
                            //string rColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],126,139,118);
                            string rColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][minId][6],150,150,150);
                            if (directSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+rColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else if (interlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+pColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else if (partialInterlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+fmColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else
                            {
                                sbLine.Append("<td style=\"background-color:#EDEDED;border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            
                            //sbLine.Append("<span style=\"color:"+pColor+"\">");
                            sbLine.Append("<span style=\"color:red\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][4],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append(" ");
                            //sbLine.Append("<span style=\"color:"+rColor+"\">");
                            sbLine.Append("<span style=\"color:green\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][5],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append("<br />");
                            //sbLine.Append("<span style=\"color:"+fmColor+"\">");
                            sbLine.Append("<span style=\"color:blue\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][6],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append(" ");
                            sbLine.Append("<span style=\"font-size:8pt;\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][0],2).ToString("0.00"));
                            sbLine.Append("</span>");
                            sbLine.Append("</td>");
                        }
                        else
                        {
                            sbLine.Append("<td style=\"border: thin solid black;text-align:center;vertical-align:middle;\">");
                            sbLine.Append("-");
                            sbLine.Append("</td>");
                        }
                    }
                    else if (srcLang==trgLang)
                    {
                        sbLine.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;text-align:center;vertical-align:middle;\">");
                        sbLine.Append("-");
                        sbLine.Append("</td>");
                    }
                    else
                    {
                        sbLine.Append("<td style=\"border: thin solid black;text-align:center;vertical-align:middle;\">");
                        sbLine.Append("-");
                        sbLine.Append("</td>");
                    }
                }
                sbLine.Append("</tr>");
                sb.AppendLine(sbLine.ToString());
            }
            sw.WriteLine(sb.ToString().Replace(".",","));
        }

        static void PrintMinPrecisionStats (Dictionary<string, Dictionary<string, List<List<double>>>> scores, List<string> langs, StreamWriter sw, double minThr)
        {
            StringBuilder sb = new StringBuilder (50000);
            foreach (string srcLang in langs) {
                sb.Append("\t");
                sb.Append(srcLang);
            }
            sb.AppendLine();
            foreach (string srcLang in langs) {
                StringBuilder sbLine = new StringBuilder(1000);
                sbLine.Append(srcLang);
                foreach(string trgLang in langs)
                {
                    sbLine.Append("\t");
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        double min = Double.MaxValue;
                        int minId = -1;
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][4]<=min && scores[srcLang][trgLang][i][4]>=minThr)
                            {
                                min = scores[srcLang][trgLang][i][4];
                                minId = i;
                            }
                        }
                        if (minId>=0)
                        {
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][0],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][4],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][5],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][6],1));
                        }
                    }
                }
                sb.AppendLine(sbLine.ToString());
            }
            sw.WriteLine(sb.ToString().Replace(".",","));
        }

        static void PrintMinRecallStatsTable (Dictionary<string, Dictionary<string, List<List<double>>>> scores, List<string> langs, StreamWriter sw, double minThr)
        {
            double minFM = 1;
            double maxFM = 0;
            double minP = 1;
            double maxP = 0;
            double minR = 1;
            double maxR = 0;
            foreach (string srcLang in langs) {
                foreach(string trgLang in langs)
                {
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][5]>=minThr){
                                if (scores[srcLang][trgLang][i][6]>maxFM) maxFM = scores[srcLang][trgLang][i][6];
                                if (scores[srcLang][trgLang][i][6]<minFM) minFM = scores[srcLang][trgLang][i][6];
                                if (scores[srcLang][trgLang][i][4]>maxP) maxP = scores[srcLang][trgLang][i][4];
                                if (scores[srcLang][trgLang][i][4]<minP) minP = scores[srcLang][trgLang][i][4];
                                if (scores[srcLang][trgLang][i][5]>maxR) maxR = scores[srcLang][trgLang][i][5];
                                if (scores[srcLang][trgLang][i][5]<minR) minR = scores[srcLang][trgLang][i][5];
                            }
                        }
                    }
                }
            }
            StringBuilder sb = new StringBuilder (50000);
            sb.Append("<tr>");
            sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\"></td>");
            foreach (string srcLang in langs) {
                sb.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">");
                sb.Append(srcLang);
                sb.Append("</td>");
            }
            sb.Append("</tr>");
            sb.AppendLine();
            foreach (string srcLang in langs) {
                StringBuilder sbLine = new StringBuilder(1000);
                sbLine.Append("<tr>");
                sbLine.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;vertical-align:middle;text-align:center;font-weight:bold;\">");
                sbLine.Append(srcLang);
                sbLine.Append("</td>");
                foreach(string trgLang in langs)
                {
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        double min = Double.MaxValue;
                        int minId = -1;
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][5]<=min && scores[srcLang][trgLang][i][5]>=minThr)
                            {
                                min = scores[srcLang][trgLang][i][5];
                                minId = i;
                            }
                        }
                        if (minId>=0)
                        {
                            
                            string fmColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][minId][6],220,220,220);
                            //string fmColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],121,135,147);
                            //string pColor = GetColor (minP,maxP,scores[srcLang][trgLang][maxId][4],100,0,0);
                            //string rColor = GetColor (minR,maxR,scores[srcLang][trgLang][maxId][5],0,100,0);
                            //string pColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],152,128,114);
                            string pColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][minId][6],185,185,185);
                            //string rColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][maxId][6],126,139,118);
                            string rColor = GetColor (minFM,maxFM,scores[srcLang][trgLang][minId][6],150,150,150);
                            if (directSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+rColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else if (interlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+pColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else if (partialInterlinguaSupport.ContainsKey(srcLang+"_"+trgLang))
                            {
                                sbLine.Append("<td style=\"background-color:"+fmColor+";border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            else
                            {
                                sbLine.Append("<td style=\"background-color:#EDEDED;border: thin solid black;text-align:center;vertical-align:middle;\">");
                            }
                            
                            //sbLine.Append("<span style=\"color:"+pColor+"\">");
                            sbLine.Append("<span style=\"color:red\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][4],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append(" ");
                            //sbLine.Append("<span style=\"color:"+rColor+"\">");
                            sbLine.Append("<span style=\"color:green\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][5],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append("<br />");
                            //sbLine.Append("<span style=\"color:"+fmColor+"\">");
                            sbLine.Append("<span style=\"color:blue\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][6],1).ToString("0.0"));
                            sbLine.Append("</span>");
                            sbLine.Append(" ");
                            sbLine.Append("<span style=\"font-size:8pt;\">");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][0],2).ToString("0.00"));
                            sbLine.Append("</span>");
                            sbLine.Append("</td>");
                        }
                        else
                        {
                            sbLine.Append("<td style=\"border: thin solid black;text-align:center;vertical-align:middle;\">");
                            sbLine.Append("-");
                            sbLine.Append("</td>");
                        }
                    }
                    else if (srcLang==trgLang)
                    {
                        sbLine.Append("<td style=\"background-color:#AAAAAA;border: thin solid black;text-align:center;vertical-align:middle;\">");
                        sbLine.Append("-");
                        sbLine.Append("</td>");
                    }
                    else
                    {
                        sbLine.Append("<td style=\"border: thin solid black;text-align:center;vertical-align:middle;\">");
                        sbLine.Append("-");
                        sbLine.Append("</td>");
                    }
                }
                sbLine.Append("</tr>");
                sb.AppendLine(sbLine.ToString());
            }
            sw.WriteLine(sb.ToString().Replace(".",","));
        }

        static void PrintMinRecallStats (Dictionary<string, Dictionary<string, List<List<double>>>> scores, List<string> langs, StreamWriter sw, double minThr)
        {
            StringBuilder sb = new StringBuilder (50000);
            foreach (string srcLang in langs) {
                sb.Append("\t");
                sb.Append(srcLang);
            }
            sb.AppendLine();
            foreach (string srcLang in langs) {
                StringBuilder sbLine = new StringBuilder(1000);
                sbLine.Append(srcLang);
                foreach(string trgLang in langs)
                {
                    sbLine.Append("\t");
                    if (scores.ContainsKey(srcLang)&&scores[srcLang].ContainsKey(trgLang))
                    {
                        double min = Double.MaxValue;
                        int minId = -1;
                        for(int i=0;i<scores[srcLang][trgLang].Count;i++)
                        {
                            if (scores[srcLang][trgLang][i][5]<=min && scores[srcLang][trgLang][i][5]>=minThr)
                            {
                                min = scores[srcLang][trgLang][i][5];
                                minId = i;
                            }
                        }
                        if (minId>=0)
                        {
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][0],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][4],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][5],1));
                            sbLine.Append("/");
                            sbLine.Append(Math.Round(scores[srcLang][trgLang][minId][6],1));
                        }
                    }
                }
                sb.AppendLine(sbLine.ToString());
            }
            sw.WriteLine(sb.ToString().Replace(".",","));
        }
    }
}
