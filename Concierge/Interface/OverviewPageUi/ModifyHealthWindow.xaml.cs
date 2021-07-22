// <copyright file="ModifyHealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.OverviewPageUi
{
    using System.Windows;
    using System.Windows.Controls;
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
            this.CurrentHpUpDown.Value = Program.CcsFile.Character.Vitality.BaseHealth;
            this.TemporaryHpUpDown.Value = Program.CcsFile.Character.Vitality.TemporaryHealth;
            this.TotalHpUpDown.Value = Program.CcsFile.Character.Vitality.MaxHealth;
        }

        private void UpdateHealth()
        {
            Program.CcsFile.Character.Vitality.BaseHealth = this.CurrentHpUpDown.Value ?? 0;
            Program.CcsFile.Character.Vitality.TemporaryHealth = this.TemporaryHpUpDown.Value ?? 0;
            Program.CcsFile.Character.Vitality.MaxHealth = this.TotalHpUpDown.Value ?? 0;
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
            Program.Modify();

            this.UpdateHealth();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateHealth();
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
