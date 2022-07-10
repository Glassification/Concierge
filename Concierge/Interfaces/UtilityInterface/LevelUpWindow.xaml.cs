// <copyright file="LevelUpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Controls;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for LevelUpWindow.xaml.
    /// </summary>
    public partial class LevelUpWindow : ConciergeWindow
    {
        public LevelUpWindow()
        {
            this.InitializeComponent();

            this.Class1DiceComboBox.ItemsSource = Enum.GetValues(typeof(HitDie)).Cast<HitDie>();
            this.Class2DiceComboBox.ItemsSource = Enum.GetValues(typeof(HitDie)).Cast<HitDie>();
            this.Class3DiceComboBox.ItemsSource = Enum.GetValues(typeof(HitDie)).Cast<HitDie>();

            this.Class1ModIntegerUpDown.Value = 0;
            this.Class2ModIntegerUpDown.Value = 0;
            this.Class3ModIntegerUpDown.Value = 0;
        }

        public override string HeaderText => "Level Up";

        public override void ShowWindow()
        {
            this.FillFields();
            this.ShowConciergeWindow();
        }

        private static void FillClass(
            ConciergeTextBox className,
            ConciergeTextBoxBackground classNameBackground,
            ConciergeComboBox hitDice,
            IntegerUpDown modifier,
            ConciergeTextButton button,
            CharacterClass characterClass)
        {
            className.Text = characterClass.Name;
            hitDice.Text = CharacterUtility.GetHitDice(characterClass.Name).ToString();

            if (characterClass.Name.IsNullOrWhiteSpace())
            {
                classNameBackground.IsEnabled = false;
                classNameBackground.Opacity = 0.5;
                className.IsEnabled = false;
                className.Opacity = 0.5;
                hitDice.IsEnabled = false;
                hitDice.Opacity = 0.5;
                modifier.IsEnabled = false;
                modifier.Opacity = 0.5;
                button.IsEnabled = false;
                button.Opacity = 0.5;
            }
            else
            {
                button.Content = $"Level Up ({characterClass.Level + 1})";
            }
        }

        private static void LevelUpClass(ConciergeComboBox hitDice, IntegerUpDown modifier, CharacterClass characterClass)
        {
            var character = Program.CcsFile.Character;
            var hitDieEnum = (HitDie)Enum.Parse(typeof(HitDie), hitDice.Text);

            character.LevelUp(hitDieEnum, characterClass.Level, modifier.Value);
        }

        private void FillFields()
        {
            var character = Program.CcsFile.Character;

            FillClass(
                this.Class1NameTextBox,
                this.Class1NameTextBoxBackground,
                this.Class1DiceComboBox,
                this.Class1ModIntegerUpDown,
                this.Class1LevelUpButton,
                character.Properties.Class1);
            FillClass(
                this.Class2NameTextBox,
                this.Class2NameTextBoxBackground,
                this.Class2DiceComboBox,
                this.Class2ModIntegerUpDown,
                this.Class2LevelUpButton,
                character.Properties.Class2);
            FillClass(
                this.Class3NameTextBox,
                this.Class3NameTextBoxBackground,
                this.Class3DiceComboBox,
                this.Class3ModIntegerUpDown,
                this.Class3LevelUpButton,
                character.Properties.Class3);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void LevelUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeTextButton button)
            {
                this.CloseConciergeWindow();
                return;
            }

            if (button.Name.Contains('1'))
            {
                LevelUpClass(this.Class1DiceComboBox, this.Class1ModIntegerUpDown, Program.CcsFile.Character.Properties.Class1);
            }
            else if (button.Name.Contains('2'))
            {
                LevelUpClass(this.Class2DiceComboBox, this.Class2ModIntegerUpDown, Program.CcsFile.Character.Properties.Class2);
            }
            else
            {
                LevelUpClass(this.Class3DiceComboBox, this.Class3ModIntegerUpDown, Program.CcsFile.Character.Properties.Class3);
            }

            this.CloseConciergeWindow();
        }
    }
}
