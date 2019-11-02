using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.Popups
{
    /// <summary>
    /// Interaction logic for EquipmentPopupWindow.xaml
    /// </summary>
    public partial class EquipmentPopupWindow : Window
    {
        public EquipmentPopupWindow()
        {
            InitializeComponent();
        }

        public Constants.PopupButons ShowPopup()
        {
            ShowDialog();

            return ButtonPress;
        }

        private Constants.PopupButons ButtonPress { get; set; }

        private void WeaponButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButons.AddWeapon;
            Hide();
        }

        private void AmmoButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButons.AddAmmo;
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButons.Cancel;
            Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonPress = Constants.PopupButons.Cancel;
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
