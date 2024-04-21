// <copyright file="ConciergeMessageWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for ConfirmCloseWindow.xaml.
    /// </summary>
    public partial class ConciergeMessageWindow : ConciergeWindow
    {
        private bool isOk;

        public ConciergeMessageWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.CloseOnEnter = true;
        }

        public override string HeaderText => "Concierge Message";

        public override string WindowName => nameof(ConciergeMessageWindow);

        public ConciergeResult ShowWindow(
            string message,
            string title,
            ConciergeButtons messageWindowButtons,
            ConciergeIcons messageWindowIcons)
        {
            this.isOk = messageWindowButtons.HasFlag(ConciergeButtons.Ok);
            this.MessageText.Text = message;
            this.MessageTitle.Text = title;
            this.SetMessageIcon(messageWindowIcons);
            this.SetMessageButtons(messageWindowButtons);

            SoundService.PlayWarning();

            this.ShowConciergeWindow();

            return this.Result;
        }

        protected override void ReturnAndClose()
        {
            this.Result = this.isOk ? ConciergeResult.OK : ConciergeResult.Yes;
            this.CloseConciergeWindow();
        }

        private void SetMessageIcon(ConciergeIcons messageWindowIcons)
        {
            this.MessageIcon.Visibility = Visibility.Visible;

            switch (messageWindowIcons)
            {
                case ConciergeIcons.Alert:
                    this.MessageIcon.Kind = PackIconKind.Alert;
                    this.MessageIcon.Foreground = Brushes.Gold;
                    break;
                case ConciergeIcons.Error:
                    this.MessageIcon.Kind = PackIconKind.AlertOctagon;
                    this.MessageIcon.Foreground = Brushes.DarkRed;
                    break;
                case ConciergeIcons.Help:
                    this.MessageIcon.Kind = PackIconKind.HelpCircle;
                    this.MessageIcon.Foreground = Brushes.DodgerBlue;
                    break;
                case ConciergeIcons.Information:
                    this.MessageIcon.Kind = PackIconKind.Information;
                    this.MessageIcon.Foreground = Brushes.DodgerBlue;
                    break;
                case ConciergeIcons.Question:
                    this.MessageIcon.Kind = PackIconKind.QuestionMarkCircle;
                    this.MessageIcon.Foreground = Brushes.DodgerBlue;
                    break;
                case ConciergeIcons.Warning:
                    this.MessageIcon.Kind = PackIconKind.WarningCircle;
                    this.MessageIcon.Foreground = Brushes.Gold;
                    break;
                case ConciergeIcons.None:
                default:
                    this.MessageIcon.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void SetMessageButtons(ConciergeButtons buttons)
        {
            this.OkButton.Visibility = buttons.HasFlagVisibility(ConciergeButtons.Ok);
            this.YesButton.Visibility = buttons.HasFlagVisibility(ConciergeButtons.Yes);
            this.NoButton.Visibility = buttons.HasFlagVisibility(ConciergeButtons.No);
            this.ApplyButton.Visibility = buttons.HasFlagVisibility(ConciergeButtons.Apply);
            this.CancelButton.Visibility = buttons.HasFlagVisibility(ConciergeButtons.Cancel);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.No;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Apply;
            this.CloseConciergeWindow();
        }
    }
}
