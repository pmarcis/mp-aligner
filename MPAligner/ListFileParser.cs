//
//  ListFileParser.cs
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
using System.Globalization;
using System.Text;
using System.IO;

namespace MPAligner
{
    public class ListFileParser
    {
        public ListFileParser ()
        {
        }
        
        public static Dictionary<string,SimpleTermEntry> Parse(string file, Encoding enc)
        {
            Dictionary<string, SimpleTermEntry> res = new Dictionary<string, SimpleTermEntry>();
            StreamReader sr = new StreamReader(file, enc);
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (!res.ContainsKey(line))
                    {
                        res.Add(line,new SimpleTermEntry(line,"","","","","",1,1));
                    }
                    else
                    {
                        res[line].count++;
                    }
                }
            }
            return res;
        }

        public static List<string> ParseList(string file, Encoding enc)
        {
            List<string> res = new List<string>();
            StreamReader sr = new StreamReader(file, enc);
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    res.Add(line);
                }
            }
            return res;
        }
    }
}

