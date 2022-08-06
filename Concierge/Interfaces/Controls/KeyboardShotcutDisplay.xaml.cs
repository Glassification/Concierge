// <copyright file="KeyboardShotcutDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for KeyboardShotcutDisplay.xaml.
    /// </summary>
    public partial class KeyboardShotcutDisplay : UserControl
    {
        public static readonly DependencyProperty ShortcutNameProperty =
            DependencyProperty.Register(
                "ShortcutName",
                typeof(string),
                typeof(KeyboardShotcutDisplay),
                new UIPropertyMetadata("Penis As"));

        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.Register(
                "CommandName",
                typeof(string),
                typeof(KeyboardShotcutDisplay),
                new UIPropertyMetadata("Ctrl+Alt+P"));

        public KeyboardShotcutDisplay()
        {
            this.InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public string ShortcutName
        {
            get { return (string)this.GetValue(ShortcutNameProperty); }
            set { this.SetValue(ShortcutNameProperty, value); }
        }

        public string CommandName
        {
            get { return (string)this.GetValue(CommandNameProperty); }
            set { this.SetValue(CommandNameProperty, value); }
        }
    }
}
