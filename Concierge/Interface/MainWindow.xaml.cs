// <copyright file="MainWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Interface.AbilitiesPageUi;
    using Concierge.Interface.CompanionPageUi;
    using Concierge.Interface.DetailsPageUi;
    using Concierge.Interface.Enums;
    using Concierge.Interface.EquipedItemsPageUi;
    using Concierge.Interface.EquipmentPageUi;
    using Concierge.Interface.HelperUi;
    using Concierge.Interface.InventoryPageUi;
    using Concierge.Interface.NotesPageUi;
    using Concierge.Interface.OverviewPageUi;
    using Concierge.Interface.SpellcastingPageUi;
    using Concierge.Interface.ToolsPageUi;
    using Concierge.Persistence;
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
        private readonly AboutConciergeWindow aboutConciergeWindow = new AboutConciergeWindow();

        private readonly AutosaveTimer autosaveTimer = new AutosaveTimer();

        private readonly InventoryPage inventoryPage = new InventoryPage();
        private readonly EquipmentPage equipmentPage = new EquipmentPage();
        private readonly AbilitiesPage abilitiesPage = new AbilitiesPage();
        private readonly OverviewPage overviewPage = new OverviewPage();
        private readonly DetailsPage detailsPage = new DetailsPage();
        private readonly NotesPage notesPage = new NotesPage();
        private readonly SpellcastingPage spellcastingPage = new SpellcastingPage();
        private readonly ToolsPage toolsPage = new ToolsPage();
        private readonly EquipedItemsPage equipedItemsPage = new EquipedItemsPage();
        private readonly CompanionPage companionPage = new CompanionPage();

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

            this.notesPage.ClearTextBox();
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

        private void MoveSelection(int index)
        {
            if (index >= 0 && index < this.ListViewMenu.Items.Count)
            {
                this.ListViewMenu.SelectedItem = this.ListViewMenu.Items[index];
                this.UpdateLayout();
                ((ListViewItem)this.ListViewMenu.ItemContainerGenerator.ContainerFromIndex(index)).Focus();
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
                case Key.D0:
                    this.MoveSelection(9);
                    break;
            }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"Expand sidebar.");

            this.ButtonCloseMenu.Visibility = Visibility.Visible;
            this.ButtonOpenMenu.Visibility = Visibility.Collapsed;

            this.GridContent.Width = GridContentWidthOpen;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
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
        }

        private void ButtonOpenCharacter_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.OpenCharacterSheet();
        }

        private void ButtonSaveCharacter_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.SaveCharacterSheet();
        }

        private void ButtonSaveCharacterAs_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.SaveCharacterSheetAs();
        }

        private void ButtonLongRest_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.LongRest();
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
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"Open About.");

            this.aboutConciergeWindow.ShowWindow();
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
            ConciergeSound.TapNavigation();
            this.WindowState = WindowState.Minimized;
        }
    }
}
