// <copyright file="ModifyHitDiceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHitDiceWindow.xaml.
    /// </summary>
    public partial class ModifyHitDiceWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyHitDiceWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.conciergePage = conciergePage;
        }

        private HitDice HitDice { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.HitDice = Program.CcsFile.Character.Vitality.HitDice;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit(HitDice hitDice)
        {
            this.HitDice = hitDice;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields()
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

        private void UpdateHitDice()
        {
            var oldItem = this.HitDice.DeepCopy() as HitDice;

            this.HitDice.TotalD6 = this.TotalD6UpDown.Value ?? 0;
            this.HitDice.TotalD8 = this.TotalD8UpDown.Value ?? 0;
            this.HitDice.TotalD10 = this.TotalD10UpDown.Value ?? 0;
            this.HitDice.TotalD12 = this.TotalD12UpDown.Value ?? 0;

            this.HitDice.SpentD6 = this.UsedD6UpDown.Value ?? 0;
            this.HitDice.SpentD8 = this.UsedD8UpDown.Value ?? 0;
            this.HitDice.SpentD10 = this.UsedD10UpDown.Value ?? 0;
            this.HitDice.SpentD12 = this.UsedD12UpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<HitDice>(this.HitDice, oldItem, this.conciergePage));
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

            this.UpdateHitDice();

            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateHitDice();
            this.Hide();
        }
    }
}
