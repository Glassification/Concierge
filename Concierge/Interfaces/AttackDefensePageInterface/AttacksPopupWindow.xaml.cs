// <copyright file="AttacksPopupWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AttackDefensePageInterface
{
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Interfaces.Components;

    /// <summary>
    /// Interaction logic for EquipmentPopupWindow.xaml.
    /// </summary>
    public partial class AttacksPopupWindow : ConciergeWindow
    {
        public AttacksPopupWindow()
        {
            this.InitializeComponent();
        }

        public override string HeaderText => "Attack Selection";

        public override PopupButtons ShowPopup()
        {
            this.ShowConciergeWindow();

            return this.ButtonPress;
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddWeapon;
            this.CloseConciergeWindow();
        }

        private void AmmoButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddAmmo;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
