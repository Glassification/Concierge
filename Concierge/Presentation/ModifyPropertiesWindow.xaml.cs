// <copyright file="ModifyPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;
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
            this.Class1ComboBox.ItemsSource = Enum.GetValues(typeof(ClassType)).Cast<ClassType>();
            this.Class2ComboBox.ItemsSource = Enum.GetValues(typeof(ClassType)).Cast<ClassType>();
            this.Class3ComboBox.ItemsSource = Enum.GetValues(typeof(ClassType)).Cast<ClassType>();
        }

        public void ShowWindow()
        {
            this.Read();
            this.ShowDialog();
        }

        private void Read()
        {
            this.NameTextBox.Text = Program.Character.Details.Name;
            this.RaceComboBox.Text = Program.Character.Details.Race;
            this.BackgroundComboBox.Text = Program.Character.Details.Background;
            this.AlignmentComboBox.Text = Program.Character.Details.Alignment;
            this.Level1UpDown.Value = Program.Character.Classess[0].Level;
            this.Level2UpDown.Value = Program.Character.Classess[1].Level;
            this.Level3UpDown.Value = Program.Character.Classess[2].Level;
            this.Class1ComboBox.Text = Program.Character.Classess[0].Name;
            this.Class2ComboBox.Text = Program.Character.Classess[1].Name;
            this.Class3ComboBox.Text = Program.Character.Classess[2].Name;
        }

        private void Write()
        {
            Program.Character.Details.Name = this.NameTextBox.Text;
            Program.Character.Details.Race = this.RaceComboBox.Text;
            Program.Character.Details.Background = this.BackgroundComboBox.Text;
            Program.Character.Details.Alignment = this.BackgroundComboBox.Text;
            Program.Character.Classess[0].Level = this.Level1UpDown.Value ?? 0;
            Program.Character.Classess[1].Level = this.Level2UpDown.Value ?? 0;
            Program.Character.Classess[2].Level = this.Level3UpDown.Value ?? 0;
            Program.Character.Classess[0].Name = this.Class1ComboBox.Text;
            Program.Character.Classess[1].Name = this.Class2ComboBox.Text;
            Program.Character.Classess[2].Name = this.Class3ComboBox.Text;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Write();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Write();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
