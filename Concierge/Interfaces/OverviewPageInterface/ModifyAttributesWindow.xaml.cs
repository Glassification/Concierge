// <copyright file="ModifyAttributesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyAttributesWindow.xaml.
    /// </summary>
    public partial class ModifyAttributesWindow : Window, IConciergeWindow
    {
        public ModifyAttributesWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Attributes Attributes { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Attributes = Program.CcsFile.Character.Attributes;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void EditAttributes(Attributes attributes)
        {
            this.Attributes = attributes;
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
            this.StrengthUpDown.UpdatingValue();
            this.DexterityUpDown.UpdatingValue();
            this.ConstitutionUpDown.UpdatingValue();
            this.IntelligenceUpDown.UpdatingValue();
            this.WisdomUpDown.UpdatingValue();
            this.CharismaUpDown.UpdatingValue();

            this.StrengthUpDown.Value = this.Attributes.Strength;
            this.DexterityUpDown.Value = this.Attributes.Dexterity;
            this.ConstitutionUpDown.Value = this.Attributes.Constitution;
            this.IntelligenceUpDown.Value = this.Attributes.Intelligence;
            this.WisdomUpDown.Value = this.Attributes.Wisdom;
            this.CharismaUpDown.Value = this.Attributes.Charisma;
        }

        private void UpdateAttributes()
        {
            this.Attributes.Strength = this.StrengthUpDown.Value ?? 0;
            this.Attributes.Dexterity = this.DexterityUpDown.Value ?? 0;
            this.Attributes.Constitution = this.ConstitutionUpDown.Value ?? 0;
            this.Attributes.Intelligence = this.IntelligenceUpDown.Value ?? 0;
            this.Attributes.Wisdom = this.WisdomUpDown.Value ?? 0;
            this.Attributes.Charisma = this.CharismaUpDown.Value ?? 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Result = ConciergeWindowResult.OK;

            this.UpdateAttributes();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateAttributes();

            this.ApplyChanges?.Invoke(this, new EventArgs());
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
