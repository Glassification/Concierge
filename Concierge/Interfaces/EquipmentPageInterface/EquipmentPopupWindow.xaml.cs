// <copyright file="EquipmentPopupWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquipmentPageInterface
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private void WeaponButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddWeapon;
            this.Hide();
        }

        private void AmmoButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddAmmo;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
