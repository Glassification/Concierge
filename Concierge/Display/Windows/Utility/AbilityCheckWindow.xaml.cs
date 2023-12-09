// <copyright file="AbilityCheckWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;

    using Concierge.Character;
    using Concierge.Character.AbilitySaves;
    using Concierge.Character.Enums;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Tools.DiceRoller;

    /// <summary>
    /// Interaction logic for AbilityCheckWindow.xaml.
    /// </summary>
    public partial class AbilityCheckWindow : ConciergeWindow
    {
        private IAbility ability;

        public AbilityCheckWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ability = SavingThrow.Empty;
        }

        public override string HeaderText => $"{this.GetName()} {this.GetAction()}";

        public override string WindowName => nameof(AbilityCheckWindow);

        public override AbilitySave ShowAbilityCheckWindow(IAbility ability, int value)
        {
            this.ability = ability;

            var name = this.GetName();
            var action = this.GetAction();

            this.HeaderLabel.Text = this.HeaderText;
            this.ModifierUpDown.Value = 0;
            this.AbilityBonusTextBox.Text = ability.Bonus.ToString();
            this.RollResultLabel.Text = $"Roll {name.GetDeterminer(true)} {name} {action}...";
            if (action.Equals("Check"))
            {
                this.AbilityLabel.Text = "Skill Bonus:";
                this.AbilityModifierLabel.Text = "Skill Modifier:";
            }
            else
            {
                this.AbilityLabel.Text = "Saving Throw Bonus:";
                this.AbilityModifierLabel.Text = "Ability Save Modifier:";
            }

            this.ShowConciergeWindow();

            return AbilitySave.None;
        }

        private string RollDice()
        {
            var modifier = int.Parse(this.AbilityBonusTextBox.Text) + this.ModifierUpDown.Value;
            var diceRoll1 = new DiceRoll(Dice.D20, 1, modifier);
            var diceRoll2 = new DiceRoll(Dice.D20, 1, modifier);

            return this.ability.StatusChecks switch
            {
                StatusChecks.Advantage => $"Rolled {(diceRoll1.Total > diceRoll2.Total ? diceRoll1 : diceRoll2)} with Advantage.",
                StatusChecks.Disadvantage => $"Rolled {(diceRoll1.Total > diceRoll2.Total ? diceRoll2 : diceRoll1)} with Disadvantage.",
                StatusChecks.Fail => $"Save was an automatic Failure. {diceRoll1}",
                _ => $"Rolled {diceRoll1}",
            };
        }

        private string GetName()
        {
            return this.ability.GetType().Name.FormatFromEnum();
        }

        private string GetAction()
        {
            var name = this.ability.GetType().Name;
            if (name.Equals(nameof(Strength)) ||
                name.Equals(nameof(Dexterity)) ||
                name.Equals(nameof(Constitution)) ||
                name.Equals(nameof(Intelligence)) ||
                name.Equals(nameof(Wisdom)) ||
                name.Equals(nameof(Charisma)))
            {
                return "Save";
            }

            return "Check";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            this.RollResultLabel.Text = this.RollDice();
        }
    }
}
