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
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHealthWindow.xaml.
    /// </summary>
    public partial class ModifyHealthWindow : Window, IConciergeWindow
    {
        public ModifyHealthWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Vitality Vitality { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Vitality = Program.CcsFile.Character.Vitality;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void EditHealth(Vitality vitality)
        {
            this.Vitality = vitality;
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
            this.CurrentHpUpDown.UpdatingValue();
            this.TemporaryHpUpDown.UpdatingValue();
            this.TotalHpUpDown.UpdatingValue();

            this.CurrentHpUpDown.Value = this.Vitality.BaseHealth;
            this.TemporaryHpUpDown.Value = this.Vitality.TemporaryHealth;
            this.TotalHpUpDown.Value = this.Vitality.MaxHealth;

            this.CurrentHpUpDown.Maximum = this.TotalHpUpDown.Value;
            this.CurrentHpUpDown.Minimum = -this.TotalHpUpDown.Value;
        }

        private void UpdateHealth()
        {
            this.Vitality.MaxHealth = this.TotalHpUpDown.Value ?? 0;
            this.Vitality.BaseHealth = this.CurrentHpUpDown.Value ?? 0;
            this.Vitality.TemporaryHealth = this.TemporaryHpUpDown.Value ?? 0;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateHealth();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateHealth();
            this.Hide();
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
