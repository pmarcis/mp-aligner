//
//  BezierFunction.cs
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
using System.Xml.Serialization;

namespace MPAligner
{
	/*
	 * The original code comes from a JavaScript implementation of Bezier curves at: http://greweb.me/2012/02/bezier-curve-based-easing-functions-from-concept-to-implementation/.
	 * This class wraps the calculation part of Bezier curves into a linear Bezier curve implementation.
	 * 
	 */

	/** MIT License
    *
    * KeySpline - use bezier curve for transition easing function
    * Copyright (c) 2012 Gaetan Renaudeau <renaudeau.gaetan@gmail.com>
    * 
    * Permission is hereby granted, free of charge, to any person obtaining a
    * copy of this software and associated documentation files (the "Software"),
    * to deal in the Software without restriction, including without limitation
    * the rights to use, copy, modify, merge, publish, distribute, sublicense,
    * and/or sell copies of the Software, and to permit persons to whom the
    * Software is furnished to do so, subject to the following conditions:
    * 
    * The above copyright notice and this permission notice shall be included in
    * all copies or substantial portions of the Software.
    * 
    * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
    * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
    * DEALINGS IN THE SOFTWARE.
    */
    [Serializable]
    public class BezierFunction
    {
        [XmlAttribute("mx1")]
        public double _mX1;
        [XmlAttribute("my1")]
        public double _mY1;
        [XmlAttribute("mx2")]
        public double _mX2;
        [XmlAttribute("my2")]
        public double _mY2;

        public BezierFunction ()
        {
            _mX1 = 0.1;
            _mY1 = 0;
            _mX2 = 0;
            _mY2 = 1;
        }

        public BezierFunction (double mX1, double mY1, double mX2, double mY2)
        {
            _mX1 = mX1;
            _mY1 = mY1;
            _mX2 = mX2;
            _mY2 = mY2;
        }
        
        public double Get(double aX)
        {
            if (_mX1 == _mY1 && _mX2==_mY2) return aX; // linear
            return CalcBezier(GetTForX(aX),_mY1,_mY2);
        }
        
        double A(double aA1, double aA2) { return 1.0 - 3.0 * aA2 + 3.0 * aA1; }
        double B(double aA1, double aA2) { return 3.0 * aA2 - 6.0 * aA1; }
        double C(double aA1) { return 3.0 * aA1; }
        
          // Returns x(t) given t, x1, and x2, or y(t) given t, y1, and y2.
          double CalcBezier(double aT, double aA1, double aA2) {
            return ((A(aA1, aA2)*aT + B(aA1, aA2))*aT + C(aA1))*aT;
          }
        
        
          // Returns dx/dt given t, x1, and x2, or dy/dt given t, y1, and y2.
          double GetSlope(double aT, double aA1, double aA2) {
            return 3.0 * A(aA1, aA2)*aT*aT + 2.0 * B(aA1, aA2) * aT + C(aA1);
          }
        
        
          double GetTForX(double aX) {
            // Newton raphson iteration
            double aGuessT = aX;
            for (int i = 0; i < 4; ++i) {
                  double currentSlope = GetSlope(aGuessT, _mX1, _mX2);
                  if (currentSlope == 0.0) return aGuessT;
                  double currentX = CalcBezier(aGuessT, _mX1, _mX2) - aX;
                  aGuessT -= currentX / currentSlope;
            }
            return aGuessT;
          }
    }
}

