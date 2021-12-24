// <copyright file="ModifyHealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHealthWindow.xaml.
    /// </summary>
    public partial class ModifyHealthWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyHealthWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.conciergePage = conciergePage;
        }

        private Health Health { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Health = Program.CcsFile.Character.Vitality.Health;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void EditHealth(Health health)
        {
            this.Health = health;
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
            this.CurrentHpUpDown.UpdatingValue();
            this.TemporaryHpUpDown.UpdatingValue();
            this.TotalHpUpDown.UpdatingValue();

            this.CurrentHpUpDown.Value = this.Health.BaseHealth;
            this.TemporaryHpUpDown.Value = this.Health.TemporaryHealth;
            this.TotalHpUpDown.Value = this.Health.MaxHealth;

            this.CurrentHpUpDown.Maximum = this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.Minimum = -this.TotalHpUpDown.Value;
        }

        private void UpdateHealth()
        {
            var oldItem = this.Health.DeepCopy() as Health;

            this.Health.MaxHealth = this.TotalHpUpDown.Value ?? 0;
            this.Health.BaseHealth = this.CurrentHpUpDown.Value ?? 0;
            this.Health.TemporaryHealth = this.TemporaryHpUpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<Health>(this.Health, oldItem, this.conciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHealth();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateHealth();
            this.HideConciergeWindow();

            Program.Modify();
        }

        private void TotalHpUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.TotalHpUpDown.Value < this.CurrentHpUpDown.Value)
            {
                this.CurrentHpUpDown.Value = this.TotalHpUpDown.Value;
            }

            this.CurrentHpUpDown.Maximum = this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.Minimum = -this.TotalHpUpDown.Value;
        }
    }
}
