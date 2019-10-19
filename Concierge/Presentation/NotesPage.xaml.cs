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
using Xceed.Wpf.Toolkit;

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
            
        }

        #endregion

        #region Methods

        public void Draw()
        {
            DrawNotes();
            DrawTreeView();
        }

        private void DrawNotes()
        {
            if (Program.Character.Chapters.Count > 0)
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

        public Document SelectedDocument { get; set; }

        #endregion

        #region Events

        private void NotesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedDocument != null)
                ;// SaveCurrentDocument();


            //SelectedDocument = (NotesTreeView?.SelectedItem as TreeViewItem)?.Tag as Document;
            Draw();
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

        }

        private void ButtonItalic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonUnderline_Click(object sender, RoutedEventArgs e)
        {

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

    }
}
