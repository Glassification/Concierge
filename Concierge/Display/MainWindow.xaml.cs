// <copyright file="MainWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Navigation;

    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Configuration;
    using Concierge.Data.Enums;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Pages;
    using Concierge.Display.Utility;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Services;
    using Concierge.Services.EasterEggs;
    using Concierge.Tools.WorkerServices;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Needs access from outside class.")]
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty ScaleValuePropertyX = DependencyProperty.Register("ScaleValueX", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValueX)));
        public static readonly DependencyProperty ScaleValuePropertyY = DependencyProperty.Register("ScaleValueY", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValueY)));

        public readonly OverviewPage OverviewPage = new ();
        public readonly InventoryPage InventoryPage = new ();
        public readonly AbilityPage AbilityPage = new ();
        public readonly AttacksPage AttacksPage = new ();
        public readonly DetailsPage DetailsPage = new ();
        public readonly SpellcastingPage SpellcastingPage = new ();
        public readonly EquipmentPage EquipmentPage = new ();
        public readonly CompanionPage CompanionPage = new ();
        public readonly ToolsPage ToolsPage = new ();
        public readonly JournalPage JournalPage = new ();

        private readonly FileAccessService fileAccessService = new ();
        private readonly AppDataReadWriter appDataReadWriter = new (Program.ErrorService);
        private readonly AutosaveService autosaveTimer = new (new FileAccessService());
        private readonly BackupService backupService = new (new FileAccessService(), ConciergeFiles.BackupDirectory);
        private readonly DateTimeWorkerService dateTimeService = new ();
        private readonly SystemWorkerService systemService = new ();
        private readonly AnimatedTimedTextWorkerService animatedTimedTextWorkerService = new (Common.Constants.StatusDisplayTime);
        private readonly CharacterCreationWizard characterCreationWizard = new ();
        private readonly EasterEggService easterEggService = new ();

        public MainWindow()
        {
            this.InitializeComponent();

            Program.UndoRedoService.StackChanged += this.UndoRedo_StackChanged;
            Program.ModifiedChanged += this.MainWindow_ModifiedChanged;

            this.GridContent.Width = GridContentWidthClose;
            this.IsMenuOpen = false;
            this.IgnoreListItemSelectionChanged = true;

            this.SetListViewItemTag();
            this.SetUndoRedoState();
            this.CollapseAll();
            this.ListViewMenu.SelectedIndex = 0;
            this.OverviewPage.Visibility = Visibility.Visible;
            this.FrameContent.Content = this.OverviewPage;
            this.DataContext = this;

            this.StartServices();
            this.SetMenuEvents();

            this.ButtonClose.ResetScaling();
            this.ButtonMinimize.ResetScaling();
            this.MaximizeButton.ResetScaling();

            Program.Logger.Info($"{nameof(MainWindow)} loaded.");
            Program.InitializeMainWindow(this);
        }

        public static double GridContentWidthOpen => SystemParameters.PrimaryScreenWidth - 200;

        public static double GridContentWidthClose => SystemParameters.PrimaryScreenWidth - 60;

        public ConciergePage? CurrentPage => (this.ListViewMenu.SelectedItem as ListViewItem)?.Tag as ConciergePage;

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

        public bool IsMenuOpen { get; set; }

        private static bool IsControl => (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

        private static bool IsShift => (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;

        private bool IgnoreListItemSelectionChanged { get; set; }

        public void CloseWindow()
        {
            var result = this.CheckSaveBeforeAction("closing");

            if (result != ConciergeResult.Cancel && result != ConciergeResult.Exit)
            {
                this.Close();
            }
        }

        public void NewCharacterSheet()
        {
            Program.Logger.Info($"Creating new character sheet.");

            var result = this.CheckSaveBeforeAction("creating a new sheet");

            if (result == ConciergeResult.Cancel)
            {
                return;
            }

            this.DisplayStatusText("Generated New Character Sheet!");
            this.ResetCharacterSheet();
            this.DrawAll();
            this.MessageBar.ClearActiveFile();

            Program.Unmodify();
        }

        public void CreateCharacterWizard()
        {
            Program.Logger.Info($"Open Character Creation.");

            var result = this.characterCreationWizard.Prompt();
            if (result != ConciergeResult.OK)
            {
                return;
            }

            result = this.CheckSaveBeforeAction("creating a new character");
            if (result == ConciergeResult.Cancel)
            {
                return;
            }

            this.ResetCharacterSheet();
            this.DrawAll();
            if (!this.characterCreationWizard.Start())
            {
                this.ResetCharacterSheet();
                this.DisplayStatusText("Canceled character creation wizard");
                this.MessageBar.ClearActiveFile();
                Program.Unmodify();
            }
            else
            {
                this.DrawAll();
                this.MessageBar.DrawActiveFile(Program.CcsFile);
            }
        }

        public void OpenSettings()
        {
            Program.Logger.Info($"Open settings.");
            ConciergeWindowService.ShowEdit(
                string.Empty,
                typeof(SettingsWindow),
                this.Window_ApplyChanges,
                ConciergePages.None);
            this.DrawAll();

            this.StartStopAutosaveTimer();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "The way she goes.")]
        public void OpenGlossary()
        {
            Program.Logger.Info($"Open glossary.");

            ConciergeWindowService.ShowNonBlockingWindow(typeof(GlossaryWindow));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "The way she goes.")]
        public void OpenCustomItems()
        {
            Program.Logger.Info($"Open custom items.");

            ConciergeWindowService.ShowWindow(typeof(CustomItemWindow));
        }

        public void OpenCharacterSheet(string file = "")
        {
            Program.Logger.Info($"Opening character sheet.");

            var result = this.CheckSaveBeforeAction("opening");
            if (result == ConciergeResult.Cancel)
            {
                return;
            }

            var ccsFile = this.fileAccessService.OpenCcs(file);
            if (ccsFile?.AbsolutePath.IsNullOrWhiteSpace() ?? true)
            {
                return;
            }

            this.autosaveTimer.Stop();

            Program.UndoRedoService.Clear();
            Program.CcsFile = ccsFile;
            Program.Unmodify();
            if (AppSettingsManager.UserSettings.Autosaving.Enabled)
            {
                this.autosaveTimer.Start(Defaults.CurrentAutosaveInterval);
            }

            this.DisplayStatusText($"Opened '{ccsFile.AbsolutePath}'");
            this.JournalPage.ClearTextBox();
            this.DrawAll(true);
            this.MessageBar.DrawActiveFile(Program.CcsFile);
        }

        public void ImportCharacter()
        {
            Program.Logger.Info($"Importing data to character sheet.");

            ConciergeWindowService.ShowWindow(typeof(ImportCharacterWindow), this.Window_ApplyChanges);

            this.DisplayStatusText("Imported Character Data!");
            this.DrawAll();
        }

        public int SaveCharacterSheet()
        {
            Program.Logger.Info($"Save character sheet.");
            this.Save(Program.CcsFile.AbsolutePath.IsNullOrWhiteSpace());
            this.MessageBar.DrawActiveFile(Program.CcsFile);
            return Common.Constants.Void;
        }

        public int SaveCharacterSheetAs()
        {
            Program.Logger.Info($"Save character sheet as.");
            this.Save(true);
            this.MessageBar.DrawActiveFile(Program.CcsFile);
            return Common.Constants.Void;
        }

        public void OpenConsole()
        {
            ConciergeWindowService.ShowWindow(typeof(ConciergeConsoleWindow), this.Window_ApplyChanges);
        }

        public void DrawAll(bool isNewCharacterSheet = false)
        {
            Program.Logger.Info($"Draw all.");

            var disposition = Program.CcsFile.Character.Disposition;

            this.CharacterHeader.Draw(disposition);
            this.InventoryPage.Draw();
            this.AbilityPage.Draw();
            this.AttacksPage.Draw();
            this.OverviewPage.Draw();
            this.DetailsPage.Draw();
            this.JournalPage.Draw(isNewCharacterSheet);
            this.SpellcastingPage.Draw();
            this.ToolsPage.Draw();
            this.EquipmentPage.Draw();
            this.CompanionPage.Draw();

            this.MessageBar.DrawVolume();

            this.UpdateStatusBar();
        }

        public int LongRest()
        {
            Program.Logger.Info($"Long rest.");

            var character = Program.CcsFile.Character;

            var oldVitality = character.Vitality.DeepCopy();
            var oldSpellSlots = character.SpellCasting.SpellSlots.DeepCopy();
            var oldCompanionHealth = character.Companion.Health.DeepCopy();
            var oldCompanionHitDice = character.Companion.HitDice.DeepCopy();
            var oldConcentratedSpell = character.SpellCasting.ConcentratedSpell;

            character.Vitality.Health.ResetHealth();
            character.Vitality.HitDice.RegainHitDice();
            character.Vitality.ResetDeathSaves();
            character.SpellCasting.SpellSlots.Reset();
            character.SpellCasting.ClearConcentration();

            character.Companion.Health.ResetHealth();
            character.Companion.HitDice.RegainHitDice();

            Program.UndoRedoService.AddCommand(
                new RestCommand(
                    oldVitality,
                    oldCompanionHealth,
                    oldCompanionHitDice,
                    oldSpellSlots,
                    oldConcentratedSpell,
                    character.Vitality.DeepCopy(),
                    character.Companion.Health.DeepCopy(),
                    character.Companion.HitDice.DeepCopy(),
                    character.SpellCasting.SpellSlots.DeepCopy()));

            this.DisplayStatusText("Long Rest Complete!   HP and Spell Slots Replenished.");
            this.DrawAll();

            return Common.Constants.Void;
        }

        public int ShortRest()
        {
            Program.Logger.Info($"Short rest.");

            var character = Program.CcsFile.Character;

            var oldVitality = character.Vitality.DeepCopy();
            var oldSpellSlots = character.SpellCasting.SpellSlots.DeepCopy();
            var oldCompanionHealth = character.Companion.Health.DeepCopy();
            var oldCompanionHitDice = character.Companion.HitDice.DeepCopy();
            var oldConcentratedSpell = character.SpellCasting.ConcentratedSpell;

            character.Companion.RollShortRestHitDice(character.Companion.HitDice.GetFirstAvailable(), character.Companion.Attributes.Constitution);
            if (character.Disposition.Class1.IsValid)
            {
                character.Vitality.RollShortRestHitDice(HitDice.GetHitDice(character.Disposition.Class1.Name), character.Attributes.Constitution);
            }

            if (character.Disposition.Class2.IsValid)
            {
                character.Vitality.RollShortRestHitDice(HitDice.GetHitDice(character.Disposition.Class2.Name), character.Attributes.Constitution);
            }

            if (character.Disposition.Class3.IsValid)
            {
                character.Vitality.RollShortRestHitDice(HitDice.GetHitDice(character.Disposition.Class3.Name), character.Attributes.Constitution);
            }

            var warlockLevel = character.GetWarlockLevel();
            if (warlockLevel > 0)
            {
                character.SpellCasting.SpellSlots.ShortRest(warlockLevel);
            }

            Program.UndoRedoService.AddCommand(
                new RestCommand(
                    oldVitality,
                    oldCompanionHealth,
                    oldCompanionHitDice,
                    oldSpellSlots,
                    oldConcentratedSpell,
                    character.Vitality.DeepCopy(),
                    character.Companion.Health.DeepCopy(),
                    character.Companion.HitDice.DeepCopy(),
                    character.SpellCasting.SpellSlots.DeepCopy()));

            this.DisplayStatusText("Short Rest Complete!   Hit Dice Used and Pact Slots Replenished.");
            this.DrawAll();

            return Common.Constants.Void;
        }

        public void LevelUp()
        {
            Program.Logger.Info($"Level up.");
            ConciergeWindowService.ShowWindow(typeof(LevelUpWindow), this.Window_ApplyChanges);

            this.DrawAll();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Way she goes.")]
        public void Search()
        {
            ConciergeWindowService.ShowWindow(typeof(SearchWindow));
        }

        public void Redo()
        {
            if (!Program.UndoRedoService.CanRedo)
            {
                return;
            }

            Program.UndoRedoService.Redo(this);
            this.DrawAll();
        }

        public void Undo()
        {
            if (!Program.UndoRedoService.CanUndo)
            {
                return;
            }

            Program.UndoRedoService.Undo(this);
            this.DrawAll();
        }

        public void PageSelection(ConciergePage? conciergePage)
        {
            if (conciergePage is null)
            {
                return;
            }

            this.CollapseAll();
            this.UpdateStatusBar();
            conciergePage.Visibility = Visibility.Visible;
            this.FrameContent.Content = conciergePage;
            conciergePage.Draw();

            Program.Logger.Info($"Navigate to {conciergePage.GetType().Name}");
        }

        public void MoveSelection(ConciergePages page)
        {
            if (page == ConciergePages.None)
            {
                return;
            }

            if (page >= 0 && ((int)page) < this.ListViewMenu.Items.Count)
            {
                this.ListViewMenu.SelectedItem = this.ListViewMenu.Items[(int)page];
                this.UpdateLayout();
                ((ListViewItem)this.ListViewMenu.ItemContainerGenerator.ContainerFromIndex((int)page))?.Focus();
            }
        }

        public void StartStopAutosaveTimer()
        {
            if (AppSettingsManager.UserSettings.Autosaving.Enabled)
            {
                this.autosaveTimer.Start(Defaults.CurrentAutosaveInterval);
            }
            else
            {
                this.autosaveTimer.Stop();
            }
        }

        public void DisplayStatusText(string message)
        {
            Program.MessageService.Add(message, MessageType.Information);
            this.animatedTimedTextWorkerService.StartWorker(message);
        }

        [LibraryImport("dwmapi.dll", EntryPoint = "DwmSetWindowAttribute")]
        internal static partial int DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE attribute,
            ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
            uint cbAttribute);

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Program.Logger.Stop();
            Application.Current.Shutdown();
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            return double.IsNaN(value) ? 1.0d : Math.Max(0.1, value);
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {
        }

        protected void ForceRoundedCorners()
        {
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            Marshal.ThrowExceptionForHR(DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint)));
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

        private void StartServices()
        {
            this.dateTimeService.TimeUpdated += this.MainWindow_TimeUpdated;
            this.animatedTimedTextWorkerService.TextUpdated += this.MainWindow_TextUpdated;
            this.systemService.SystemUpdated += this.MainWindow_SystemUpdated;

            this.dateTimeService.StartWorker(string.Empty);
            this.systemService.StartWorker(string.Empty);

            this.backupService.Start();
            if (AppSettingsManager.UserSettings.Autosaving.Enabled)
            {
                this.autosaveTimer.Start(Defaults.CurrentAutosaveInterval);
            }
        }

        private void SetMenuEvents()
        {
            this.MenuButton.NewCharacterMenuItem.AddClickEvent(this.NewCharacterButton_Click);
            this.MenuButton.OpenCharacterMenuItem.AddClickEvent(this.OpenCharacterButton_Click);
            this.MenuButton.ImportCharacterMenuItem.AddClickEvent(this.ImportCharacterButton_Click);
            this.MenuButton.SaveCharacterMenuItem.AddClickEvent(this.SaveCharacterButton_Click);
            this.MenuButton.SaveAsCharacterMenuItem.AddClickEvent(this.SaveCharacterAsButton_Click);
            this.MenuButton.UndoMenuItem.AddClickEvent(this.UndoButton_Click);
            this.MenuButton.RedoMenuItem.AddClickEvent(this.RedoButton_Click);
            this.MenuButton.CharacterCreationMenuItem.AddClickEvent(this.CharacterCreationButton_Click);
            this.MenuButton.LevelUpMenuItem.AddClickEvent(this.LevelUpButton_Click);
            this.MenuButton.ShortRestMenuItem.AddClickEvent(this.ShortRestButton_Click);
            this.MenuButton.LongRestMenuItem.AddClickEvent(this.LongRestButton_Click);
            this.MenuButton.CharacterPropertiesMenuItem.AddClickEvent(this.PropertiesButton_Click);
            this.MenuButton.ConsoleMenuItem.AddClickEvent(this.ConsoleButton_Click);
            this.MenuButton.CustomItemsMenuItem.AddClickEvent(this.CustomItemsButton_Click);
            this.MenuButton.SearchMenuItem.AddClickEvent(this.SearchButton_Click);
            this.MenuButton.ExportAppDataMenuItem.AddClickEvent(this.ExportAppDataButton_Click);
            this.MenuButton.ImportAppDataMenuItem.AddClickEvent(this.ImportAppDataButton_Click);
            this.MenuButton.MessageHistoryMenuItem.AddClickEvent(this.MessageHistoryButton_Click);
            this.MenuButton.KeyboardMenuItem.AddClickEvent(this.OnScreenKeyboardButton_Click);
            this.MenuButton.SettingsMenuItem.AddClickEvent(this.SettingsButton_Click);
            this.MenuButton.AboutMenuItem.AddClickEvent(this.AboutButton_Click);
            this.MenuButton.HelpMenuItem.AddClickEvent(this.GlossaryButton_Click);
            this.MenuButton.ExitMenuItem.AddClickEvent(this.ButtonClose_Click);
        }

        private void SetUndoRedoState()
        {
            DisplayUtility.SetControlEnableState(this.MenuButton.UndoMenuItem, Program.UndoRedoService.CanUndo);
            DisplayUtility.SetControlEnableState(this.MenuButton.RedoMenuItem, Program.UndoRedoService.CanRedo);
        }

        private void Save(bool saveAs)
        {
            this.JournalPage.SaveTextBox();
            Program.CcsFile.IsEmpty = false;

            if (saveAs)
            {
                if (this.fileAccessService.SaveAs(Program.CcsFile))
                {
                    this.DisplayStatusText($"Save As '{Program.CcsFile.AbsolutePath}'");
                }
            }
            else
            {
                this.fileAccessService.Save(Program.CcsFile);
                this.DisplayStatusText($"Save '{Program.CcsFile.AbsolutePath}'");
            }
        }

        private void UpdateStatusBar()
        {
            this.MessageBar.DrawTime();
        }

        private void CalculateScale()
        {
            double yScale = this.Height / SystemParameters.PrimaryScreenHeight;
            double xScale = this.Width / SystemParameters.PrimaryScreenWidth;
            this.ScaleValueX = (double)OnCoerceScaleValueX(this.MainGrid, xScale);
            this.ScaleValueY = (double)OnCoerceScaleValueY(this.MainGrid, yScale);
        }

        private ConciergeResult CheckSaveBeforeAction(string action)
        {
            if (!Program.IsModified)
            {
                return ConciergeResult.No;
            }

            var name = Program.CcsFile.FileName;
            var result = ConciergeMessageBox.Show(
                $"Do you want to save the changes you made to {(name.IsNullOrWhiteSpace() ? "this sheet" : name)} before {action}?\n\nYour changes will be lost if you don't save them.",
                "Warning",
                ConciergeButtons.Yes | ConciergeButtons.No | ConciergeButtons.Cancel,
                ConciergeIcons.Warning);

            if (result == ConciergeResult.Yes)
            {
                this.SaveCharacterSheet();
            }

            return result;
        }

        private void CollapseAll()
        {
            this.InventoryPage.Visibility = Visibility.Collapsed;
            this.AttacksPage.Visibility = Visibility.Collapsed;
            this.AbilityPage.Visibility = Visibility.Collapsed;
            this.DetailsPage.Visibility = Visibility.Collapsed;
            this.JournalPage.Visibility = Visibility.Collapsed;
            this.SpellcastingPage.Visibility = Visibility.Collapsed;
            this.ToolsPage.Visibility = Visibility.Collapsed;
            this.OverviewPage.Visibility = Visibility.Collapsed;
            this.EquipmentPage.Visibility = Visibility.Collapsed;
            this.CompanionPage.Visibility = Visibility.Collapsed;
        }

        private void ResetCharacterSheet()
        {
            Program.CcsFile = new CcsFile();
            Program.UndoRedoService.Clear();

            this.JournalPage.ClearTextBox();

            this.autosaveTimer.Stop();
        }

        private void SetListViewItemTag()
        {
            this.GetListViewItem(ConciergePages.Overview).Tag = this.OverviewPage;
            this.GetListViewItem(ConciergePages.Details).Tag = this.DetailsPage;
            this.GetListViewItem(ConciergePages.Attacks).Tag = this.AttacksPage;
            this.GetListViewItem(ConciergePages.Abilities).Tag = this.AbilityPage;
            this.GetListViewItem(ConciergePages.Equipment).Tag = this.EquipmentPage;
            this.GetListViewItem(ConciergePages.Inventory).Tag = this.InventoryPage;
            this.GetListViewItem(ConciergePages.Spellcasting).Tag = this.SpellcastingPage;
            this.GetListViewItem(ConciergePages.Companion).Tag = this.CompanionPage;
            this.GetListViewItem(ConciergePages.Journal).Tag = this.JournalPage;
            this.GetListViewItem(ConciergePages.Tools).Tag = this.ToolsPage;
        }

        private ListViewItem GetListViewItem(ConciergePages page)
        {
            if (this.ListViewMenu.Items[(int)page] is ListViewItem item)
            {
                return item;
            }

            return new ListViewItem();
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

        private void ShiftPlusKeyPress(KeyEventArgs keyEvent)
        {
            if (!IsShift)
            {
                return;
            }

            var keyPressed = keyEvent.Key;
            if (keyEvent.Key == Key.System)
            {
                keyPressed = keyEvent.SystemKey;
            }

            switch (keyPressed)
            {
                case Key.Tab:
                    break;
            }
        }

        private void ControlPlusKeyPress(KeyEventArgs keyEvent)
        {
            if (!IsControl)
            {
                return;
            }

            var keyPressed = keyEvent.Key;
            if (keyEvent.Key == Key.System)
            {
                keyPressed = keyEvent.SystemKey;
            }

            switch (keyPressed)
            {
                case Key.A:
                    ConciergeWindowService.ShowWindow(typeof(AboutConciergeWindow));
                    break;
                case Key.C:
                    this.OpenCustomItems();
                    break;
                case Key.F:
                    this.Search();
                    break;
                case Key.H:
                    this.OpenGlossary();
                    break;
                case Key.I:
                    this.ImportCharacter();
                    break;
                case Key.K:
                    SystemUtility.OpenOnScreenKeyboard();
                    break;
                case Key.N:
                    this.NewCharacterSheet();
                    break;
                case Key.O:
                    this.OpenCharacterSheet();
                    break;
                case Key.P:
                    this.OpenCharacterProperties();
                    break;
                case Key.R:
                    _ = IsShift ? this.LongRest() : this.ShortRest();
                    break;
                case Key.S:
                    _ = IsShift ? this.SaveCharacterSheetAs() : this.SaveCharacterSheet();
                    break;
                case Key.U:
                    this.LevelUp();
                    break;
                case Key.OemMinus:
                    this.WindowState = WindowState.Minimized;
                    break;
                case Key.OemPlus:
                    this.WindowState = WindowState.Maximized;
                    break;
                case Key.W:
                    this.CreateCharacterWizard();
                    break;
                case Key.Y:
                    this.Redo();
                    break;
                case Key.Z:
                    this.Undo();
                    break;
                case Key.D1:
                    this.MoveSelection(ConciergePages.Overview);
                    break;
                case Key.D2:
                    this.MoveSelection(ConciergePages.Details);
                    break;
                case Key.D3:
                    this.MoveSelection(ConciergePages.Attacks);
                    break;
                case Key.D4:
                    this.MoveSelection(ConciergePages.Equipment);
                    break;
                case Key.D5:
                    this.MoveSelection(ConciergePages.Inventory);
                    break;
                case Key.D6:
                    this.MoveSelection(ConciergePages.Spellcasting);
                    break;
                case Key.D7:
                    this.MoveSelection(ConciergePages.Abilities);
                    break;
                case Key.D8:
                    this.MoveSelection(ConciergePages.Companion);
                    break;
                case Key.D9:
                    this.MoveSelection(ConciergePages.Journal);
                    break;
                case Key.D0:
                    this.MoveSelection(ConciergePages.Tools);
                    break;
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
            if (Program.IsTyping)
            {
                return;
            }

            this.easterEggService.Evaluate(e.Key);

            // Move off Side Bar to avoid reset
            this.FrameContent.Focus();

            this.ControlPlusKeyPress(e);
            this.ShiftPlusKeyPress(e);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.CloseWindow();
        }

        private void NewCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.NewCharacterSheet();
        }

        private void OpenCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.OpenCharacterSheet();
        }

        private void ImportCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.ImportCharacter();
        }

        private void SaveCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.SaveCharacterSheet();
        }

        private void SaveCharacterAsButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.SaveCharacterSheetAs();
        }

        private void LongRestButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.LongRest();
        }

        private void ShortRestButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.ShortRest();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.PageSelection((sender as ListViewItem)?.Tag as ConciergePage);
        }

        private void OpenCharacterProperties()
        {
            Program.Logger.Info($"Open properties.");

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Disposition,
                typeof(PropertiesWindow),
                this.Window_ApplyChanges,
                ConciergePages.None);

            this.DrawAll();
        }

        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.OpenCharacterProperties();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.OpenSettings();
        }

        private void MessageHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            ConciergeWindowService.ShowWindow(typeof(MessageHistoryWindow));
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            Program.Logger.Info($"Open About.");

            ConciergeWindowService.ShowWindow(typeof(AboutConciergeWindow));
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.WindowState = WindowState.Minimized;
        }

        private void CharacterCreationButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.CreateCharacterWizard();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.ChangeWindowState();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.Search();
        }

        private void ExportAppDataButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            var fileName = this.fileAccessService.SaveFile(0, FileConstants.ZipFilter, "zip", FileConstants.DefaultAppDataFileName);
            if (this.appDataReadWriter.Write(fileName))
            {
                this.DisplayStatusText($"Successfully Exported {Path.GetFileName(fileName)}");
            }
        }

        private void ImportAppDataButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            var fileName = this.fileAccessService.OpenFile(0, FileConstants.ZipFilter, "zip", FileConstants.DefaultAppDataFileName);
            if (!fileName.IsNullOrWhiteSpace())
            {
                var result = ConciergeMessageBox.Show(
                    "This will permanently overwrite the existing Concierge AppData. Do you want to continue?",
                    "Warning",
                    ConciergeButtons.Yes | ConciergeButtons.No,
                    ConciergeIcons.Warning);

                if (result == ConciergeResult.Yes && this.appDataReadWriter.Read(fileName))
                {
                    this.DisplayStatusText($"Successfully Imported {Path.GetFileName(fileName)}");
                }
            }
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(LevelUpWindow):
                case nameof(ImageWindow):
                case nameof(PropertiesWindow):
                case nameof(SettingsWindow):
                case nameof(ImportCharacterWindow):
                case nameof(ConciergeConsoleWindow):
                    this.DrawAll();
                    break;
            }
        }

        private void CharacterHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && !this.MenuButton.MenuPopup.IsOpen)
            {
                this.ChangeWindowState();
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.Redo();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.Undo();
        }

        private void UndoRedo_StackChanged(object sender, EventArgs e)
        {
            this.SetUndoRedoState();
            if (sender is not UndoRedoService service)
            {
                return;
            }

            if (service.UpdateAutosaveTimer)
            {
                this.StartStopAutosaveTimer();
            }
        }

        private void MainWindow_ModifiedChanged(object sender, EventArgs e)
        {
            this.ModifiedStatus.Visibility = ((bool)sender) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MainWindow_TimeUpdated(object sender, EventArgs e)
        {
            this.MessageBar.DrawTime();
        }

        private void MainWindow_SystemUpdated(object sender, EventArgs e)
        {
            this.MessageBar.DrawWifi();
            this.MessageBar.DrawBattery();
        }

        private void MainWindow_TextUpdated(object sender, EventArgs e)
        {
            if (sender is string updatedText)
            {
                this.MessageBar.DrawInformation(updatedText);
            }
        }

        private void LevelUpButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.LevelUp();
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ListViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void GlossaryButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.OpenGlossary();
        }

        private void CustomItemsButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.OpenCustomItems();
        }

        private void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            this.OpenConsole();
        }

        private void OnScreenKeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            SystemUtility.OpenOnScreenKeyboard();
        }

        private void ConciergeMainWindow_Drop(object sender, DragEventArgs e)
        {
            if (ConciergeDragDrop.IsHandled)
            {
                ConciergeDragDrop.Reset();
                return;
            }

            var file = ConciergeDragDrop.Capture(e.Data, ".ccs");
            if (!file.IsValid)
            {
                ConciergeMessageBox.ShowError($"Could not open '{file.FilePath}'\nOnly valid .ccs files can be dropped in Concierge.");
                return;
            }

            this.OpenCharacterSheet(file.FilePath);
        }
    }
}
