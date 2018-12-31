using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        public static string ReplaceSpecialCharsForUrl(this String url)
        {
            return url.Replace(" ", "%20").Replace("+", "%2B");
        }

        public static string FormatFileSize(this string str)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = double.Parse(str);
            int order = 0;

            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            string result = String.Format("{0:0.##} {1}", len, sizes[order]);

            return result;
        }

        public static bool ContainsAny(this string haystack, IEnumerable<string> needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToLowerCamelCase(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string ToDelimitedString(this List<string> list, string separator = ":", bool insertSpaces = false, string delimiter = "")
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string initialStr = list[i];
                var currentString = (delimiter == string.Empty) ? initialStr : String.Format("{1}{0}{1}", initialStr, delimiter);
                if (i < list.Count - 1)
                {
                    currentString += separator;
                    if (insertSpaces)
                    {
                        currentString += ' ';
                    }
                }
                result.Append(currentString);
            }
            return result.ToString();
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static bool IsNullOrZero(this string str)
        {
            if (String.IsNullOrWhiteSpace(str) || str == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsSplitable(this string str, char seperator)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            else
            {
                if (str.Split(seperator).Length > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region Converters

        public static Byte ToByte(this string str)
        {
            return Convert.ToByte(str);
        }

        public static Byte[] ToByteArray(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return Encoding.UTF8.GetBytes(str);
            }
        }

        public static Int16 ToInt16(this string str)
        {
            return Convert.ToInt16(str);
        }

        public static Int32 ToInt32(this string str)
        {
            return Convert.ToInt32(str);
        }

        public static Int64 ToInt64(this string str)
        {
            return Convert.ToInt64(str);
        }

        public static Single ToSingle(this string str)
        {
            return Convert.ToSingle(str);
        }

        public static Double ToDouble(this string str)
        {
            return Convert.ToDouble(str);
        }

        public static Decimal ToDecimal(this string str)
        {
            return Convert.ToDecimal(str);
        }

        #endregion
    }
}
