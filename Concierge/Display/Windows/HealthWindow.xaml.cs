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
        private Health health = new ();

        public HealthWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.TotalHpUpDown);
            this.SetMouseOverEvents(this.CurrentHpUpDown);
            this.SetMouseOverEvents(this.TemporaryHpUpDown);
        }

        public override string HeaderText => "Edit Health";

        public override string WindowName => nameof(HealthWindow);

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.health = Program.CcsFile.Character.Vitality.Health;
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

            this.health = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            this.UpdateHealth();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            Program.Drawing();

            this.TotalHpUpDown.Value = this.health.MaxHealth;
            this.CurrentHpUpDown.Value = this.health.BaseHealth;
            this.TemporaryHpUpDown.Value = this.health.TemporaryHealth;

            this.CurrentHpUpDown.Maximum = this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.Minimum = -this.TotalHpUpDown.Value;

            Program.NotDrawing();
        }

        private void UpdateHealth()
        {
            var oldItem = this.health.DeepCopy();

            this.health.MaxHealth = this.TotalHpUpDown.Value;
            this.health.BaseHealth = this.CurrentHpUpDown.Value;
            this.health.TemporaryHealth = this.TemporaryHpUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<Health>(this.health, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHealth();
            this.InvokeApplyChanges();
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
