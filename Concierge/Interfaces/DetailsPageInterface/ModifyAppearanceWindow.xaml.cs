// <copyright file="ModifyAppearanceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyAppearanceWindow.xaml.
    /// </summary>
    public partial class ModifyAppearanceWindow : Window, IConciergeWindow
    {
        public ModifyAppearanceWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Appearance Appearance { get; set; }

        private MessageWindowResult Result { get; set; }

        public MessageWindowResult ShowWizardSetup()
        {
            this.Appearance = Program.CcsFile.Character.Appearance;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit(Appearance appearance)
        {
            this.Appearance = appearance;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.AgeUpDown.UpdatingValue();

            this.GenderTextBox.Text = this.Appearance.Gender;
            this.AgeUpDown.Value = this.Appearance.Age;
            this.HeightTextBox.Text = this.Appearance.Height;
            this.WeightTextBox.Text = this.Appearance.Weight;
            this.SkinColourTextBox.Text = this.Appearance.SkinColour;
            this.EyeColourTextBox.Text = this.Appearance.EyeColour;
            this.HairColourTextBox.Text = this.Appearance.HairColour;
            this.DistinguishingMarksTextBox.Text = this.Appearance.DistinguishingMarks;
        }

        private void UpdateAppearance()
        {
            this.Appearance.Gender = this.GenderTextBox.Text;
            this.Appearance.Age = this.AgeUpDown.Value ?? 0;
            this.Appearance.Height = this.HeightTextBox.Text;
            this.Appearance.Weight = this.WeightTextBox.Text;
            this.Appearance.SkinColour = this.SkinColourTextBox.Text;
            this.Appearance.EyeColour = this.EyeColourTextBox.Text;
            this.Appearance.HairColour = this.HairColourTextBox.Text;
            this.Appearance.DistinguishingMarks = this.DistinguishingMarksTextBox.Text;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = MessageWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = MessageWindowResult.OK;

            this.UpdateAppearance();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateAppearance();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Cancel;
            this.Hide();
        }
    }
}
