﻿// <copyright file="AboutConciergeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.Windows;

    using Concierge.Interfaces.Components;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for AboutConciergeWindow.xaml.
    /// </summary>
    public partial class AboutConciergeWindow : ConciergeWindow
    {
        private const char CopyrightSymbol = (char)169;

        public AboutConciergeWindow()
        {
            this.InitializeComponent();
        }

        public void ShowWindow()
        {
            this.Read();
            this.ShowConciergeWindow();
        }

        private void Read()
        {
            this.VersionField.Text = Program.AssemblyVersion;
            this.DesignerField.Text = Constants.Designer;
            this.LicenseField.Text = Constants.License;
            this.CopyrightField.Text = $"{CopyrightSymbol}{Constants.Copyright}";
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