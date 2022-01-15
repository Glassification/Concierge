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
            this.ConciergePage = ConciergePage.None;
        }

        private HitDice HitDice { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.HitDice = Program.CcsFile.Character.Vitality.HitDice;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T hitDice)
        {
            var castItem = hitDice as HitDice;
            this.HitDice = castItem;

            this.FillFields();
            this.ShowConciergeWindow();
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

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateHitDice();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateHitDice();
            this.HideConciergeWindow();

            Program.Modify();
        }
    }
}
