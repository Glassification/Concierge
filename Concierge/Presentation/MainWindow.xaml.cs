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
    using Concierge.Presentation.EquipedItemsPageUi;
    using Concierge.Presentation.EquipmentPageUi;
    using Concierge.Presentation.HelperUi;
    using Concierge.Presentation.InventoryPageUi;
    using Concierge.Presentation.NotesPageUi;
    using Concierge.Presentation.OverviewPageUi;
    using Concierge.Presentation.SpellcastingPageUi;
    using Concierge.Presentation.ToolsPageUi;
    using Concierge.Services;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileAccessService fileAccessService = new FileAccessService();
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
        private readonly EquipedItemsPage EquipedItemsPage = new EquipedItemsPage();

        public MainWindow()
        {
            this.InitializeComponent();

            this.GridContent.Width = GridContentWidthClose;

            this.CollapseAll();
            this.OverviewPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.OverviewPage;

            this.DataContext = this;

            if (Program.CcsFile.AutosaveEnable)
            {
                this.autosaveTimer.Start(Constants.AutosaveIntervals[Program.CcsFile.AutosaveInterval]);
            }

            this.DrawAll();

            Program.Logger.Info($"{nameof(MainWindow)} loaded.");
        }

        public static double GridContentWidthOpen => SystemParameters.PrimaryScreenWidth - 200;

        public static double GridContentWidthClose => SystemParameters.PrimaryScreenWidth - 60;

        private static bool IsControl => (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

        private static bool IsShift => (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

        public void CloseWindow()
        {
            if (Program.Modified)
            {
                switch (Program.ConciergeMessageWindow.ShowWindow(
                    "You have unsaved changes, would you like to save before closing?",
                    "Warning",
                    MessageWindowButtons.YesNoCancel,
                    MessageWindowIcons.Question))
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
            Program.Logger.Info($"Creating new character sheet.");

            Program.CcsFile = new CcsFile();

            this.NotesPage.ClearTextBox();
            this.DrawAll();

            this.autosaveTimer.Stop();
        }

        public void OpenCharacterSheet()
        {
            Program.Logger.Info($"Opening character sheet.");

            var ccsFile = this.fileAccessService.Open();

            if (ccsFile == null)
            {
                return;
            }

            this.autosaveTimer.Stop();

            Program.CcsFile = ccsFile;
            if (Program.CcsFile.AutosaveEnable)
            {
                this.autosaveTimer.Start(Constants.AutosaveIntervals[Program.CcsFile.AutosaveInterval]);
            }

            this.DrawAll();
        }

        public void SaveCharacterSheet()
        {
            Program.Logger.Info($"Save character sheet.");

            if (!Program.CcsFile.AbsolutePath.IsNullOrWhiteSpace())
            {
                this.NotesPage.SaveTextBox();
                this.fileAccessService.Save(Program.CcsFile, false);
            }
            else
            {
                this.SaveCharacterSheetAs();
            }
        }

        public void SaveCharacterSheetAs()
        {
            Program.Logger.Info($"Save as character sheet.");

            this.NotesPage.SaveTextBox();
            this.fileAccessService.Save(Program.CcsFile, true);
        }

        public void DrawAll()
        {
            this.TextCharacterName.Text = Program.CcsFile.Character.Details.Name;
            this.TextCharacterRace.Text = Program.CcsFile.Character.Details.Race;
            this.TextCharacterBackground.Text = Program.CcsFile.Character.Details.Background;
            this.TextCharacterAlignment.Text = Program.CcsFile.Character.Details.Alignment;

            this.TextCharacterLevel.Text = Program.CcsFile.Character.Level > 0 ? "Level " + Program.CcsFile.Character.Level : string.Empty;

            this.TextCharacterClass.Text = Program.CcsFile.Character.GetClasses;

            this.InventoryPage.Draw();
            this.AbilitiesPage.Draw();
            this.EquipmentPage.Draw();
            this.OverviewPage.Draw();
            this.DetailsPage.Draw();
            this.NotesPage.Draw();
            this.SpellcastingPage.Draw();
            this.ToolsPage.Draw();
            this.EquipedItemsPage.Draw();
        }

        public void LongRest()
        {
            Program.Logger.Info($"Long rest.");
            Program.CcsFile.Character.LongRest();

            this.DrawAll();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Program.Logger.Info("Closing Concierge.");
            Application.Current.Shutdown();
        }

        private void CollapseAll()
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.EquipedItemsPage.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            this.FrameContent.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void MainWindow_KeyPress(object sender, KeyEventArgs e)
        {
            if (Program.Typing || !IsControl)
            {
                return;
            }

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
                    if (IsShift)
                    {
                        this.SaveCharacterSheetAs();
                    }
                    else
                    {
                        this.SaveCharacterSheet();
                    }

                    break;
                case Key.OemMinus:
                    this.WindowState = WindowState.Minimized;
                    break;
                case Key.OemPlus:
                    this.WindowState = WindowState.Maximized;
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
                case Key.D9:
                    this.MoveSelection(8);
                    break;
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
            Program.Logger.Info($"Expand sidebar.");

            this.ButtonCloseMenu.Visibility = Visibility.Visible;
            this.ButtonOpenMenu.Visibility = Visibility.Collapsed;

            this.GridContent.Width = GridContentWidthOpen;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Collapse sidebar.");

            this.ButtonCloseMenu.Visibility = Visibility.Collapsed;
            this.ButtonOpenMenu.Visibility = Visibility.Visible;

            this.GridContent.Width = GridContentWidthClose;
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
            Program.Logger.Info($"Navigate to Notes page.");

            this.CollapseAll();
            this.NotesPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.NotesPage;
            this.NotesPage.Draw();
        }

        private void ItemInventory_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Inventory page.");

            this.CollapseAll();
            this.InventoryPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.InventoryPage;
            this.InventoryPage.Draw();
        }

        private void ItemEquipedItems_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Equiped Items page.");

            this.CollapseAll();
            this.EquipedItemsPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.EquipedItemsPage;
            this.EquipedItemsPage.Draw();
        }

        private void ItemDetails_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Details page.");

            this.CollapseAll();
            this.DetailsPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.DetailsPage;
            this.DetailsPage.Draw();
        }

        private void ItemAbilities_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Abilities page.");

            this.CollapseAll();
            this.AbilitiesPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.AbilitiesPage;
            this.AbilitiesPage.Draw();
        }

        private void ItemEquipment_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Equipment page.");

            this.CollapseAll();
            this.EquipmentPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.EquipmentPage;
            this.EquipmentPage.Draw();
        }

        private void ItemOverview_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Overview page.");

            this.CollapseAll();
            this.OverviewPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.OverviewPage;
            this.OverviewPage.Draw();
        }

        private void ItemSpellcasting_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Spellcasting page.");

            this.CollapseAll();
            this.SpellcastingPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.SpellcastingPage;
            this.SpellcastingPage.Draw();
        }

        private void ItemTools_Selected(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Navigate to Tools page.");

            this.CollapseAll();
            this.ToolsPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.ToolsPage;
            this.ToolsPage.Draw();
        }

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Open properties.");

            this.modifyPropertiesWindow.ShowWindow();
            this.DrawAll();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Open settings.");

            this.settingsWindow.ShowWindow();
            this.OverviewPage.Draw();
            this.DetailsPage.Draw();

            if (Program.CcsFile.AutosaveEnable)
            {
                this.autosaveTimer.Start(Constants.AutosaveIntervals[Program.CcsFile.AutosaveInterval]);
            }
            else
            {
                this.autosaveTimer.Stop();
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = Brushes.White;
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
