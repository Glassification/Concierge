// <copyright file="ConciergeRichTextBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Services;

    public sealed class ConciergeRichTextBox : RichTextBox
    {
        private const int MaxUndoQueue = 25;

        public ConciergeRichTextBox()
            : base()
        {
            this.Margin = new Thickness(3);
            this.UndoLimit = MaxUndoQueue;

            this.GotFocus += this.TextBox_GotFocus;
            this.LostFocus += this.TextBox_LostFocus;
            this.MouseEnter += this.TextBox_MouseEnter;
            this.MouseLeave += this.TextBox_MouseLeave;
        }

        public void LoadCurrentDocument(string text)
        {
            var dataFormat = DataFormats.Rtf;
            if (!text.IsRtf())
            {
                text = string.Empty;
                dataFormat = DataFormats.Text;
                this.SetDefaultFontStyle();
            }

            var stream = new MemoryStream(Encoding.Default.GetBytes(text));
            this.Document.Blocks.Clear();
            this.Selection.Load(stream, dataFormat);
        }

        public string SaveCurrentDocument()
        {
            var range = new TextRange(this.Document.ContentStart, this.Document.ContentEnd);
            try
            {
                using var rtfMemoryStream = new MemoryStream();
                using var rtfStreamWriter = new StreamWriter(rtfMemoryStream);
                range.Save(rtfMemoryStream, DataFormats.Rtf);

                rtfMemoryStream.Flush();
                rtfMemoryStream.Position = 0;
                var sr = new StreamReader(rtfMemoryStream);

                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
                return string.Empty;
            }
        }

        public void SetDefaultFontStyle()
        {
            this.FontSize = FontService.DefaultSize;
            this.FontFamily = FontService.DefaultFont;
            this.Foreground = Brushes.White;
        }

        public void ResetUndoQueue()
        {
            this.UndoLimit = 0;
            this.UndoLimit = MaxUndoQueue;
        }

        public void ClearHighlightSelection()
        {
            this.SelectAll();
            this.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, ConciergeColors.ControlForeBlue);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Program.Typing();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Program.NotTyping();
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
