// <copyright file="SpellcastingSelectionWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;

    /// <summary>
    /// Interaction logic for SpellcastingSelectionWindow.xaml.
    /// </summary>
    public partial class SpellcastingSelectionWindow : Window
    {
        public SpellcastingSelectionWindow()
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
