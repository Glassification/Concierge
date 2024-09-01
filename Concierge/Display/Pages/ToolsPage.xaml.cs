// <copyright file="ToolsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Services;
    using Concierge.Tools;
    using Concierge.Tools.DiceRoller;
    using Concierge.Tools.Enums;
    using Concierge.Tools.LootDivider;

    /// <summary>
    /// Interaction logic for ToolsPage.xaml.
    /// </summary>
    public partial class ToolsPage : ConciergePage
    {
        private readonly HistoryReadWriter historyReadWriter;
        private readonly string diceHistoryFile = Path.Combine(ConciergeFiles.HistoryDirectory, ConciergeFiles.DiceHistoryName);
        private readonly List<Player> players = [];
        private readonly List<IDiceRoll> rollHistory = [];
        private readonly History diceHistory;

        public ToolsPage()
        {
            this.InitializeComponent();

            this.historyReadWriter = new HistoryReadWriter(Program.ErrorService);
            this.diceHistory = new History(this.historyReadWriter.ReadList<string>(this.diceHistoryFile), string.Empty);
            this.CoinComboBox.ItemsSource = ComboBoxGenerator.DivideLootComboBox(ConciergeBrushes.ControlForeDarkBlue);
            this.CoinComboBox.Text = CoinType.Gold.ToString();
            this.ConciergePages = ConciergePages.Tools;

            this.SetDefaultDiceValues();
            this.SetDefaultDivideValues();

            this.D4DiceRollDisplay.Initialize("Light", Brushes.White);
            this.D6DiceRollDisplay.Initialize("Dark", Brushes.White);
            this.D8DiceRollDisplay.Initialize("Light", Brushes.White);
            this.D10DiceRollDisplay.Initialize("Dark", Brushes.White);
            this.D100DiceRollDisplay.Initialize("Light", Brushes.White);
            this.D12DiceRollDisplay.Initialize("Dark", Brushes.White);
            this.D20DiceRollDisplay.Initialize("Light", Brushes.White);

            this.PlayersInput.Initialize(DivideLootSelection.Players);
            this.CopperInput.Initialize(DivideLootSelection.Copper);
            this.SilverInput.Initialize(DivideLootSelection.Silver);
            this.ElectrumInput.Initialize(DivideLootSelection.Electrum);
            this.GoldInput.Initialize(DivideLootSelection.Gold);
            this.PlatinumInput.Initialize(DivideLootSelection.Platinum);
        }

        public override void Draw(bool isNewCharacterSheet = false)
        {
            if (isNewCharacterSheet)
            {
                this.ClearDiceHistory();
                this.ClearDivideLoot();
            }
            else
            {
                this.DrawDiceHistory();
                this.DrawDivideLoot();
            }
        }

        public override void Edit(object itemToEdit)
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
            this.rollHistory.ForEach(dice => this.RollDiceHistoryDataGrid.Items.Add(dice));
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
            foreach (var player in this.players)
            {
                this.DivideLootDataGrid.Items.Add(player);
                totalValue += player.Total;
            }
        }

        private void GetPlayers()
        {
            this.players.Clear();

            var party = Program.CcsFile.Character.Adventurers.Where(x => x.Status == PartyStatus.Alive).ToList();
            var namedPlayers = party.Count == this.PlayersInput.InputValue;
            for (int i = 0; i < this.PlayersInput.InputValue; i++)
            {
                this.players.Add(new Player(namedPlayers ? party[i].CharacterName : $"Player {i + 1}"));
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
            var maxValue = loot.Total / this.players.Count;
            if (this.players.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < Constants.Currencies; i++)
            {
                while (loot.CurrencyList[i] > 0)
                {
                    for (int j = 0; j < this.players.Count; j++)
                    {
                        if (loot.CurrencyList[i] < 1)
                        {
                            break;
                        }
                        else if (this.players[j].Total < maxValue)
                        {
                            this.players[j].CurrencyList[i]++;
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
            this.PlayersInput.ClearInputValue();
            this.CopperInput.ClearInputValue();
            this.SilverInput.ClearInputValue();
            this.ElectrumInput.ClearInputValue();
            this.GoldInput.ClearInputValue();
            this.PlatinumInput.ClearInputValue();
        }

        private void ParseCustomInput()
        {
            var input = this.CustomInputTextBox.Text;
            if (input.IsNullOrWhiteSpace())
            {
                return;
            }

            var result = new CustomDiceRoll(input);

            this.rollHistory.Add(result);
            this.diceHistory.Add(input);
            this.DrawDiceHistory();
            this.CustomInputTextBox.Text = string.Empty;
            this.CustomResult.Text = result.Total.ToString();

            this.historyReadWriter.Append(this.diceHistoryFile, input);
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
                    this.CustomInputTextBox.Text = this.diceHistory.Backward();
                    this.CustomInputTextBox.Select(this.CustomInputTextBox.Text.Length, 0);
                    break;
                case HistoryDirection.Forward:
                    this.CustomInputTextBox.Text = this.diceHistory.Forward();
                    this.CustomInputTextBox.Select(this.CustomInputTextBox.Text.Length, 0);
                    break;
            }
        }

        private void ClearDivideLoot()
        {
            this.SetDefaultDivideValues();
            this.DivideLootDataGrid.Items.Clear();
            this.players.Clear();
            this.CoinComboBox.Text = CoinType.Gold.ToString();
        }

        private void ClearDiceHistory()
        {
            this.SetDefaultDiceValues();
            this.rollHistory.Clear();
            this.DrawDiceHistory();
            this.CustomInputTextBox.Text = string.Empty;
        }

        private DivideLootInputControl GetCoinSpinner(string coin)
        {
            if (coin.EqualsIgnoreCase("players"))
            {
                return this.PlayersInput;
            }
            else if (coin.EqualsIgnoreCase("copper"))
            {
                return this.CopperInput;
            }
            else if (coin.EqualsIgnoreCase("silver"))
            {
                return this.SilverInput;
            }
            else if (coin.EqualsIgnoreCase("electrum"))
            {
                return this.ElectrumInput;
            }
            else if (coin.EqualsIgnoreCase("platinum"))
            {
                return this.PlatinumInput;
            }

            return this.GoldInput;
        }

        private void ResetHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearDiceHistory();
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
                this.rollHistory.Add(diceRoll);
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
                var lootInput = this.GetCoinSpinner(this.CoinComboBox.Text);
                lootInput.SetInputValue(amount);
            }
        }

        private void Input_Selection(object sender, RoutedEventArgs e)
        {
            if (sender is DivideLootInputControl divideLootInput)
            {
                SoundService.PlayUpdateValue();
                this.CoinComboBox.Text = divideLootInput.LabelText;
            }
        }
    }
}
