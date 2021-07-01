// <copyright file="ProficiencyPopupWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Characters.Enums;

    /// <summary>
    /// Interaction logic for ProficiencyPopupWindow.xaml.
    /// </summary>
    public partial class ProficiencyPopupWindow : Window
    {
        public ProficiencyPopupWindow()
        {
            this.InitializeComponent();
        }

        private PopupButtons ButtonPress { get; set; }

        public PopupButtons ShowPopup()
        {
            this.ShowDialog();

            return this.ButtonPress;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.Hide();
        }

        private void WeaponButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.WeaponProficiency;
            this.Hide();
        }

        private void ArmorButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.ArmorProficiency;
            this.Hide();
        }

        private void ShieldButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.ShieldProficiency;
            this.Hide();
        }

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.ToolProficiency;
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
            }
        }
    }
}
