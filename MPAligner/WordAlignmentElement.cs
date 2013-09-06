//
//  WordAlignmentElement.cs
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
using System.Text;

namespace MPAligner
{
    public class WordAlignmentElement : IComparable<WordAlignmentElement>
    {
        public WordAlignmentElement ()
        {
            isLevenshtein = false;
        }
        
        [XmlAttribute("fromStartIndex")]
        public int fromStartIndex;
        [XmlAttribute("toStartIndex")]
        public int toStartIndex;
        [XmlAttribute("isLevenshtein")]
        public bool isLevenshtein;
        
        [XmlAttribute("toLen")]
        public double toLen;
        [XmlAttribute("fromLen")]
        public double fromLen;
        [XmlAttribute("fromOverlap")]
        public double fromOverlap;
        [XmlAttribute("toOverlap")]
        public double toOverlap;
        [XmlAttribute("fromId")]
        public int fromId;
        [XmlAttribute("toId")]
        public int toId;
        /// <summary>
        /// The type of the from element.
        /// 0 - dictionary,
        /// 1 - simple translit,
        /// 2 - target,
        /// 3 - translit
        /// </summary>
         [XmlAttribute("fromType")]
        public short fromType;
        /// <summary>
        /// The type of the to element.
        /// 0 - dictionary,
        /// 1 - simple translit,
        /// 2 - target,
        /// 3 - translit
        /// </summary>
        [XmlAttribute("toType")]
        public short toType;
        [XmlAttribute("fromTypeId")]
        public int fromTypeId;
        [XmlAttribute("toTypeId")]
        public int toTypeId;
        /// <summary>
        /// The direction.
        /// 1 - src to trg
        /// -1 - trg to src
        /// </summary>
        public int direction = 1;
        [XmlIgnore]
        public bool[] alignmentMap;
        [XmlText]
        public string alignmentMapStr {
            get {
                if (alignmentMap!=null)
                {
                    StringBuilder sb = new StringBuilder(alignmentMap.Length);
                    foreach(bool alingment in alignmentMap)
                    {
                        sb.Append(alingment.ToString());
                    }
                    return sb.ToString();
                }
                return null;
            }
            set {
            }
        }

        #region IComparable implementation
        public int CompareTo (WordAlignmentElement other)
        {
            // Ascending sort of toId's if the 
            if (this.fromId == other.fromId)
            {
                if (this.toId == other.toId)
                {
                    return this.fromStartIndex.CompareTo(other.fromStartIndex);
                }
                return this.fromStartIndex.CompareTo(other.fromStartIndex);
            }
            if (this.toId==other.toId)
            {
                return this.toStartIndex.CompareTo(other.toStartIndex);
            }
            // Default to salary sort. [High to low]
            return this.fromId.CompareTo(other.fromId);
        }
        #endregion

    }
}

