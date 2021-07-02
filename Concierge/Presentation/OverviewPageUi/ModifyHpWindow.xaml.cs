// <copyright file="ModifyHpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.OverviewPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModifyHpWindow.xaml.
    /// </summary>
    public partial class ModifyHpWindow : Window
    {
        public ModifyHpWindow()
        {
            this.InitializeComponent();
        }

        private bool IsOk { get; set; }

        public void AddHP()
        {
            this.HeaderTextBlock.Text = "Add HP";

            this.ShowDialog();

            if (this.IsOk)
            {
                Program.Character.Vitality.BaseHealth += this.HpUpDown.Value ?? 0;
                Program.Character.Vitality.BaseHealth = Math.Min(Program.Character.Vitality.BaseHealth, Program.Character.Vitality.MaxHealth);

                Program.Modified = true;
            }
        }

        public void SubtractHP()
        {
            this.HeaderTextBlock.Text = "Subract HP";

            this.ShowDialog();

            if (this.IsOk)
            {
                Program.Character.Vitality.BaseHealth -= this.HpUpDown.Value ?? 0;
                Program.Character.Vitality.BaseHealth = Math.Max(Program.Character.Vitality.BaseHealth, 0);

                Program.Modified = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.IsOk = false;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsOk = false;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsOk = false;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsOk = true;
            this.Hide();
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
