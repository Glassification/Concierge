// <copyright file="StringExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    using Concierge.Configuration;

    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

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

        public static string ReplaceMultiple(this string str, string newCharacter, params string[] oldCharacters)
        {
            var cleanedString = str;

            foreach (var character in oldCharacters)
            {
                cleanedString = cleanedString.Replace(character, newCharacter);
            }

            return cleanedString;
        }

        public static string FormatFromEnum(this string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return string.Empty;
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

        public static string RtfToText(this string str)
        {
            var richTextBox = new RichTextBox();

            var stream = new MemoryStream(Encoding.Default.GetBytes(str));
            richTextBox.Selection.Load(stream, DataFormats.Rtf);

            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);

            return textRange.Text;
        }

        public static string Strip(this string str, string textToStrip)
        {
            return str.Replace(textToStrip, string.Empty);
        }

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

        public static Brush ToBrush(this string colorName)
        {
            try
            {
                colorName = colorName.Strip(" ").Strip("-").Strip(".").Strip("'");

                if (AppSettingsManager.CustomColors.ContainsKey(colorName))
                {
                    colorName = AppSettingsManager.CustomColors[colorName];
                }

                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorName));
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
                return Brushes.Transparent;
            }
        }
    }
}
