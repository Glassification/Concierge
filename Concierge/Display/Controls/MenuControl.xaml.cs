// <copyright file="MenuControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Configuration;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for MenuControl.xaml.
    /// </summary>
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            this.InitializeComponent();

            this.UndoMenuItem.AddClickEvent(this.MenuButton_Click);
            this.RedoMenuItem.AddClickEvent(this.MenuButton_Click);
            this.HelpMenuItem.AddClickEvent(this.MenuButton_Click);
            this.AboutMenuItem.AddClickEvent(this.MenuButton_Click);

            this.NewCharacterMenuItem.AddClickEvent(this.MenuButton_Click);
            this.OpenCharacterMenuItem.AddClickEvent(this.MenuButton_Click);
            this.ImportCharacterMenuItem.AddClickEvent(this.MenuButton_Click);
            this.SaveCharacterMenuItem.AddClickEvent(this.MenuButton_Click);
            this.SaveAsCharacterMenuItem.AddClickEvent(this.MenuButton_Click);

            this.CharacterCreationMenuItem.AddClickEvent(this.MenuButton_Click);
            this.PartyMenuItem.AddClickEvent(this.MenuButton_Click);
            this.LevelUpMenuItem.AddClickEvent(this.MenuButton_Click);
            this.ShortRestMenuItem.AddClickEvent(this.MenuButton_Click);
            this.LongRestMenuItem.AddClickEvent(this.MenuButton_Click);
            this.CharacterPropertiesMenuItem.AddClickEvent(this.MenuButton_Click);

            this.ConsoleMenuItem.AddClickEvent(this.MenuButton_Click);
            this.CustomItemsMenuItem.AddClickEvent(this.MenuButton_Click);
            this.SearchMenuItem.AddClickEvent(this.MenuButton_Click);
            this.ExportAppDataMenuItem.AddClickEvent(this.MenuButton_Click);
            this.ImportAppDataMenuItem.AddClickEvent(this.MenuButton_Click);
            this.MessageHistoryMenuItem.AddClickEvent(this.MenuButton_Click);
            this.KeyboardMenuItem.AddClickEvent(this.MenuButton_Click);
            this.SettingsMenuItem.AddClickEvent(this.MenuButton_Click);

            this.CheckExpertMode();
        }

        private void CheckExpertMode()
        {
            if (!AppSettingsManager.StartUp.ExpertMode)
            {
                this.ConsoleMenuItem.Visibility = Visibility.Collapsed;
                this.ExportAppDataMenuItem.Visibility = Visibility.Collapsed;
                this.ImportAppDataMenuItem.Visibility = Visibility.Collapsed;
                this.MessageHistoryMenuItem.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.PopupToggleButton.IsChecked = false;
            this.FileMenuItem.RealButton.IsChecked = false;
            this.CharacterMenuItem.RealButton.IsChecked = false;
            this.ToolsMenuItem.RealButton.IsChecked = false;
        }

        private void FileMenuPopup_Closed(object sender, EventArgs e)
        {
            this.FileMenuItem.RealButton.IsChecked = false;
        }

        private void CharacterMenuPopup_Closed(object sender, EventArgs e)
        {
            this.CharacterMenuItem.RealButton.IsChecked = false;
        }

        private void ToolsMenuPopup_Closed(object sender, EventArgs e)
        {
            this.ToolsMenuItem.RealButton.IsChecked = false;
        }

        private void PopupToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MenuButtonIcon.Kind = PackIconKind.MenuOpen;
        }

        private void PopupToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.MenuButtonIcon.Kind = PackIconKind.Menu;
        }
    }
}
