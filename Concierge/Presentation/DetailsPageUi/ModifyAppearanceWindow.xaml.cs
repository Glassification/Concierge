﻿// <copyright file="ModifyAppearanceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModifyAppearanceWindow.xaml.
    /// </summary>
    public partial class ModifyAppearanceWindow : Window
    {
        public ModifyAppearanceWindow()
        {
            this.InitializeComponent();
        }

        public void ShowEdit()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.GenderTextBox.Text = Program.CcsFile.Character.Appearance.Gender;
            this.AgeTextBox.Text = Program.CcsFile.Character.Appearance.Age;
            this.HeightTextBox.Text = Program.CcsFile.Character.Appearance.Height;
            this.WeightTextBox.Text = Program.CcsFile.Character.Appearance.Weight;
            this.SkinColourTextBox.Text = Program.CcsFile.Character.Appearance.SkinColour;
            this.EyeColourTextBox.Text = Program.CcsFile.Character.Appearance.EyeColour;
            this.HairColourTextBox.Text = Program.CcsFile.Character.Appearance.HairColour;
            this.DistinguishingMarksTextBox.Text = Program.CcsFile.Character.Appearance.DistinguishingMarks;
        }

        private void UpdateAppearance()
        {
            Program.CcsFile.Character.Appearance.Gender = this.GenderTextBox.Text;
            Program.CcsFile.Character.Appearance.Age = this.AgeTextBox.Text;
            Program.CcsFile.Character.Appearance.Height = this.HeightTextBox.Text;
            Program.CcsFile.Character.Appearance.Weight = this.WeightTextBox.Text;
            Program.CcsFile.Character.Appearance.SkinColour = this.SkinColourTextBox.Text;
            Program.CcsFile.Character.Appearance.EyeColour = this.EyeColourTextBox.Text;
            Program.CcsFile.Character.Appearance.HairColour = this.HairColourTextBox.Text;
            Program.CcsFile.Character.Appearance.DistinguishingMarks = this.DistinguishingMarksTextBox.Text;

            Program.Modified = true;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateAppearance();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateAppearance();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
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
