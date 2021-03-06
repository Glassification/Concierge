﻿// <copyright file="ToolsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.ToolsPageUi
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Tools;
    using Xceed.Wpf.Toolkit;

    /// <summary>
    /// Interaction logic for ToolsPage.xaml.
    /// </summary>
    public partial class ToolsPage : Page
    {
        public ToolsPage()
        {
            this.InitializeComponent();

            this.Players = new List<Player>();
            this.DiceHistory = new List<Die>();
            this.Random = new Random();

            this.SetDefaultDivideValues();
            this.SetDefaultDiceValues();
        }

        private List<Player> Players { get; set; }

        private Random Random { get; }

        private List<Die> DiceHistory { get; }

        public void Draw()
        {
            this.FillDivideLootGrid();
            this.FillDiceHistoryGrid();
        }

        private void SetDefaultDivideValues()
        {
            this.PlayersInput.Text = "0";
            this.CopperInput.Text = "0";
            this.SilverInput.Text = "0";
            this.ElectrumInput.Text = "0";
            this.GoldInput.Text = "0";
            this.PlatinumInput.Text = "0";
        }

        private void FillDivideLootGrid()
        {
            this.DivideLootDataGrid.Items.Clear();

            foreach (var player in this.Players)
            {
                this.DivideLootDataGrid.Items.Add(player);
            }
        }

        private void GetPlayers()
        {
            int.TryParse(this.PlayersInput.Text, out int numPlayers);
            this.Players.Clear();

            for (int i = 0; i < numPlayers; i++)
            {
                this.Players.Add(new Player($"Player {i + 1}"));
            }
        }

        private Player GetLoot()
        {
            int.TryParse(this.CopperInput.Text, out int cp);
            int.TryParse(this.SilverInput.Text, out int sp);
            int.TryParse(this.ElectrumInput.Text, out int ep);
            int.TryParse(this.GoldInput.Text, out int gp);
            int.TryParse(this.PlatinumInput.Text, out int pp);

            return new Player(cp, sp, ep, gp, pp);
        }

        private void Distribute(Player loot)
        {
            var maxValue = loot.Total / this.Players.Count;

            if (this.Players.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < Player.CURRENCIES; i++)
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

        private void ButtonDivideLoot_Click(object sender, RoutedEventArgs e)
        {
            this.DivideLootDataGrid.Items.Clear();

            this.GetPlayers();
            var loot = this.GetLoot();

            this.Distribute(loot);

            this.FillDivideLootGrid();
        }

        private void ButtonResetLoot_Click(object sender, RoutedEventArgs e)
        {
            this.SetDefaultDivideValues();
            this.DivideLootDataGrid.Items.Clear();
            this.Players.Clear();
        }

        private void DivideLootDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Equals("0"))
            {
                ((TextBox)sender).Text = string.Empty;
            }
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(((TextBox)sender).Text, out _))
            {
                ((TextBox)sender).Text = "0";
            }
        }

        private void FillDiceHistoryGrid()
        {
            this.RollDiceHistoryDataGrid.Items.Clear();

            foreach (var dice in this.DiceHistory)
            {
                this.RollDiceHistoryDataGrid.Items.Add(dice);
            }

            if (this.RollDiceHistoryDataGrid.Items.Count > 0)
            {
                this.RollDiceHistoryDataGrid.SelectedItem = this.RollDiceHistoryDataGrid.Items[this.RollDiceHistoryDataGrid.Items.Count - 1];
                this.RollDiceHistoryDataGrid.UpdateLayout();
                this.RollDiceHistoryDataGrid.ScrollIntoView(this.RollDiceHistoryDataGrid.SelectedItem);
            }
        }

        private void SetDefaultDiceValues()
        {
            this.SetDieValue(this.D4NumberUpDown, this.D4ModifierUpDown, this.D4Plus, this.D4Result);
            this.SetDieValue(this.D6NumberUpDown, this.D6ModifierUpDown, this.D6Plus, this.D6Result);
            this.SetDieValue(this.D8NumberUpDown, this.D8ModifierUpDown, this.D8Plus, this.D8Result);
            this.SetDieValue(this.D10NumberUpDown, this.D10ModifierUpDown, this.D10Plus, this.D10Result);
            this.SetDieValue(this.D100NumberUpDown, this.D100ModifierUpDown, this.D100Plus, this.D100Result);
            this.SetDieValue(this.D12NumberUpDown, this.D12ModifierUpDown, this.D12Plus, this.D12Result);
            this.SetDieValue(this.D20NumberUpDown, this.D20ModifierUpDown, this.D20Plus, this.D20Result);
            this.SetDieValue(this.DxNumberUpDown, this.DxModifierUpDown, this.DxPlus, this.DxResult);
            this.DxDieUpDown.Value = 1;
        }

        private void SetDieValue(
            IntegerUpDown dieNumber,
            IntegerUpDown modifierNumber,
            RadioButton plusButton,
            TextBlock resultNumber)
        {
            dieNumber.Value = 1;
            modifierNumber.Value = 0;
            plusButton.IsChecked = true;
            resultNumber.Text = "0";
        }

        private string RollDice(int diceNumber, int diceSides, int modified, bool isPlus)
        {
            var total = 0;
            var rolledDice = new int[diceNumber];

            for (int i = 0; i < diceNumber; i++)
            {
                var val = this.Random.Next(1, diceSides + 1);
                total += val;
                rolledDice[i] = val;
            }

            if (isPlus)
            {
                total += modified;
            }
            else
            {
                total -= modified;
            }

            total = Math.Max(1, total);

            this.DiceHistory.Add(new Die($"({diceNumber}d{diceSides}) {(isPlus ? "+" : "-")}{modified}", rolledDice, total));
            this.FillDiceHistoryGrid();

            return total.ToString();
        }

        private void ButtonResetHistory_Click(object sender, RoutedEventArgs e)
        {
            this.SetDefaultDiceValues();
            this.DiceHistory.Clear();
            this.FillDiceHistoryGrid();
        }

        private void ButtonRollD4_Click(object sender, RoutedEventArgs e)
        {
            this.D4Result.Text = this.RollDice((int)this.D4NumberUpDown.Value, 4, (int)this.D4ModifierUpDown.Value, (bool)this.D4Plus.IsChecked);
        }

        private void ButtonRollD6_Click(object sender, RoutedEventArgs e)
        {
            this.D6Result.Text = this.RollDice((int)this.D6NumberUpDown.Value, 6, (int)this.D6ModifierUpDown.Value, (bool)this.D6Plus.IsChecked);
        }

        private void ButtonRollD8_Click(object sender, RoutedEventArgs e)
        {
            this.D8Result.Text = this.RollDice((int)this.D8NumberUpDown.Value, 8, (int)this.D8ModifierUpDown.Value, (bool)this.D8Plus.IsChecked);
        }

        private void ButtonRollD10_Click(object sender, RoutedEventArgs e)
        {
            this.D10Result.Text = this.RollDice((int)this.D10NumberUpDown.Value, 10, (int)this.D10ModifierUpDown.Value, (bool)this.D10Plus.IsChecked);
        }

        private void ButtonRollD100_Click(object sender, RoutedEventArgs e)
        {
            this.D100Result.Text = this.RollDice((int)this.D100NumberUpDown.Value, 100, (int)this.D100ModifierUpDown.Value, (bool)this.D100Plus.IsChecked);
        }

        private void ButtonRollD12_Click(object sender, RoutedEventArgs e)
        {
            this.D12Result.Text = this.RollDice((int)this.D12NumberUpDown.Value, 12, (int)this.D12ModifierUpDown.Value, (bool)this.D12Plus.IsChecked);
        }

        private void ButtonRollD20_Click(object sender, RoutedEventArgs e)
        {
            this.D20Result.Text = this.RollDice((int)this.D20NumberUpDown.Value, 20, (int)this.D20ModifierUpDown.Value, (bool)this.D20Plus.IsChecked);
        }

        private void ButtonRollDx_Click(object sender, RoutedEventArgs e)
        {
            this.DxResult.Text = this.RollDice((int)this.DxNumberUpDown.Value, (int)this.DxDieUpDown.Value, (int)this.DxModifierUpDown.Value, (bool)this.DxPlus.IsChecked);
        }
    }
}
