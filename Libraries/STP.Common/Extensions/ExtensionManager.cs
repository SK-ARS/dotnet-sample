#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;

#endregion

namespace STP.Common.Extensions
{
    public static class ExtensionManager
    {
        /// <summary>
        ///     ExtractNumbers
        /// </summary>
        /// <param name="source"> </param>
        /// <returns> </returns>
        public static string ExtractNumbers(this string source)
        {
            return string.Join(null, Regex.Split(source, "[^\\d]"));
        }

        /// <summary>
        ///     Get the character at index location i.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static char CharAt(this string s, int i)
        {
            return s[i];
        }

        /// <summary>
        ///     Replace the string at the specified index i with replacement string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="replacement"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string ReplaceAt(this string s, string replacement, int i)
        {
            string prefix = string.Empty, postfix = string.Empty;
            try
            {
                prefix = s.Substring(0, i);
            }
            catch
            {
            }

            try
            {
                postfix = s.Substring(i + 1, s.Length - (i + 1));
            }
            catch
            {
            }

            return prefix + replacement + postfix;
        }

        /// <summary>
        ///     Get the character at index location i.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string StringAt(this string s, int i)
        {
            return s.Substring(i, 1);
        }

        public static bool IsNull(this string str)
        {
            return (string.IsNullOrEmpty(str));
        }

