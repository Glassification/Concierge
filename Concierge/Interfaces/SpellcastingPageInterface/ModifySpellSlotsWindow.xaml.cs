// <copyright file="ModifySpellSlotsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifySpellSlotsWindow.xaml.
    /// </summary>
    public partial class ModifySpellSlotsWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifySpellSlotsWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.conciergePage = conciergePage;
        }

        private SpellSlots SpellSlots { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.SpellSlots = Program.CcsFile.Character.SpellSlots;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void ShowEdit(SpellSlots spellSlots)
        {
            this.ApplyButton.Visibility = Visibility.Visible;
            this.SpellSlots = spellSlots;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void SetUpDownUpdating()
        {
            this.UsedPactUpDown.UpdatingValue();
            this.Used1UpDown.UpdatingValue();
            this.Used2UpDown.UpdatingValue();
            this.Used3UpDown.UpdatingValue();
            this.Used4UpDown.UpdatingValue();
            this.Used5UpDown.UpdatingValue();
            this.Used6UpDown.UpdatingValue();
            this.Used7UpDown.UpdatingValue();
            this.Used8UpDown.UpdatingValue();
            this.Used9UpDown.UpdatingValue();

            this.TotalPactUpDown.UpdatingValue();
            this.Total1UpDown.UpdatingValue();
            this.Total2UpDown.UpdatingValue();
            this.Total3UpDown.UpdatingValue();
            this.Total4UpDown.UpdatingValue();
            this.Total5UpDown.UpdatingValue();
            this.Total6UpDown.UpdatingValue();
            this.Total7UpDown.UpdatingValue();
            this.Total8UpDown.UpdatingValue();
            this.Total9UpDown.UpdatingValue();
        }

        private void FillFields()
        {
            this.SetUpDownUpdating();

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

            this.SpellSlots.PactUsed = this.UsedPactUpDown.Value ?? 0;
            this.SpellSlots.FirstUsed = this.Used1UpDown.Value ?? 0;
            this.SpellSlots.SecondUsed = this.Used2UpDown.Value ?? 0;
            this.SpellSlots.ThirdUsed = this.Used3UpDown.Value ?? 0;
            this.SpellSlots.FourthUsed = this.Used4UpDown.Value ?? 0;
            this.SpellSlots.FifthUsed = this.Used5UpDown.Value ?? 0;
            this.SpellSlots.SixthUsed = this.Used6UpDown.Value ?? 0;
            this.SpellSlots.SeventhUsed = this.Used7UpDown.Value ?? 0;
            this.SpellSlots.EighthUsed = this.Used8UpDown.Value ?? 0;
            this.SpellSlots.NinethUsed = this.Used9UpDown.Value ?? 0;

            this.SpellSlots.PactTotal = this.TotalPactUpDown.Value ?? 0;
            this.SpellSlots.FirstTotal = this.Total1UpDown.Value ?? 0;
            this.SpellSlots.SecondTotal = this.Total2UpDown.Value ?? 0;
            this.SpellSlots.ThirdTotal = this.Total3UpDown.Value ?? 0;
            this.SpellSlots.FourthTotal = this.Total4UpDown.Value ?? 0;
            this.SpellSlots.FifthTotal = this.Total5UpDown.Value ?? 0;
            this.SpellSlots.SixthTotal = this.Total6UpDown.Value ?? 0;
            this.SpellSlots.SeventhTotal = this.Total7UpDown.Value ?? 0;
            this.SpellSlots.EighthTotal = this.Total8UpDown.Value ?? 0;
            this.SpellSlots.NinethTotal = this.Total9UpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<SpellSlots>(this.SpellSlots, oldItem, this.conciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateSpellSlots();
            this.HideConciergeWindow();

            Program.Modify();
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
            this.HideConciergeWindow();
        }
    }
}
