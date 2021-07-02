// <copyright file="ModifyHealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.OverviewPageUi
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModifyHealthWindow.xaml.
    /// </summary>
    public partial class ModifyHealthWindow : Window
    {
        public ModifyHealthWindow()
        {
            this.InitializeComponent();
        }

        public void EditHealth()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.CurrentHpUpDown.Value = Program.Character.Vitality.BaseHealth;
            this.TemporaryHpUpDown.Value = Program.Character.Vitality.TemporaryHealth;
            this.TotalHpUpDown.Value = Program.Character.Vitality.MaxHealth;
        }

        private void UpdateHealth()
        {
            Program.Character.Vitality.BaseHealth = this.CurrentHpUpDown.Value ?? 0;
            Program.Character.Vitality.TemporaryHealth = this.TemporaryHpUpDown.Value ?? 0;
            Program.Character.Vitality.MaxHealth = this.TotalHpUpDown.Value ?? 0;

            Program.Modified = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHealth();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHealth();
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
