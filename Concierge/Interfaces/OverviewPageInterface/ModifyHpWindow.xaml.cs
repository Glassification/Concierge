// <copyright file="ModifyHpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Statuses;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHpWindow.xaml.
    /// </summary>
    public partial class ModifyHpWindow : Window
    {
        public ModifyHpWindow()
        {
            this.InitializeComponent();
            this.PreviousHeal = 0;
            this.PreviousDamage = 0;
        }

        private string HeaderText => this.IsHealing ? "Heal" : "Damage";

        private ConciergeWindowResult Result { get; set; }

        private int PreviousHeal { get; set; }

        private int PreviousDamage { get; set; }

        private bool IsHealing { get; set; }

        public void ShowHeal(Vitality vitality)
        {
            this.IsHealing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.HpUpDown.Value = this.PreviousHeal;

            this.ShowDialog();
            this.SetPreviousValue();

            if (this.Result == ConciergeWindowResult.OK)
            {
                vitality.Heal(this.HpUpDown.Value ?? 0);
            }
        }

        public void ShowDamage(Vitality vitality)
        {
            this.IsHealing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.HpUpDown.Value = this.PreviousDamage;

            this.ShowDialog();
            this.SetPreviousValue();

            if (this.Result == ConciergeWindowResult.OK)
            {
                vitality.Damage(this.HpUpDown.Value ?? 0);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void SetPreviousValue()
        {
            if (this.IsHealing)
            {
                this.PreviousHeal = this.HpUpDown.Value ?? 0;
            }
            else
            {
                this.PreviousDamage = this.HpUpDown.Value ?? 0;
            }
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Result = ConciergeWindowResult.OK;
            this.Hide();
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
