// <copyright file="StringExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Media;

    using Concierge.Common.Dtos;

    /// <summary>
    /// Provides extension methods for string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex rtfRegex = new (@"\\([a-z]{1,32})(-?\d{1,10})?[ ]?|\\'([0-9a-f]{2})|\\([^a-z])|([{}])|[\r\n]+|(.)", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly List<string> destinations =
        [
            "aftncn", "aftnsep", "aftnsepc", "annotation", "atnauthor", "atndate", "atnicn", "atnid",
            "atnparent", "atnref", "atntime", "atrfend", "atrfstart", "author", "background",
            "bkmkend", "bkmkstart", "blipuid", "buptim", "category", "colorschememapping",
            "colortbl", "comment", "company", "creatim", "datafield", "datastore", "defchp", "defpap",
            "do", "doccomm", "docvar", "dptxbxtext", "ebcend", "ebcstart", "factoidname", "falt",
            "fchars", "ffdeftext", "ffentrymcr", "ffexitmcr", "ffformat", "ffhelptext", "ffl",
            "ffname", "ffstattext", "field", "file", "filetbl", "fldinst", "fldrslt", "fldtype",
            "fname", "fontemb", "fontfile", "fonttbl", "footer", "footerf", "footerl", "footerr",
            "footnote", "formfield", "ftncn", "ftnsep", "ftnsepc", "g", "generator", "gridtbl",
            "header", "headerf", "headerl", "headerr", "hl", "hlfr", "hlinkbase", "hlloc", "hlsrc",
            "hsv", "htmltag", "info", "keycode", "keywords", "latentstyles", "lchars", "levelnumbers",
            "leveltext", "lfolevel", "linkval", "list", "listlevel", "listname", "listoverride",
            "listoverridetable", "listpicture", "liststylename", "listtable", "listtext",
            "lsdlockedexcept", "macc", "maccPr", "mailmerge", "maln", "malnScr", "manager", "margPr",
            "mbar", "mbarPr", "mbaseJc", "mbegChr", "mborderBox", "mborderBoxPr", "mbox", "mboxPr",
            "mchr", "mcount", "mctrlPr", "md", "mdeg", "mdegHide", "mden", "mdiff", "mdPr", "me",
            "mendChr", "meqArr", "meqArrPr", "mf", "mfName", "mfPr", "mfunc", "mfuncPr", "mgroupChr",
            "mgroupChrPr", "mgrow", "mhideBot", "mhideLeft", "mhideRight", "mhideTop", "mhtmltag",
            "mlim", "mlimloc", "mlimlow", "mlimlowPr", "mlimupp", "mlimuppPr", "mm", "mmaddfieldname",
            "mmath", "mmathPict", "mmathPr", "mmaxdist", "mmc", "mmcJc", "mmconnectstr",
            "mmconnectstrdata", "mmcPr", "mmcs", "mmdatasource", "mmheadersource", "mmmailsubject",
            "mmodso", "mmodsofilter", "mmodsofldmpdata", "mmodsomappedname", "mmodsoname",
            "mmodsorecipdata", "mmodsosort", "mmodsosrc", "mmodsotable", "mmodsoudl",
            "mmodsoudldata", "mmodsouniquetag", "mmPr", "mmquery", "mmr", "mnary", "mnaryPr",
            "mnoBreak", "mnum", "mobjDist", "moMath", "moMathPara", "moMathParaPr", "mopEmu",
            "mphant", "mphantPr", "mplcHide", "mpos", "mr", "mrad", "mradPr", "mrPr", "msepChr",
            "mshow", "mshp", "msPre", "msPrePr", "msSub", "msSubPr", "msSubSup", "msSubSupPr", "msSup",
            "msSupPr", "mstrikeBLTR", "mstrikeH", "mstrikeTLBR", "mstrikeV", "msub", "msubHide",
            "msup", "msupHide", "mtransp", "mtype", "mvertJc", "mvfmf", "mvfml", "mvtof", "mvtol",
            "mzeroAsc", "mzeroDesc", "mzeroWid", "nesttableprops", "nextfile", "nonesttables",
            "objalias", "objclass", "objdata", "object", "objname", "objsect", "objtime", "oldcprops",
            "oldpprops", "oldsprops", "oldtprops", "oleclsid", "operator", "panose", "password",
            "passwordhash", "pgp", "pgptbl", "picprop", "pict", "pn", "pnseclvl", "pntext", "pntxta",
            "pntxtb", "printim", "private", "propname", "protend", "protstart", "protusertbl", "pxe",
            "result", "revtbl", "revtim", "rsidtbl", "rxe", "shp", "shpgrp", "shpinst",
            "shppict", "shprslt", "shptxt", "sn", "sp", "staticval", "stylesheet", "subject", "sv",
            "svb", "tc", "template", "themedata", "title", "txe", "ud", "upr", "userprops",
            "wgrffmtfilter", "windowcaption", "writereservation", "writereservhash", "xe", "xform",
            "xmlattrname", "xmlattrvalue", "xmlclose", "xmlname", "xmlnstbl",
            "xmlopen",
        ];

        private static readonly Dictionary<string, string> specialCharacters = new ()
        {
            { "par", "\n" },
            { "sect", "\n\n" },
            { "page", "\n\n" },
            { "line", "\n" },
            { "tab", "\t" },
            { "emdash", "\u2014" },
            { "endash", "\u2013" },
            { "emspace", "\u2003" },
            { "enspace", "\u2002" },
            { "qmspace", "\u2005" },
            { "bullet", "\u2022" },
            { "lquote", "\u2018" },
            { "rquote", "\u2019" },
            { "ldblquote", "\u201C" },
            { "rdblquote", "\u201D" },
        };

        /// <summary>
        /// Determines whether the specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="str">The string to test.</param>
        /// <returns>true if the string is null, empty, or consists only of white-space characters; otherwise, false.</returns>
        public static bool IsNullOrWhiteSpace(this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Determines whether the specified string is null or empty.
        /// </summary>
        /// <param name="str">The string to test.</param>
        /// <returns>true if the string is null or empty; otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Counts the occurrences of a specified character in the string.
        /// </summary>
        /// <param name="str">The string to count characters in.</param>
        /// <param name="character">The character to count.</param>
        /// <returns>The number of occurrences of the character in the string.</returns>
        public static int CountCharacter(this string str, char character)
        {
            var count = 0;
            var regex = new Regex("\".*?\"");

            str = regex.Replace(str, m => m.Value.Replace(',', '@'));
            var array = str.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == character)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Formats the string by inserting spaces before each uppercase letter (from an enum representation).
        /// </summary>
        /// <param name="str">The string to format.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatFromEnum(this string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            if (str.IsAllUpper())
            {
                return str;
            }

            var charArray = str.ToCharArray();
            var offset = 0;

            for (int i = 1; i < charArray.Length; i++)
            {
                if (char.IsUpper(charArray[i]))
                {
                    str = str.Insert(i + offset, " ");
                    offset++;
                }
            }

            return str;
        }

        /// <summary>
        /// Checks if each char in a string is uppercase.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <returns>true if all characters are uppercase; otherwise, false.</returns>
        public static bool IsAllUpper(this string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsUpper(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes the specified text from the string.
        /// </summary>
        /// <param name="str">The string to strip.</param>
        /// <param name="textToStrip">The text to remove.</param>
        /// <returns>The modified string.</returns>
        public static string Strip(this string str, string textToStrip)
        {
            return str.Replace(textToStrip, string.Empty);
        }

        /// <summary>
        /// Removes the specified texts from the string.
        /// </summary>
        /// <param name="str">The string to strip.</param>
        /// <param name="textToStrip">The texts to remove.</param>
        /// <returns>The modified string.</returns>
        public static string Strip(this string str, params string[] textToStrip)
        {
            foreach (var text in textToStrip)
            {
                str = str.Replace(text, string.Empty);
            }

            return str;
        }

        /// <summary>
        /// Removes the specified texts from the string.
        /// </summary>
        /// <param name="str">The string to strip.</param>
        /// <param name="textToStrip">The texts to remove.</param>
        /// <returns>The modified string.</returns>
        public static string Strip(this string str, StringComparison stringComparison, params string[] textToStrip)
        {
            foreach (var text in textToStrip)
            {
                str = str.Replace(text, string.Empty, stringComparison);
            }

            return str;
        }

        /// <summary>
        /// Determines whether the specified pattern is a valid regular expression.
        /// </summary>
        /// <param name="pattern">The pattern to test.</param>
        /// <returns>true if the pattern is a valid regular expression; otherwise, false.</returns>
        public static bool IsValidRegex(this string pattern)
        {
            if (pattern.IsNullOrWhiteSpace())
            {
                return false;
            }

            try
            {
                Regex.Match(string.Empty, pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts a string representation of a color to a Color object.
        /// If the input string is null, empty, or consists only of white space, the method returns Colors.Transparent.
        /// If the conversion fails, the method also returns Colors.Transparent.
        /// </summary>
        /// <param name="colorName">The string representation of the color to convert.</param>
        /// <returns>A Color object representing the converted color, or Colors.Transparent if the conversion fails.</returns>
        public static Color ToColor(this string? colorName)
        {
            if (colorName?.IsNullOrWhiteSpace() ?? true)
            {
                return Colors.Transparent;
            }

            try
            {
                colorName = colorName.Strip(" ", "-", ".", "'");
                var cc = TypeDescriptor.GetConverter(typeof(Color));

                return (Color?)cc?.ConvertFromString(colorName) ?? Colors.Transparent;
            }
            catch (Exception)
            {
                return Colors.Transparent;
            }
        }

        /// <summary>
        /// Formats the given string by inserting a space before each uppercase letter (excluding the first letter).
        /// </summary>
        /// <param name="name">The string to format.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatColorName(this string name)
        {
            var charArray = name.ToArray();
            var offset = 0;

            for (int i = 1; i < charArray.Length; i++)
            {
                if (char.IsUpper(charArray[i]))
                {
                    name = name.Insert(i + offset, " ");
                    offset++;
                }
            }

            return name;
        }

        /// <summary>
        /// Checks whether the given string is in RTF (Rich Text Format) format.
        /// </summary>
        /// <param name="text">The string to check.</param>
        /// <returns>True if the string is in RTF format, false otherwise.</returns>
        public static bool IsRtf(this string text)
        {
            return !text.IsNullOrWhiteSpace() && text.TrimStart().StartsWith(@"{\rtf", StringComparison.Ordinal);
        }

        /// <summary>
        /// Strips the Rich Text Format (RTF) formatting from the input string and returns the plain text.
        /// If the input string is null or consists only of white space, the method returns an empty string.
        /// </summary>
        /// <param name="inputRtf">The RTF-formatted input string to strip the formatting from.</param>
        /// <returns>The plain text with RTF formatting removed.</returns>
        public static string StripRichTextFormat(this string inputRtf)
        {
            if (inputRtf.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            var stack = new Stack<StackEntryDto>();
            var ignorable = false;              // Whether this group (and all inside it) are "ignorable".
            int ucskip = 1;                      // Number of ASCII characters to skip after a unicode character.
            int curskip = 0;                     // Number of ASCII characters left to skip
            var outList = new List<string>();    // Output buffer.

            MatchCollection matches = rtfRegex.Matches(inputRtf);

            if (matches.Count == 0)
            {
                return string.Empty;
            }

            foreach (Match match in matches.Cast<Match>())
            {
                string word = match.Groups[1].Value;
                string arg = match.Groups[2].Value;
                string hex = match.Groups[3].Value;
                string character = match.Groups[4].Value;
                string brace = match.Groups[5].Value;
                string tchar = match.Groups[6].Value;

                if (!brace.IsNullOrEmpty())
                {
                    curskip = 0;
                    if (brace == "{")
                    {
                        stack.Push(new StackEntryDto(ucskip, ignorable));
                    }
                    else if (brace == "}")
                    {
                        var entry = stack.Pop();
                        ucskip = entry.NumberOfCharactersToSkip;
                        ignorable = entry.Ignorable;
                    }
                }
                else if (!character.IsNullOrEmpty())
                {
                    curskip = 0;
                    if (character == "~")
                    {
                        if (!ignorable)
                        {
                            outList.Add("\xA0");
                        }
                    }
                    else if ("{}\\".Contains(character))
                    {
                        if (!ignorable)
                        {
                            outList.Add(character);
                        }
                    }
                    else if (character == "*")
                    {
                        ignorable = true;
                    }
                }
                else if (!word.IsNullOrEmpty())
                {
                    curskip = 0;
                    if (destinations.Contains(word))
                    {
                        ignorable = true;
                    }
                    else if (ignorable)
                    {
                    }
                    else if (specialCharacters.TryGetValue(word, out string? value))
                    {
                        outList.Add(value);
                    }
                    else if (word == "uc")
                    {
                        ucskip = int.Parse(arg);
                    }
                    else if (word == "u")
                    {
                        int c = int.Parse(arg);
                        if (c < 0)
                        {
                            c += 0x10000;
                        }

                        if (c >= 0x000000 && c <= 0x10ffff && (c < 0x00d800 || c > 0x00dfff))
                        {
                            outList.Add(char.ConvertFromUtf32(c));
                        }
                        else
                        {
                            outList.Add("?");
                        }

                        curskip = ucskip;
                    }
                }
                else if (!hex.IsNullOrEmpty())
                {
                    if (curskip > 0)
                    {
                        curskip -= 1;
                    }
                    else if (!ignorable)
                    {
                        int c = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                        outList.Add(char.ConvertFromUtf32(c));
                    }
                }
                else if (!tchar.IsNullOrEmpty())
                {
                    if (curskip > 0)
                    {
                        curskip -= 1;
                    }
                    else if (!ignorable)
                    {
                        outList.Add(tchar);
                    }
                }
            }

            return string.Join(string.Empty, [.. outList]);
        }

        /// <summary>
        /// Removes empty strings from the input string array and returns a new array containing only non-empty strings.
        /// </summary>
        /// <param name="array">The input string array.</param>
        /// <returns>A new string array without empty strings.</returns>
        public static string[] RemoveEmpty(this string[] array)
        {
            var list = new List<string>();
            foreach (var item in array)
            {
                if (!item.IsNullOrWhiteSpace())
                {
                    list.Add(item);
                }
            }

            return [.. list];
        }

        /// <summary>
        /// Replaces the last occurrence of a substring in the input string with a new substring and returns the modified string.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <param name="from">The substring to replace.</param>
        /// <param name="to">The new substring to insert.</param>
        /// <returns>The modified string with the last occurrence of the specified substring replaced.</returns>
        public static string ReplaceLast(this string str, string from, string to)
        {
            var index = str.LastIndexOf(from);
            var newStr = str.Remove(index, from.Length);

            newStr = newStr.Insert(index, to);

            return newStr;
        }

        /// <summary>
        /// Checks if the provided string matches any of the specified string values
        /// using a case-insensitive comparison with invariant culture.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="find">An array of string values to compare against.</param>
        /// <returns>
        /// <c>true</c> if the provided string matches any of the specified string values;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAny(this string str, params string[] find)
        {
            foreach (var item in find)
            {
                if (str.Equals(item, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves the appropriate determiner ("a" or "an") for a given string, optionally in lowercase.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <param name="lowercase">Specifies whether the determiner should be in lowercase.</param>
        /// <returns>The determiner ("a" or "an") based on the first character of the input string.</returns>
        public static string GetDeterminer(this string str, bool lowercase)
        {
            if (str.Length == 0)
            {
                return lowercase ? "a" : "A";
            }

            var isVowel = "aeiouAEIOU".Contains(str[0]);
            if (isVowel)
            {
                return lowercase ? "an" : "An";
            }

            return lowercase ? "a" : "A";
        }
    }
}
