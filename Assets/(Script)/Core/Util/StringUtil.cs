using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace edu.tnu.dgd.util
{
    public static class StringUtil
    {
        public static string[] HexTbl = Enumerable.Range(0, 256).Select(v => v.ToString("X2")).ToArray();

        public static string ToHex(this IEnumerable<byte> array)
        {
            return ToHex(array, "");
        }

        public static string ToHex(this IEnumerable<byte> array, string separator)
        {
            if (separator == null)
            {
                separator = "";
            }

            System.Text.StringBuilder s = new System.Text.StringBuilder();
            bool first = true;
            foreach (var v in array)
            {
                if (!first)
                {
                    s.Append(separator + HexTbl[v]);
                    first = false;
                }
                else
                {
                    s.Append(HexTbl[v]);
                }

            }
            return s.ToString();
        }

        public static string ToHex(this byte[] array)
        {
            return ToHex(array, "");
        }

        public static string ToHex(this byte[] array, string separator)
        {
            if (separator == null)
            {
                separator = "";
            }
            System.Text.StringBuilder s = new System.Text.StringBuilder(array.Length * (2 + separator.Length) - separator.Length);
            bool first = true;
            foreach (var v in array)
            {
                if (!first)
                {
                    s.Append(separator + HexTbl[v]);
                    first = false;
                }
                else
                {
                    s.Append(HexTbl[v]);
                }

            }
            return s.ToString();
        }
    }

}