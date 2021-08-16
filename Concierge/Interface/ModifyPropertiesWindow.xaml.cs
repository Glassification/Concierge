// <copyright file="ModifyPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyPropertiesWindow.xaml.
    /// </summary>
    public partial class ModifyPropertiesWindow : Window
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

        public void ShowWindow()
        {
            this.Read();
            this.ShowDialog();
        }

        private void Read()
        {
            Program.Logger.Info($"Read character sheet.");

            this.Level1UpDown.UpdatingValue();
            this.Level2UpDown.UpdatingValue();
            this.Level3UpDown.UpdatingValue();

            this.NameTextBox.Text = Program.CcsFile.Character.Details.Name;
            this.RaceComboBox.Text = Program.CcsFile.Character.Details.Race;
            this.BackgroundComboBox.Text = Program.CcsFile.Character.Details.Background;
            this.AlignmentComboBox.Text = Program.CcsFile.Character.Details.Alignment;
            this.Level1UpDown.Value = Program.CcsFile.Character.Class1.Level;
            this.Level2UpDown.Value = Program.CcsFile.Character.Class2.Level;
            this.Level3UpDown.Value = Program.CcsFile.Character.Class3.Level;
            this.Class1ComboBox.Text = Program.CcsFile.Character.Class1.Name;
            this.Class2ComboBox.Text = Program.CcsFile.Character.Class2.Name;
            this.Class3ComboBox.Text = Program.CcsFile.Character.Class3.Name;
        }

        private void Write()
        {
            Program.Logger.Info($"Write character sheet.");

            Program.CcsFile.Character.Details.Name = this.NameTextBox.Text;
            Program.CcsFile.Character.Details.Race = this.RaceComboBox.Text;
            Program.CcsFile.Character.Details.Background = this.BackgroundComboBox.Text;
            Program.CcsFile.Character.Details.Alignment = this.AlignmentComboBox.Text;
            Program.CcsFile.Character.Class1.Level = this.Level1UpDown.Value ?? 0;
            Program.CcsFile.Character.Class2.Level = this.Level2UpDown.Value ?? 0;
            Program.CcsFile.Character.Class3.Level = this.Level3UpDown.Value ?? 0;
            Program.CcsFile.Character.Class1.Name = this.Class1ComboBox.Text;
            Program.CcsFile.Character.Class2.Name = this.Class2ComboBox.Text;
            Program.CcsFile.Character.Class3.Name = this.Class3ComboBox.Text;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Program.Logger.Info($"ESCAPE key pressed.");
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"OK button click.");
            Program.Modify();

            this.Write();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Apply button click.");
            Program.Modify();

            this.Write();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Cancel button click.");

            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"Close button click.");

            this.Hide();
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}
