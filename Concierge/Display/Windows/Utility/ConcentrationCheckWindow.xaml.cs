// <copyright file="ConcentrationCheckWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Character.AbilitySaves;
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Tools.DiceRoller;

    /// <summary>
    /// Interaction logic for ConcentrationCheckWindow.xaml.
    /// </summary>
    public partial class ConcentrationCheckWindow : ConciergeWindow
    {
        private IAbility ability;
        private AbilitySave result;
        private int difficultyClass;

        public ConcentrationCheckWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ability = SavingThrow.Empty;
        }

        public override string HeaderText => "Concentration Check";

        public override string WindowName => nameof(ConcentrationCheckWindow);

        public override AbilitySave ShowAbilityCheckWindow(IAbility ability, int value)
        {
            this.difficultyClass = Constants.Concentration(value);
            this.ability = ability;

            this.ConstitutionBonusUpDown.Value = 0;
            this.ConstitutionSaveTextBox.Text = ability.Bonus.ToString();
            this.DamageTextBox.Text = value.ToString();
            this.DifficultyClassTextBox.Text = this.difficultyClass.ToString();
            this.RollResultLabel.Text = "Roll a constitution check...";

            this.ShowConciergeWindow();

            return this.result;
        }

        private string RollDice()
        {
            var modifier = int.Parse(this.ConstitutionSaveTextBox.Text) + this.ConstitutionBonusUpDown.Value;
            var diceRoll1 = new DiceRoll(Dice.D20, 1, modifier);
            var diceRoll2 = new DiceRoll(Dice.D20, 1, modifier);

            switch (this.ability.StatusChecks)
            {
                default:
                case StatusChecks.Normal:
                    this.result = diceRoll1.Total >= this.difficultyClass ? AbilitySave.Success : AbilitySave.Failure;
                    return $"Save was a {this.result}. {diceRoll1}";
                case StatusChecks.Advantage:
                    var advRoll = diceRoll1.Total > diceRoll2.Total ? diceRoll1 : diceRoll2;
                    this.result = advRoll.Total >= this.difficultyClass ? AbilitySave.Success : AbilitySave.Failure;
                    return $"Save was a {this.result}. {advRoll} with Advantage.";
                case StatusChecks.Disadvantage:
                    var disRoll = diceRoll1.Total > diceRoll2.Total ? diceRoll2 : diceRoll1;
                    this.result = disRoll.Total >= this.difficultyClass ? AbilitySave.Success : AbilitySave.Failure;
                    return $"Save was a {this.result}. {disRoll} with Disadvantage.";
                case StatusChecks.Fail:
                    this.result = AbilitySave.Failure;
                    return $"Save was an automatic {this.result}. {diceRoll1}";
            }
        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            this.RollResultLabel.Text = this.RollDice();
            this.RollResultLabel.Foreground = this.result == AbilitySave.Success ? ConciergeBrushes.Mint : Brushes.IndianRed;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.result = AbilitySave.None;
            this.CloseConciergeWindow();
        }
    }
}
