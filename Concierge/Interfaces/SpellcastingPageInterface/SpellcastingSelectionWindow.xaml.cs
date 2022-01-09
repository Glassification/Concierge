// <copyright file="SpellcastingSelectionWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System.Windows;

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

        public override PopupButtons ShowPopup()
        {
            this.ShowConciergeWindow();

            return this.ButtonPress;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.HideConciergeWindow();
        }

        private void SpellClassButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddMagicClass;
            this.HideConciergeWindow();
        }

        private void SpellButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddSpell;
            this.HideConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.HideConciergeWindow();
        }
    }
}
