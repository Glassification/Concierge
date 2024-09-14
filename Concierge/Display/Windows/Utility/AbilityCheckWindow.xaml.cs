// <copyright file="AbilityCheckWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;

    using Concierge.Character.Aspects;
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
        private Attribute? attribute;
        private Skill? skill;

        public AbilityCheckWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => $"{this.GetName()} {this.GetAction()}";

        public override string WindowName => nameof(AbilityCheckWindow);

        public override AbilitySave ShowAbilityCheckWindow(Attribute attribute, int value)
        {
            this.attribute = attribute;

            var name = this.GetName();
            var action = this.GetAction();

            this.HeaderLabel.Text = this.HeaderText;
            this.ModifierUpDown.Value = 0;
            this.AbilityBonusTextBox.Text = (attribute.Proficiency ? (attribute.Bonus + Program.CcsFile.Character.ProficiencyBonus) : attribute.Bonus).ToString();
            this.RollResultLabel.Text = $"Roll {name.GetDeterminer(true)} {name} {action}...";
            this.AbilityLabel.Text = "Saving Throw Bonus:";
            this.AbilityModifierLabel.Text = "Ability Save Modifier:";

            this.ShowConciergeWindow();

            return AbilitySave.None;
        }

        public override AbilitySave ShowAbilityCheckWindow(Skill skill, int value)
        {
            this.skill = skill;

            var name = this.GetName();
            var action = this.GetAction();

            this.HeaderLabel.Text = this.HeaderText;
            this.ModifierUpDown.Value = 0;
            this.AbilityBonusTextBox.Text = Program.CcsFile.Character.Attributes.GetSkillBonus(skill, Program.CcsFile.Character.ProficiencyBonus).ToString();
            this.RollResultLabel.Text = $"Roll {name.GetDeterminer(true)} {name} {action}...";
            this.AbilityLabel.Text = "Skill Bonus:";
            this.AbilityModifierLabel.Text = "Skill Modifier:";

            this.ShowConciergeWindow();

            return AbilitySave.None;
        }

        private string RollDice()
        {
            var modifier = int.Parse(this.AbilityBonusTextBox.Text) + this.ModifierUpDown.Value;
            var diceRoll1 = new DiceRoll(Dice.D20, 1, modifier);
            var diceRoll2 = new DiceRoll(Dice.D20, 1, modifier);

            return this.GetStatus() switch
            {
                StatusChecks.Advantage => $"Rolled {(diceRoll1.Total > diceRoll2.Total ? diceRoll1 : diceRoll2)} with Advantage.",
                StatusChecks.Disadvantage => $"Rolled {(diceRoll1.Total > diceRoll2.Total ? diceRoll2 : diceRoll1)} with Disadvantage.",
                StatusChecks.Fail => $"Save was an automatic Failure. {diceRoll1}",
                _ => $"Rolled {diceRoll1}",
            };
        }

        private string GetName()
        {
            return (this.skill is null ? this.attribute?.Type.PascalCase() : this.skill?.Type.PascalCase()) ?? string.Empty;
        }

        private StatusChecks GetStatus()
        {
            var vitality = Program.CcsFile.Character.Vitality;
            return (this.skill is null ? this.attribute?.GetSaveStatus(vitality) : this.skill?.GetStatus(vitality)) ?? StatusChecks.Normal;
        }

        private string GetAction()
        {
            return this.attribute is null ? "Check" : "Save";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Logger.Info($"{this.HeaderText} {this.RollResultLabel.Text}.");
            this.ReturnAndClose();
        }

        private void RollButton_Click(object sender, RoutedEventArgs e)
        {
            this.RollResultLabel.Text = this.RollDice();
        }
    }
}
