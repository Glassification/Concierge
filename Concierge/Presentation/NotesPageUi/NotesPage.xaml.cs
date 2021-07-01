// <copyright file="NotesPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.NotesPageUi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    using Concierge.Characters.Collections;

    /// <summary>
    /// Interaction logic for NotesPage.xaml.
    /// </summary>
    public partial class NotesPage : Page
    {
        public NotesPage()
        {
            this.InitializeComponent();
            this.SelectedDocument = null;
            this.CurrentDocumentText = string.Empty;
            this.Lock = false;
            this.FontFamilyList.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            this.FontSizeList.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            // ButtonUndo.IsEnabled = false;

            // ButtonRedo.IsEnabled = false;
        }

        public Document SelectedDocument { get; set; }

        public string CurrentDocumentText { get; set; }

        private bool Lock { get; set; }

        public void Draw()
        {
            this.DrawTreeView();
        }

        public void ClearTextBox()
        {
            this.NotesTextBox.Document.Blocks.Clear();
        }

        public void SaveTextBox()
        {
            if (this.SelectedDocument != null)
            {
                this.SelectedDocument.RTF = this.SaveCurrentDocument();

                Program.Modified = true;
            }
        }

        private void DrawTreeView()
        {
            this.NotesTreeView.Items.Clear();

            foreach (var chapter in Program.Character.Chapters)
            {
                var treeViewChapter = new TreeViewItem()
                {
                    Header = chapter.Name,
                    Tag = chapter,
                    Foreground = Brushes.White,
                };

                foreach (var document in chapter.Documents)
                {
                    var treeViewDocument = new TreeViewItem()
                    {
                        Header = document.Name,
                        Tag = document,
                        Foreground = Brushes.White,
                    };

                    treeViewChapter.Items.Add(treeViewDocument);
                }

                this.NotesTreeView.Items.Add(treeViewChapter);
            }
        }

        private void LoadCurrentDocument(string text)
        {
            var stream = new MemoryStream(Encoding.Default.GetBytes(text));
            this.NotesTextBox.Document.Blocks.Clear();
            this.NotesTextBox.Selection.Load(stream, DataFormats.Rtf);
        }

        private string SaveCurrentDocument()
        {
            var range = new TextRange(this.NotesTextBox.Document.ContentStart, this.NotesTextBox.Document.ContentEnd);

            try
            {
                using (var rtfMemoryStream = new MemoryStream())
                using (var rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                {
                    range.Save(rtfMemoryStream, DataFormats.Rtf);

                    rtfMemoryStream.Flush();
                    rtfMemoryStream.Position = 0;
                    var sr = new StreamReader(rtfMemoryStream);

                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return string.Empty;
            }
        }

        private void NotesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!this.Lock)
            {
                this.Lock = true;

                var treeViewItem = this.NotesTreeView?.SelectedItem as TreeViewItem;

                if (treeViewItem?.Parent is TreeViewItem)
                {
                    if (this.SelectedDocument != null)
                    {
                        this.SelectedDocument.RTF = this.SaveCurrentDocument();
                    }

                    this.NotesTextBox.IsUndoEnabled = false;
                    this.NotesTextBox.IsUndoEnabled = true;
                    this.SelectedDocument = treeViewItem.Tag as Document;
                    this.LoadCurrentDocument(this.SelectedDocument.RTF);
                }

                this.Lock = false;
            }
        }

        private void ButtonCut_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.NotesTextBox.Selection.Text);
            this.NotesTextBox.Selection.Text = string.Empty;
        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.NotesTextBox.Selection.Text);
        }

        private void ButtonPaste_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Selection.Text = Clipboard.GetText();
        }

        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Undo();
        }

        private void ButtonRedo_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Redo();
        }

        private void ButtonBold_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.ButtonBold.IsChecked)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void ButtonItalic_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.ButtonItalic.IsChecked)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void ButtonUnderline_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.ButtonUnderline.IsChecked)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
        }

        private void NotesTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object obj;

            obj = this.NotesTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            this.ButtonBold.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(FontWeights.Bold);

            obj = this.NotesTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            this.ButtonItalic.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(FontStyles.Italic);

            obj = this.NotesTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            this.ButtonUnderline.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(TextDecorations.Underline);

            obj = this.NotesTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            this.FontFamilyList.SelectedItem = obj;

            obj = this.NotesTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
            this.FontSizeList.Text = obj == DependencyProperty.UnsetValue ? string.Empty : obj.ToString();

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            this.ColorPicker.SelectedColor = obj == DependencyProperty.UnsetValue ? Colors.White : (Color)ColorConverter.ConvertFromString(obj.ToString());

            // ButtonUndo.IsEnabled = NotesTextBox.CanUndo;
            // ButtonRedo.IsEnabled = NotesTextBox.CanRedo;
        }

        private void FontFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.FontFamilyList.SelectedItem != null)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, this.FontFamilyList.SelectedItem);
            }
        }

        private void FontSizeList_TextChanged(object sender, TextChangedEventArgs e)
        {
            double test;

            if (double.TryParse(this.FontSizeList.Text, out test))
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, this.FontSizeList.Text);
            }
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, this.ColorPicker.SelectedColor.ToString());
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.SaveTextBox();
            this.ClearTextBox();

            TreeViewItem item = this.NotesTreeView?.SelectedItem as TreeViewItem;

            if (item != null)
            {
                item.IsSelected = false;
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var item = this.NotesTreeView?.SelectedItem as TreeViewItem;

            if (item?.Parent is TreeViewItem)
            {
                (item.Parent as TreeViewItem).Items.MoveCurrentTo(item);
                (item.Parent as TreeViewItem).Items.MoveCurrentToPrevious();
            }
            else
            {
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
