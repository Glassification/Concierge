// <copyright file="HealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for HealthWindow.xaml.
    /// </summary>
    public partial class HealthWindow : ConciergeWindow
    {
        public HealthWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.Health = new Health();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.TotalHpUpDown);
            this.SetFocusEvents(this.CurrentHpUpDown);
            this.SetFocusEvents(this.TemporaryHpUpDown);
        }

        public override string HeaderText => "Edit Health";

        public override string WindowName => nameof(HealthWindow);

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
            this.CurrentHpUpDown.Maximum = this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.Minimum = -this.TotalHpUpDown.Value;

            if (this.TotalHpUpDown.Delta > 0 && this.TotalHpUpDown.Value - 1 == this.CurrentHpUpDown.Value)
            {
                this.CurrentHpUpDown.Value = this.TotalHpUpDown.Value;
            }
        }
    }
}
