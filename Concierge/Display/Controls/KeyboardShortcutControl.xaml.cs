// <copyright file="KeyboardShortcutControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for KeyboardShortcutControl.xaml.
    /// </summary>
    public partial class KeyboardShortcutControl : UserControl
    {
        public static readonly DependencyProperty ShortcutNameProperty =
            DependencyProperty.Register(
                "ShortcutName",
                typeof(string),
                typeof(KeyboardShortcutControl),
                new UIPropertyMetadata("Penis As"));

        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.Register(
                "CommandName",
                typeof(string),
                typeof(KeyboardShortcutControl),
                new UIPropertyMetadata("Ctrl+Alt+P"));

        public KeyboardShortcutControl()
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
