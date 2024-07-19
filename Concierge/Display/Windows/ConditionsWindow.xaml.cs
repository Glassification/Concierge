// <copyright file="ConditionsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    using Condition = Concierge.Character.Vitals.Condition;

    /// <summary>
    /// Interaction logic for ConditionsWindow.xaml.
    /// </summary>
    public partial class ConditionsWindow : ConciergeWindow
    {
        private Status status = new ();

        public ConditionsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.FatiguedComboBox.ItemsSource = ComboBoxGenerator.ExhaustionLevelComboBox();
            this.ConciergePage = ConciergePages.None;
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
        }

        public override string HeaderText => "Edit Conditions";

        public override string WindowName => nameof(ConditionsWindow);

        public override void ShowEdit<T>(T conditions)
        {
            if (conditions is not Status castItem)
            {
                return;
            }

            this.status = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.UpdateConditions();
            this.CloseConciergeWindow();
        }

        private static void SetStatus(ConciergeCheckBox checkBox, Condition condition)
        {
            condition.Status = (checkBox.IsChecked ?? false) ? ConditionStatus.Afflicted : ConditionStatus.Normal;
        }

        private void FillFields()
        {
            Program.Drawing();

            this.BlindedCheckBox.IsChecked = this.status.Blinded.IsAfflicted();
            this.CharmedCheckBox.IsChecked = this.status.Charmed.IsAfflicted();
            this.DeafenedCheckBox.IsChecked = this.status.Deafened.IsAfflicted();
            this.DeathCheckBox.IsChecked = this.status.Death.IsAfflicted();
            this.FatiguedComboBox.Text = this.status.Exhaustion.Status.PascalCase();
            this.FrightenedCheckBox.IsChecked = this.status.Frightened.IsAfflicted();
            this.GrappledCheckBox.IsChecked = this.status.Grappled.IsAfflicted();
            this.IncapacitatedCheckBox.IsChecked = this.status.Incapacitated.IsAfflicted();
            this.InvisibleCheckBox.IsChecked = this.status.Invisible.IsAfflicted();
            this.ParalyzedCheckBox.IsChecked = this.status.Paralyzed.IsAfflicted();
            this.PetrifiedCheckBox.IsChecked = this.status.Petrified.IsAfflicted();
            this.PoisonedCheckBox.IsChecked = this.status.Poisoned.IsAfflicted();
            this.ProneCheckBox.IsChecked = this.status.Prone.IsAfflicted();
            this.RestrainedCheckBox.IsChecked = this.status.Restrained.IsAfflicted();
            this.StunnedCheckBox.IsChecked = this.status.Stunned.IsAfflicted();
            this.UnconsciousCheckBox.IsChecked = this.status.Unconscious.IsAfflicted();

            Program.NotDrawing();
        }

        private void UpdateConditions()
        {
            var oldItem = this.status.DeepCopy();

            SetStatus(this.BlindedCheckBox, this.status.Blinded);
            SetStatus(this.CharmedCheckBox, this.status.Charmed);
            SetStatus(this.DeafenedCheckBox, this.status.Deafened);
            SetStatus(this.DeathCheckBox, this.status.Death);
            SetStatus(this.FrightenedCheckBox, this.status.Frightened);
            SetStatus(this.GrappledCheckBox, this.status.Grappled);
            SetStatus(this.IncapacitatedCheckBox, this.status.Incapacitated);
            SetStatus(this.InvisibleCheckBox, this.status.Invisible);
            SetStatus(this.ParalyzedCheckBox, this.status.Paralyzed);
            SetStatus(this.PetrifiedCheckBox, this.status.Petrified);
            SetStatus(this.PoisonedCheckBox, this.status.Poisoned);
            SetStatus(this.ProneCheckBox, this.status.Prone);
            SetStatus(this.RestrainedCheckBox, this.status.Restrained);
            SetStatus(this.StunnedCheckBox, this.status.Stunned);
            SetStatus(this.UnconsciousCheckBox, this.status.Unconscious);
            this.status.Exhaustion.Status = this.FatiguedComboBox.Text.ToEnum<ConditionStatus>();

            Program.UndoRedoService.AddCommand(new EditCommand<Status>(this.status, oldItem, this.ConciergePage));
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
    }
}
