// <copyright file="SpellcastingSelectionWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Interfaces.Components;

    /// <summary>
    /// Interaction logic for SpellcastingSelectionWindow.xaml.
    /// </summary>
    public partial class SpellcastingSelectionWindow : ConciergeWindow
    {
        public SpellcastingSelectionWindow()
        {
            this.InitializeComponent();
        }

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

        private void SpellClassButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddMagicClass;
            this.Hide();
        }

        private void SpellButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddSpell;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.Hide();
        }
    }
}
