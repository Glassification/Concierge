// <copyright file="ToolsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.ToolsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Interfaces.Controls;
    using Concierge.Interfaces.Enums;
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
        private const string Zero = "0";
        private const string MagicNumber = "5";

        private readonly string[] playerNames = new string[] { "Colby", "Daniel", "Kaleigh", "Thomas", "Travis" };

        public ToolsPage()
        {
            this.InitializeComponent();

            this.Players = new List<Player>();
            this.RollHistory = new List<IDiceRoll>();
            this.DiceHistory = new DiceHistory();

            this.SetDefaultDivideValues();
            this.SetDefaultDiceValues();
        }

        public ConciergePage ConciergePage => ConciergePage.Tools;

        public bool HasEditableDataGrid => false;

        private List<Player> Players { get; set; }

        private List<IDiceRoll> RollHistory { get; }

        private DiceHistory DiceHistory { get; set; }

        private bool DivideLootInputHasFocus
        {
            get
            {
                return
                    this.CopperInput.IsFocused ||
                    this.SilverInput.IsFocused ||
                    this.ElectrumInput.IsFocused ||
                    this.GoldInput.IsFocused ||
                    this.PlatinumInput.IsFocused ||
                    this.PlayersInput.IsFocused;
            }
        }

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
            else if (itemToEdit is DiceRoll diceRoll)
            {
                Clipboard.SetText(diceRoll.Total.ToString());
            }
        }

        private static void SetDieValue(
            IntegerUpDown dieNumber,
            IntegerUpDown modifierNumber,
            RadioButton plusButton,
            TextBlock resultNumber)
        {
            dieNumber.Value = 1;
            modifierNumber.Value = 0;
            plusButton.IsChecked = true;
            resultNumber.Text = Zero;
        }

        private void SetDefaultDivideValues()
        {
            this.PlayersInput.Text = Zero;
            this.CopperInput.Text = Zero;
            this.SilverInput.Text = Zero;
            this.ElectrumInput.Text = Zero;
            this.GoldInput.Text = Zero;
            this.PlatinumInput.Text = Zero;
        }

        private void DrawDivideLoot()
        {
            this.DivideLootDataGrid.Items.Clear();

            foreach (var player in this.Players)
            {
                this.DivideLootDataGrid.Items.Add(player);
            }
        }

        private void GetPlayers()
        {
            _ = int.TryParse(this.PlayersInput.Text, out int numPlayers);
            numPlayers = Math.Max(0, numPlayers);
            this.Players.Clear();

            for (int i = 0; i < numPlayers; i++)
            {
                if (this.RealNamesToggleButton.IsChecked ?? false)
                {
                    this.Players.Add(new Player(this.playerNames[i]));
                }
                else
                {
                    this.Players.Add(new Player($"Player {i + 1}"));
                }
            }
        }

        private Loot GetLoot()
        {
            _ = int.TryParse(this.CopperInput.Text, out int cp);
            _ = int.TryParse(this.SilverInput.Text, out int sp);
            _ = int.TryParse(this.ElectrumInput.Text, out int ep);
            _ = int.TryParse(this.GoldInput.Text, out int gp);
            _ = int.TryParse(this.PlatinumInput.Text, out int pp);

            cp = Math.Max(0, cp);
            sp = Math.Max(0, sp);
            ep = Math.Max(0, ep);
            gp = Math.Max(0, gp);
            pp = Math.Max(0, pp);

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
            SetDieValue(this.D4NumberUpDown, this.D4ModifierUpDown, this.D4Plus, this.D4Result);
            SetDieValue(this.D6NumberUpDown, this.D6ModifierUpDown, this.D6Plus, this.D6Result);
            SetDieValue(this.D8NumberUpDown, this.D8ModifierUpDown, this.D8Plus, this.D8Result);
            SetDieValue(this.D10NumberUpDown, this.D10ModifierUpDown, this.D10Plus, this.D10Result);
            SetDieValue(this.D100NumberUpDown, this.D100ModifierUpDown, this.D100Plus, this.D100Result);
            SetDieValue(this.D12NumberUpDown, this.D12ModifierUpDown, this.D12Plus, this.D12Result);
            SetDieValue(this.D20NumberUpDown, this.D20ModifierUpDown, this.D20Plus, this.D20Result);
            SetDieValue(this.DxNumberUpDown, this.DxModifierUpDown, this.DxPlus, this.DxResult);
            this.DxDieUpDown.Value = 1;
            this.CustomResult.Text = Zero;
        }

        private string RollDice(int diceNumber, int diceSides, int modified, bool isPlus)
        {
            var rolledDice = DiceRoll.RollDice(diceNumber, diceSides);
            var total = rolledDice.Sum();

            if (isPlus)
            {
                total += modified;
            }
            else
            {
                total -= modified;
            }

            total = Math.Max(1, total);

            this.RollHistory.Add(new DiceRoll(diceSides, rolledDice, $" {(isPlus ? " + " : " - ")}{modified}"));
            this.DrawDiceHistory();

            return total.ToString();
        }

        private void ClearDivideLoot()
        {
            this.SetDefaultDivideValues();
            this.DivideLootDataGrid.Items.Clear();
            this.Players.Clear();
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

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Equals(Zero))
            {
                ((TextBox)sender).Text = string.Empty;
            }
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            var isNumber = int.TryParse(((TextBox)sender).Text, out int num);

            if (!isNumber || num <= 0)
            {
                ((TextBox)sender).Text = Zero;
            }
        }

        private void ButtonResetHistory_Click(object sender, RoutedEventArgs e)
        {
            this.SetDefaultDiceValues();
            this.RollHistory.Clear();
            this.DrawDiceHistory();
            this.CustomInputTextBox.Text = string.Empty;
        }

        private void ButtonRoll_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            switch (button.Name)
            {
                case "ButtonRollD4":
                    this.D4Result.Text = this.RollDice(this.D4NumberUpDown.Value, 4, this.D4ModifierUpDown.Value, this.D4Plus.IsChecked ?? false);
                    break;
                case "ButtonRollD6":
                    this.D6Result.Text = this.RollDice(this.D6NumberUpDown.Value, 6, this.D6ModifierUpDown.Value, this.D6Plus.IsChecked ?? false);
                    break;
                case "ButtonRollD8":
                    this.D8Result.Text = this.RollDice(this.D8NumberUpDown.Value, 8, this.D8ModifierUpDown.Value, this.D8Plus.IsChecked ?? false);
                    break;
                case "ButtonRollD10":
                    this.D10Result.Text = this.RollDice(this.D10NumberUpDown.Value, 10, this.D10ModifierUpDown.Value, this.D10Plus.IsChecked ?? false);
                    break;
                case "ButtonRollD100":
                    this.D100Result.Text = this.RollDice(this.D100NumberUpDown.Value, 100, this.D100ModifierUpDown.Value, this.D100Plus.IsChecked ?? false);
                    break;
                case "ButtonRollD12":
                    this.D12Result.Text = this.RollDice(this.D12NumberUpDown.Value, 12, this.D12ModifierUpDown.Value, this.D12Plus.IsChecked ?? false);
                    break;
                case "ButtonRollD20":
                    this.D20Result.Text = this.RollDice(this.D20NumberUpDown.Value, 20, this.D20ModifierUpDown.Value, this.D20Plus.IsChecked ?? false);
                    break;
                case "ButtonRollDx":
                    this.DxResult.Text = this.RollDice(this.DxNumberUpDown.Value, this.DxDieUpDown.Value, this.DxModifierUpDown.Value, this.DxPlus.IsChecked ?? false);
                    break;
            }
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (this.DivideLootInputHasFocus)
                    {
                        this.DivideLoot();
                    }
                    else if (this.CustomInputTextBox.IsFocused)
                    {
                        this.ParseCustomInput();
                    }

                    break;
                case Key.Escape:
                    if (this.DivideLootInputHasFocus)
                    {
                        this.ClearDivideLoot();
                    }

                    break;
            }
        }

        private void PlayersInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.PlayersInput.Text.Equals(MagicNumber))
            {
                this.RealNamesToggleButton.IsChecked = false;
                this.RealNamesToggleButton.Foreground = Brushes.SteelBlue;
                this.RealNamesToggleButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.RealNamesToggleButton.IsChecked = false;
                this.RealNamesToggleButton.Visibility = Visibility.Hidden;
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
                    }

                    break;
                case Key.Down:
                    if (this.CustomInputTextBox.IsFocused)
                    {
                        this.CustomInputTextBox.Text = this.DiceHistory.Forward();
                    }

                    break;
            }
        }
    }
}
