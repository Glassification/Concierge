// <copyright file="ModifyPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces
{
    using System;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyPropertiesWindow.xaml.
    /// </summary>
    public partial class ModifyPropertiesWindow : ConciergeWindow
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

        private CharacterProperties CharacterProperties { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;
            this.CharacterProperties = Program.CcsFile.Character.Properties;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T properties)
        {
            var castItem = properties as CharacterProperties;
            this.CharacterProperties = castItem;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        private void FillFields()
        {
            this.NameTextBox.Text = this.CharacterProperties.Name;
            this.RaceComboBox.Text = this.CharacterProperties.Race;
            this.BackgroundComboBox.Text = this.CharacterProperties.Background;
            this.AlignmentComboBox.Text = this.CharacterProperties.Alignment;
            this.Level1UpDown.Value = this.CharacterProperties.Class1.Level;
            this.Level2UpDown.Value = this.CharacterProperties.Class2.Level;
            this.Level3UpDown.Value = this.CharacterProperties.Class3.Level;
            this.Class1ComboBox.Text = this.CharacterProperties.Class1.Name;
            this.Class2ComboBox.Text = this.CharacterProperties.Class2.Name;
            this.Class3ComboBox.Text = this.CharacterProperties.Class3.Name;
        }

        private void UpdateProperties()
        {
            var oldItem = this.CharacterProperties.DeepCopy();

            this.CharacterProperties.Name = this.NameTextBox.Text;
            this.CharacterProperties.Race = this.RaceComboBox.Text;
            this.CharacterProperties.Background = this.BackgroundComboBox.Text;
            this.CharacterProperties.Alignment = this.AlignmentComboBox.Text;
            this.CharacterProperties.Class1.Level = this.Level1UpDown.Value;
            this.CharacterProperties.Class2.Level = this.Level2UpDown.Value;
            this.CharacterProperties.Class3.Level = this.Level3UpDown.Value;
            this.CharacterProperties.Class1.Name = this.Class1ComboBox.Text;
            this.CharacterProperties.Class2.Name = this.Class2ComboBox.Text;
            this.CharacterProperties.Class3.Name = this.Class3ComboBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<CharacterProperties>(this.CharacterProperties, oldItem, ConciergePage.None));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateProperties();
            this.HideConciergeWindow();

            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateProperties();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;

            this.HideConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;

            this.HideConciergeWindow();
        }
    }
}
