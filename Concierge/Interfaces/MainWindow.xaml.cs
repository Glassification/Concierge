// <copyright file="MainWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.CompanionPageInterface;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.EquipmentPageInterface;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.HelperInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.NotesPageInterface;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;
    using Concierge.Interfaces.ToolsPageInterface;
    using Concierge.Persistence;
    using Concierge.Services;
    using Concierge.Tools;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileAccessService fileAccessService = new ();
        private readonly SettingsWindow settingsWindow = new ();
        private readonly ModifyPropertiesWindow modifyPropertiesWindow = new ();
        private readonly AboutConciergeWindow aboutConciergeWindow = new ();

        private readonly AutosaveTimer autosaveTimer = new ();
        private readonly CharacterCreationWizard characterCreationWizard = new ();

        private readonly InventoryPage inventoryPage = new ();
        private readonly EquipmentPage equipmentPage = new ();
        private readonly AbilitiesPage abilitiesPage = new ();
        private readonly OverviewPage overviewPage = new ();
        private readonly DetailsPage detailsPage = new ();
        private readonly NotesPage notesPage = new ();
        private readonly SpellcastingPage spellcastingPage = new ();
        private readonly ToolsPage toolsPage = new ();
        private readonly EquippedItemsPage equipedItemsPage = new ();
        private readonly CompanionPage companionPage = new ();

        public MainWindow()
        {
            this.InitializeComponent();

            this.GridContent.Width = GridContentWidthClose;

            this.CollapseAll();
            this.overviewPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.overviewPage;

            this.DataContext = this;

            if (Program.CcsFile.AutosaveEnable)
            {
                this.autosaveTimer.Start(Constants.AutosaveIntervals[Program.CcsFile.AutosaveInterval]);
            }

            this.DrawAll();

            Program.Logger.Info($"{nameof(MainWindow)} loaded.");
            Program.Unmodify();
        }

        public static double GridContentWidthOpen => SystemParameters.PrimaryScreenWidth - 200;

        public static double GridContentWidthClose => SystemParameters.PrimaryScreenWidth - 60;

        private static bool IsControl => (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

        private static bool IsShift => (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

        private bool IgnoreSecondPress { get; set; }

        public void CloseWindow()
        {
            var result = this.CheckSaveBeforeAction("closing");

            if (result != MessageWindowResult.Cancel)
            {
                this.Close();
            }
        }

        public void NewCharacterSheet()
        {
            Program.Logger.Info($"Creating new character sheet.");

            var result = this.CheckSaveBeforeAction("creating a new sheet");

            if (result == MessageWindowResult.Cancel)
            {
                return;
            }

            this.ResetCharacterSheet();
            this.DrawAll();

            Program.Unmodify();
        }

        public void CreateCharacterWizard()
        {
            Program.Logger.Info($"Open Character Creation.");

            var result = this.CheckSaveBeforeAction("creating a new character");

            if (result == MessageWindowResult.Cancel)
            {
                return;
            }

            this.ResetCharacterSheet();
            this.characterCreationWizard.Start();
        }

        public void OpenCharacterSheet()
        {
            Program.Logger.Info($"Opening character sheet.");

            var result = this.CheckSaveBeforeAction("opening");

            if (result == MessageWindowResult.Cancel)
            {
                return;
            }

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
                this.notesPage.SaveTextBox();
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

            this.notesPage.SaveTextBox();
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

            this.inventoryPage.Draw();
            this.abilitiesPage.Draw();
            this.equipmentPage.Draw();
            this.overviewPage.Draw();
            this.detailsPage.Draw();
            this.notesPage.Draw();
            this.spellcastingPage.Draw();
            this.toolsPage.Draw();
            this.equipedItemsPage.Draw();
            this.companionPage.Draw();
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

        private MessageWindowResult CheckSaveBeforeAction(string action)
        {
            if (!Program.Modified)
            {
                return MessageWindowResult.No;
            }

            var result = Program.ConciergeMessageWindow.ShowWindow(
                $"You have unsaved changes. Would you like to save before {action}?",
                "Warning",
                MessageWindowButtons.YesNoCancel,
                MessageWindowIcons.Warning);

            if (result == MessageWindowResult.Yes)
            {
                this.SaveCharacterSheet();
            }

            return result;
        }

        private void CollapseAll()
        {
            this.inventoryPage.Visibility = Visibility.Collapsed;
            this.equipmentPage.Visibility = Visibility.Collapsed;
            this.abilitiesPage.Visibility = Visibility.Collapsed;
            this.detailsPage.Visibility = Visibility.Collapsed;
            this.notesPage.Visibility = Visibility.Collapsed;
            this.spellcastingPage.Visibility = Visibility.Collapsed;
            this.toolsPage.Visibility = Visibility.Collapsed;
            this.overviewPage.Visibility = Visibility.Collapsed;
            this.equipedItemsPage.Visibility = Visibility.Collapsed;
            this.companionPage.Visibility = Visibility.Collapsed;
        }

        private void PageSelection(IConciergePage conciergePage)
        {
            ConciergeSound.TapNavigation();

            var page = conciergePage as Page;

            this.CollapseAll();
            page.Visibility = Visibility.Visible;
            this.FrameContent.Content = page;
            conciergePage.Draw();

            Program.Logger.Info($"Navigate to {page.GetType().Name}");
        }

        private void ResetCharacterSheet()
        {
            Program.CcsFile = new CcsFile();

            this.notesPage.ClearTextBox();
            this.DrawAll();

            this.autosaveTimer.Stop();
        }

        private void MoveSelection(ConciergePage page)
        {
            if (page >= 0 && ((int)page) < this.ListViewMenu.Items.Count)
            {
                this.ListViewMenu.SelectedItem = this.ListViewMenu.Items[(int)page];
                this.UpdateLayout();
                ((ListViewItem)this.ListViewMenu.ItemContainerGenerator.ContainerFromIndex((int)page)).Focus();
            }
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
                case Key.C:
                    this.CreateCharacterWizard();
                    break;
                case Key.H:
                    this.aboutConciergeWindow.ShowWindow();
                    break;
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
                    this.MoveSelection(ConciergePage.Overview);
                    break;
                case Key.D2:
                    this.MoveSelection(ConciergePage.Details);
                    break;
                case Key.D3:
                    this.MoveSelection(ConciergePage.Equipment);
                    break;
                case Key.D4:
                    this.MoveSelection(ConciergePage.Abilities);
                    break;
                case Key.D5:
                    this.MoveSelection(ConciergePage.EquippedItems);
                    break;
                case Key.D6:
                    this.MoveSelection(ConciergePage.Inventory);
                    break;
                case Key.D7:
                    this.MoveSelection(ConciergePage.Spellcasting);
                    break;
                case Key.D8:
                    this.MoveSelection(ConciergePage.Companion);
                    break;
                case Key.D9:
                    this.MoveSelection(ConciergePage.Tools);
                    break;
                case Key.D0:
                    this.MoveSelection(ConciergePage.Notes);
                    break;
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
            ConciergeSound.TapNavigation();
            this.NewCharacterSheet();
            this.IgnoreSecondPress = true;
        }

        private void ButtonOpenCharacter_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.OpenCharacterSheet();
            this.IgnoreSecondPress = true;
        }

        private void ButtonSaveCharacter_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.SaveCharacterSheet();
            this.IgnoreSecondPress = true;
        }

        private void ButtonSaveCharacterAs_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.SaveCharacterSheetAs();
            this.IgnoreSecondPress = true;
        }

        private void ButtonLongRest_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.LongRest();
            this.IgnoreSecondPress = true;
        }

        private void ItemNotes_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.notesPage);
        }

        private void ItemInventory_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.inventoryPage);
        }

        private void ItemEquipedItems_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.equipedItemsPage);
        }

        private void ItemDetails_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.detailsPage);
        }

        private void ItemAbilities_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.abilitiesPage);
        }

        private void ItemEquipment_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.equipmentPage);
        }

        private void ItemOverview_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.overviewPage);
        }

        private void ItemSpellcasting_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.spellcastingPage);
        }

        private void ItemCompanion_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.companionPage);
        }

        private void ItemTools_Selected(object sender, RoutedEventArgs e)
        {
            this.PageSelection(this.toolsPage);
        }

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"Open properties.");

            this.modifyPropertiesWindow.ShowWindow();
            this.DrawAll();

            this.IgnoreSecondPress = true;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"Open settings.");

            this.settingsWindow.ShowWindow();
            this.overviewPage.Draw();
            this.detailsPage.Draw();

            if (Program.CcsFile.AutosaveEnable)
            {
                this.autosaveTimer.Start(Constants.AutosaveIntervals[Program.CcsFile.AutosaveInterval]);
            }
            else
            {
                this.autosaveTimer.Stop();
            }

            this.IgnoreSecondPress = true;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"Open About.");

            this.aboutConciergeWindow.ShowWindow();

            this.IgnoreSecondPress = true;
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void PopupBoxButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.PopupBoxControl.Foreground = Brushes.Black;
        }

        private void PopupBoxButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.PopupBoxControl.Foreground = Brushes.White;
        }

        private void PopupBoxButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IgnoreSecondPress)
            {
                this.PopupBoxControl.IsPopupOpen = true;
            }

            this.IgnoreSecondPress = false;
        }

        private void CharacterCreationButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.CreateCharacterWizard();
            this.IgnoreSecondPress = true;
        }
    }
}
