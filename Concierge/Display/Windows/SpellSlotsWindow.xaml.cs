// <copyright file="SpellSlotsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for SpellSlotsWindow.xaml.
    /// </summary>
    public partial class SpellSlotsWindow : ConciergeWindow
    {
        public SpellSlotsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.SpellSlots = new SpellSlots();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.UsedPactUpDown);
            this.SetFocusEvents(this.Used1UpDown);
            this.SetFocusEvents(this.Used2UpDown);
            this.SetFocusEvents(this.Used3UpDown);
            this.SetFocusEvents(this.Used4UpDown);
            this.SetFocusEvents(this.Used5UpDown);
            this.SetFocusEvents(this.Used6UpDown);
            this.SetFocusEvents(this.Used7UpDown);
            this.SetFocusEvents(this.Used8UpDown);
            this.SetFocusEvents(this.Used9UpDown);
            this.SetFocusEvents(this.TotalPactUpDown);
            this.SetFocusEvents(this.Total1UpDown);
            this.SetFocusEvents(this.Total2UpDown);
            this.SetFocusEvents(this.Total3UpDown);
            this.SetFocusEvents(this.Total4UpDown);
            this.SetFocusEvents(this.Total5UpDown);
            this.SetFocusEvents(this.Total6UpDown);
            this.SetFocusEvents(this.Total7UpDown);
            this.SetFocusEvents(this.Total8UpDown);
            this.SetFocusEvents(this.Total9UpDown);
        }

        public override string HeaderText => "Edit Spell Slots";

        public override string WindowName => nameof(SpellSlotsWindow);

        private SpellSlots SpellSlots { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.SpellSlots = Program.CcsFile.Character.Magic.SpellSlots;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.SetSpentLimit();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T spellSlots)
        {
            if (spellSlots is not SpellSlots castItem)
            {
                return;
            }

            this.SpellSlots = castItem;
            this.FillFields();
            this.SetSpentLimit();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateSpellSlots();
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields()
        {
            this.FillTotal();
            this.FillUsed();
        }

        private void FillUsed()
        {
            this.UsedPactUpDown.Value = this.SpellSlots.PactUsed;
            this.Used1UpDown.Value = this.SpellSlots.FirstUsed;
            this.Used2UpDown.Value = this.SpellSlots.SecondUsed;
            this.Used3UpDown.Value = this.SpellSlots.ThirdUsed;
            this.Used4UpDown.Value = this.SpellSlots.FourthUsed;
            this.Used5UpDown.Value = this.SpellSlots.FifthUsed;
            this.Used6UpDown.Value = this.SpellSlots.SixthUsed;
            this.Used7UpDown.Value = this.SpellSlots.SeventhUsed;
            this.Used8UpDown.Value = this.SpellSlots.EighthUsed;
            this.Used9UpDown.Value = this.SpellSlots.NinethUsed;
        }

        private void FillTotal()
        {
            this.TotalPactUpDown.Value = this.SpellSlots.PactTotal;
            this.Total1UpDown.Value = this.SpellSlots.FirstTotal;
            this.Total2UpDown.Value = this.SpellSlots.SecondTotal;
            this.Total3UpDown.Value = this.SpellSlots.ThirdTotal;
            this.Total4UpDown.Value = this.SpellSlots.FourthTotal;
            this.Total5UpDown.Value = this.SpellSlots.FifthTotal;
            this.Total6UpDown.Value = this.SpellSlots.SixthTotal;
            this.Total7UpDown.Value = this.SpellSlots.SeventhTotal;
            this.Total8UpDown.Value = this.SpellSlots.EighthTotal;
            this.Total9UpDown.Value = this.SpellSlots.NinethTotal;
        }

        private void UpdateSpellSlots()
        {
            var oldItem = this.SpellSlots.DeepCopy();

            this.SpellSlots.PactUsed = this.UsedPactUpDown.Value;
            this.SpellSlots.FirstUsed = this.Used1UpDown.Value;
            this.SpellSlots.SecondUsed = this.Used2UpDown.Value;
            this.SpellSlots.ThirdUsed = this.Used3UpDown.Value;
            this.SpellSlots.FourthUsed = this.Used4UpDown.Value;
            this.SpellSlots.FifthUsed = this.Used5UpDown.Value;
            this.SpellSlots.SixthUsed = this.Used6UpDown.Value;
            this.SpellSlots.SeventhUsed = this.Used7UpDown.Value;
            this.SpellSlots.EighthUsed = this.Used8UpDown.Value;
            this.SpellSlots.NinethUsed = this.Used9UpDown.Value;

            this.SpellSlots.PactTotal = this.TotalPactUpDown.Value;
            this.SpellSlots.FirstTotal = this.Total1UpDown.Value;
            this.SpellSlots.SecondTotal = this.Total2UpDown.Value;
            this.SpellSlots.ThirdTotal = this.Total3UpDown.Value;
            this.SpellSlots.FourthTotal = this.Total4UpDown.Value;
            this.SpellSlots.FifthTotal = this.Total5UpDown.Value;
            this.SpellSlots.SixthTotal = this.Total6UpDown.Value;
            this.SpellSlots.SeventhTotal = this.Total7UpDown.Value;
            this.SpellSlots.EighthTotal = this.Total8UpDown.Value;
            this.SpellSlots.NinethTotal = this.Total9UpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<SpellSlots>(this.SpellSlots, oldItem, this.ConciergePage));
        }

        private void SetSpentLimit()
        {
            this.UsedPactUpDown.Maximum = this.TotalPactUpDown.Value;
            this.Used1UpDown.Maximum = this.Total1UpDown.Value;
            this.Used2UpDown.Maximum = this.Total2UpDown.Value;
            this.Used3UpDown.Maximum = this.Total3UpDown.Value;
            this.Used4UpDown.Maximum = this.Total4UpDown.Value;
            this.Used5UpDown.Maximum = this.Total5UpDown.Value;
            this.Used6UpDown.Maximum = this.Total6UpDown.Value;
            this.Used7UpDown.Maximum = this.Total7UpDown.Value;
            this.Used8UpDown.Maximum = this.Total8UpDown.Value;
            this.Used9UpDown.Maximum = this.Total9UpDown.Value;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSpellSlots();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void Total_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.SetSpentLimit();
        }
    }
}
