// Decompiled with JetBrains decompiler
// Type: STAR.Framework.Utility.Text
// Assembly: STAR.Framework.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09c4f98813f6b683
// MVID: C0759794-9AB4-40FB-8B71-6E464E708E5C
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Framework.Utility.1.0.0\lib\net40\STAR.Framework.Utility.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace STAR.Framework.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class Text
    {
        /// <summary>Called to avoid null reference exceptions.</summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <remarks>
        ///     - Checks if the 'text' parameter is null.
        ///     - If not null then Trims the 'text' parameter.
        ///     - If not null after trimming then returns the original 'text' parameter.
        ///     - Otherwise returns String.Empty.
        /// </remarks>
        public static string GetString(string text)
        {
            return STAR.Framework.Utility.Text.GetString(text, string.Empty);
        }

        /// <summary>Called to avoid null reference exceptions.</summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <remarks>
        ///     - Checks if the 'text' parameter is null.
        ///     - If not null then Trims the 'text' parameter.
        ///     - If not null after trimming then returns the original 'text' parameter.
        ///     - Otherwise returns the 'defaultValue' parameter.
        /// </remarks>
        public static string GetString(string text, string defaultValue)
        {
            string str = defaultValue;
            if (text != null && !string.IsNullOrEmpty(text.Trim()))
                str = text.Trim();
            return str;
        }

        /// <summary>
        /// Useful for debugging. Returns the word 'NULL', 'EMPTY',
        /// or the actual value of the 'text' parameter.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetStringInfo(string text)
        {
            string str = text;
            if (text == null)
                str = "NULL";
            else if (string.IsNullOrEmpty(text))
                str = "EMPTY";
            return str;
        }

        public static string BuildCommaDelimitedList<T>(List<T> list)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                for (int index = 0; index < list.Count; ++index)
                    stringBuilder1.AppendFormat("{0}, ", (object)list[index]);
            }
            if (stringBuilder1.Length > 0)
            {
                StringBuilder stringBuilder2 = stringBuilder1;
                int startIndex = stringBuilder2.Length - 2;
                int length = 2;
                stringBuilder2.Remove(startIndex, length);
            }
            return stringBuilder1.ToString();
        }

        /// <summary>
        /// Trims the end of a string starting at the last occurrance of a comma.
        /// </summary>
        /// <param name="commaDelimitedStringBuilder">The comma delimited string builder.</param>
        public static string TrimEnd(string text)
        {
            return STAR.Framework.Utility.Text.TrimEnd(text, ',');
        }

        /// <summary>
        /// Trims the end of a string starting at the last occurrance of a character.
        /// </summary>
        /// <param name="commaDelimitedStringBuilder">The comma delimited string builder.</param>
        public static string TrimEnd(string text, char lastCharacter)
        {
            string str = text;
            if (string.IsNullOrEmpty(text))
                return str;
            STAR.Framework.Utility.Text.TrimEnd(new StringBuilder(text), lastCharacter);
            return str;
        }

        /// <summary>
        /// Trims the end of a string starting at the last occurrance of a comma.
        /// </summary>
        /// <param name="instance">The comma delimited string builder.</param>
        public static void TrimEnd(StringBuilder instance)
        {
            STAR.Framework.Utility.Text.TrimEnd(instance, ',');
        }

        /// <summary>
        /// Trims the end of a string starting at the last occurrance of a character.
        /// </summary>
        /// <param name="commaDelimitedStringBuilder">The comma delimited string builder.</param>
        public static void TrimEnd(StringBuilder instance, char lastCharacter)
        {
            if (instance == null)
                return;
            int startIndex = instance.ToString().LastIndexOf(lastCharacter);
            int length = instance.Length - startIndex;
            if (startIndex <= 0 || startIndex >= instance.Length)
                return;
            instance = instance.Remove(startIndex, length);
        }

        /// <summary>
        /// Replaces one single quote with two single quotes when a text value is used in an in-line sql query.
        /// </summary>
        /// <param name="textValue">The text value.</param>
        /// <returns></returns>
        /// <remarks>
        /// For example...
        /// 
        ///     select * from MyTable where FirstName = 'O'Neal'
        /// 
        /// ...would fail and should be formatted as...
        /// 
        ///     select * from MyTable where FirstName = 'O''Neal'
        /// 
        /// </remarks>
        public static string GetSingleQuotedSqlText(string textValue)
        {
            string str = textValue;
            if (!string.IsNullOrEmpty(textValue) && textValue.Contains("'"))
                str = textValue.Replace("'", "''");
            return str;
        }

        public static class RegEx
        {
            public static string ReplaceNewLinesWithHtmlBreaks(string textWithNewLines)
            {
                return new Regex("\n").Replace(textWithNewLines, "<br />");
            }

            public static string ReplaceHtmlBreaksWithNewLines(string textHtmlBreaks)
            {
                return new Regex("<br />").Replace(textHtmlBreaks, Environment.NewLine);
            }
        }
    }
}
