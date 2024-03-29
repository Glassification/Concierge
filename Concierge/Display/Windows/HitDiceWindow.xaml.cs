﻿// <copyright file="HitDiceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for HitDiceWindow.xaml.
    /// </summary>
    public partial class HitDiceWindow : ConciergeWindow
    {
        public HitDiceWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.HitDice = new HitDice();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.UsedD6UpDown);
            this.SetMouseOverEvents(this.UsedD8UpDown);
            this.SetMouseOverEvents(this.UsedD10UpDown);
            this.SetMouseOverEvents(this.UsedD12UpDown);
            this.SetMouseOverEvents(this.TotalD6UpDown);
            this.SetMouseOverEvents(this.TotalD8UpDown);
            this.SetMouseOverEvents(this.TotalD10UpDown);
            this.SetMouseOverEvents(this.TotalD12UpDown);
        }

        public override string HeaderText => "Edit Hit Dice";

        public override string WindowName => nameof(HitDiceWindow);

        private HitDice HitDice { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.HitDice = Program.CcsFile.Character.Vitality.HitDice;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.SetUsedLimit();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T hitDice)
        {
            if (hitDice is not HitDice castItem)
            {
                return;
            }

            this.HitDice = castItem;
            this.FillFields();
            this.SetUsedLimit();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            this.UpdateHitDice();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
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
            var oldItem = this.HitDice.DeepCopy();

            this.HitDice.TotalD6 = this.TotalD6UpDown.Value;
            this.HitDice.TotalD8 = this.TotalD8UpDown.Value;
            this.HitDice.TotalD10 = this.TotalD10UpDown.Value;
            this.HitDice.TotalD12 = this.TotalD12UpDown.Value;

            this.HitDice.SpentD6 = this.UsedD6UpDown.Value;
            this.HitDice.SpentD8 = this.UsedD8UpDown.Value;
            this.HitDice.SpentD10 = this.UsedD10UpDown.Value;
            this.HitDice.SpentD12 = this.UsedD12UpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<HitDice>(this.HitDice, oldItem, this.ConciergePage));
        }

        private void SetUsedLimit()
        {
            this.UsedD6UpDown.Maximum = this.TotalD6UpDown.Value;
            this.UsedD8UpDown.Maximum = this.TotalD8UpDown.Value;
            this.UsedD10UpDown.Maximum = this.TotalD10UpDown.Value;
            this.UsedD12UpDown.Maximum = this.TotalD12UpDown.Value;
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

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHitDice();
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void TotalUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.SetUsedLimit();
        }
    }
}
