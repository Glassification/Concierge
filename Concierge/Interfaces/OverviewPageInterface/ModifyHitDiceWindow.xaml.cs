// <copyright file="ModifyHitDiceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System.Windows;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyHitDiceWindow.xaml.
    /// </summary>
    public partial class ModifyHitDiceWindow : ConciergeWindow
    {
        public ModifyHitDiceWindow()
        {
            this.InitializeComponent();
            this.ForceRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.HitDice = new HitDice();
        }

        public override string HeaderText => "Edit Hit Dice";

        private HitDice HitDice { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
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
            this.Result = ConciergeWindowResult.OK;

            this.UpdateHitDice();
            this.CloseConciergeWindow();

            Program.Modify();
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
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHitDice();
            this.InvokeApplyChanges();

            Program.Modify();
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