        public static string FromBytesToKiloBytes(this string str)
        {
            //1 kilobytes = 1024 bytes
            if (!string.IsNullOrEmpty(str))
            {
                return (decimal.Round((str.ToDouble()/1024).ToDecimal(), 2)).ToString(CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        public static string IfNull(this string str, string valueIfNull)
        {
            if (string.IsNullOrEmpty(str))
            {
                return valueIfNull;
            }
            return str;
        }

        public static string IfNullGetEmpty(this string str)
        {
            return IfNull(str, string.Empty);
        }

        /// <summary>
        ///     If str is Null Or Empty, get the replacement value.  Otherwise return the same string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replacementValue"></param>
        /// <returns></returns>
        public static string IfNullOrEmpty(this string str, string replacementValue)
        {
            string returnValue = str;
            if (string.IsNullOrEmpty(str))
            {
                returnValue = replacementValue;
            }
            return returnValue;
        }

        public static bool IsNullOrEquals(this string str, string equalsValue)
        {
            return (str.IsNull() || str == equalsValue);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        ///     Get the last "length" letters of a string. If the string is less than the length, then return the original string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Last(this string source, int length)
        {
            if (source.Length <= length)
            {
                return source;
            }
            return source.Substring(source.Length - length, length);
        }

        /// <summary>
        ///     Remove a character from a string. If the string is null or empty, return null or empty also.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="charToRemove"></param>
        /// <returns></returns>
        public static string RemoveChar(this string source, char charToRemove)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            var stringBuilder = new StringBuilder();
            foreach (char character in source)
            {
                if (!character.Equals(charToRemove))
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString();
        }

        public static string GetCompressedDottedString(this string val, int length)
        {
            string str;
            if (length == 0)
            {
                length = 10;
            }
            if (val.Length > length)
            {
                str = val.Substring(0, length);
                str = str + ".....";
            }
            else
            {
                str = val;
            }
            return str;
        }

        public static string Delimit(this IEnumerable<string> source, string delimiter)
        {
            if (source == null)
            {
                return null;
            }

            var stringBuilder = new StringBuilder();
            foreach (string item in source)
            {
                stringBuilder.Append(item);
                stringBuilder.Append(delimiter);
            }

            // remove the last delimiter if at least one item was added.
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Length = stringBuilder.Length - delimiter.Length;
            }
            return stringBuilder.ToString();
        }

        public static string DelimitIfNotNullOrEmpty(this IEnumerable<string> source, string delimiter)
        {
            return source.Where(p => !string.IsNullOrEmpty(p)).Delimit(delimiter);
        }

        public static string FilterCommaIfEmptyValues(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string[] filtersCommas = source.Split(",".ToCharArray());
                string s = filtersCommas.Where(sub => !string.IsNullOrEmpty(sub)).Aggregate(string.Empty,
                    (current, sub) =>
                        current + (sub + ", "));
                return s.TrimEnd(", ".ToCharArray()).TrimEnd(",".ToCharArray());
            }
            return string.Empty;
        }

        /// <summary>
        ///     Check if File Exist for extensions such as doc, docx, xls, xlsx, rtf, pdf, txt, wav, mp3, mp4, zip, rar, png, jpg,
        ///     jpeg, gif, bmp
        /// </summary>
        /// <param name="strFilePath"> </param>
        /// <returns> </returns>
        public static bool IsValidFile(this string strFilePath)
        {
            bool isValid = false;
            if (File.Exists(strFilePath))
            {
                isValid = IsValidExtension(strFilePath);
            }
            return isValid;
        }

        public static bool IfFileExist(this string filePath)
        {
            return File.Exists(HttpContext.Current.Server.MapPath(filePath));
        }

        /// <summary>
        ///     Check for Valid File Extension
        /// </summary>
        /// <param name="strExtension"> </param>
        /// <returns> </returns>
        public static bool IsValidExtension(this string strExtension)
        {
            bool isValid = false;
            switch (new FileInfo(strExtension).Extension.ToLower())
            {
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".rtf":
                case ".pdf":
                case ".txt":
                case ".wav":
                case ".mp3":
                case ".mp4":
                case ".zip":
                case ".rar":
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".bmp":
                    isValid = true;
                    break;
            }
            return isValid;
        }

        /// <summary>
        ///     Checks for valid Url
        /// </summary>
        /// <param name="url"> </param>
        /// <returns> </returns>
        public static bool IsUrl(this string url)
        {
            return Regex.IsMatch(url, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        /// <summary>
        ///     Checks for valid email
        /// </summary>
        /// <param name="email"> </param>
        /// <returns> </returns>
        public static bool IsValidEmail(this string email)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(email,
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        ///     ExtractFileName
        /// </summary>
        /// <param name="fullName"> </param>
        /// <returns> </returns>
        public static string ExtractFileName(this string fullName)
        {
            string szRetVal = string.Empty;
            if (fullName.Length > 0)
            {
                string szTmp = string.Empty;
                int j = fullName.Length - 1;
                if (j > 0)
                {
                    while (fullName[j] != '\\')
                    {
                        szTmp += fullName[j];
                        j--;
                    }
                    // you have the reversed file name
                    // replace characters...
                    int i = szTmp.Length;
                    while (i != 0)
                    {
                        i--;
                        szRetVal += szTmp[i];
                    }
                }
            }
            return szRetVal;
        }

        /// <summary>
        ///     Generate a random number
        /// </summary>
        /// <param name="iMin"> Min value </param>
        /// <param name="iMax"> Max Value </param>
        /// <param name="iCount"> Count </param>
        /// <returns> </returns>
        private static string RandomNumber(int iMin, int iMax, int iCount)
        {
            //ToDo: Move this code to Helper Utility for Everybody
            string s = null;
            int i;

            var rnd = new Random();
            if (iCount == 0)
            {
                iCount = 1;
            }
            for (i = 1; i <= iCount; i++)
            {
                ushort sngR =
                    Decimal.ToUInt16(
                        Decimal.Parse(((iMax - iMin + 1)*rnd.NextDouble() + iMin).ToString(CultureInfo.InvariantCulture)));
                s = s + Convert.ToString(sngR);
            }
            return s;
        }

        /// <summary>
        ///     Get file size conversions
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToSize(this double bytes)
        {
            var culture = CultureInfo.CurrentUICulture;
            const string format = "#,0.0";

            if (bytes < 1024)
                return bytes.ToString("#,0", culture);
            bytes /= 1024;
            if (bytes < 1024)
                return bytes.ToString(format, culture) + " KB";
            bytes /= 1024;
            if (bytes < 1024)
                return bytes.ToString(format, culture) + " MB";
            bytes /= 1024;
            if (bytes < 1024)
                return bytes.ToString(format, culture) + " GB";
            bytes /= 1024;
            return bytes.ToString(format, culture) + " TB";
        }

        /// <summary>
        ///     Encodes a string to be represented as a string literal. The format
        ///     is essentially a JSON string.
        ///     The string returned includes outer quotes
        ///     Example Output: "Hello \"Rick\"!\r\nRock on"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(this string s)
        {
            var sb = new StringBuilder();
            //sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        var i = (int) c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            //sb.Append("\"");
            //string output = Regex.Escape(s) + @"(.*?)";
            return sb.ToString();
        }

        /// <summary>
        ///     Get Country Name
        /// </summary>
        /// <param name="ipAddress"> </param>
        /// <returns> </returns>
        public static double GetIPNumber(this string ipAddress)
        {
            string nowip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(nowip))
            {
                nowip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            string dottedip;
            double Dot2LongIP = 0;
            double PrevPos = 0;
            dottedip = nowip;

            for (int i = 1; i <= 4; i++)
            {
                double pos = Strings.InStr((int) (PrevPos + 1), dottedip, ".", (CompareMethod) 1);
                if (i == 4)
                {
                    pos = Strings.Len(dottedip) + 1;
                }
                var num = (double) Conversion.Int(Strings.Mid(dottedip, (int) (PrevPos + 1), (int) (pos - PrevPos - 1)));
                PrevPos = pos;
                Dot2LongIP = ((num%256)*(Math.Pow(256, (4 - i)))) + Dot2LongIP;
            }

            return Dot2LongIP;
        }

        #region Csv Helpers

        public static List<string> ToStringList(this string csv)
        {
            var stringList = new List<string>();

            string[] arr = csv.Split(',');
            if (arr.Length > 0)
            {
                stringList.AddRange(arr.Where(t => !string.IsNullOrWhiteSpace(t)));
            }
            return stringList;
        }

        public static List<int> ToIntList(this string csv)
        {
            var intList = new List<int>();
            string[] splitStrings = csv.Split(',');

            foreach (string splitItem in splitStrings)
            {
                int i;
                int.TryParse(splitItem, out i);
                intList.Add(i);
            }
            return intList;
        }

        public static int GetCount(this string csv)
        {
            List<string> strList = ToStringList(csv);
            return strList.Count;
        }

        public static string GetSingleWithDots(this string csv)
        {
            string s = string.Empty;
            List<string> strList = ToStringList(csv);
            if (strList.Count > 0)
            {
                s = strList.Count > 1 ? string.Format("{0} ...more", strList[0]) : strList[0];
            }
            return s;
        }

        public static string GetSingleWithDots(this string value, int length)
        {
            string s;
            if (value.Length > length)
            {
                string t = value.Substring(0, length);
                s = value.Length > length ? string.Format("{0} ...", t) : value;
            }
            else
            {
                s = value;
            }
            return s;
        }

        public static string GetAllSelectedValues(this ListBox listBox)
        {
            string selectValues = string.Empty;
            if (listBox != null && listBox.Items.Count > 0)
            {
                selectValues = listBox.Items.Cast<ListItem>()
                    .Where(item => item.Selected)
                    .Aggregate(selectValues, (current, item) => current + string.Format("{0},", item.Value));
                selectValues = selectValues.TrimEnd(",".ToCharArray());
            }
            return selectValues;
        }

        public static string GetAllSelectedText(this ListBox listBox)
        {
            string selectText = string.Empty;
            if (listBox != null && listBox.Items.Count > 0)
            {
                selectText = listBox.Items.Cast<ListItem>()
                    .Where(item => item.Selected)
                    .Aggregate(selectText, (current, item) => current + string.Format("'{0}',", item.Text));
                selectText = selectText.TrimEnd(",".ToCharArray());
            }
            return selectText;
        }

        #endregion Csv Helpers

        #region Casting

        /// <summary>
        ///     Converts an object to a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(this object value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        /// <summary>
        ///     Converts an object to a byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this object value)
        {
            return value == null ? byte.MinValue : (byte) value;
        }

        /// <summary>
        ///     Converts an object to a byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this object value)
        {
            return value == null ? null : (byte[]) value;
        }

        /// <summary>
        ///     Converts an object to an int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static char ToChar(this object value)
        {
            if (value == null)
            {
                return char.Parse(string.Empty);
            }
            char c;
            char.TryParse(value.ToString(), out c);
            return c;
        }
        /// <summary>
        ///     Converts an object to an int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            if (value == null)
            {
                return 0;
            }
            int i;
            int.TryParse(value.ToString(), out i);
            return i;
        }

        /// <summary>
        ///     Converts an object to a long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this object value)
        {
            if (value == null)
            {
                return 0;
            }
            long l;
            long.TryParse(value.ToString(), out l);
            return l;
        }

        /// <summary>
        ///     Converts an object to a decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value)
        {
            if (value == null)
            {
                return decimal.Zero;
            }
            decimal d;
            decimal.TryParse(value.ToString(), out d);
            return d;
        }

        /// <summary>
        ///     Converts an object to a boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this object value)
        {
            if (value == null)
            {
                return false;
            }
            bool b;
            bool.TryParse(value.ToString(), out b);
            return b;
        }

        /// <summary>
        ///     Converts an object to a DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value)
        {
            if (value == null)
            {
                return DateTime.MinValue;
            }
            DateTime dt;
            DateTime.TryParse(value.ToString(), out dt);
            return dt;
        }

        /// <summary>
        ///     Converts an object to a double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this object value)
        {
            if (value == null)
            {
                return 0;
            }
            double d;
            double.TryParse(value.ToString(), out d);
            return d;
        }

        /// <summary>
        ///     Converts an object to a float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this object value)
        {
            if (value == null)
            {
                return 0;
            }
            float d;
            float.TryParse(value.ToString(), out d);
            return d;
        }

        /// <summary>
        ///     Converts an object to a GUID
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object value)
        {
            return value == null ? Guid.Empty : (Guid) (value);
        }


        public static int StringToInt(string iSTPtring)
        {
            int result;
            return int.TryParse(iSTPtring, out result) ? result : 0;
        }

        public static DateTime ToDate(this string dateString)
        {
            DateTime result;
            return DateTime.TryParse(dateString, out result) ? result : DateTime.MinValue;
        }

        public static bool StringToBool(this string boolString)
        {
            bool result;
            return bool.TryParse(boolString, out result) && result;
        }

        /// <summary>
        ///     Escape's the Carriage return and new Line
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetHTMLEscapeCarriageReturn(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value.Replace(Environment.NewLine, "<br />");
            }
            return value;
        }

        /// <summary>
        ///     Converts an object to a decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string By100(this object value)
        {
            if (value == null)
            {
                return decimal.Zero.ToString(CultureInfo.InvariantCulture);
            }
            decimal d;
            decimal.TryParse(value.ToString(), out d);
            d = d*100;
            d = decimal.Round(d, 0);
            return d.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region Getting Chunks

        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.ChunkBy(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            // Flag to signal end of source sequence.
            const bool noMoreSourceElemeSTP = true;

            // Auto-generated iterator for the source array.       
            var enumerator = source.GetEnumerator();

            // Move to the first element in the source sequence.
            if (!enumerator.MoveNext()) yield break;

            // Iterate through source sequence and create a copy of each Chunk.
            // On each pass, the iterator advances to the first element of the next "Chunk"
            // in the source sequence. This loop corresponds to the outer foreach loop that
            // executes the query.
            while (true)
            {
                // Get the key for the current Chunk. The source iterator will churn through
                // the source sequence until it finds an element with a key that doesn't match.
                var key = keySelector(enumerator.Current);

                // Make a new Chunk (group) object that initially has one GroupItem, which is a copy of the current source element.
                var current = new Chunk<TKey, TSource>(key, enumerator,
                    value => comparer.Equals(key, keySelector(value)));

                // Return the Chunk. A Chunk is an IGrouping<TKey,TSource>, which is the return value of the ChunkBy method.
                // At this point the Chunk only has the first element in its source sequence. The remaining elemeSTP will be
                // returned only when the client code foreach's over this chunk. See Chunk.GetEnumerator for more info.
                yield return current;

                // Check to see whether (a) the chunk has made a copy of all its source elemeSTP or 
                // (b) the iterator has reached the end of the source sequence. If the caller uses an inner
                // foreach loop to iterate the chunk items, and that loop ran to completion,
                // then the Chunk.GetEnumerator method will already have made
                // copies of all chunk items before we get here. If the Chunk.GetEnumerator loop did not
                // enumerate all elemeSTP in the chunk, we need to do it here to avoid corrupting the iterator
                // for clieSTP that may be calling us on a separate thread.
                if (current.CopyAllChunkElemeSTP() == noMoreSourceElemeSTP)
                {
                    yield break;
                }
            }
        }

        // A Chunk is a contiguous group of one or more source elemeSTP that have the same key. A Chunk 
        // has a key and a list of ChunkItem objects, which are copies of the elemeSTP in the source sequence.
        private class Chunk<TKey, TSource> : IGrouping<TKey, TSource>
        {
            // INVARIANT: DoneCopyingChunk == true || 
            //   (predicate != null && predicate(enumerator.Current) && current.Value == enumerator.Current)

            // A Chunk has a linked list of ChunkItems, which represent the elemeSTP in the current chunk. Each ChunkItem
            // has a reference to the next ChunkItem in the list.

            // The value that is used to determine matching elemeSTP
            private readonly ChunkItem head;
            private readonly TKey key;
            private readonly object mLock;

            // Stores a reference to the enumerator for the source sequence
            private IEnumerator<TSource> enumerator;
            private bool isLastSourceElement;

            // A reference to the predicate that is used to compare keys.
            private Func<TSource, bool> predicate;

            // Stores the conteSTP of the first source element that
            // belongs with this chunk.

            // End of the list. It is repositioned each time a new
            // ChunkItem is added.
            private ChunkItem tail;

            // Flag to indicate the source iterator has reached the end of the source sequence.

            // REQUIRES: enumerator != null && predicate != null
            public Chunk(TKey key, IEnumerator<TSource> enumerator, Func<TSource, bool> predicate)
            {
                this.key = key;
                this.enumerator = enumerator;
                this.predicate = predicate;

                // A Chunk always contains at least one element.
                head = new ChunkItem(enumerator.Current);

                // The end and beginning are the same until the list contains > 1 elemeSTP.
                tail = head;

                mLock = new object();
            }

            // Indicates that all chunk elemeSTP have been copied to the list of ChunkItems, 
            // and the source enumerator is either at the end, or else on an element with a new key.
            // the tail of the linked list is set to null in the CopyNextChunkElement method if the
            // key of the next element does not match the current chunk's key, or there are no more elemeSTP in the source.
            private bool DoneCopyingChunk
            {
                get { return tail == null; }
            }

            // Adds one ChunkItem to the current group
            // REQUIRES: !DoneCopyingChunk && lock(this)

            public TKey Key
            {
                get { return key; }
            }

            // Invoked by the inner foreach loop. This method stays just one step ahead
            // of the client requests. It adds the next element of the chunk only after
            // the clieSTP requests the last element in the list so far.
            public IEnumerator<TSource> GetEnumerator()
            {
                //Specify the initial element to enumerate.
                ChunkItem current = head;

                // There should always be at least one ChunkItem in a Chunk.
                while (current != null)
                {
                    // Yield the current item in the list.
                    yield return current.Value;

                    // Copy the next item from the source sequence, 
                    // if we are at the end of our local list.
                    lock (mLock)
                    {
                        if (current == tail)
                        {
                            CopyNextChunkElement();
                        }
                    }

                    // Move to the next ChunkItem in the list.
                    current = current.Next;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private void CopyNextChunkElement()
            {
                // Try to advance the iterator on the source sequence.
                // If MoveNext returns false we are at the end, and isLastSourceElement is set to true
                isLastSourceElement = !enumerator.MoveNext();

                // If we are (a) at the end of the source, or (b) at the end of the current chunk
                // then null out the enumerator and predicate for reuse with the next chunk.
                if (isLastSourceElement || !predicate(enumerator.Current))
                {
                    enumerator = null;
                    predicate = null;
                }
                else
                {
                    tail.Next = new ChunkItem(enumerator.Current);
                }

                // tail will be null if we are at the end of the chunk elemeSTP
                // This check is made in DoneCopyingChunk.
                tail = tail.Next;
            }

            // Called after the end of the last chunk was reached. It first checks whether
            // there are more elemeSTP in the source sequence. If there are, it 
            // Returns true if enumerator for this chunk was exhausted.
            internal bool CopyAllChunkElemeSTP()
            {
                while (true)
                {
                    lock (mLock)
                    {
                        if (DoneCopyingChunk)
                        {
                            // If isLastSourceElement is false,
                            // it signals to the outer iterator
                            // to continue iterating.
                            return isLastSourceElement;
                        }
                        CopyNextChunkElement();
                    }
                }
            }

            private class ChunkItem
            {
                public readonly TSource Value;
                public ChunkItem Next;

                public ChunkItem(TSource value)
                {
                    Value = value;
                }
            }
        }

        #endregion
    }
}