using Concierge.Persistence;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Concierge.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private SaveFileDialog saveFileDialog = new SaveFileDialog();

        InventoryPage InventoryPage = new InventoryPage();

        public MainWindow()
        {
            InitializeComponent();


            GridContent.Width = GridContentWidthClose;

            InventoryPage.Visibility = Visibility.Collapsed;
            FrameContent.Content = InventoryPage;

            DataContext = this;
        }

        public void CloseWindow()
        {
            if (Program.Modified)
            {
                // TODO Display popup - would you like to close without saving?
            }

            Close();
        }

        public void NewCharacterSheet()
        {
            Program.CcsFile = null;
            Program.Character.Reset();

            DrawAll();
        }

        public void OpenCharacterSheet()
        {
            if (openFileDialog.ShowDialog() ?? false)
            {
                Program.CcsFile = new CcsFile();
                Program.CcsFile.Path = openFileDialog.FileName;

                Program.Character.Reset();
                CharacterLoader.LoadCharacterSheet(Program.Character, Program.CcsFile);

                DrawAll();
            }
        }

        public void SaveCharacterSheet()
        {
            
            if (Program.CcsFile != null)
            {
                CharacterSaver.SaveCharacterSheet(Program.Character, Program.CcsFile);
            }
            else
            {
                SaveCharacterSheetAs();
            }
        }

        public void SaveCharacterSheetAs()
        {
            if (saveFileDialog.ShowDialog() ?? false)
            {

                Program.CcsFile = new CcsFile();
                Program.CcsFile.Path = saveFileDialog.FileName;

                CharacterSaver.SaveCharacterSheet(Program.Character, Program.CcsFile);
            }
        }

        public void DrawAll()
        {
            TextCharacterName.Text = Program.Character.Details.Name;
            TextCharacterRace.Text = Program.Character.Details.Race;

            if (Program.Character.Level > 0)
            {
                TextCharacterLevel.Text = "Level " + Program.Character.Level;
            }
            else
            {
                TextCharacterLevel.Text = "";
            }

            TextCharacterClass.Text = Program.Character.GetClasses;

            InventoryPage.Draw();
        }

        public void LongRest()
        {
            Program.Character.LongRest();

            DrawAll();
        }

        private bool IsControl()
        {
            return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        }

        private bool IsShift()
        {
            return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        }

        public double GridContentWidthOpen
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth - 200;
            }
        }

        public double GridContentWidthClose
        {
            get
            {
                return SystemParameters.PrimaryScreenWidth - 60;
            }
        }

        private void MainWindow_KeyPress(object sender, KeyEventArgs e)
        {
            if (!Program.Typing)
            {
                switch (e.Key)
                {
                    // Long Rest
                    case Key.L:
                        if (IsControl())
                        {
                            LongRest();
                        }
                        break;
                    // New Character Sheet
                    case Key.N:
                        if (IsControl())
                        {
                            NewCharacterSheet();
                        }
                        break;
                    // Open Character Sheet
                    case Key.O:
                        if (IsControl())
                        {
                            OpenCharacterSheet();
                        }
                        break;
                    // Close Window 
                    case Key.Q:
                        if (IsControl())
                        {
                            CloseWindow();
                        }
                        break;
                    // Save Character Sheet
                    case Key.S:
                        if (IsControl() && IsShift())
                        {
                            SaveCharacterSheetAs();
                        }
                        else if (IsControl())
                        {
                            SaveCharacterSheet();
                        }
                        break;
                    // --------------------------------------------------------------
                    // Page 1
                    case Key.D1:
                        break;
                    // Page 2
                    case Key.D2:
                        break;
                    // Page 3
                    case Key.D3:
                        break;
                    // Page 4
                    case Key.D4:
                        break;
                    // Page 5
                    case Key.D5:
                        break;
                    // Page 6
                    case Key.D6:
                        break;
                    // Page 7
                    case Key.D7:
                        break;
                    // Page 8
                    case Key.D8:
                        break;
                }
            }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;

            GridContent.Width = GridContentWidthOpen;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;

            GridContent.Width = GridContentWidthClose;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void ButtonNewCharacter_Click(object sender, RoutedEventArgs e)
        {
            NewCharacterSheet();
        }

        private void ButtonOpenCharacter_Click(object sender, RoutedEventArgs e)
        {
            OpenCharacterSheet();
        }

        private void ButtonSaveCharacter_Click(object sender, RoutedEventArgs e)
        {
            SaveCharacterSheet();
        }

        private void ButtonSaveCharacterAs_Click(object sender, RoutedEventArgs e)
        {
            SaveCharacterSheetAs();
        }

        private void ButtonLongRest_Click(object sender, RoutedEventArgs e)
        {
            LongRest();
        }

        private void ItemNotes_Selected(object sender, RoutedEventArgs e)
        {
            InventoryPage.Visibility = Visibility.Collapsed;
        }

        private void ItemInventory_Selected(object sender, RoutedEventArgs e)
        {
            InventoryPage.Visibility = Visibility.Visible;
            DrawAll();
        }

        private void ItemDetails_Selected(object sender, RoutedEventArgs e)
        {
            InventoryPage.Visibility = Visibility.Collapsed;
        }

        private void ItemEquipment_Selected(object sender, RoutedEventArgs e)
        {
            InventoryPage.Visibility = Visibility.Collapsed;
        }

        private void ItemOverview_Selected(object sender, RoutedEventArgs e)
        {
            InventoryPage.Visibility = Visibility.Collapsed;
        }

        private void ItemSpellcasting_Selected(object sender, RoutedEventArgs e)
        {
            InventoryPage.Visibility = Visibility.Collapsed;
        }
    }
}
