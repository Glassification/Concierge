// <copyright file="ModifyAttributesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.OverviewPageUi
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ModifyAttributesWindow.xaml.
    /// </summary>
    public partial class ModifyAttributesWindow : Window
    {
        public ModifyAttributesWindow()
        {
            this.InitializeComponent();
        }

        public void EditAttributes()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.StrengthUpDown.Value = Program.Character.Attributes.Strength;
            this.DexterityUpDown.Value = Program.Character.Attributes.Dexterity;
            this.ConstitutionUpDown.Value = Program.Character.Attributes.Constitution;
            this.IntelligenceUpDown.Value = Program.Character.Attributes.Intelligence;
            this.WisdomUpDown.Value = Program.Character.Attributes.Wisdom;
            this.CharismaUpDown.Value = Program.Character.Attributes.Charisma;
        }

        private void UpdateAttributes()
        {
            Program.Character.Attributes.Strength = this.StrengthUpDown.Value ?? 0;
            Program.Character.Attributes.Dexterity = this.DexterityUpDown.Value ?? 0;
            Program.Character.Attributes.Constitution = this.ConstitutionUpDown.Value ?? 0;
            Program.Character.Attributes.Intelligence = this.IntelligenceUpDown.Value ?? 0;
            Program.Character.Attributes.Wisdom = this.WisdomUpDown.Value ?? 0;
            Program.Character.Attributes.Charisma = this.CharismaUpDown.Value ?? 0;
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
            this.UpdateAttributes();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateAttributes();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
