using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Concierge.Presentation
{
    /// <summary>
    /// Interaction logic for ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : Page
    {
        public ToolsPage()
        {
            InitializeComponent();

            Players = new List<Player>();
            DiceHistory = new List<Die>();
            Random = new Random();

            SetDefaultDivideValues();
            SetDefaultDiceValues();
        }

        public void Draw()
        {
            FillDivideLootGrid();
            FillDiceHistoryGrid();
        }

        #region Divide Loot

        #region Methods

        private void SetDefaultDivideValues()
        {
            PlayersInput.Text = "0";
            CopperInput.Text = "0";
            SilverInput.Text = "0";
            ElectrumInput.Text = "0";
            GoldInput.Text = "0";
            PlatinumInput.Text = "0";
        }

        private void FillDivideLootGrid()
        {
            DivideLootDataGrid.Items.Clear();

            foreach (var player in Players)
            {
                DivideLootDataGrid.Items.Add(player);
            }
        }

        private void GetPlayers()
        {
            int numPlayers;

            int.TryParse(PlayersInput.Text, out numPlayers);
            Players.Clear();

            for (int i = 0; i < numPlayers; i++)
            {
                Players.Add(new Player($"Player {i+1}"));
            }
        }

        private Player GetLoot()
        {
            int cp, sp, ep, gp, pp;

            int.TryParse(CopperInput.Text, out cp);
            int.TryParse(SilverInput.Text, out sp);
            int.TryParse(ElectrumInput.Text, out ep);
            int.TryParse(GoldInput.Text, out gp);
            int.TryParse(PlatinumInput.Text, out pp);

            return new Player(cp, sp, ep, gp, pp);
        }

        private void Distribute(Player loot)
        {
            bool end = false;
            double maxValue = loot.Total / Players.Count;

            if (Players.Count > 0)
            {
                for (int i = 0; i < Player.CURRENCIES; i++)
                {
                    while (loot.currency[i] > 0)
                    {
                        for (int j = 0; j < Players.Count && !end; j++)
                        {
                            if (loot.currency[i] < 1)
                            {
                                end = true;
                            }
                            else if (Players[j].Total < maxValue)
                            {
                                Players[j].currency[i]++;
                                loot.currency[i]--;
                            }
                        }
                        end = false;
                    }
                }
            }
        }

        #endregion

        #region Accessors

        private List<Player> Players { get; set; }

        #endregion

        #region Events

        private void ButtonDivideLoot_Click(object sender, RoutedEventArgs e)
        {
            Player loot;

            DivideLootDataGrid.Items.Clear();

            GetPlayers();
            loot = GetLoot();

            Distribute(loot);

            FillDivideLootGrid();
        }

        private void ButtonResetLoot_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultDivideValues();
            DivideLootDataGrid.Items.Clear();
            Players.Clear();
        }

        private void DivideLootDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void PlayersInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PlayersInput.Text.Equals("0"))
            {
                PlayersInput.Text = string.Empty;
            }
        }

        private void PlayersInput_LostFocus(object sender, RoutedEventArgs e)
        {
            int result;

            if (!int.TryParse(PlayersInput.Text, out result))
            {
                PlayersInput.Text = "0";
            }
        }

        private void CopperInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CopperInput.Text.Equals("0"))
            {
                CopperInput.Text = string.Empty;
            }
        }

        private void CopperInput_LostFocus(object sender, RoutedEventArgs e)
        {
            int result;

            if (!int.TryParse(CopperInput.Text, out result))
            {
                CopperInput.Text = "0";
            }
        }

        private void SilverInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SilverInput.Text.Equals("0"))
            {
                SilverInput.Text = string.Empty;
            }
        }

        private void SilverInput_LostFocus(object sender, RoutedEventArgs e)
        {
            int result;

            if (!int.TryParse(SilverInput.Text, out result))
            {
                SilverInput.Text = "0";
            }
        }

        private void ElectrumInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ElectrumInput.Text.Equals("0"))
            {
                ElectrumInput.Text = string.Empty;
            }
        }

        private void ElectrumInput_LostFocus(object sender, RoutedEventArgs e)
        {
            int result;

            if (!int.TryParse(ElectrumInput.Text, out result))
            {
                ElectrumInput.Text = "0";
            }
        }

        private void GoldInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GoldInput.Text.Equals("0"))
            {
                GoldInput.Text = string.Empty;
            }
        }

        private void GoldInput_LostFocus(object sender, RoutedEventArgs e)
        {
            int result;

            if (!int.TryParse(GoldInput.Text, out result))
            {
                GoldInput.Text = "0";
            }
        }

        private void PlatinumInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PlatinumInput.Text.Equals("0"))
            {
                PlatinumInput.Text = string.Empty;
            }
        }

        private void PlatinumInput_LostFocus(object sender, RoutedEventArgs e)
        {
            int result;

            if (!int.TryParse(PlatinumInput.Text, out result))
            {
                PlatinumInput.Text = "0";
            }
        }

        #endregion

        #endregion

        #region Roll Dice

        private void FillDiceHistoryGrid()
        {
            RollDiceHistoryDataGrid.Items.Clear();

            foreach (var dice in DiceHistory)
            {
                RollDiceHistoryDataGrid.Items.Add(dice);
            }
        }

        private void SetDefaultDiceValues()
        {
            D4NumberUpDown.Value = 1;
            D4ModifierUpDown.Value = 0;
            D4Plus.IsChecked = true;
            D4Result.Text = "0";

            D6NumberUpDown.Value = 1;
            D6ModifierUpDown.Value = 0;
            D6Plus.IsChecked = true;
            D6Result.Text = "0";

            D8NumberUpDown.Value = 1;
            D8ModifierUpDown.Value = 0;
            D8Plus.IsChecked = true;
            D8Result.Text = "0";

            D10NumberUpDown.Value = 1;
            D10ModifierUpDown.Value = 0;
            D10Plus.IsChecked = true;
            D10Result.Text = "0";

            D100NumberUpDown.Value = 1;
            D100ModifierUpDown.Value = 0;
            D100Plus.IsChecked = true;
            D100Result.Text = "0";

            D12NumberUpDown.Value = 1;
            D12ModifierUpDown.Value = 0;
            D12Plus.IsChecked = true;
            D12Result.Text = "0";

            D20NumberUpDown.Value = 1;
            D20ModifierUpDown.Value = 0;
            D20Plus.IsChecked = true;
            D20Result.Text = "0";

            DxDieUpDown.Value = 1;
            DxNumberUpDown.Value = 1;
            DxModifierUpDown.Value = 0;
            DxPlus.IsChecked = true;
            DxResult.Text = "0";
        }

        private string RollDice(int diceNumber, int diceSides, int modified, bool isPlus)
        {
            int total = 0, val;
            int[] rolledDice = new int[diceNumber];

            for (int i = 0; i < diceNumber; i++)
            {
                val = Random.Next(1, diceSides + 1);
                total += val;
                rolledDice[i] = val;
            }

            if (isPlus)
                total += modified;
            else
                total -= modified;

            total = Math.Max(1, total);

            DiceHistory.Add(new Die($"({diceNumber}d{diceSides}) {(isPlus?"+":"-")}{modified}", rolledDice, total));
            FillDiceHistoryGrid();

            return total.ToString();
        }

        private Random Random { get; }
        private List<Die> DiceHistory { get; }

        #endregion

        private void ButtonResetHistory_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultDiceValues();
            DiceHistory.Clear();
            FillDiceHistoryGrid();
        }

        private void ButtonRollD4_Click(object sender, RoutedEventArgs e)
        {
            D4Result.Text = RollDice((int)D4NumberUpDown.Value, 4, (int)D4ModifierUpDown.Value, (bool)D4Plus.IsChecked);
        }

        private void ButtonRollD6_Click(object sender, RoutedEventArgs e)
        {
            D6Result.Text = RollDice((int)D6NumberUpDown.Value, 6, (int)D6ModifierUpDown.Value, (bool)D6Plus.IsChecked);
        }

        private void ButtonRollD8_Click(object sender, RoutedEventArgs e)
        {
            D8Result.Text = RollDice((int)D8NumberUpDown.Value, 8, (int)D8ModifierUpDown.Value, (bool)D8Plus.IsChecked);
        }

        private void ButtonRollD10_Click(object sender, RoutedEventArgs e)
        {
            D10Result.Text = RollDice((int)D10NumberUpDown.Value, 10, (int)D10ModifierUpDown.Value, (bool)D10Plus.IsChecked);
        }

        private void ButtonRollD100_Click(object sender, RoutedEventArgs e)
        {
            D100Result.Text = RollDice((int)D100NumberUpDown.Value, 100, (int)D100ModifierUpDown.Value, (bool)D100Plus.IsChecked);
        }

        private void ButtonRollD12_Click(object sender, RoutedEventArgs e)
        {
            D12Result.Text = RollDice((int)D12NumberUpDown.Value, 12, (int)D12ModifierUpDown.Value, (bool)D12Plus.IsChecked);
        }

        private void ButtonRollD20_Click(object sender, RoutedEventArgs e)
        {
            D20Result.Text = RollDice((int)D20NumberUpDown.Value, 20, (int)D20ModifierUpDown.Value, (bool)D20Plus.IsChecked);
        }

        private void ButtonRollDx_Click(object sender, RoutedEventArgs e)
        {
            DxResult.Text = RollDice((int)DxNumberUpDown.Value, (int)DxDieUpDown.Value, (int)DxModifierUpDown.Value, (bool)DxPlus.IsChecked);
        }
    }
}
