// <copyright file="ModifySpellSlotsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifySpellSlotsWindow.xaml.
    /// </summary>
    public partial class ModifySpellSlotsWindow : Window, IConciergeModifyWindow
    {
        public ModifySpellSlotsWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit()
        {
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
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

            var spellSlots = Program.CcsFile.Character.SpellSlots;

            this.UsedPactUpDown.Value = spellSlots.PactUsed;
            this.Used1UpDown.Value = spellSlots.FirstUsed;
            this.Used2UpDown.Value = spellSlots.SecondUsed;
            this.Used3UpDown.Value = spellSlots.ThirdUsed;
            this.Used4UpDown.Value = spellSlots.FourthUsed;
            this.Used5UpDown.Value = spellSlots.FifthUsed;
            this.Used6UpDown.Value = spellSlots.SixthUsed;
            this.Used7UpDown.Value = spellSlots.SeventhUsed;
            this.Used8UpDown.Value = spellSlots.EighthUsed;
            this.Used9UpDown.Value = spellSlots.NinethUsed;

            this.TotalPactUpDown.Value = spellSlots.PactTotal;
            this.Total1UpDown.Value = spellSlots.FirstTotal;
            this.Total2UpDown.Value = spellSlots.SecondTotal;
            this.Total3UpDown.Value = spellSlots.ThirdTotal;
            this.Total4UpDown.Value = spellSlots.FourthTotal;
            this.Total5UpDown.Value = spellSlots.FifthTotal;
            this.Total6UpDown.Value = spellSlots.SixthTotal;
            this.Total7UpDown.Value = spellSlots.SeventhTotal;
            this.Total8UpDown.Value = spellSlots.EighthTotal;
            this.Total9UpDown.Value = spellSlots.NinethTotal;
        }

        private void UpdateSpellSlots()
        {
            var spellSlots = Program.CcsFile.Character.SpellSlots;

            spellSlots.PactUsed = this.UsedPactUpDown.Value ?? 0;
            spellSlots.FirstUsed = this.Used1UpDown.Value ?? 0;
            spellSlots.SecondUsed = this.Used2UpDown.Value ?? 0;
            spellSlots.ThirdUsed = this.Used3UpDown.Value ?? 0;
            spellSlots.FourthUsed = this.Used4UpDown.Value ?? 0;
            spellSlots.FifthUsed = this.Used5UpDown.Value ?? 0;
            spellSlots.SixthUsed = this.Used6UpDown.Value ?? 0;
            spellSlots.SeventhUsed = this.Used7UpDown.Value ?? 0;
            spellSlots.EighthUsed = this.Used8UpDown.Value ?? 0;
            spellSlots.NinethUsed = this.Used9UpDown.Value ?? 0;

            spellSlots.PactTotal = this.TotalPactUpDown.Value ?? 0;
            spellSlots.FirstTotal = this.Total1UpDown.Value ?? 0;
            spellSlots.SecondTotal = this.Total2UpDown.Value ?? 0;
            spellSlots.ThirdTotal = this.Total3UpDown.Value ?? 0;
            spellSlots.FourthTotal = this.Total4UpDown.Value ?? 0;
            spellSlots.FifthTotal = this.Total5UpDown.Value ?? 0;
            spellSlots.SixthTotal = this.Total6UpDown.Value ?? 0;
            spellSlots.SeventhTotal = this.Total7UpDown.Value ?? 0;
            spellSlots.EighthTotal = this.Total8UpDown.Value ?? 0;
            spellSlots.NinethTotal = this.Total9UpDown.Value ?? 0;
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateSpellSlots();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateSpellSlots();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
