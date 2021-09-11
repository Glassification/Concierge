// <copyright file="ModifyHpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
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
        }

        private ConciergeWindowResult Result { get; set; }

        public void AddHP(Vitality vitality)
        {
            this.HeaderTextBlock.Text = "Heal";
            this.HpUpDown.Value = 0;

            this.ShowDialog();

            if (this.Result == ConciergeWindowResult.OK)
            {
                vitality.Heal(this.HpUpDown.Value ?? 0);
            }
        }

        public void SubtractHP(Vitality vitality)
        {
            this.HeaderTextBlock.Text = "Damage";
            this.HpUpDown.Value = 0;

            this.ShowDialog();

            if (this.Result == ConciergeWindowResult.OK)
            {
                vitality.Damage(this.HpUpDown.Value ?? 0);
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
    }
}
