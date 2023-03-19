// <copyright file="WealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
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
        }

        public override string HeaderText => "Edit Wealth";

        public override string WindowName => nameof(WealthWindow);

        private int CP { get; set; }

        private int SP { get; set; }

        private int EP { get; set; }

        private int GP { get; set; }

        private int PP { get; set; }

        private Wealth SelectedWealth { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.SelectedWealth = Program.CcsFile.Character.Wealth;
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
            this.Result = ConciergeWindowResult.OK;
            var oldItem = this.SelectedWealth.DeepCopy();

            this.AddSelectedAmount();

            this.SelectedWealth.Copper = this.CP;
            this.SelectedWealth.Silver = this.SP;
            this.SelectedWealth.Electrum = this.EP;
            this.SelectedWealth.Gold = this.GP;
            this.SelectedWealth.Platinum = this.PP;

            Program.UndoRedoService.AddCommand(new EditCommand<Wealth>(this.SelectedWealth, oldItem, this.ConciergePage));
            Program.Modify();

            this.CloseConciergeWindow();
        }

        private void ClearFields(CoinType coinType = CoinType.Gold)
        {
            this.AddRadioButton.IsChecked = true;
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

        private int GetAmount()
        {
            return this.AddRadioButton.IsChecked ?? false
                ? this.AmountUpDown.Value
                : this.SubtractRadioButton.IsChecked ?? false ? this.AmountUpDown.Value * -1 : 0;
        }

        private void AddSelectedAmount()
        {
            if (this.CpRadioButton.IsChecked ?? false)
            {
                this.CP += this.GetAmount();
            }
            else if (this.SpRadioButton.IsChecked ?? false)
            {
                this.SP += this.GetAmount();
            }
            else if (this.EpRadioButton.IsChecked ?? false)
            {
                this.EP += this.GetAmount();
            }
            else if (this.GpRadioButton.IsChecked ?? false)
            {
                this.GP += this.GetAmount();
            }
            else if (this.PpRadioButton.IsChecked ?? false)
            {
                this.PP += this.GetAmount();
            }
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
            this.AddSelectedAmount();
            this.FillFields();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
