// <copyright file="ModifyHitDiceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Statuses;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHitDiceWindow.xaml.
    /// </summary>
    public partial class ModifyHitDiceWindow : Window, IConciergeWindow
    {
        public ModifyHitDiceWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private HitDice HitDice { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.HitDice = Program.CcsFile.Character.Vitality.HitDice;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.SetHitDice();
            this.ShowDialog();

            return this.Result;
        }

        public void ModifyHitDice(HitDice hitDice)
        {
            this.HitDice = hitDice;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.SetHitDice();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void SetHitDice()
        {
            this.TotalD6UpDown.UpdatingValue();
            this.TotalD8UpDown.UpdatingValue();
            this.TotalD10UpDown.UpdatingValue();
            this.TotalD12UpDown.UpdatingValue();
            this.UsedD6UpDown.UpdatingValue();
            this.UsedD8UpDown.UpdatingValue();
            this.UsedD10UpDown.UpdatingValue();
            this.UsedD12UpDown.UpdatingValue();

            this.TotalD6UpDown.Value = this.HitDice.TotalD6;
            this.TotalD8UpDown.Value = this.HitDice.TotalD8;
            this.TotalD10UpDown.Value = this.HitDice.TotalD10;
            this.TotalD12UpDown.Value = this.HitDice.TotalD12;

            this.UsedD6UpDown.Value = this.HitDice.SpentD6;
            this.UsedD8UpDown.Value = this.HitDice.SpentD8;
            this.UsedD10UpDown.Value = this.HitDice.SpentD10;
            this.UsedD12UpDown.Value = this.HitDice.SpentD12;
        }

        private void GetHitDice()
        {
            this.HitDice.TotalD6 = this.TotalD6UpDown.Value ?? 0;
            this.HitDice.TotalD8 = this.TotalD8UpDown.Value ?? 0;
            this.HitDice.TotalD10 = this.TotalD10UpDown.Value ?? 0;
            this.HitDice.TotalD12 = this.TotalD12UpDown.Value ?? 0;

            this.HitDice.SpentD6 = this.UsedD6UpDown.Value ?? 0;
            this.HitDice.SpentD8 = this.UsedD8UpDown.Value ?? 0;
            this.HitDice.SpentD10 = this.UsedD10UpDown.Value ?? 0;
            this.HitDice.SpentD12 = this.UsedD12UpDown.Value ?? 0;
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

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.GetHitDice();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.GetHitDice();
            this.Hide();
        }
    }
}
