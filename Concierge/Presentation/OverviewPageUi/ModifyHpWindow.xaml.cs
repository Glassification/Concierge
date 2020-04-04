using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Concierge.Presentation.OverviewPageUi
{
    /// <summary>
    /// Interaction logic for ModifyHpWindow.xaml
    /// </summary>
    public partial class ModifyHpWindow : Window
    {
        public ModifyHpWindow()
        {
            InitializeComponent();
        }

        public void AddHP()
        {
            HeaderTextBlock.Text = "Add HP";

            ShowDialog();

            if (IsOk)
            {
                Program.Character.Vitality.BaseHealth += HpUpDown.Value ?? 0;
                Program.Character.Vitality.BaseHealth = Math.Min(Program.Character.Vitality.BaseHealth, Program.Character.Vitality.MaxHealth);
            }
        }

        public void SubtractHP()
        {
            HeaderTextBlock.Text = "Subract HP";

            ShowDialog();

            if (IsOk)
            {
                Program.Character.Vitality.BaseHealth -= HpUpDown.Value ?? 0;
                Program.Character.Vitality.BaseHealth = Math.Max(Program.Character.Vitality.BaseHealth, 0);
            }
        }

        private bool IsOk { get; set; }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    IsOk = false;
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsOk = false;
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsOk = false;
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            IsOk = true;
            Hide();
        }
    }
}
