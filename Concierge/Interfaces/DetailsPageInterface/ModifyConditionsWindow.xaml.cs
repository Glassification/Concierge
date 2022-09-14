// <copyright file="ModifyConditionsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Statuses.ConditionStatus;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for MondifyConditionsWindow.xaml.
    /// </summary>
    public partial class MondifyConditionsWindow : ConciergeWindow
    {
        public MondifyConditionsWindow()
        {
            this.InitializeComponent();

            this.FatiguedComboBox.ItemsSource = Enum.GetValues(typeof(ExhaustionLevel)).Cast<ExhaustionLevel>();
            this.ConciergePage = ConciergePage.None;
            this.Conditions = new Conditions();
        }

        public override string HeaderText => "Edit Conditions";

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

            Program.Modify();
        }

        private void FillFields()
        {
            this.BlindedCheckBox.IsChecked = this.Conditions.Blinded.Afflicted;
            this.CharmedCheckBox.IsChecked = this.Conditions.Charmed.Afflicted;
            this.DeafenedCheckBox.IsChecked = this.Conditions.Deafened.Afflicted;
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
            this.EncumbranceTextBox.Text = this.Conditions.Encumbrance.EncumbranceLevel.ToString();
        }

        private void UpdateConditions()
        {
            var oldItem = this.Conditions.DeepCopy();

            this.Conditions.Blinded.Afflicted = this.BlindedCheckBox.IsChecked ?? false;
            this.Conditions.Charmed.Afflicted = this.CharmedCheckBox.IsChecked ?? false;
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

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
