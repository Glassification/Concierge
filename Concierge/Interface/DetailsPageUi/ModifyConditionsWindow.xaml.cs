// <copyright file="ModifyConditionsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.DetailsPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;
    using Concierge.Characters.Status;
    using Concierge.Utility;

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

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Conditions Conditions { get; set; }

        public void ShowEdit(Conditions conditions)
        {
            this.Conditions = conditions;

            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.BlindedComboBox.Text = this.Conditions.Blinded.Equals("Cured") ? "Cured" : "Afflicted";
            this.CharmedComboBox.Text = this.Conditions.Charmed.Equals("Cured") ? "Cured" : "Afflicted";
            this.DeafenedComboBox.Text = this.Conditions.Deafened.Equals("Cured") ? "Cured" : "Afflicted";
            this.FatiguedComboBox.Text = this.Conditions.Fatigued;
            this.FrightenedComboBox.Text = this.Conditions.Frightened.Equals("Cured") ? "Cured" : "Afflicted";
            this.GrappledComboBox.Text = this.Conditions.Grappled.Equals("Cured") ? "Cured" : "Afflicted";
            this.IncapacitatedComboBox.Text = this.Conditions.Incapacitated.Equals("Cured") ? "Cured" : "Afflicted";
            this.InvisibleComboBox.Text = this.Conditions.Invisible.Equals("Cured") ? "Cured" : "Afflicted";
            this.ParalyzedComboBox.Text = this.Conditions.Paralyzed.Equals("Cured") ? "Cured" : "Afflicted";
            this.PetrifiedComboBox.Text = this.Conditions.Petrified.Equals("Cured") ? "Cured" : "Afflicted";
            this.PoisonedComboBox.Text = this.Conditions.Poisoned.Equals("Cured") ? "Cured" : "Afflicted";
            this.ProneComboBox.Text = this.Conditions.Prone.Equals("Cured") ? "Cured" : "Afflicted";
            this.RestrainedComboBox.Text = this.Conditions.Restrained.Equals("Cured") ? "Cured" : "Afflicted";
            this.StunnedComboBox.Text = this.Conditions.Stunned.Equals("Cured") ? "Cured" : "Afflicted";
            this.UnconsciousComboBox.Text = this.Conditions.Unconscious.Equals("Cured") ? "Cured" : "Afflicted";
            this.EncumbranceTextBox.Text = this.Conditions.Encumbrance;
        }

        private void UpdateConditions()
        {
            this.Conditions.Blinded = this.BlindedComboBox.Text.Equals("Cured") ? "Cured" : "Blinded";
            this.Conditions.Charmed = this.CharmedComboBox.Text.Equals("Cured") ? "Cured" : "Charmed";
            this.Conditions.Deafened = this.DeafenedComboBox.Text.Equals("Cured") ? "Cured" : "Deafened";
            this.Conditions.Fatigued = this.FatiguedComboBox.Text;
            this.Conditions.Frightened = this.FrightenedComboBox.Text.Equals("Cured") ? "Cured" : "Frightened";
            this.Conditions.Grappled = this.GrappledComboBox.Text.Equals("Cured") ? "Cured" : "Grappled";
            this.Conditions.Incapacitated = this.IncapacitatedComboBox.Text.Equals("Cured") ? "Cured" : "Incapacitated";
            this.Conditions.Invisible = this.InvisibleComboBox.Text.Equals("Cured") ? "Cured" : "Invisible";
            this.Conditions.Paralyzed = this.ParalyzedComboBox.Text.Equals("Cured") ? "Cured" : "Paralyzed";
            this.Conditions.Petrified = this.PetrifiedComboBox.Text.Equals("Cured") ? "Cured" : "Petrified";
            this.Conditions.Poisoned = this.PoisonedComboBox.Text.Equals("Cured") ? "Cured" : "Poisoned";
            this.Conditions.Prone = this.ProneComboBox.Text.Equals("Cured") ? "Cured" : "Prone";
            this.Conditions.Restrained = this.RestrainedComboBox.Text.Equals("Cured") ? "Cured" : "Restrained";
            this.Conditions.Stunned = this.StunnedComboBox.Text.Equals("Cured") ? "Cured" : "Stunned";
            this.Conditions.Unconscious = this.UnconsciousComboBox.Text.Equals("Cured") ? "Cured" : "Unconscious";
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
            ConciergeSound.TapNavigation();
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

            this.UpdateConditions();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.TapNavigation();

            this.UpdateConditions();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
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
