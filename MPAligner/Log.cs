//
//  Log.cs
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

namespace MPAligner
{
    public class Log
    {
        public Log ()
        {
        }
        
        public static LogLevelType confLogLevel = LogLevelType.LIMITED_OUTPUT;
        
        public static void Write (string message, LogLevelType level, MPAlignerConfiguration conf=null)
        {
            if ((conf==null && level>=confLogLevel) || (conf!=null && level >= conf.logLevel && conf.logLevel!= LogLevelType.NONE)) {
                DateTime date = DateTime.Now;
                string dateStr = date.ToString("yyyy-MM-dd HH:mm:ss");
                if (level != LogLevelType.ERROR)
                {
                    Console.Write("[MPAligner] [");
                    Console.Write(level.ToString());
                    Console.Write("] ");
                    Console.Write(dateStr);
                    Console.Write(" ");
                    Console.WriteLine(message);
                }
                else
                {
                    Console.Error.Write("[MPAligner] [");
                    Console.Error.Write(level.ToString());
                    Console.Error.Write("] ");
                    Console.Error.Write(dateStr);
                    Console.Error.Write(" ");
                    Console.Error.WriteLine(message);
                }
            }
        }
    }
}

