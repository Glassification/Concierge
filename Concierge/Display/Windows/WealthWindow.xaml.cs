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
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for WealthWindow.xaml.
    /// </summary>
    public partial class WealthWindow : ConciergeWindow
    {
        private Wealth selectedWealth = new ();
        private int cP;
        private int sP;
        private int eP;
        private int gP;
        private int pP;

        public WealthWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePages.None;
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

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.selectedWealth = Program.CcsFile.Character.Wealth;
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

            this.selectedWealth = castItem;
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
            var oldItem = this.selectedWealth.DeepCopy();

            this.selectedWealth.Copper = this.cP;
            this.selectedWealth.Silver = this.sP;
            this.selectedWealth.Electrum = this.eP;
            this.selectedWealth.Gold = this.gP;
            this.selectedWealth.Platinum = this.pP;

            Program.UndoRedoService.AddCommand(new EditCommand<Wealth>(this.selectedWealth, oldItem, this.ConciergePage));
        }

        private void ClearFields(CoinType coinType = CoinType.Gold)
        {
            Program.Drawing();

            this.SelectCoinTypeRadioButton(coinType);
            this.AmountUpDown.Value = 0;

            this.cP = this.selectedWealth.Copper;
            this.sP = this.selectedWealth.Silver;
            this.eP = this.selectedWealth.Electrum;
            this.gP = this.selectedWealth.Gold;
            this.pP = this.selectedWealth.Platinum;

            Program.NotDrawing();
        }

        private void FillFields()
        {
            Program.Drawing();

            this.AmountUpDown.Value = 0;
            this.CopperField.Text = this.cP.ToString();
            this.SilverField.Text = this.sP.ToString();
            this.ElectrumField.Text = this.eP.ToString();
            this.GoldField.Text = this.gP.ToString();
            this.PlatinumField.Text = this.pP.ToString();

            Program.NotDrawing();
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
                this.cP = Math.Max(0, this.cP + amount);
            }
            else if (this.SpRadioButton.IsChecked ?? false)
            {
                this.sP = Math.Max(0, this.sP + amount);
            }
            else if (this.EpRadioButton.IsChecked ?? false)
            {
                this.eP = Math.Max(0, this.eP + amount);
            }
            else if (this.GpRadioButton.IsChecked ?? false)
            {
                this.gP = Math.Max(0, this.gP + amount);
            }
            else if (this.PpRadioButton.IsChecked ?? false)
            {
                this.pP = Math.Max(0, this.pP + amount);
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
