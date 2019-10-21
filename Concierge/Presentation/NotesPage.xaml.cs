using Concierge.Characters.Collections;
using Concierge.Presentation.DialogBoxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Concierge.Presentation
{
    /// <summary>
    /// Interaction logic for NotesPage.xaml
    /// </summary>
    public partial class NotesPage : Page
    {

        #region Constructor
        ColorPickerWindow colorPicker = new ColorPickerWindow();
        public NotesPage()
        {
            InitializeComponent();
            SelectedDocument = null;
            CurrentDocumentText = "";
            Lock = false;
            FontFamilyList.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontSizeList.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        #endregion

        #region Methods

        public void Draw()
        {
            //DrawNotes();
            DrawTreeView();
        }

        private void DrawNotes()
        {
            if (Program.Character.Chapters.Count > 0 && Program.Character.Chapters[0].Documents.Count > 0)
            {
                CurrentDocumentText = Program.Character.Chapters[0].Documents[0].RTF;
                LoadCurrentDocument(CurrentDocumentText);
            }
        }

        private void DrawTreeView()
        {
            NotesTreeView.Items.Clear();

            foreach (var chapter in Program.Character.Chapters)
            {
                TreeViewItem treeViewChapter = new TreeViewItem()
                {
                    Header = chapter.Name,
                    Tag = chapter,
                    Foreground = Brushes.White
                };

                foreach (var document in chapter.Documents)
                {
                    TreeViewItem treeViewDocument = new TreeViewItem()
                    {
                        Header = document.Name,
                        Tag = document,
                        Foreground = Brushes.White
                    };

                    treeViewChapter.Items.Add(treeViewDocument);
                }

                NotesTreeView.Items.Add(treeViewChapter);
            }
        }

        private void LoadCurrentDocument(string text)
        {
            MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(text));
            NotesTextBox.Document.Blocks.Clear();
            NotesTextBox.Selection.Load(stream, DataFormats.Rtf);
        }

        private string SaveCurrentDocument()
        {
            TextRange range = new TextRange(NotesTextBox.Document.ContentStart, NotesTextBox.Document.ContentEnd);

            try
            {
                using (MemoryStream rtfMemoryStream = new MemoryStream())
                {
                    using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                    {
                        range.Save(rtfMemoryStream, DataFormats.Rtf);

                        rtfMemoryStream.Flush();
                        rtfMemoryStream.Position = 0;
                        StreamReader sr = new StreamReader(rtfMemoryStream);
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return string.Empty;
            }
        }

        #endregion

        #region Accessors

        public string CurrentDocumentText { get; set; }
        private bool Lock { get; set; }
        public Document SelectedDocument { get; set; }

        #endregion

        #region Events

        private void NotesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem treeViewItem;

            if (!Lock)
            {
                Lock = true;

                treeViewItem = (NotesTreeView?.SelectedItem as TreeViewItem);

                if (treeViewItem?.Parent is TreeViewItem)
                {
                    if (SelectedDocument != null)
                    {
                        SelectedDocument.RTF = SaveCurrentDocument();
                    }

                    SelectedDocument = treeViewItem.Tag as Document;
                    LoadCurrentDocument(SelectedDocument.RTF);
                }

                Lock = false;
            }
        }

        #region Toolstrip Events

        private void ButtonCut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonPaste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonRedo_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonBold_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ButtonBold.IsChecked)
            {
                NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void ButtonItalic_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ButtonItalic.IsChecked)
            {
                NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void ButtonUnderline_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ButtonUnderline.IsChecked)
            {
                NotesTextBox.Selection.ApplyPropertyValue(TextDecoration.PenProperty, TextDecorations.Underline);
            }
            else
            {
                //NotesTextBox.Selection.ApplyPropertyValue(TextDecoration.PenProperty, TextDecorations.);
            }
        }

        private void ButtonFont_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSpellcheck_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonColor_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBullets_Click(object sender, RoutedEventArgs e)
        {
           // NotesTextBox.Selection.ApplyPropertyValue(Inline.)
        }

        private void ButtonNumbering_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAlignLeft_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAlignCentre_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAlignRight_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #endregion

        private void NotesTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object obj;

            obj = NotesTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            ButtonBold.IsChecked = (obj != DependencyProperty.UnsetValue) && (obj.Equals(FontWeights.Bold));

            obj = NotesTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ButtonItalic.IsChecked = (obj != DependencyProperty.UnsetValue) && (obj.Equals(FontStyles.Italic));
        }

        private void FontFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FontSizeList_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
