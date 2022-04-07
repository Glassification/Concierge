// <copyright file="ModifyHealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System.Windows;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHealthWindow.xaml.
    /// </summary>
    public partial class ModifyHealthWindow : ConciergeWindow
    {
        public ModifyHealthWindow()
        {
            this.InitializeComponent();
            this.ConciergePage = ConciergePage.None;
            this.Health = new Health();
        }

        public override string HeaderText => "Edit Health";

        private Health Health { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Health = Program.CcsFile.Character.Vitality.Health;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T health)
        {
            if (health is not Health castItem)
            {
                return;
            }

            this.Health = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateHealth();
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields()
        {
            this.CurrentHpUpDown.Value = this.Health.BaseHealth;
            this.TemporaryHpUpDown.Value = this.Health.TemporaryHealth;
            this.TotalHpUpDown.Value = this.Health.MaxHealth;

            this.CurrentHpUpDown.Maximum = this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.Minimum = -this.TotalHpUpDown.Value;

            this.CurrentHpUpDown.UpdateSpinnerStatus();
        }

        private void UpdateHealth()
        {
            var oldItem = this.Health.DeepCopy();

            this.Health.MaxHealth = this.TotalHpUpDown.Value;
            this.Health.BaseHealth = this.CurrentHpUpDown.Value;
            this.Health.TemporaryHealth = this.TemporaryHpUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<Health>(this.Health, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHealth();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void TotalHpUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (this.TotalHpUpDown.Value < this.CurrentHpUpDown.Value)
            {
                this.CurrentHpUpDown.Value = this.TotalHpUpDown.Value;
            }

            this.CurrentHpUpDown.Maximum = this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.Minimum = -this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.UpdateSpinnerStatus();
        }
    }
}
