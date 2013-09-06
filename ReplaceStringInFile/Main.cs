//
//  Main.cs
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
using System.IO;
using System.Text;

namespace ReplaceStringInFile
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            string text = File.ReadAllText(args[0],Encoding.UTF8);
            File.WriteAllText(args[1],text.Replace(args[2],args[3]),new UTF8Encoding(false));
        }
    }
}
