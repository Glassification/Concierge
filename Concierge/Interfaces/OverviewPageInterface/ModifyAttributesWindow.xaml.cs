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

        private MessageWindowResult Result { get; set; }

        public MessageWindowResult ShowWizardSetup()
        {
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Collapsed;
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

        private void ClearFields()
        {
            this.StrengthUpDown.UpdatingValue();
            this.DexterityUpDown.UpdatingValue();
            this.ConstitutionUpDown.UpdatingValue();
            this.IntelligenceUpDown.UpdatingValue();
            this.WisdomUpDown.UpdatingValue();
            this.CharismaUpDown.UpdatingValue();

            this.StrengthUpDown.Value = 0;
            this.DexterityUpDown.Value = 0;
            this.ConstitutionUpDown.Value = 0;
            this.IntelligenceUpDown.Value = 0;
            this.WisdomUpDown.Value = 0;
            this.CharismaUpDown.Value = 0;
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
                    this.Result = MessageWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Result = MessageWindowResult.OK;

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
            this.Result = MessageWindowResult.Cancel;
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Cancel;
            this.Hide();
        }
    }
}
