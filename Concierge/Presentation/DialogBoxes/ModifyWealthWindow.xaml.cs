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

namespace Concierge.Presentation.DialogBoxes
{
    /// <summary>
    /// Interaction logic for ModifyWealthWindow.xaml
    /// </summary>
    public partial class ModifyWealthWindow : Window
    {
        public ModifyWealthWindow()
        {
            InitializeComponent();
        }

        public void ShowWindow()
        {
            ClearFields();

            CP = Program.Character.Wealth.Copper;
            SP = Program.Character.Wealth.Silver;
            EP = Program.Character.Wealth.Electrum;
            GP = Program.Character.Wealth.Gold;
            PP = Program.Character.Wealth.Platinum;

            FillFields();

            ShowDialog();
        }

        private void ClearFields()
        {
            AddRadioButton.IsChecked = true;
            CpRadioButton.IsChecked = true;
            AmountUpDown.Value = 0;
        }

        private void FillFields()
        {
            CopperField.Text = CP.ToString();
            SilverField.Text = SP.ToString();
            ElectrumField.Text = EP.ToString();
            GoldField.Text = GP.ToString();
            PlatinumField.Text = PP.ToString();
        }

        private int GetAmount()
        {
            if (AddRadioButton.IsChecked ?? false)
            {
                return AmountUpDown.Value ?? 0;
            }
            else if (SubtractRadioButton.IsChecked ?? false)
            {
                return (AmountUpDown.Value ?? 0) * -1;
            }
            else
            {
                return 0;
            }
        }

        private int CP { get; set; }
        private int SP { get; set; }
        private int EP { get; set; }
        private int GP { get; set; }
        private int PP { get; set; }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Wealth.Copper = CP;
            Program.Character.Wealth.Silver = SP;
            Program.Character.Wealth.Electrum = EP;
            Program.Character.Wealth.Gold = GP;
            Program.Character.Wealth.Platinum = PP;

            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CpRadioButton.IsChecked ?? false)
            {
                CP += GetAmount();
            }
            else if (SpRadioButton.IsChecked ?? false)
            {
                SP += GetAmount();
            }
            else if (EpRadioButton.IsChecked ?? false)
            {
                EP += GetAmount();
            }
            else if (GpRadioButton.IsChecked ?? false)
            {
                GP += GetAmount();
            }
            else if (PpRadioButton.IsChecked ?? false)
            {
                PP += GetAmount();
            }

            FillFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
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
    }
}
