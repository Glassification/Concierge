// <copyright file="AboutConciergeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.Windows;

    using Concierge.Interfaces.Components;
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
            this.ForceRoundedCorners();
        }

        public override string HeaderText => "About Concierge";

        public override void ShowWindow()
        {
            this.Read();
            this.ShowConciergeWindow();
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
