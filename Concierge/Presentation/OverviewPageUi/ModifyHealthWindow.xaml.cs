using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.OverviewPageUi
{
    /// <summary>
    /// Interaction logic for ModifyHealthWindow.xaml
    /// </summary>
    public partial class ModifyHealthWindow : Window
    {
        public ModifyHealthWindow()
        {
            InitializeComponent();
        }

        public void EditHealth()
        {
            FillFields();
            ShowDialog();
        }

        private void FillFields()
        {
            CurrentHpUpDown.Value = Program.Character.Vitality.BaseHealth;
            TemporaryHpUpDown.Value = Program.Character.Vitality.TemporaryHealth;
            TotalHpUpDown.Value = Program.Character.Vitality.MaxHealth;
        }

        private void UpdateHealth()
        {
            Program.Character.Vitality.BaseHealth = CurrentHpUpDown.Value ?? 0;
            Program.Character.Vitality.TemporaryHealth = TemporaryHpUpDown.Value ?? 0;
            Program.Character.Vitality.MaxHealth = TotalHpUpDown.Value ?? 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateHealth();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateHealth();
            Hide();
        }
    }
}
