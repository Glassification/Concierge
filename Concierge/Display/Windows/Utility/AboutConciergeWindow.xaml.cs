﻿// <copyright file="AboutConciergeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Windows;

    using Concierge.Display.Components;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for AboutConciergeWindow.xaml.
    /// </summary>
    public partial class AboutConciergeWindow : ConciergeWindow
    {
        private const char CopyrightSymbol = (char)169;

        public AboutConciergeWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "About Concierge";

        public override object? ShowWindow()
        {
            this.Read();
            this.ShowConciergeWindow();

            return null;
        }

        private void Read()
        {
            this.VersionField.Text = $"{Program.AssemblyVersion}{(Program.IsDebug ? " - Debug" : string.Empty)}";
            this.DesignerField.Text = Constants.Designer;
            this.LicenseField.Text = Constants.License;
            this.CopyrightField.Text = Program.IsDebug ? GitUtility.BranchName : $"{CopyrightSymbol}{Constants.Copyright}";
            this.CopyrightLabel.Text = Program.IsDebug ? "Git Branch:" : "Copyright:";
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