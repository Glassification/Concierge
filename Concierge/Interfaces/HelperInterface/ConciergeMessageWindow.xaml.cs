// <copyright file="ConciergeMessageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.HelperInterface
{
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for ConfirmCloseWindow.xaml.
    /// </summary>
    public partial class ConciergeMessageWindow : Window
    {
        public ConciergeMessageWindow()
        {
            this.InitializeComponent();
        }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWindow(
            string message,
            string title,
            ConciergeWindowButtons messageWindowButtons,
            ConciergeWindowIcons messageWindowIcons)
        {
            this.MessageText.Text = message;
            this.MessageTitle.Text = title;
            this.SetMessageIcon(messageWindowIcons);
            this.SetMessageButtons(messageWindowButtons);

            ConciergeSound.Warning();

            this.ShowDialog();

            return this.Result;
        }

        private void SetMessageIcon(ConciergeWindowIcons messageWindowIcons)
        {
            this.MessageIcon.Visibility = Visibility.Visible;

            switch (messageWindowIcons)
            {
                case ConciergeWindowIcons.Alert:
                    this.MessageIcon.Kind = PackIconKind.Alert;
                    break;
                case ConciergeWindowIcons.Error:
                    this.MessageIcon.Kind = PackIconKind.Error;
                    break;
                case ConciergeWindowIcons.Help:
                    this.MessageIcon.Kind = PackIconKind.HelpCircle;
                    break;
                case ConciergeWindowIcons.Information:
                    this.MessageIcon.Kind = PackIconKind.Information;
                    break;
                case ConciergeWindowIcons.Question:
                    this.MessageIcon.Kind = PackIconKind.QuestionMarkCircle;
                    break;
                case ConciergeWindowIcons.Warning:
                    this.MessageIcon.Kind = PackIconKind.WarningCircle;
                    break;
                case ConciergeWindowIcons.None:
                default:
                    this.MessageIcon.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void SetMessageButtons(ConciergeWindowButtons messageWindowButtons)
        {
            switch (messageWindowButtons)
            {
                case ConciergeWindowButtons.YesNoCancel:
                    this.CollapseAllButtons();
                    this.YesButton.Visibility = Visibility.Visible;
                    this.NoButton.Visibility = Visibility.Visible;
                    this.CancelButton.Visibility = Visibility.Visible;
                    break;
                case ConciergeWindowButtons.YesNo:
                    this.CollapseAllButtons();
                    this.YesButton.Visibility = Visibility.Visible;
                    this.NoButton.Visibility = Visibility.Visible;
                    break;
                case ConciergeWindowButtons.Ok:
                    this.CollapseAllButtons();
                    this.OkButton.Visibility = Visibility.Visible;
                    break;
                case ConciergeWindowButtons.OkCancel:
                    this.CollapseAllButtons();
                    this.OkButton.Visibility = Visibility.Visible;
                    this.CancelButton.Visibility = Visibility.Visible;
                    break;
                case ConciergeWindowButtons.OkApply:
                    this.CollapseAllButtons();
                    this.OkButton.Visibility = Visibility.Visible;
                    this.ApplyButton.Visibility = Visibility.Visible;
                    break;
                case ConciergeWindowButtons.OkApplyCancel:
                    this.CollapseAllButtons();
                    this.OkButton.Visibility = Visibility.Visible;
                    this.ApplyButton.Visibility = Visibility.Visible;
                    this.CancelButton.Visibility = Visibility.Visible;
                    break;
                default:
                    this.CollapseAllButtons();
                    break;
            }
        }

        private void CollapseAllButtons()
        {
            this.YesButton.Visibility = Visibility.Collapsed;
            this.NoButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.ApplyButton.Visibility = Visibility.Collapsed;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.No;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;
            this.Hide();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Yes;
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Apply;
            this.Hide();
        }
    }
}
