// <copyright file="HelpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Windows;

    using Concierge.Display.Components;

    /// <summary>
    /// Interaction logic for HelpWindow.xaml.
    /// </summary>
    public partial class HelpWindow : ConciergeWindow
    {
        public HelpWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Help";

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();
            return null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }
    }
}