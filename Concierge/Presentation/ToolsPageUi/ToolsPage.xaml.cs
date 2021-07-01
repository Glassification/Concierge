// <copyright file="ToolsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.ToolsPageUi
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Tools;

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

        private void PlayersInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.PlayersInput.Text.Equals("0"))
            {
                this.PlayersInput.Text = string.Empty;
            }
        }

        private void PlayersInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.PlayersInput.Text, out _))
            {
                this.PlayersInput.Text = "0";
            }
        }

        private void CopperInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.CopperInput.Text.Equals("0"))
            {
                this.CopperInput.Text = string.Empty;
            }
        }

        private void CopperInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.CopperInput.Text, out _))
            {
                this.CopperInput.Text = "0";
            }
        }

        private void SilverInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.SilverInput.Text.Equals("0"))
            {
                this.SilverInput.Text = string.Empty;
            }
        }

        private void SilverInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.SilverInput.Text, out _))
            {
                this.SilverInput.Text = "0";
            }
        }

        private void ElectrumInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.ElectrumInput.Text.Equals("0"))
            {
                this.ElectrumInput.Text = string.Empty;
            }
        }

        private void ElectrumInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.ElectrumInput.Text, out _))
            {
                this.ElectrumInput.Text = "0";
            }
        }

        private void GoldInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.GoldInput.Text.Equals("0"))
            {
                this.GoldInput.Text = string.Empty;
            }
        }

        private void GoldInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.GoldInput.Text, out _))
            {
                this.GoldInput.Text = "0";
            }
        }

        private void PlatinumInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.PlatinumInput.Text.Equals("0"))
            {
                this.PlatinumInput.Text = string.Empty;
            }
        }

        private void PlatinumInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.PlatinumInput.Text, out _))
            {
                this.PlatinumInput.Text = "0";
            }
        }

        private void FillDiceHistoryGrid()
        {
            this.RollDiceHistoryDataGrid.Items.Clear();

            foreach (var dice in this.DiceHistory)
            {
                this.RollDiceHistoryDataGrid.Items.Add(dice);
            }
        }

        private void SetDefaultDiceValues()
        {
            this.D4NumberUpDown.Value = 1;
            this.D4ModifierUpDown.Value = 0;
            this.D4Plus.IsChecked = true;
            this.D4Result.Text = "0";

            this.D6NumberUpDown.Value = 1;
            this.D6ModifierUpDown.Value = 0;
            this.D6Plus.IsChecked = true;
            this.D6Result.Text = "0";

            this.D8NumberUpDown.Value = 1;
            this.D8ModifierUpDown.Value = 0;
            this.D8Plus.IsChecked = true;
            this.D8Result.Text = "0";

            this.D10NumberUpDown.Value = 1;
            this.D10ModifierUpDown.Value = 0;
            this.D10Plus.IsChecked = true;
            this.D10Result.Text = "0";

            this.D100NumberUpDown.Value = 1;
            this.D100ModifierUpDown.Value = 0;
            this.D100Plus.IsChecked = true;
            this.D100Result.Text = "0";

            this.D12NumberUpDown.Value = 1;
            this.D12ModifierUpDown.Value = 0;
            this.D12Plus.IsChecked = true;
            this.D12Result.Text = "0";

            this.D20NumberUpDown.Value = 1;
            this.D20ModifierUpDown.Value = 0;
            this.D20Plus.IsChecked = true;
            this.D20Result.Text = "0";

            this.DxDieUpDown.Value = 1;
            this.DxNumberUpDown.Value = 1;
            this.DxModifierUpDown.Value = 0;
            this.DxPlus.IsChecked = true;
            this.DxResult.Text = "0";
        }

        private string RollDice(int diceNumber, int diceSides, int modified, bool isPlus)
        {
            int total = 0, val;
            int[] rolledDice = new int[diceNumber];

            for (int i = 0; i < diceNumber; i++)
            {
                val = this.Random.Next(1, diceSides + 1);
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
