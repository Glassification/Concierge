// <copyright file="MainWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Persistence;
    using Concierge.Presentation.AbilitiesPageUi;
    using Concierge.Presentation.DetailsPageUi;
    using Concierge.Presentation.EquipmentPageUi;
    using Concierge.Presentation.InventoryPageUi;
    using Concierge.Presentation.NotesPageUi;
    using Concierge.Presentation.OverviewPageUi;
    using Concierge.Presentation.SpellcastingPageUi;
    using Concierge.Presentation.ToolsPageUi;
    using Concierge.Utility;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private SaveFileDialog saveFileDialog = new SaveFileDialog();

        InventoryPage InventoryPage = new InventoryPage();
        EquipmentPage EquipmentPage = new EquipmentPage();
        AbilitiesPage AbilitiesPage = new AbilitiesPage();
        OverviewPage OverviewPage = new OverviewPage();
        DetailsPage DetailsPage = new DetailsPage();
        NotesPage NotesPage = new NotesPage();
        SpellcastingPage SpellcastingPage = new SpellcastingPage();
        ToolsPage ToolsPage = new ToolsPage();

        public MainWindow()
        {
            this.InitializeComponent();

            this.Style = (Style)this.FindResource(typeof(Window));

            this.GridContent.Width = this.GridContentWidthClose;

            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.OverviewPage;

            this.DataContext = this;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            App.Current.Shutdown();
        }

        public void CloseWindow()
        {
            if (Program.Modified)
            {
                // TODO Display popup - would you like to close without saving?
            }

            this.Close();
        }

        public void NewCharacterSheet()
        {
            Program.CcsFile = null;
            Program.Character.Reset();

            this.NotesPage.ClearTextBox();
            this.DrawAll();
        }

        public void OpenCharacterSheet()
        {
            if (this.openFileDialog.ShowDialog() ?? false)
            {
                Program.CcsFile = new CcsFile();
                Program.CcsFile.Path = this.openFileDialog.FileName;

                Program.Character.Reset();
                CharacterLoader.LoadCharacterSheet(Program.Character, Program.CcsFile);

                this.DrawAll();
            }
        }

        public void SaveCharacterSheet()
        {
            if (Program.CcsFile != null)
            {
                this.NotesPage.SaveTextBox();
                CharacterSaver.SaveCharacterSheet(Program.Character, Program.CcsFile);
            }
            else
            {
                this.SaveCharacterSheetAs();
            }
        }

        public void SaveCharacterSheetAs()
        {
            if (this.saveFileDialog.ShowDialog() ?? false)
            {
                Program.CcsFile = new CcsFile();
                Program.CcsFile.Path = this.saveFileDialog.FileName;

                this.NotesPage.SaveTextBox();
                CharacterSaver.SaveCharacterSheet(Program.Character, Program.CcsFile);
            }
        }

        public void DrawAll()
        {
            this.TextCharacterName.Text = Program.Character.Details.Name;
            this.TextCharacterRace.Text = Program.Character.Details.Race;
            this.TextCharacterBackground.Text = Program.Character.Details.Background;
            this.TextCharacterAlignment.Text = Program.Character.Details.Alignment;

            if (Program.Character.Level > 0)
            {
                this.TextCharacterLevel.Text = "Level " + Program.Character.Level;
            }
            else
            {
                this.TextCharacterLevel.Text = string.Empty;
            }

            if (Program.Character.Level > 0 && Program.Character.Level <= Constants.MAX_LEVEL)
            {
                this.TextCharacterXp.Text = Program.Character.Details.Experience + "/" + Program.Character.ExperienceToLevel + " Experience";
            }
            else
            {
                this.TextCharacterLevel.Text = string.Empty;
            }

            this.TextCharacterClass.Text = Program.Character.GetClasses;

            this.InventoryPage.Draw();
            this.AbilitiesPage.Draw();
            this.EquipmentPage.Draw();
            this.OverviewPage.Draw();
            this.DetailsPage.Draw();
            this.NotesPage.Draw();
            this.SpellcastingPage.Draw();
            this.ToolsPage.Draw();
        }

        public void LongRest()
        {
            Program.Character.LongRest();

            this.DrawAll();
        }

        private bool IsControl()
        {
            return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        }

        private bool IsShift()
        {
            return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        }

        public double GridContentWidthOpen => SystemParameters.PrimaryScreenWidth - 200;

        public double GridContentWidthClose => SystemParameters.PrimaryScreenWidth - 60;

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            this.FrameContent.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void MainWindow_KeyPress(object sender, KeyEventArgs e)
        {
            if (!Program.Typing)
            {
                switch (e.Key)
                {
                    // Long Rest
                    case Key.L:
                        if (this.IsControl())
                        {
                            this.LongRest();
                        }
                        break;
                    // New Character Sheet
                    case Key.N:
                        if (this.IsControl())
                        {
                            this.NewCharacterSheet();
                        }
                        break;
                    // Open Character Sheet
                    case Key.O:
                        if (this.IsControl())
                        {
                            this.OpenCharacterSheet();
                        }
                        break;
                    // Close Window
                    case Key.Q:
                        if (this.IsControl())
                        {
                            this.CloseWindow();
                        }
                        break;
                    // Save Character Sheet
                    case Key.S:
                        if (this.IsControl() && this.IsShift())
                        {
                            this.SaveCharacterSheetAs();
                        }
                        else if (this.IsControl())
                        {
                            this.SaveCharacterSheet();
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
            this.ButtonCloseMenu.Visibility = Visibility.Visible;
            this.ButtonOpenMenu.Visibility = Visibility.Collapsed;

            this.GridContent.Width = this.GridContentWidthOpen;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonCloseMenu.Visibility = Visibility.Collapsed;
            this.ButtonOpenMenu.Visibility = Visibility.Visible;

            this.GridContent.Width = this.GridContentWidthClose;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow();
        }

        private void ButtonNewCharacter_Click(object sender, RoutedEventArgs e)
        {
            this.NewCharacterSheet();
        }

        private void ButtonOpenCharacter_Click(object sender, RoutedEventArgs e)
        {
            this.OpenCharacterSheet();
        }

        private void ButtonSaveCharacter_Click(object sender, RoutedEventArgs e)
        {
            this.SaveCharacterSheet();
        }

        private void ButtonSaveCharacterAs_Click(object sender, RoutedEventArgs e)
        {
            this.SaveCharacterSheetAs();
        }

        private void ButtonLongRest_Click(object sender, RoutedEventArgs e)
        {
            this.LongRest();
        }

        private void ItemNotes_Selected(object sender, RoutedEventArgs e)
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.NotesPage;
            this.NotesPage.Draw();
        }

        private void ItemInventory_Selected(object sender, RoutedEventArgs e)
        {
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.InventoryPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.InventoryPage;
            this.InventoryPage.Draw();
        }

        private void ItemDetails_Selected(object sender, RoutedEventArgs e)
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.DetailsPage;
            this.DetailsPage.Draw();
        }

        private void ItemAbilities_Selected(object sender, RoutedEventArgs e)
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.AbilitiesPage;
            this.AbilitiesPage.Draw();
        }

        private void ItemEquipment_Selected(object sender, RoutedEventArgs e)
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.EquipmentPage;
            this.EquipmentPage.Draw();
        }

        private void ItemOverview_Selected(object sender, RoutedEventArgs e)
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.OverviewPage;
            this.OverviewPage.Draw();
        }

        private void ItemSpellcasting_Selected(object sender, RoutedEventArgs e)
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Visible;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.FrameContent.Content = this.SpellcastingPage;
            this.SpellcastingPage.Draw();
        }

        private void ItemTools_Selected(object sender, RoutedEventArgs e)
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.ToolsPage;
            this.ToolsPage.Draw();
        }

        private void ButtonOpenMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ButtonOpenMenu.Foreground = Brushes.Black;
        }

        private void ButtonOpenMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ButtonOpenMenu.Foreground = Brushes.White;
        }

        private void ButtonCloseMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ButtonCloseMenu.Foreground = Brushes.Black;
        }

        private void ButtonCloseMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ButtonCloseMenu.Foreground = Brushes.White;
        }

        private void ButtonClose_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ButtonClose.Foreground = Brushes.Black;
        }

        private void ButtonClose_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ButtonClose.Foreground = Brushes.White;
        }
    }
}
