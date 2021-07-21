// <copyright file="ModifyConditionsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;

    /// <summary>
    /// Interaction logic for MondifyConditionsWindow.xaml.
    /// </summary>
    public partial class MondifyConditionsWindow : Window
    {
        public MondifyConditionsWindow()
        {
            this.InitializeComponent();

            this.BlindedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.CharmedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.DeafenedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.FatiguedComboBox.ItemsSource = Enum.GetValues(typeof(ExhaustionLevel)).Cast<ExhaustionLevel>();
            this.FrightenedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.GrappledComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.IncapacitatedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.InvisibleComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.ParalyzedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.PetrifiedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.PoisonedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.ProneComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.RestrainedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.StunnedComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
            this.UnconsciousComboBox.ItemsSource = Enum.GetValues(typeof(ConditionTypes)).Cast<ConditionTypes>();
        }

        public void ShowEdit()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.BlindedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Blinded.Equals("Cured") ? "Cured" : "Afflicted";
            this.CharmedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Charmed.Equals("Cured") ? "Cured" : "Afflicted";
            this.DeafenedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Deafened.Equals("Cured") ? "Cured" : "Afflicted";
            this.FatiguedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Fatigued;
            this.FrightenedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Frightened.Equals("Cured") ? "Cured" : "Afflicted";
            this.GrappledComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Grappled.Equals("Cured") ? "Cured" : "Afflicted";
            this.IncapacitatedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Incapacitated.Equals("Cured") ? "Cured" : "Afflicted";
            this.InvisibleComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Invisible.Equals("Cured") ? "Cured" : "Afflicted";
            this.ParalyzedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Paralyzed.Equals("Cured") ? "Cured" : "Afflicted";
            this.PetrifiedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Petrified.Equals("Cured") ? "Cured" : "Afflicted";
            this.PoisonedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Poisoned.Equals("Cured") ? "Cured" : "Afflicted";
            this.ProneComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Prone.Equals("Cured") ? "Cured" : "Afflicted";
            this.RestrainedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Restrained.Equals("Cured") ? "Cured" : "Afflicted";
            this.StunnedComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Stunned.Equals("Cured") ? "Cured" : "Afflicted";
            this.UnconsciousComboBox.Text = Program.CcsFile.Character.Vitality.Conditions.Unconscious.Equals("Cured") ? "Cured" : "Afflicted";
            this.EncumbranceTextBox.Text = Program.CcsFile.Character.Vitality.Conditions.Encumbrance;
        }

        private void UpdateConditions()
        {
            Program.CcsFile.Character.Vitality.Conditions.Blinded = this.BlindedComboBox.Text.Equals("Cured") ? "Cured" : "Blinded";
            Program.CcsFile.Character.Vitality.Conditions.Charmed = this.CharmedComboBox.Text.Equals("Cured") ? "Cured" : "Charmed";
            Program.CcsFile.Character.Vitality.Conditions.Deafened = this.DeafenedComboBox.Text.Equals("Cured") ? "Cured" : "Deafened";
            Program.CcsFile.Character.Vitality.Conditions.Fatigued = this.FatiguedComboBox.Text;
            Program.CcsFile.Character.Vitality.Conditions.Frightened = this.FrightenedComboBox.Text.Equals("Cured") ? "Cured" : "Frightened";
            Program.CcsFile.Character.Vitality.Conditions.Grappled = this.GrappledComboBox.Text.Equals("Cured") ? "Cured" : "Grappled";
            Program.CcsFile.Character.Vitality.Conditions.Incapacitated = this.IncapacitatedComboBox.Text.Equals("Cured") ? "Cured" : "Incapacitated";
            Program.CcsFile.Character.Vitality.Conditions.Invisible = this.InvisibleComboBox.Text.Equals("Cured") ? "Cured" : "Invisible";
            Program.CcsFile.Character.Vitality.Conditions.Paralyzed = this.ParalyzedComboBox.Text.Equals("Cured") ? "Cured" : "Paralyzed";
            Program.CcsFile.Character.Vitality.Conditions.Petrified = this.PetrifiedComboBox.Text.Equals("Cured") ? "Cured" : "Petrified";
            Program.CcsFile.Character.Vitality.Conditions.Poisoned = this.PoisonedComboBox.Text.Equals("Cured") ? "Cured" : "Poisoned";
            Program.CcsFile.Character.Vitality.Conditions.Prone = this.ProneComboBox.Text.Equals("Cured") ? "Cured" : "Prone";
            Program.CcsFile.Character.Vitality.Conditions.Restrained = this.RestrainedComboBox.Text.Equals("Cured") ? "Cured" : "Restrained";
            Program.CcsFile.Character.Vitality.Conditions.Stunned = this.StunnedComboBox.Text.Equals("Cured") ? "Cured" : "Stunned";
            Program.CcsFile.Character.Vitality.Conditions.Unconscious = this.UnconsciousComboBox.Text.Equals("Cured") ? "Cured" : "Unconscious";

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
            this.UpdateConditions();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateConditions();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
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
