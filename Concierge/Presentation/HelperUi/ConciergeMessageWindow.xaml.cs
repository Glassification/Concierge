// <copyright file="ConciergeMessageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.HelperUi
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Presentation.Enums;

    /// <summary>
    /// Interaction logic for ConfirmCloseWindow.xaml.
    /// </summary>
    public partial class ConciergeMessageWindow : Window
    {
        public ConciergeMessageWindow()
        {
            this.InitializeComponent();
        }

        private MessageWindowResult Result { get; set; }

        public MessageWindowResult ShowWindow(string message, MessageWindowButtons messageWindowButtons)
        {
            this.MessageText.Text = message;

            switch (messageWindowButtons)
            {
                case MessageWindowButtons.YesNoCancel:
                    this.YesButton.Visibility = Visibility.Visible;
                    this.NoButton.Visibility = Visibility.Visible;
                    this.CancelButton.Visibility = Visibility.Visible;
                    this.OkButton.Visibility = Visibility.Collapsed;
                    break;
                case MessageWindowButtons.YesNo:
                    this.YesButton.Visibility = Visibility.Visible;
                    this.NoButton.Visibility = Visibility.Visible;
                    this.CancelButton.Visibility = Visibility.Collapsed;
                    this.OkButton.Visibility = Visibility.Collapsed;
                    break;
                case MessageWindowButtons.Ok:
                    this.YesButton.Visibility = Visibility.Collapsed;
                    this.NoButton.Visibility = Visibility.Collapsed;
                    this.CancelButton.Visibility = Visibility.Collapsed;
                    this.OkButton.Visibility = Visibility.Visible;
                    break;
                case MessageWindowButtons.OkCancel:
                    this.YesButton.Visibility = Visibility.Collapsed;
                    this.NoButton.Visibility = Visibility.Collapsed;
                    this.CancelButton.Visibility = Visibility.Visible;
                    this.OkButton.Visibility = Visibility.Visible;
                    break;
            }

            this.ShowDialog();

            return this.Result;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = MessageWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Exit;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Cancel;
            this.Hide();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.No;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.OK;
            this.Hide();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MessageWindowResult.Yes;
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
