// <copyright file="HitDiceWindow.xaml.cs" company="Thomas Beckett">
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
        private HitDice hitDice = new ();

        public HitDiceWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePages.None;
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

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.hitDice = Program.CcsFile.Character.Vitality.HitDice;
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

            this.hitDice = castItem;
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
            Program.Drawing();

            this.TotalD6UpDown.Value = this.hitDice.TotalD6;
            this.TotalD8UpDown.Value = this.hitDice.TotalD8;
            this.TotalD10UpDown.Value = this.hitDice.TotalD10;
            this.TotalD12UpDown.Value = this.hitDice.TotalD12;

            this.UsedD6UpDown.Value = this.hitDice.SpentD6;
            this.UsedD8UpDown.Value = this.hitDice.SpentD8;
            this.UsedD10UpDown.Value = this.hitDice.SpentD10;
            this.UsedD12UpDown.Value = this.hitDice.SpentD12;

            Program.NotDrawing();
        }

        private void UpdateHitDice()
        {
            var oldItem = this.hitDice.DeepCopy();

            this.hitDice.TotalD6 = this.TotalD6UpDown.Value;
            this.hitDice.TotalD8 = this.TotalD8UpDown.Value;
            this.hitDice.TotalD10 = this.TotalD10UpDown.Value;
            this.hitDice.TotalD12 = this.TotalD12UpDown.Value;

            this.hitDice.SpentD6 = this.UsedD6UpDown.Value;
            this.hitDice.SpentD8 = this.UsedD8UpDown.Value;
            this.hitDice.SpentD10 = this.UsedD10UpDown.Value;
            this.hitDice.SpentD12 = this.UsedD12UpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<HitDice>(this.hitDice, oldItem, this.ConciergePage));
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
