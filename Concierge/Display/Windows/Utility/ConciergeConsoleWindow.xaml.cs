// <copyright file="ConciergeConsoleWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Common;
    using Concierge.Console;
    using Concierge.Console.Enums;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for ConciergeConsoleWindow.xaml.
    /// </summary>
    public partial class ConciergeConsoleWindow : ConciergeWindow
    {
        private readonly ConciergeConsole console = new ();

        private bool caretChanging;

        public ConciergeConsoleWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.DataContext = this.console;
            this.console.Exited += this.ConciergeConsole_Exited;
        }

        public override string HeaderText => "Console";

        public override string WindowName => nameof(ConciergeConsoleWindow);

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();
            return null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void ConciergeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.InputBlock.Focus();
            this.InputBlock.CaretIndex = this.InputBlock.Text.Length;
        }

        private void InputBlock_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.console.ConsoleInput = this.InputBlock.Text;
                    var result = this.console.Execute();
                    this.InputBlock.Focus();
                    this.InputBlock.CaretIndex = this.InputBlock.Text.Length;
                    if (result == ResultType.Success || result == ResultType.Warning)
                    {
                        this.InvokeApplyChanges();
                    }

                    break;
            }
        }

        private void InputBlock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    this.InputBlock.Text = this.console.History.Backward();
                    this.InputBlock.CaretIndex = this.InputBlock.Text.Length;
                    break;
                case Key.Down:
                    this.InputBlock.Text = this.console.History.Forward();
                    this.InputBlock.CaretIndex = this.InputBlock.Text.Length;
                    break;
                case Key.Back:
                    e.Handled = this.InputBlock.CaretIndex <= Constants.ConsolePrompt.Length;
                    break;
            }

            this.ConsoleScroller.ScrollToBottom();
        }

        private void InputBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.caretChanging)
            {
                return;
            }

            this.caretChanging = true;
            if (this.InputBlock.CaretIndex < Constants.ConsolePrompt.Length)
            {
                this.InputBlock.CaretIndex = Constants.ConsolePrompt.Length;
            }

            this.caretChanging = false;
        }

        private void ConciergeConsole_Exited(object sender, EventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }
    }
}
