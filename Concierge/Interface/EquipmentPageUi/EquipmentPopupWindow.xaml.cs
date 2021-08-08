// <copyright file="EquipmentPopupWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.EquipmentPageUi
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquipmentPopupWindow.xaml.
    /// </summary>
    public partial class EquipmentPopupWindow : Window
    {
        public EquipmentPopupWindow()
        {
            this.InitializeComponent();
        }

        private PopupButtons ButtonPress { get; set; }

        public PopupButtons ShowPopup()
        {
            this.ShowDialog();

            return this.ButtonPress;
        }

        private void WeaponButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.ButtonPress = PopupButtons.AddWeapon;
            this.Hide();
        }

        private void AmmoButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.ButtonPress = PopupButtons.AddAmmo;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.ButtonPress = PopupButtons.Cancel;
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.ButtonPress = PopupButtons.Cancel;
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.ButtonPress = PopupButtons.Cancel;
                    this.Hide();
                    break;
                default:
                    break;
            }
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
