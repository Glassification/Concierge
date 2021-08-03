// <copyright file="ModifyAttributesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.OverviewPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Characteristics;

    /// <summary>
    /// Interaction logic for ModifyAttributesWindow.xaml.
    /// </summary>
    public partial class ModifyAttributesWindow : Window
    {
        public ModifyAttributesWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Attributes Attributes { get; set; }

        public void EditAttributes(Attributes attributes)
        {
            this.Attributes = attributes;

            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
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
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

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
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
