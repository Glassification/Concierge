// <copyright file="HelpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.Windows;

    using Concierge.Interfaces.Components;

    /// <summary>
    /// Interaction logic for HelpWindow.xaml.
    /// </summary>
    public partial class HelpWindow : ConciergeWindow
    {
        public HelpWindow()
        {
            this.InitializeComponent();
        }

        public override void ShowWindow()
        {
            this.ShowConciergeWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.HideConciergeWindow();
        }
    }
}