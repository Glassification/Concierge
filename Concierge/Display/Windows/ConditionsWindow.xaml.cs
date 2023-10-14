// <copyright file="ConditionsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Vitals.ConditionStates;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for ConditionsWindow.xaml.
    /// </summary>
    public partial class ConditionsWindow : ConciergeWindow
    {
        public ConditionsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.FatiguedComboBox.ItemsSource = Enum.GetValues(typeof(ExhaustionLevel)).Cast<ExhaustionLevel>();
            this.EncumbranceComboBox.ItemsSource = StringUtility.FormatEnumForDisplay(typeof(EncumbranceLevel));
            this.ConciergePage = ConciergePage.None;
            this.Conditions = new Conditions();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.BlindedCheckBox);
            this.SetMouseOverEvents(this.CharmedCheckBox);
            this.SetMouseOverEvents(this.DeathCheckBox);
            this.SetMouseOverEvents(this.DeafenedCheckBox);
            this.SetMouseOverEvents(this.FatiguedComboBox);
            this.SetMouseOverEvents(this.FrightenedCheckBox);
            this.SetMouseOverEvents(this.GrappledCheckBox);
            this.SetMouseOverEvents(this.IncapacitatedCheckBox);
            this.SetMouseOverEvents(this.InvisibleCheckBox);
            this.SetMouseOverEvents(this.ParalyzedCheckBox);
            this.SetMouseOverEvents(this.PetrifiedCheckBox);
            this.SetMouseOverEvents(this.PoisonedCheckBox);
            this.SetMouseOverEvents(this.RestrainedCheckBox);
            this.SetMouseOverEvents(this.ProneCheckBox);
            this.SetMouseOverEvents(this.StunnedCheckBox);
            this.SetMouseOverEvents(this.UnconsciousCheckBox);
            this.SetMouseOverEvents(this.EncumbranceComboBox);
            this.SetMouseOverEvents(this.EncumbranceCheckBox);
        }

        public override string HeaderText => "Edit Conditions";

        public override string WindowName => nameof(ConditionsWindow);

        private Conditions Conditions { get; set; }

        public override void ShowEdit<T>(T conditions)
        {
            if (conditions is not Conditions castItem)
            {
                return;
            }

            this.Conditions = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.UpdateConditions();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            this.BlindedCheckBox.IsChecked = this.Conditions.Blinded.Afflicted;
            this.CharmedCheckBox.IsChecked = this.Conditions.Charmed.Afflicted;
            this.DeafenedCheckBox.IsChecked = this.Conditions.Deafened.Afflicted;
            this.DeathCheckBox.IsChecked = this.Conditions.Dead.Afflicted;
            this.FatiguedComboBox.Text = this.Conditions.Fatigued.ExhaustionLevel.ToString();
            this.FrightenedCheckBox.IsChecked = this.Conditions.Frightened.Afflicted;
            this.GrappledCheckBox.IsChecked = this.Conditions.Grappled.Afflicted;
            this.IncapacitatedCheckBox.IsChecked = this.Conditions.Incapacitated.Afflicted;
            this.InvisibleCheckBox.IsChecked = this.Conditions.Invisible.Afflicted;
            this.ParalyzedCheckBox.IsChecked = this.Conditions.Paralyzed.Afflicted;
            this.PetrifiedCheckBox.IsChecked = this.Conditions.Petrified.Afflicted;
            this.PoisonedCheckBox.IsChecked = this.Conditions.Poisoned.Afflicted;
            this.ProneCheckBox.IsChecked = this.Conditions.Prone.Afflicted;
            this.RestrainedCheckBox.IsChecked = this.Conditions.Restrained.Afflicted;
            this.StunnedCheckBox.IsChecked = this.Conditions.Stunned.Afflicted;
            this.UnconsciousCheckBox.IsChecked = this.Conditions.Unconscious.Afflicted;
            this.EncumbranceComboBox.Text = this.Conditions.Encumbered.EncumbranceLevel.ToString().FormatFromEnum();
            this.EncumbranceCheckBox.IsChecked = this.Conditions.Encumbered.OverrideEncumbrance;

            DisplayUtility.SetControlEnableState(this.EncumbranceComboBox, this.Conditions.Encumbered.OverrideEncumbrance);
        }

        private void UpdateConditions()
        {
            var oldItem = this.Conditions.DeepCopy();

            this.Conditions.Blinded.Afflicted = this.BlindedCheckBox.IsChecked ?? false;
            this.Conditions.Charmed.Afflicted = this.CharmedCheckBox.IsChecked ?? false;
            this.Conditions.Dead.Afflicted = this.DeathCheckBox.IsChecked ?? false;
            this.Conditions.Deafened.Afflicted = this.DeafenedCheckBox.IsChecked ?? false;
            this.Conditions.Fatigued.ExhaustionLevel = (ExhaustionLevel)Enum.Parse(typeof(ExhaustionLevel), this.FatiguedComboBox.Text);
            this.Conditions.Frightened.Afflicted = this.FrightenedCheckBox.IsChecked ?? false;
            this.Conditions.Grappled.Afflicted = this.GrappledCheckBox.IsChecked ?? false;
            this.Conditions.Incapacitated.Afflicted = this.IncapacitatedCheckBox.IsChecked ?? false;
            this.Conditions.Invisible.Afflicted = this.InvisibleCheckBox.IsChecked ?? false;
            this.Conditions.Paralyzed.Afflicted = this.ParalyzedCheckBox.IsChecked ?? false;
            this.Conditions.Petrified.Afflicted = this.PetrifiedCheckBox.IsChecked ?? false;
            this.Conditions.Poisoned.Afflicted = this.PoisonedCheckBox.IsChecked ?? false;
            this.Conditions.Prone.Afflicted = this.ProneCheckBox.IsChecked ?? false;
            this.Conditions.Restrained.Afflicted = this.RestrainedCheckBox.IsChecked ?? false;
            this.Conditions.Stunned.Afflicted = this.StunnedCheckBox.IsChecked ?? false;
            this.Conditions.Unconscious.Afflicted = this.UnconsciousCheckBox.IsChecked ?? false;
            this.Conditions.Encumbered.OverrideEncumbrance = this.EncumbranceCheckBox.IsChecked ?? false;

            if (this.Conditions.Encumbered.OverrideEncumbrance)
            {
                this.Conditions.Encumbered.EncumbranceLevelOverride = (EncumbranceLevel)Enum.Parse(typeof(EncumbranceLevel), this.EncumbranceComboBox.Text.Strip(" "));
            }

            Program.UndoRedoService.AddCommand(new EditCommand<Conditions>(this.Conditions, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateConditions();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void EncumbranceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.EncumbranceComboBox, true);
        }

        private void EncumbranceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.EncumbranceComboBox, false);
        }
    }
}
