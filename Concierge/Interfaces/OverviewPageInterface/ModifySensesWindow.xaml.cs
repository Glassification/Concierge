// <copyright file="ModifySensesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for ModifySensesWindow.xaml.
    /// </summary>
    public partial class ModifySensesWindow : ConciergeWindow
    {
        public ModifySensesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.VisionComboBox.ItemsSource = Enum.GetValues(typeof(VisionTypes)).Cast<VisionTypes>();
            this.ConciergePage = ConciergePage.None;
        }

        public override string HeaderText => "Edit Senses";

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.FillFields();
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T item)
        {
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateSenses();
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields()
        {
            var character = Program.CcsFile.Character;

            this.InitiativeTextBlock.Text = character.Initiative.ToString();
            this.InitiativeBonusUpDown.Value = character.Senses.InitiativeBonus;
            this.PerceptionTextBlock.Text = character.PassivePerception.ToString();
            this.PerceptionBonusUpDown.Value = character.Senses.PerceptionBonus;
            this.VisionComboBox.Text = character.Senses.Vision.ToString();
            this.MovementTextBlock.Text = character.Senses.Movement.ToString();
            this.BaseMovementUpDown.Value = character.Senses.BaseMovement;
            this.MovementBonusUpDown.Value = character.Senses.MovementBonus;
        }

        private void UpdateSenses()
        {
            var senses = Program.CcsFile.Character.Senses;
            var oldItem = senses.DeepCopy();

            senses.InitiativeBonus = this.InitiativeBonusUpDown.Value;
            senses.PerceptionBonus = this.PerceptionBonusUpDown.Value;
            senses.Vision = (VisionTypes)Enum.Parse(typeof(VisionTypes), this.VisionComboBox.Text);
            senses.BaseMovement = this.BaseMovementUpDown.Value;
            senses.MovementBonus = this.MovementBonusUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<Senses>(senses, oldItem, this.ConciergePage));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
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
            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.InitiativeTextBlock.Text = (CharacterUtility.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity) + this.InitiativeBonusUpDown.Value).ToString();
            this.PerceptionTextBlock.Text = (Constants.BasePerception + Program.CcsFile.Character.Skill.Perception.Bonus + this.PerceptionBonusUpDown.Value).ToString();
            this.MovementTextBlock.Text = Senses.GetMovement(this.BaseMovementUpDown.Value + this.MovementBonusUpDown.Value).ToString();
        }
    }
}
