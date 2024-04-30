// <copyright file="ToolsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools;
    using Concierge.Tools.DiceRoller;
    using Concierge.Tools.Enums;
    using Concierge.Tools.LootDivider;

    /// <summary>
    /// Interaction logic for ToolsPage.xaml.
    /// </summary>
    public partial class ToolsPage : Page, IConciergePage
    {
        private readonly HistoryReadWriter historyReadWriter;
        private readonly string diceHistoryFile = Path.Combine(ConciergeFiles.HistoryDirectory, ConciergeFiles.DiceHistoryName);

        public ToolsPage()
        {
            this.InitializeComponent();

            this.historyReadWriter = new HistoryReadWriter(Program.ErrorService);
            this.Players = [];
            this.RollHistory = [];
            this.DiceHistory = new History(this.historyReadWriter.ReadList<string>(this.diceHistoryFile), string.Empty);
            this.CoinComboBox.ItemsSource = ComboBoxGenerator.DivideLootComboBox(ConciergeBrushes.ControlForeDarkBlue);
            this.CoinComboBox.Text = CoinType.Gold.ToString();

            this.SetDefaultDiceValues();
            this.SetDefaultDivideValues();

            this.D4DiceRollDisplay.Initialize("LightControlButtonStyle", Brushes.White);
            this.D6DiceRollDisplay.Initialize("DarkControlButtonStyle", Brushes.White);
            this.D8DiceRollDisplay.Initialize("LightControlButtonStyle", Brushes.White);
            this.D10DiceRollDisplay.Initialize("DarkControlButtonStyle", Brushes.White);
            this.D100DiceRollDisplay.Initialize("LightControlButtonStyle", Brushes.White);
            this.D12DiceRollDisplay.Initialize("DarkControlButtonStyle", Brushes.White);
            this.D20DiceRollDisplay.Initialize("LightControlButtonStyle", Brushes.White);

            this.PlayersInput.Initialize("PlayerControlButtonStyle", Brushes.White);
            this.CopperInput.Initialize("CopperControlButtonStyle", Brushes.Black);
            this.SilverInput.Initialize("SilverControlButtonStyle", Brushes.Black);
            this.ElectrumInput.Initialize("ElectrumControlButtonStyle", Brushes.Black);
            this.GoldInput.Initialize("GoldControlButtonStyle", Brushes.Black);
            this.PlatinumInput.Initialize("PlatinumControlButtonStyle", Brushes.Black);
        }

        public ConciergePage ConciergePage => ConciergePage.Tools;

        public bool HasEditableDataGrid => false;

        private List<Player> Players { get; set; }

        private List<IDiceRoll> RollHistory { get; }

        private History DiceHistory { get; set; }

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawDiceHistory();
            this.DrawDivideLoot();
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

        public void DrawDiceHistory()
        {
            this.RollDiceHistoryDataGrid.Items.Clear();
            this.RollHistory.ForEach(dice => this.RollDiceHistoryDataGrid.Items.Add(dice));

            if (this.RollDiceHistoryDataGrid.Items.Count > 0)
            {
                this.RollDiceHistoryDataGrid.SelectedItem = this.RollDiceHistoryDataGrid.Items[^1];
                this.RollDiceHistoryDataGrid.UpdateLayout();
                this.RollDiceHistoryDataGrid.ScrollIntoView(this.RollDiceHistoryDataGrid.SelectedItem);
            }
        }

        public void DrawDivideLoot()
        {
            var totalValue = 0.0;
            this.DivideLootDataGrid.Items.Clear();

            foreach (var player in this.Players)
            {
                this.DivideLootDataGrid.Items.Add(player);
                totalValue += player.Total;
            }
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

        private void SetDefaultDiceValues()
        {
            this.D4DiceRollDisplay.ResetDiceValue();
            this.D6DiceRollDisplay.ResetDiceValue();
            this.D8DiceRollDisplay.ResetDiceValue();
            this.D10DiceRollDisplay.ResetDiceValue();
            this.D100DiceRollDisplay.ResetDiceValue();
            this.D12DiceRollDisplay.ResetDiceValue();
            this.D20DiceRollDisplay.ResetDiceValue();

            this.CustomResult.Text = "0";
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

        private void ParseCustomInput()
        {
            var input = this.CustomInputTextBox.Text;
            if (input.IsNullOrWhiteSpace())
            {
                return;
            }

            try
            {
                var result = new CustomDiceRoll(input);

                this.RollHistory.Add(result);
                this.DiceHistory.Add(input);
                this.DrawDiceHistory();
                this.CustomInputTextBox.Text = string.Empty;
                this.CustomResult.Text = result.Total.ToString();

                this.historyReadWriter.Append(this.diceHistoryFile, input);
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
        }

        private void ScrollCustomInputHistory(HistoryDirection direction)
        {
            if (!this.CustomInputTextBox.IsFocused)
            {
                return;
            }

            switch (direction)
            {
                case HistoryDirection.Backward:
                    this.CustomInputTextBox.Text = this.DiceHistory.Backward();
                    this.CustomInputTextBox.Select(this.CustomInputTextBox.Text.Length, 0);
                    break;
                case HistoryDirection.Forward:
                    this.CustomInputTextBox.Text = this.DiceHistory.Forward();
                    this.CustomInputTextBox.Select(this.CustomInputTextBox.Text.Length, 0);
                    break;
            }
        }

        private void ClearDivideLoot()
        {
            this.SetDefaultDivideValues();
            this.DivideLootDataGrid.Items.Clear();
            this.Players.Clear();
            this.CoinComboBox.Text = CoinType.Gold.ToString();
        }

        private TextBlock GetCoinSpinner(string coin)
        {
            if (coin.Equals("players", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.PlayersInput.CoinAmount;
            }

            if (coin.Equals("copper", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.CopperInput.CoinAmount;
            }

            if (coin.Equals("silver", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.SilverInput.CoinAmount;
            }

            if (coin.Equals("electrum", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.ElectrumInput.CoinAmount;
            }

            if (coin.Equals("platinum", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.PlatinumInput.CoinAmount;
            }

            return this.GoldInput.CoinAmount;
        }

        private void ResetHistoryButton_Click(object sender, RoutedEventArgs e)
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
            this.ScrollCustomInputHistory(e.Key == Key.Up ? HistoryDirection.Backward : e.Key == Key.Down ? HistoryDirection.Forward : HistoryDirection.None);
        }

        private void DiceRollDisplay_DiceRolled(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is DiceRoll diceRoll)
            {
                this.RollHistory.Add(diceRoll);
                this.DrawDiceHistory();
            }
        }

        private void CustomInputTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.ScrollCustomInputHistory(e.Delta < 0 ? HistoryDirection.Forward : HistoryDirection.Backward);
        }

        private void DivideLootButton_Click(object sender, RoutedEventArgs e)
        {
            this.DivideLoot();
        }

        private void ResetLootButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearDivideLoot();
        }

        private void ClearLootButton_Click(object sender, RoutedEventArgs e)
        {
            this.DivideLootDataGrid.UnselectAll();
        }

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.RollDiceHistoryDataGrid.UnselectAll();
        }

        private void AddCoinButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ConciergeDesignButton button && button.Tag is int amount)
            {
                var textBlock = this.GetCoinSpinner(this.CoinComboBox.Text);
                textBlock.Text = $"{int.Parse(textBlock.Text) + amount}";
            }
        }
    }
}
