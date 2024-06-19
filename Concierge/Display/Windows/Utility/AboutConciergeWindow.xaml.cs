// <copyright file="AboutConciergeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.Windows;

    using Concierge.Common;
    using Concierge.Display.Components;

    /// <summary>
    /// Interaction logic for AboutConciergeWindow.xaml.
    /// </summary>
    public partial class AboutConciergeWindow : ConciergeWindow
    {
        private const char CopyrightSymbol = (char)169; // nice
        private const int VersionOffset = 4;

        public AboutConciergeWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.CloseOnEnter = true;
        }

        public override string HeaderText => "About Concierge";

        public override string WindowName => nameof(AboutConciergeWindow);

        public override object? ShowWindow()
        {
            this.FillFields();
            this.ShowConciergeWindow();

            return null;
        }

        private void FillFields()
        {
            this.VersionField.Text = $"{Program.AssemblyVersion}{(Program.IsDebug ? " - Debug" : string.Empty)}";
            this.DotNetField.Text = $"{Environment.Version} - C# {Environment.Version.Major + VersionOffset}";
            this.DesignerField.Text = Constants.Designer;
            this.LicenseField.Text = Constants.License;
            this.CopyrightField.Text = $"{CopyrightSymbol}{Constants.Copyright}";
            this.CopyrightLabel.Text = "Copyright:";
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
