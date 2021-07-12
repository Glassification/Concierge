// <copyright file="ModifySensesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.OverviewPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;

    /// <summary>
    /// Interaction logic for ModifySensesWindow.xaml.
    /// </summary>
    public partial class ModifySensesWindow : Window
    {
        public ModifySensesWindow()
        {
            this.InitializeComponent();
            this.VisionComboBox.ItemsSource = Enum.GetValues(typeof(VisionTypes)).Cast<VisionTypes>();
        }

        public void EditSenses()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.InitiativeTextBox.Text = Program.CcsFile.Character.Initiative.ToString();
            this.InitiativeBonusUpDown.Value = Program.CcsFile.Character.Details.InitiativeBonus;
            this.PerceptionTextBox.Text = Program.CcsFile.Character.PassivePerception.ToString();
            this.PerceptionBonusUpDown.Value = Program.CcsFile.Character.Details.PerceptionBonus;
            this.VisionComboBox.Text = Program.CcsFile.Character.Details.Vision.ToString();
            this.MovementTextBox.Text = Program.CcsFile.Character.Details.Movement.ToString();
            this.BaseMovementUpDown.Value = Program.CcsFile.Character.Details.BaseMovement;
        }

        private void UpdateSenses()
        {
            Program.CcsFile.Character.Details.InitiativeBonus = this.InitiativeBonusUpDown.Value ?? 0;
            Program.CcsFile.Character.Details.PerceptionBonus = this.PerceptionBonusUpDown.Value ?? 0;
            Program.CcsFile.Character.Details.Vision = (VisionTypes)Enum.Parse(typeof(VisionTypes), this.VisionComboBox.Text);
            Program.CcsFile.Character.Details.BaseMovement = this.BaseMovementUpDown.Value ?? 0;

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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSenses();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSenses();
            this.FillFields();
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
