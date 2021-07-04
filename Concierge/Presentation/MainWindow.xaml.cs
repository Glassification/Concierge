// <copyright file="MainWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Persistence;
    using Concierge.Presentation.AbilitiesPageUi;
    using Concierge.Presentation.DetailsPageUi;
    using Concierge.Presentation.Enums;
    using Concierge.Presentation.EquipmentPageUi;
    using Concierge.Presentation.HelperUi;
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
        private readonly OpenFileDialog openFileDialog = new OpenFileDialog();
        private readonly SaveFileDialog saveFileDialog = new SaveFileDialog();
        private readonly SettingsWindow settingsWindow = new SettingsWindow();
        private readonly ModifyPropertiesWindow modifyPropertiesWindow = new ModifyPropertiesWindow();

        private readonly AutosaveTimer autosaveTimer = new AutosaveTimer();

        private readonly InventoryPage InventoryPage = new InventoryPage();
        private readonly EquipmentPage EquipmentPage = new EquipmentPage();
        private readonly AbilitiesPage AbilitiesPage = new AbilitiesPage();
        private readonly OverviewPage OverviewPage = new OverviewPage();
        private readonly DetailsPage DetailsPage = new DetailsPage();
        private readonly NotesPage NotesPage = new NotesPage();
        private readonly SpellcastingPage SpellcastingPage = new SpellcastingPage();
        private readonly ToolsPage ToolsPage = new ToolsPage();

        public MainWindow()
        {
            this.InitializeComponent();

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

        public double GridContentWidthOpen => SystemParameters.PrimaryScreenWidth - 200;

        public double GridContentWidthClose => SystemParameters.PrimaryScreenWidth - 60;

        public void CloseWindow()
        {
            if (Program.Modified)
            {
                switch (Program.ConciergeMessageWindow.ShowWindow(
                    "You have unsaved changes, would you like to save before closing?",
                    MessageWindowButtons.YesNoCancel))
                {
                    case MessageWindowResult.OK:
                        this.SaveCharacterSheet();
                        this.Close();
                        break;
                    case MessageWindowResult.No:
                        this.Close();
                        break;
                    default:
                    case MessageWindowResult.Cancel:
                        break;
                }
            }
            else
            {
                this.Close();
            }
        }

        public void NewCharacterSheet()
        {
            Program.CcsFile = null;
            Program.Character.Reset();

            this.NotesPage.ClearTextBox();
            this.DrawAll();

            this.autosaveTimer.Stop();
        }

        public void OpenCharacterSheet()
        {
            if (this.openFileDialog.ShowDialog() ?? false)
            {
                this.autosaveTimer.Stop();

                Program.CcsFile = new CcsFile
                {
                    Path = this.openFileDialog.FileName,
                };

                Program.Character.Reset();
                CharacterLoader.LoadCharacterSheet(Program.Character, Program.CcsFile);

                if (Settings.AutosaveEnable)
                {
                    this.autosaveTimer.Start(Constants.AutosaveIntervals[Settings.AutosaveInterval]);
                }

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private bool IsControl()
        {
            return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        }

        private bool IsShift()
        {
            return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            this.FrameContent.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void MainWindow_KeyPress(object sender, KeyEventArgs e)
        {
            if (!Program.Typing)
            {
                if (this.IsControl())
                {
                    switch (e.Key)
                    {
                        case Key.L:
                            this.LongRest();
                            break;
                        case Key.N:
                            this.NewCharacterSheet();
                            break;
                        case Key.O:
                            this.OpenCharacterSheet();
                            break;
                        case Key.P:
                            this.modifyPropertiesWindow.ShowWindow();
                            this.DrawAll();
                            break;
                        case Key.Q:
                            this.CloseWindow();
                            break;
                        case Key.S:
                            if (this.IsShift())
                            {
                                this.SaveCharacterSheetAs();
                            }
                            else
                            {
                                this.SaveCharacterSheet();
                            }

                            break;
                        case Key.D1:
                            this.MoveSelection(0);
                            break;
                        case Key.D2:
                            this.MoveSelection(1);
                            break;
                        case Key.D3:
                            this.MoveSelection(2);
                            break;
                        case Key.D4:
                            this.MoveSelection(3);
                            break;
                        case Key.D5:
                            this.MoveSelection(4);
                            break;
                        case Key.D6:
                            this.MoveSelection(5);
                            break;
                        case Key.D7:
                            this.MoveSelection(6);
                            break;
                        case Key.D8:
                            this.MoveSelection(7);
                            break;
                    }
                }
            }
        }

        private void MoveSelection(int index)
        {
            if (index >= 0 && index < this.ListViewMenu.Items.Count)
            {
                this.ListViewMenu.SelectedItem = this.ListViewMenu.Items[index];
                this.UpdateLayout();
                ((ListViewItem)this.ListViewMenu.ItemContainerGenerator.ContainerFromIndex(index)).Focus();
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

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyPropertiesWindow.ShowWindow();
            this.DrawAll();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.settingsWindow.ShowWindow();
            this.OverviewPage.Draw();
            this.DetailsPage.Draw();

            if (Settings.AutosaveEnable)
            {
                this.autosaveTimer.Start(Constants.AutosaveIntervals[Settings.AutosaveInterval]);
            }
            else
            {
                this.autosaveTimer.Stop();
            }
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

        private void ButtonMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ButtonMinimize.Foreground = Brushes.Black;
        }

        private void ButtonMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ButtonMinimize.Foreground = Brushes.White;
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
