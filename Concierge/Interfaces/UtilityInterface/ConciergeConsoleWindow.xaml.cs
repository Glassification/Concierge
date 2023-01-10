// <copyright file="ConciergeConsoleWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Console;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ConciergeConsoleWindow.xaml.
    /// </summary>
    public partial class ConciergeConsoleWindow : ConciergeWindow
    {
        public ConciergeConsoleWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.Console = new ConciergeConsole();
            this.DataContext = this.Console;
            this.HandleEnter = true;
            this.CaretChanging = false;
        }

        public override string HeaderText => "Console";

        private ConciergeConsole Console { get; set; }

        private bool CaretChanging { get; set; }

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();
            return null;
        }

        private void LimitBackspace(KeyEventArgs e)
        {
            if (this.InputBlock.CaretIndex <= ConciergeConsole.Prompt.Length)
            {
                e.Handled = true;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
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
                    this.Console.ConsoleInput = this.InputBlock.Text;
                    this.Console.Run();
                    this.InputBlock.Focus();
                    this.InputBlock.CaretIndex = this.InputBlock.Text.Length;
                    this.ConsoleScroller.ScrollToBottom();
                    break;
            }
        }

        private void InputBlock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    this.InputBlock.Text = this.Console.History.Backward();
                    this.InputBlock.CaretIndex = this.InputBlock.Text.Length;
                    break;
                case Key.Down:
                    this.InputBlock.Text = this.Console.History.Forward();
                    this.InputBlock.CaretIndex = this.InputBlock.Text.Length;
                    break;
                case Key.Back:
                    this.LimitBackspace(e);
                    break;
            }
        }

        private void InputBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.CaretChanging)
            {
                return;
            }

            this.CaretChanging = true;
            if (this.InputBlock.CaretIndex < ConciergeConsole.Prompt.Length)
            {
                this.InputBlock.CaretIndex = ConciergeConsole.Prompt.Length;
            }

            this.CaretChanging = false;
        }
    }
}
