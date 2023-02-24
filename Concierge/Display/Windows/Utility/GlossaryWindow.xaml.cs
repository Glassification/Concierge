// <copyright file="GlossaryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;

    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for GlossaryWindow.xaml.
    /// </summary>
    public partial class GlossaryWindow : ConciergeWindow
    {
        public GlossaryWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Glossary";

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();
            return null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }
    }
}
