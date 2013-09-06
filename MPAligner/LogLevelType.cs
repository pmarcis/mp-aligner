//
//  LogLevelType.cs
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
    public enum LogLevelType
	{
		NONE = 0,
		FULL_OUTPUT = 1,
        LIMITED_OUTPUT = 2,
        WARNING = 3,
        ERROR = 4
    }
}

