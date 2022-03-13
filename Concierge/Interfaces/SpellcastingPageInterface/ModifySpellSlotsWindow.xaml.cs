// <copyright file="ModifySpellSlotsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System.Windows;

    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifySpellSlotsWindow.xaml.
    /// </summary>
    public partial class ModifySpellSlotsWindow : ConciergeWindow
    {
        public ModifySpellSlotsWindow()
        {
            this.InitializeComponent();
            this.ConciergePage = ConciergePage.None;
            this.SpellSlots = new SpellSlots();
        }

        public override string HeaderText => "Edit Spell Slots";

        private SpellSlots SpellSlots { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.SpellSlots = Program.CcsFile.Character.SpellSlots;
            this.CancelButton.Content = buttonText;

            this.FillFields();
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
    }
}
