// <copyright file="WealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Windows;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for WealthWindow.xaml.
    /// </summary>
    public partial class WealthWindow : ConciergeWindow
    {
        public WealthWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.SelectedWealth = new Wealth();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.AmountUpDown);
            this.SetMouseOverEvents(this.Add1Button);
            this.SetMouseOverEvents(this.Add10Button);
            this.SetMouseOverEvents(this.Add100Button);
            this.SetMouseOverEvents(this.Add1000Button);
            this.SetMouseOverEvents(this.PlusButton);
            this.SetMouseOverEvents(this.MinusButton);
            this.SetMouseOverEvents(this.CpRadioButton);
            this.SetMouseOverEvents(this.SpRadioButton);
            this.SetMouseOverEvents(this.EpRadioButton);
            this.SetMouseOverEvents(this.GpRadioButton);
            this.SetMouseOverEvents(this.PpRadioButton);
        }

        public override string HeaderText => "Edit Wealth";

        public override string WindowName => nameof(WealthWindow);

        private int CP { get; set; }

        private int SP { get; set; }

        private int EP { get; set; }

        private int GP { get; set; }

        private int PP { get; set; }

        private Wealth SelectedWealth { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.SelectedWealth = Program.CcsFile.Character.Wealth;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T wealth, object sender)
        {
            if (wealth is not Wealth castItem)
            {
                return;
            }

            if (sender is not WealthControl wealthControl)
            {
                return;
            }

            this.SelectedWealth = castItem;
            this.ClearFields(wealthControl.SelectedCoin);
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;
            this.UpdateWealth();
            this.CloseConciergeWindow();
        }

        private void UpdateWealth()
        {
            var oldItem = this.SelectedWealth.DeepCopy();

            this.SelectedWealth.Copper = this.CP;
            this.SelectedWealth.Silver = this.SP;
            this.SelectedWealth.Electrum = this.EP;
            this.SelectedWealth.Gold = this.GP;
            this.SelectedWealth.Platinum = this.PP;

            Program.UndoRedoService.AddCommand(new EditCommand<Wealth>(this.SelectedWealth, oldItem, this.ConciergePage));
        }

        private void ClearFields(CoinType coinType = CoinType.Gold)
        {
            this.SelectCoinTypeRadioButton(coinType);
            this.AmountUpDown.Value = 0;

            this.CP = this.SelectedWealth.Copper;
            this.SP = this.SelectedWealth.Silver;
            this.EP = this.SelectedWealth.Electrum;
            this.GP = this.SelectedWealth.Gold;
            this.PP = this.SelectedWealth.Platinum;
        }

        private void FillFields()
        {
            this.AmountUpDown.Value = 0;
            this.CopperField.Text = this.CP.ToString();
            this.SilverField.Text = this.SP.ToString();
            this.ElectrumField.Text = this.EP.ToString();
            this.GoldField.Text = this.GP.ToString();
            this.PlatinumField.Text = this.PP.ToString();
        }

        private void SelectCoinTypeRadioButton(CoinType coinType)
        {
            switch (coinType)
            {
                case CoinType.Copper:
                    this.CpRadioButton.IsChecked = true;
                    break;
                case CoinType.Silver:
                    this.SpRadioButton.IsChecked = true;
                    break;
                case CoinType.Electrum:
                    this.EpRadioButton.IsChecked = true;
                    break;
                case CoinType.Gold:
                default:
                    this.GpRadioButton.IsChecked = true;
                    break;
                case CoinType.Platinum:
                    this.PpRadioButton.IsChecked = true;
                    break;
            }
        }

        private void AddSelectedAmount(int amount)
        {
            if (this.CpRadioButton.IsChecked ?? false)
            {
                this.CP = Math.Max(0, this.CP + amount);
            }
            else if (this.SpRadioButton.IsChecked ?? false)
            {
                this.SP = Math.Max(0, this.SP + amount);
            }
            else if (this.EpRadioButton.IsChecked ?? false)
            {
                this.EP = Math.Max(0, this.EP + amount);
            }
            else if (this.GpRadioButton.IsChecked ?? false)
            {
                this.GP = Math.Max(0, this.GP + amount);
            }
            else if (this.PpRadioButton.IsChecked ?? false)
            {
                this.PP = Math.Max(0, this.PP + amount);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateWealth();
            this.InvokeApplyChanges();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ConciergeDesignButton button && button.Tag is int amount)
            {
                this.AmountUpDown.Value += amount;
            }
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddSelectedAmount(this.AmountUpDown.Value);
            this.FillFields();
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddSelectedAmount(-this.AmountUpDown.Value);
            this.FillFields();
        }
    }
}
