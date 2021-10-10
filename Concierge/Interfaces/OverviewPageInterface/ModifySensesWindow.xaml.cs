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
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySensesWindow.xaml.
    /// </summary>
    public partial class ModifySensesWindow : Window, IConciergeModifyWindow
    {
        public ModifySensesWindow()
        {
            this.InitializeComponent();
            this.VisionComboBox.ItemsSource = Enum.GetValues(typeof(VisionTypes)).Cast<VisionTypes>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.FillFields();
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.ShowDialog();

            return this.Result;
        }

        public void EditSenses()
        {
            this.ApplyButton.Visibility = Visibility.Visible;
            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void FillFields()
        {
            this.InitiativeBonusUpDown.UpdatingValue();
            this.PerceptionBonusUpDown.UpdatingValue();
            this.BaseMovementUpDown.UpdatingValue();

            this.InitiativeTextBlock.Text = Program.CcsFile.Character.Initiative.ToString();
            this.InitiativeBonusUpDown.Value = Program.CcsFile.Character.Details.InitiativeBonus;
            this.PerceptionTextBlock.Text = Program.CcsFile.Character.PassivePerception.ToString();
            this.PerceptionBonusUpDown.Value = Program.CcsFile.Character.Details.PerceptionBonus;
            this.VisionComboBox.Text = Program.CcsFile.Character.Details.Vision.ToString();
            this.MovementTextBlock.Text = Program.CcsFile.Character.Details.Movement.ToString();
            this.BaseMovementUpDown.Value = Program.CcsFile.Character.Details.BaseMovement;
        }

        private void UpdateSenses()
        {
            Program.CcsFile.Character.Details.InitiativeBonus = this.InitiativeBonusUpDown.Value ?? 0;
            Program.CcsFile.Character.Details.PerceptionBonus = this.PerceptionBonusUpDown.Value ?? 0;
            Program.CcsFile.Character.Details.Vision = (VisionTypes)Enum.Parse(typeof(VisionTypes), this.VisionComboBox.Text);
            Program.CcsFile.Character.Details.BaseMovement = this.BaseMovementUpDown.Value ?? 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateSenses();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateSenses();
            this.FillFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.InitiativeTextBlock.Text = (Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity) + (this.InitiativeBonusUpDown.Value ?? 0)).ToString();
            this.PerceptionTextBlock.Text = (Constants.BasePerception + Program.CcsFile.Character.Skill.Perception.Bonus + (this.PerceptionBonusUpDown.Value ?? 0)).ToString();
            this.MovementTextBlock.Text = Details.GetMovement(this.BaseMovementUpDown.Value ?? 0).ToString();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
