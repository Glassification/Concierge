// <copyright file="ConciergeMessageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for ConfirmCloseWindow.xaml.
    /// </summary>
    public partial class ConciergeMessageWindow : ConciergeWindow
    {
        public ConciergeMessageWindow()
        {
            this.InitializeComponent();
        }

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

            this.ShowConciergeWindow();

            return this.Result;
        }

        private void SetMessageIcon(ConciergeWindowIcons messageWindowIcons)
        {
            this.MessageIcon.Visibility = Visibility.Visible;

            switch (messageWindowIcons)
            {
                case ConciergeWindowIcons.Alert:
                    this.MessageIcon.Kind = PackIconKind.Alert;
                    this.MessageIcon.Foreground = Brushes.Gold;
                    break;
                case ConciergeWindowIcons.Error:
                    this.MessageIcon.Kind = PackIconKind.AlertOctagon;
                    this.MessageIcon.Foreground = Brushes.DarkRed;
                    break;
                case ConciergeWindowIcons.Help:
                    this.MessageIcon.Kind = PackIconKind.HelpCircle;
                    this.MessageIcon.Foreground = Brushes.DodgerBlue;
                    break;
                case ConciergeWindowIcons.Information:
                    this.MessageIcon.Kind = PackIconKind.Information;
                    this.MessageIcon.Foreground = Brushes.DodgerBlue;
                    break;
                case ConciergeWindowIcons.Question:
                    this.MessageIcon.Kind = PackIconKind.QuestionMarkCircle;
                    this.MessageIcon.Foreground = Brushes.DodgerBlue;
                    break;
                case ConciergeWindowIcons.Warning:
                    this.MessageIcon.Kind = PackIconKind.WarningCircle;
                    this.MessageIcon.Foreground = Brushes.Gold;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.No;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;
            this.HideConciergeWindow();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Yes;
            this.HideConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Apply;
            this.HideConciergeWindow();
        }
    }
}
