// <copyright file="SensesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Details;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for SensesWindow.xaml.
    /// </summary>
    public partial class SensesWindow : ConciergeWindow
    {
        public SensesWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.VisionComboBox.ItemsSource = ComboBoxGenerator.VisionComboBox();
            this.ConciergePage = ConciergePage.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.BaseMovementUpDown);
            this.SetMouseOverEvents(this.VisionComboBox);
            this.SetMouseOverEvents(this.InitiativeBonusUpDown);
            this.SetMouseOverEvents(this.PerceptionBonusUpDown);
            this.SetMouseOverEvents(this.MovementBonusUpDown);
            this.SetMouseOverEvents(this.InspirationCheckBox);
            this.SetMouseOverEvents(this.InitiativeTextBox, this.InitiativeTextBackground);
            this.SetMouseOverEvents(this.PerceptionTextBox, this.PerceptionTextBackground);
            this.SetMouseOverEvents(this.MovementTextBox, this.MovementTextBackground);
        }

        public override string HeaderText => "Edit Senses";

        public override string WindowName => nameof(SensesWindow);

        public override ConciergeResult ShowWizardSetup(string buttonText)
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
            this.Result = ConciergeResult.OK;

            this.UpdateSenses();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            var character = Program.CcsFile.Character;

            this.InitiativeTextBox.Text = character.Initiative.ToString();
            this.InitiativeBonusUpDown.Value = character.Detail.Senses.InitiativeBonus;
            this.PerceptionTextBox.Text = character.PassivePerception.ToString();
            this.PerceptionBonusUpDown.Value = character.Detail.Senses.PerceptionBonus;
            this.VisionComboBox.Text = character.Detail.Senses.Vision.ToString().FormatFromPascalCase();
            this.MovementTextBox.Text = character.GetMovement().ToString();
            this.BaseMovementUpDown.Value = character.Detail.Senses.BaseMovement;
            this.MovementBonusUpDown.Value = character.Detail.Senses.MovementBonus;
            this.InspirationCheckBox.IsChecked = character.Detail.Senses.Inspiration;
        }

        private void UpdateSenses()
        {
            var senses = Program.CcsFile.Character.Detail.Senses;
            var oldItem = senses.DeepCopy();

            senses.InitiativeBonus = this.InitiativeBonusUpDown.Value;
            senses.PerceptionBonus = this.PerceptionBonusUpDown.Value;
            senses.Vision = this.VisionComboBox.Text.Strip(" ").ToEnum<VisionTypes>();
            senses.BaseMovement = this.BaseMovementUpDown.Value;
            senses.MovementBonus = this.MovementBonusUpDown.Value;
            senses.Inspiration = this.InspirationCheckBox.IsChecked ?? false;

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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            var attributes = Program.CcsFile.Character.Attributes;

            this.InitiativeTextBox.Text = (attributes.Dexterity.Bonus + this.InitiativeBonusUpDown.Value).ToString();
            this.PerceptionTextBox.Text = (Constants.BasePerception + attributes.GetSkillBonus(attributes.Perception, Program.CcsFile.Character.ProficiencyBonus) + this.PerceptionBonusUpDown.Value).ToString();
            this.MovementTextBox.Text = Program.CcsFile.Character.GetMovement(this.BaseMovementUpDown.Value + this.MovementBonusUpDown.Value).ToString();
        }
    }
}
