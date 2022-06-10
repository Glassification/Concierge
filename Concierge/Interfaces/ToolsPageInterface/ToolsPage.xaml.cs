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
    using System.Windows.Media;

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
            this.D4DiceRollDisplay.ResetDiceValue();
            this.D6DiceRollDisplay.ResetDiceValue();
            this.D8DiceRollDisplay.ResetDiceValue();
            this.D10DiceRollDisplay.ResetDiceValue();
            this.D100DiceRollDisplay.ResetDiceValue();
            this.D12DiceRollDisplay.ResetDiceValue();
            this.D20DiceRollDisplay.ResetDiceValue();
            this.DxDiceRollDisplay.ResetDiceValue();

            this.CustomResult.Text = Zero;
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
