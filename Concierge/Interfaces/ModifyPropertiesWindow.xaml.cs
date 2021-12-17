// <copyright file="ModifyPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyPropertiesWindow.xaml.
    /// </summary>
    public partial class ModifyPropertiesWindow : ConciergeWindow, IConciergeModifyWindow
    {
        public ModifyPropertiesWindow()
        {
            this.InitializeComponent();
            this.AlignmentComboBox.ItemsSource = Constants.Alignment;
            this.RaceComboBox.ItemsSource = Constants.Races;
            this.BackgroundComboBox.ItemsSource = Constants.Backgrounds;
            this.Class1ComboBox.ItemsSource = Constants.Classes;
            this.Class2ComboBox.ItemsSource = Constants.Classes;
            this.Class3ComboBox.ItemsSource = Constants.Classes;

            Program.Logger.Info($"Initialized {nameof(ModifyPropertiesWindow)}.");
        }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit()
        {
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields()
        {
            var properties = Program.CcsFile.Character.Properties;

            this.Level1UpDown.UpdatingValue();
            this.Level2UpDown.UpdatingValue();
            this.Level3UpDown.UpdatingValue();

            this.NameTextBox.Text = properties.Name;
            this.RaceComboBox.Text = properties.Race;
            this.BackgroundComboBox.Text = properties.Background;
            this.AlignmentComboBox.Text = properties.Alignment;
            this.Level1UpDown.Value = properties.Class1.Level;
            this.Level2UpDown.Value = properties.Class2.Level;
            this.Level3UpDown.Value = properties.Class3.Level;
            this.Class1ComboBox.Text = properties.Class1.Name;
            this.Class2ComboBox.Text = properties.Class2.Name;
            this.Class3ComboBox.Text = properties.Class3.Name;
        }

        private void UpdateProperties()
        {
            var properties = Program.CcsFile.Character.Properties;
            var oldItem = properties.DeepCopy() as CharacterProperties;

            properties.Name = this.NameTextBox.Text;
            properties.Race = this.RaceComboBox.Text;
            properties.Background = this.BackgroundComboBox.Text;
            properties.Alignment = this.AlignmentComboBox.Text;
            properties.Class1.Level = this.Level1UpDown.Value ?? 0;
            properties.Class2.Level = this.Level2UpDown.Value ?? 0;
            properties.Class3.Level = this.Level3UpDown.Value ?? 0;
            properties.Class1.Name = this.Class1ComboBox.Text;
            properties.Class2.Name = this.Class2ComboBox.Text;
            properties.Class3.Name = this.Class3ComboBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<CharacterProperties>(properties, oldItem, ConciergePage.None));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateProperties();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateProperties();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;

            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;

            this.Hide();
        }
    }
}
