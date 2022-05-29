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
    using System.Windows.Navigation;

    using Concierge.Character.Characteristics;
    using Concierge.Configuration;
    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.CompanionPageInterface;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.NotesPageInterface;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;
    using Concierge.Interfaces.ToolsPageInterface;
    using Concierge.Interfaces.UtilityInterface;
    using Concierge.Persistence;
    using Concierge.Services;
    using Concierge.Services.WorkerServices;
    using Concierge.Tools;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;
    using MaterialDesignThemes.Wpf;

    using Constants = Concierge.Utility.Constants;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Needs access from outside class.")]
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty ScaleValuePropertyX = DependencyProperty.Register("ScaleValueX", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValueX)));
        public static readonly DependencyProperty ScaleValuePropertyY = DependencyProperty.Register("ScaleValueY", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValueY)));

        public readonly InventoryPage InventoryPage = new ();
        public readonly AttackDefensePage AttackDefensePage = new ();
        public readonly AbilitiesPage AbilitiesPage = new ();
        public readonly OverviewPage OverviewPage = new ();
        public readonly DetailsPage DetailsPage = new ();
        public readonly NotesPage NotesPage = new ();
        public readonly SpellcastingPage SpellcastingPage = new ();
        public readonly ToolsPage ToolsPage = new ();
        public readonly EquippedItemsPage EquippedItemsPage = new ();
        public readonly CompanionPage CompanionPage = new ();

        private readonly FileAccessService fileAccessService = new ();
        private readonly CommandLineService commandLineService = new ();
        private readonly MainWindowService mainWindowService;
        private readonly AutosaveService autosaveTimer = new ();
        private readonly DateTimeWorkerService dateTimeService = new ();
        private readonly AnimatedTimedTextWorkerService animatedTimedTextWorkerService = new (17);
        private readonly CharacterCreationWizard characterCreationWizard = new ();

        public MainWindow()
        {
            this.InitializeComponent();

            Program.UndoRedoService.StackChanged += this.UndoRedo_StackChanged;
            Program.ModifiedChanged += this.MainWindow_ModifiedChanged;
            this.dateTimeService.TimeUpdated += this.MainWindow_TimeUpdated;
            this.animatedTimedTextWorkerService.TextUpdated += this.MainWindow_TextUpdated;

            this.mainWindowService = new MainWindowService(this.ListViewItem_Selected);
            this.GridContent.Width = GridContentWidthClose;
            this.IgnoreListItemSelectionChanged = true;
            this.MessageBar.Background = ConciergeColors.ProficiencyBrush;
            this.CollapseAll();
            this.GenerateListViewItems();
            this.ListViewMenu.SelectedIndex = 0;
            this.OverviewPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.OverviewPage;
            this.dateTimeService.StartWorker(string.Empty);

            this.DataContext = this;

            if (AppSettingsManager.UserSettings.AutosaveEnabled)
            {
                this.autosaveTimer.Start(Constants.CurrentAutosaveInterval);
            }

            this.PopupBoxButton.ResetScaling();
            this.ButtonClose.ResetScaling();
            this.ButtonMinimize.ResetScaling();
            this.MaximizeButton.ResetScaling();
            this.commandLineService.ReadCommandLineArgs();
            this.DrawAll();

            Program.Logger.Info($"{nameof(MainWindow)} loaded.");
            Program.InitializeMainWindow(this);
            Program.Unmodify();
        }

        public static double GridContentWidthOpen => SystemParameters.PrimaryScreenWidth - 200;

        public static double GridContentWidthClose => SystemParameters.PrimaryScreenWidth - 60;

        public IConciergePage? CurrentPage => (this.ListViewMenu.SelectedItem as ListViewItem)?.Tag as IConciergePage;

        public double ScaleValueX
        {
            get => (double)this.GetValue(ScaleValuePropertyX);

            set => this.SetValue(ScaleValuePropertyX, value);
        }

        public double ScaleValueY
        {
            get => (double)this.GetValue(ScaleValuePropertyY);

            set => this.SetValue(ScaleValuePropertyY, value);
        }

        private static bool IsControl => (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

        private static bool IsShift => (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

        private bool IgnoreSecondPress { get; set; }

        private bool IgnoreListItemSelectionChanged { get; set; }

        public void CloseWindow()
        {
            var result = this.CheckSaveBeforeAction("closing");

            if (result != ConciergeWindowResult.Cancel && result != ConciergeWindowResult.Exit)
            {
                this.Close();
            }
        }

        public void NewCharacterSheet()
        {
            Program.Logger.Info($"Creating new character sheet.");

            var result = this.CheckSaveBeforeAction("creating a new sheet");

            if (result == ConciergeWindowResult.Cancel)
            {
                return;
            }

            this.animatedTimedTextWorkerService.StartWorker("Generated New Character Sheet!");
            this.ResetCharacterSheet();
            this.DrawAll();

            Program.Unmodify();
        }

        public void CreateCharacterWizard()
        {
            Program.Logger.Info($"Open Character Creation.");

            var result = this.CheckSaveBeforeAction("creating a new character");

            if (result == ConciergeWindowResult.Cancel)
            {
                return;
            }

            this.ResetCharacterSheet();
            this.characterCreationWizard.Start();
            this.DrawAll();
        }

        public void OpenSettings()
        {
            Program.Logger.Info($"Open settings.");

            ConciergeWindowService.ShowEdit<string>(
                string.Empty,
                typeof(SettingsWindow),
                this.Window_ApplyChanges,
                ConciergePage.None);
            this.DrawAll();

            this.StartStopAutosaveTimer();
        }

        public void OpenCharacterSheet()
        {
            Program.Logger.Info($"Opening character sheet.");

            var result = this.CheckSaveBeforeAction("opening");

            if (result == ConciergeWindowResult.Cancel)
            {
                return;
            }

            Program.UndoRedoService.Clear();
            var ccsFile = this.fileAccessService.OpenCcs();

            if (ccsFile == null)
            {
                return;
            }

            this.autosaveTimer.Stop();

            Program.CcsFile = ccsFile;
            Program.Unmodify();
            if (AppSettingsManager.UserSettings.AutosaveEnabled)
            {
                this.autosaveTimer.Start(Constants.CurrentAutosaveInterval);
            }

            this.animatedTimedTextWorkerService.StartWorker($"Opened '{ccsFile.AbsolutePath}'");
            this.NotesPage.ClearTextBox();
            this.DrawAll();
        }

        public void SaveCharacterSheet()
        {
            Program.Logger.Info($"Save character sheet.");
            this.Save(Program.CcsFile.AbsolutePath.IsNullOrWhiteSpace());
        }

        public void SaveCharacterSheetAs()
        {
            Program.Logger.Info($"Save as character sheet.");
            this.Save(true);
        }

        public void DrawAll()
        {
            Program.Logger.Info($"Draw all.");

            this.mainWindowService.GenerateCharacterStatusBar(this.CharacterHeaderPanel, Program.CcsFile.Character);
            this.UpdateStatusBar(this.CurrentPage?.ConciergePage ?? ConciergePage.None);

            this.InventoryPage.Draw();
            this.AbilitiesPage.Draw();
            this.AttackDefensePage.Draw();
            this.OverviewPage.Draw();
            this.DetailsPage.Draw();
            this.NotesPage.Draw();
            this.SpellcastingPage.Draw();
            this.ToolsPage.Draw();
            this.EquippedItemsPage.Draw();
            this.CompanionPage.Draw();
        }

        public void LongRest()
        {
            Program.Logger.Info($"Long rest.");
            Program.CcsFile.Character.LongRest();
            Program.Modify();

            this.animatedTimedTextWorkerService.StartWorker("Long Rest Complete!   HP and Spell Slots Replenished.");
            this.DrawAll();
        }

        public void Search()
        {
            ConciergeWindowService.ShowWindow(typeof(SearchWindow));
            this.IgnoreSecondPress = true;
        }

        public void Redo()
        {
            Program.UndoRedoService.Redo(this);
            this.DrawAll();
        }

        public void Undo()
        {
            Program.UndoRedoService.Undo(this);
            this.DrawAll();
        }

        public void PageSelection(IConciergePage? conciergePage)
        {
            if (conciergePage is not Page page)
            {
                return;
            }

            this.CollapseAll();
            this.UpdateStatusBar(conciergePage.ConciergePage);
            page.Visibility = Visibility.Visible;
            this.FrameContent.Content = page;
            conciergePage.Draw();

            Program.Logger.Info($"Navigate to {page.GetType().Name}");
        }

        public void MoveSelection(ConciergePage page)
        {
            if (page >= 0 && ((int)page) < this.ListViewMenu.Items.Count)
            {
                this.ListViewMenu.SelectedItem = this.ListViewMenu.Items[(int)page];
                this.UpdateLayout();
                ((ListViewItem)this.ListViewMenu.ItemContainerGenerator.ContainerFromIndex((int)page)).Focus();
            }
        }

        public void StartStopAutosaveTimer()
        {
            if (AppSettingsManager.UserSettings.AutosaveEnabled)
            {
                this.autosaveTimer.Start(Constants.CurrentAutosaveInterval);
            }
            else
            {
                this.autosaveTimer.Stop();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Program.Logger.Info("Closing Concierge.");
            Application.Current.Shutdown();
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            return double.IsNaN(value) ? 1.0d : Math.Max(0.1, value);
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {
        }

        private static object OnCoerceScaleValueX(DependencyObject o, object value)
        {
            return o is MainWindow mainWindow ? mainWindow.OnCoerceScaleValue((double)value) : value;
        }

        private static object OnCoerceScaleValueY(DependencyObject o, object value)
        {
            return o is MainWindow mainWindow ? mainWindow.OnCoerceScaleValue((double)value) : value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is MainWindow mainWindow)
            {
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
            }
        }

        private void Save(bool saveAs)
        {
            this.NotesPage.SaveTextBox();

            if (saveAs)
            {
                this.fileAccessService.SaveAs(Program.CcsFile);
                this.animatedTimedTextWorkerService.StartWorker($"Save As '{Program.CcsFile.AbsolutePath}'");
            }
            else
            {
                this.fileAccessService.Save(Program.CcsFile);
                this.animatedTimedTextWorkerService.StartWorker($"Save '{Program.CcsFile.AbsolutePath}'");
            }
        }

        private void UpdateStatusBar(ConciergePage conciergePage)
        {
            this.DateTimeTextBlock.Text = ConciergeDateTime.StatusMenuNow;
            this.DateTimeTextBlock.ToolTip = ConciergeDateTime.ToolTipNow;
            this.CurrentPageNameTextBlock.Text = DisplayUtility.FormatConciergePageForDisplay(conciergePage);
        }

        private void CalculateScale()
        {
            double yScale = this.Height / SystemParameters.PrimaryScreenHeight;
            double xScale = this.Width / SystemParameters.PrimaryScreenWidth;
            this.ScaleValueX = (double)OnCoerceScaleValueX(this.MainGrid, xScale);
            this.ScaleValueY = (double)OnCoerceScaleValueY(this.MainGrid, yScale);
        }

        private void GenerateListViewItems()
        {
            var items = this.ListViewMenu.Items;
            var service = this.mainWindowService;

            items.Add(service.GenerateListViewItem(this.OverviewPage, "Overview", PackIconKind.Globe));
            items.Add(service.GenerateListViewItem(this.DetailsPage, "Details", PackIconKind.Details));
            items.Add(service.GenerateListViewItem(this.AttackDefensePage, "Attack and Defense", PackIconKind.ShieldHalfFull));
            items.Add(service.GenerateListViewItem(this.AbilitiesPage, "Abilities", PackIconKind.Brain));
            items.Add(service.GenerateListViewItem(this.EquippedItemsPage, "Equipped Items", PackIconKind.Person));
            items.Add(service.GenerateListViewItem(this.InventoryPage, "Inventory", PackIconKind.Backpack));
            items.Add(service.GenerateListViewItem(this.SpellcastingPage, "Spellcasting", PackIconKind.Magic));
            items.Add(service.GenerateListViewItem(this.CompanionPage, "Companion", PackIconKind.PersonAdd));
            items.Add(service.GenerateListViewItem(this.ToolsPage, "Tools", PackIconKind.Tools));
            items.Add(service.GenerateListViewItem(this.NotesPage, "Notes", PackIconKind.Pen));
        }

        private ConciergeWindowResult CheckSaveBeforeAction(string action)
        {
            if (!Program.IsModified)
            {
                return ConciergeWindowResult.No;
            }

            var name = Program.CcsFile.FileName;
            var result = ConciergeMessageBox.Show(
                $"Do you want to save the changes you made to {(name.IsNullOrWhiteSpace() ? "this sheet" : name)} before {action}?\n\nYour changes will be lost if you don't save them.",
                "Warning",
                ConciergeWindowButtons.YesNoCancel,
                ConciergeWindowIcons.Warning);

            if (result == ConciergeWindowResult.Yes)
            {
                this.SaveCharacterSheet();
            }

            return result;
        }

        private void CollapseAll()
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.AttackDefensePage.Visibility = Visibility.Collapsed;
            this.AbilitiesPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.NotesPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.EquippedItemsPage.Visibility = Visibility.Collapsed;
            this.CompanionPage.Visibility = Visibility.Collapsed;
        }

        private void ResetCharacterSheet()
        {
            Program.CcsFile = new CcsFile();
            Program.UndoRedoService.Clear();

            this.NotesPage.ClearTextBox();

            this.autosaveTimer.Stop();
        }

        private void ChangeWindowState()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.MaximizeIcon.Kind = PackIconKind.WindowMaximize;
                this.WindowState = WindowState.Normal;
                this.MaximizeButton.ToolTip = "Maximize";
                this.BorderThickness = new Thickness(1);
            }
            else
            {
                this.MaximizeIcon.Kind = PackIconKind.WindowRestore;
                this.WindowState = WindowState.Maximized;
                this.MaximizeButton.ToolTip = "Restore Down";
                this.BorderThickness = new Thickness(0);
            }
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            this.FrameContent.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void MainGrid_SizeChanged(object sender, EventArgs e)
        {
            this.CalculateScale();
        }

        private void MainWindow_KeyPress(object sender, KeyEventArgs e)
        {
            EasterEggController.KonamiCode(e.Key);

            if (Program.IsTyping || !IsControl)
            {
                return;
            }

            // Move off Side Bar to avoid reset
            this.FrameContent.Focus();

            switch (e.Key)
            {
                case Key.A:
                    ConciergeWindowService.ShowWindow(typeof(AboutConciergeWindow));
                    break;
                case Key.F:
                    this.Search();
                    break;
                case Key.H:
                    ConciergeWindowService.ShowWindow(typeof(HelpWindow));
                    break;
                case Key.I:
                    this.OpenSettings();
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
                    ConciergeWindowService.ShowEdit<CharacterProperties>(
                        Program.CcsFile.Character.Properties,
                        typeof(ModifyPropertiesWindow),
                        this.Window_ApplyChanges,
                        ConciergePage.None);
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
                case Key.Y:
                    this.Redo();
                    break;
                case Key.Z:
                    this.Undo();
                    break;
                case Key.D1:
                    this.MoveSelection(ConciergePage.Overview);
                    break;
                case Key.D2:
                    this.MoveSelection(ConciergePage.Details);
                    break;
                case Key.D3:
                    this.MoveSelection(ConciergePage.AttackDefense);
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

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.PageSelection((sender as ListViewItem)?.Tag as IConciergePage);
        }

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"Open properties.");

            ConciergeWindowService.ShowEdit<CharacterProperties>(
                Program.CcsFile.Character.Properties,
                typeof(ModifyPropertiesWindow),
                this.Window_ApplyChanges,
                ConciergePage.None);
            this.DrawAll();

            this.IgnoreSecondPress = true;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.OpenSettings();
            this.IgnoreSecondPress = true;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"Open About.");

            ConciergeWindowService.ShowWindow(typeof(AboutConciergeWindow));

            this.IgnoreSecondPress = true;
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeWindowState();
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<CharacterImage>(
                Program.CcsFile.Character.CharacterIcon,
                typeof(ModifyCharacterImageWindow),
                this.Window_ApplyChanges,
                ConciergePage.None);
            this.mainWindowService.GenerateCharacterStatusBar(this.CharacterHeaderPanel, Program.CcsFile.Character);
            this.IgnoreSecondPress = true;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Search();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyCharacterImageWindow":
                case "ModifyPropertiesWindow":
                case "SettingsWindow":
                    this.DrawAll();
                    break;
            }
        }

        private void CharacterHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.ChangeWindowState();
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ButtonRedo_Click(object sender, RoutedEventArgs e)
        {
            this.Redo();
            this.IgnoreSecondPress = true;
        }

        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {
            this.Undo();
            this.IgnoreSecondPress = true;
        }

        private void UndoRedo_StackChanged(object sender, EventArgs e)
        {
            this.ButtonUndo.IsEnabled = Program.UndoRedoService.CanUndo;
            this.ButtonRedo.IsEnabled = Program.UndoRedoService.CanRedo;
            if (sender is not UndoRedoService service)
            {
                return;
            }

            if (service.UpdateAutosaveTimer)
            {
                this.StartStopAutosaveTimer();
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowWindow(typeof(HelpWindow));
            this.IgnoreSecondPress = true;
        }

        private void MainWindow_ModifiedChanged(object sender, EventArgs e)
        {
            this.ModifiedStatus.Visibility = ((bool)sender) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MainWindow_TimeUpdated(object sender, EventArgs e)
        {
            this.DateTimeTextBlock.Text = ConciergeDateTime.StatusMenuNow;
            this.DateTimeTextBlock.ToolTip = ConciergeDateTime.ToolTipNow;
        }

        private void MainWindow_TextUpdated(object sender, EventArgs e)
        {
            if (sender is string updatedText)
            {
                this.AlertMessageTextBlock.Text = updatedText;
            }
        }
    }
}
