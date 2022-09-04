// <copyright file="ToolsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.ToolsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Concierge.Character.Statuses;
    using Concierge.Interfaces.Enums;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools.DiceRolling;
    using Concierge.Tools.DiceRolling.Dice;
    using Concierge.Tools.DivideLoot;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ToolsPage.xaml.
    /// </summary>
    public partial class ToolsPage : Page, IConciergePage
    {
        public ToolsPage()
        {
            this.InitializeComponent();

            this.Players = new List<Player>();
            this.RollHistory = new List<IDiceRoll>();
            this.DiceHistory = new DiceHistory(DiceHistoryReadWriter.Read());

            this.SetDefaultDivideValues();
            this.SetDefaultDiceValues();
        }

        public ConciergePage ConciergePage => ConciergePage.Tools;

        public bool HasEditableDataGrid => false;

        private List<Player> Players { get; set; }

        private List<IDiceRoll> RollHistory { get; }

        private DiceHistory DiceHistory { get; set; }

        public void Draw()
        {
            this.DrawDivideLoot();
            this.DrawDiceHistory();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Player player)
            {
                Clipboard.SetText(player.Total.ToString());
            }
            else if (itemToEdit is IDiceRoll diceRoll)
            {
                Clipboard.SetText(diceRoll.Total.ToString());
            }
        }

        private void SetDefaultDivideValues()
        {
            this.PlayersInput.ResetInputValue();
            this.CopperInput.ResetInputValue();
            this.SilverInput.ResetInputValue();
            this.ElectrumInput.ResetInputValue();
            this.GoldInput.ResetInputValue();
            this.PlatinumInput.ResetInputValue();
        }

        private void DrawDivideLoot()
        {
            var totalValue = 0.0;
            this.DivideLootDataGrid.Items.Clear();

            foreach (var player in this.Players)
            {
                this.DivideLootDataGrid.Items.Add(player);
                totalValue += player.Total;
            }

            this.TotalSplitGoldField.Text = this.Players.IsEmpty() ?
                string.Empty :
                $"Total Gold: {Wealth.FormatGoldValue(totalValue)}";
        }

        private void GetPlayers()
        {
            this.Players.Clear();

            for (int i = 0; i < this.PlayersInput.InputValue; i++)
            {
                this.Players.Add(new Player($"Player {i + 1}"));
            }
        }

        private Loot GetLoot()
        {
            var cp = this.CopperInput.InputValue;
            var sp = this.SilverInput.InputValue;
            var ep = this.ElectrumInput.InputValue;
            var gp = this.GoldInput.InputValue;
            var pp = this.PlatinumInput.InputValue;

            return new Loot(cp, sp, ep, gp, pp);
        }

        private void Distribute(Loot loot)
        {
            var maxValue = loot.Total / this.Players.Count;

            if (this.Players.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < Constants.Currencies; i++)
            {
                while (loot.CurrencyList[i] > 0)
                {
                    for (int j = 0; j < this.Players.Count; j++)
                    {
                        if (loot.CurrencyList[i] < 1)
                        {
                            break;
                        }
                        else if (this.Players[j].Total < maxValue)
                        {
                            this.Players[j].CurrencyList[i]++;
                            loot.CurrencyList[i]--;
                        }
                    }
                }
            }
        }

        private void DivideLoot()
        {
            this.DivideLootDataGrid.Items.Clear();

            this.GetPlayers();
            var loot = this.GetLoot();

            this.Distribute(loot);

            this.DrawDivideLoot();
        }

        private void DrawDiceHistory()
        {
            this.RollDiceHistoryDataGrid.Items.Clear();

            foreach (var dice in this.RollHistory)
            {
                this.RollDiceHistoryDataGrid.Items.Add(dice);
            }

            if (this.RollDiceHistoryDataGrid.Items.Count > 0)
            {
                this.RollDiceHistoryDataGrid.SelectedItem = this.RollDiceHistoryDataGrid.Items[^1];
                this.RollDiceHistoryDataGrid.UpdateLayout();
                this.RollDiceHistoryDataGrid.ScrollIntoView(this.RollDiceHistoryDataGrid.SelectedItem);
            }
        }

        private void SetDefaultDiceValues()
        {
            this.D4DiceRollDisplay.ResetDiceValue();
            this.D6DiceRollDisplay.ResetDiceValue();
            this.D8DiceRollDisplay.ResetDiceValue();
            this.D10DiceRollDisplay.ResetDiceValue();
            this.D100DiceRollDisplay.ResetDiceValue();
            this.D12DiceRollDisplay.ResetDiceValue();
            this.D20DiceRollDisplay.ResetDiceValue();
            this.DxDiceRollDisplay.ResetDiceValue();

            this.CustomResult.Text = "0";
        }

        private void ClearDivideLoot()
        {
            this.SetDefaultDivideValues();
            this.DivideLootDataGrid.Items.Clear();
            this.Players.Clear();
            this.TotalSplitGoldField.Text = string.Empty;
        }

        private void ParseCustomInput()
        {
            var input = this.CustomInputTextBox.Text;
            if (input.IsNullOrWhiteSpace())
            {
                return;
            }

            try
            {
                var stack = DiceParser.Parse(input);
                var result = new CustomDiceRoll(stack);

                this.RollHistory.Add(result);
                this.DiceHistory.Add(input);
                this.DrawDiceHistory();
                this.CustomInputTextBox.Text = string.Empty;
                this.CustomResult.Text = result.Total.ToString();

                DiceHistoryReadWriter.Write(input);
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
        }

        private void ButtonDivideLoot_Click(object sender, RoutedEventArgs e)
        {
            this.DivideLoot();
        }

        private void ButtonResetLoot_Click(object sender, RoutedEventArgs e)
        {
            this.ClearDivideLoot();
        }

        private void DivideLootDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ButtonResetHistory_Click(object sender, RoutedEventArgs e)
        {
            this.SetDefaultDiceValues();
            this.RollHistory.Clear();
            this.DrawDiceHistory();
            this.CustomInputTextBox.Text = string.Empty;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (this.CustomInputTextBox.IsFocused)
                    {
                        this.ParseCustomInput();
                    }

                    break;
            }
        }

        private void ParseInputButton_Click(object sender, RoutedEventArgs e)
        {
            this.ParseCustomInput();
        }

        private void CustomInputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (this.CustomInputTextBox.IsFocused)
                    {
                        this.CustomInputTextBox.Text = this.DiceHistory.Backward();
                        this.CustomInputTextBox.Select(this.CustomInputTextBox.Text.Length, 0);
                    }

                    break;
                case Key.Down:
                    if (this.CustomInputTextBox.IsFocused)
                    {
                        this.CustomInputTextBox.Text = this.DiceHistory.Forward();
                        this.CustomInputTextBox.Select(this.CustomInputTextBox.Text.Length, 0);
                    }

                    break;
            }
        }

        private void DiceRollDisplay_DiceRolled(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is DiceRoll diceRoll)
            {
                this.RollHistory.Add(diceRoll);
                this.DrawDiceHistory();
            }
        }
    }
}
