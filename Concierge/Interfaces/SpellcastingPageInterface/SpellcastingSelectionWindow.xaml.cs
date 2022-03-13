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

        public override string HeaderText => "Spellcasting Selection";

        public override PopupButtons ShowPopup()
        {
            this.ShowConciergeWindow();

            return this.ButtonPress;
        }

        protected override void ReturnAndClose()
        {
            this.ButtonPress = PopupButtons.AddSpell;
            this.CloseConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.CloseConciergeWindow();
        }

        private void SpellClassButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.AddMagicClass;
            this.CloseConciergeWindow();
        }

        private void SpellButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPress = PopupButtons.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
