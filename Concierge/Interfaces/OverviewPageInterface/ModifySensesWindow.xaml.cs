// <copyright file="ModifySensesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySensesWindow.xaml.
    /// </summary>
    public partial class ModifySensesWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifySensesWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.VisionComboBox.ItemsSource = Enum.GetValues(typeof(VisionTypes)).Cast<VisionTypes>();
            this.conciergePage = conciergePage;
        }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.FillFields();
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void EditSenses()
        {
            this.ApplyButton.Visibility = Visibility.Visible;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields()
        {
            this.InitiativeBonusUpDown.UpdatingValue();
            this.PerceptionBonusUpDown.UpdatingValue();
            this.BaseMovementUpDown.UpdatingValue();

            this.InitiativeTextBlock.Text = Program.CcsFile.Character.Initiative.ToString();
            this.InitiativeBonusUpDown.Value = Program.CcsFile.Character.Senses.InitiativeBonus;
            this.PerceptionTextBlock.Text = Program.CcsFile.Character.PassivePerception.ToString();
            this.PerceptionBonusUpDown.Value = Program.CcsFile.Character.Senses.PerceptionBonus;
            this.VisionComboBox.Text = Program.CcsFile.Character.Senses.Vision.ToString();
            this.MovementTextBlock.Text = Program.CcsFile.Character.Senses.Movement.ToString();
            this.BaseMovementUpDown.Value = Program.CcsFile.Character.Senses.BaseMovement;
        }

        private void UpdateSenses()
        {
            var senses = Program.CcsFile.Character.Senses;
            var oldItem = senses.DeepCopy() as Senses;

            senses.InitiativeBonus = this.InitiativeBonusUpDown.Value ?? 0;
            senses.PerceptionBonus = this.PerceptionBonusUpDown.Value ?? 0;
            senses.Vision = (VisionTypes)Enum.Parse(typeof(VisionTypes), this.VisionComboBox.Text);
            senses.BaseMovement = this.BaseMovementUpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<Senses>(senses, oldItem, this.conciergePage));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateSenses();
            this.HideConciergeWindow();

            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSenses();
            this.FillFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.InitiativeTextBlock.Text = (Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity) + (this.InitiativeBonusUpDown.Value ?? 0)).ToString();
            this.PerceptionTextBlock.Text = (Constants.BasePerception + Program.CcsFile.Character.Skill.Perception.Bonus + (this.PerceptionBonusUpDown.Value ?? 0)).ToString();
            this.MovementTextBlock.Text = Senses.GetMovement(this.BaseMovementUpDown.Value ?? 0).ToString();
        }
    }
}
