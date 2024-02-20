// <copyright file="LevelUpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.Windows;

    using Concierge.Character;
    using Concierge.Character.Dispositions;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for LevelUpWindow.xaml.
    /// </summary>
    public partial class LevelUpWindow : ConciergeWindow
    {
        public LevelUpWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.Class1DiceComboBox.ItemsSource = ComboBoxGenerator.DiceComboBox();
            this.Class2DiceComboBox.ItemsSource = ComboBoxGenerator.DiceComboBox();
            this.Class3DiceComboBox.ItemsSource = ComboBoxGenerator.DiceComboBox();

            this.Class1ModIntegerUpDown.Value = 0;
            this.Class2ModIntegerUpDown.Value = 0;
            this.Class3ModIntegerUpDown.Value = 0;
        }

        public override string HeaderText => "Level Up";

        public override string WindowName => nameof(LevelUpWindow);

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override object? ShowWindow()
        {
            this.FillFields();
            this.ShowConciergeWindow();

            return null;
        }

        private static void FillClass(
            ConciergeTextBox className,
            ConciergeTextBoxBackground classNameBackground,
            ConciergeComboBox hitDice,
            IntegerUpDownControl modifier,
            ConciergeTextButton button,
            Class characterClass)
        {
            className.Text = characterClass.Name;
            hitDice.Text = HitDice.GetHitDice(characterClass.Name).ToString();

            if (characterClass.Name.IsNullOrWhiteSpace())
            {
                DisplayUtility.SetControlEnableState(classNameBackground, false);
                DisplayUtility.SetControlEnableState(className, false);
                DisplayUtility.SetControlEnableState(hitDice, false);
                DisplayUtility.SetControlEnableState(modifier, false);
                DisplayUtility.SetControlEnableState(button, false);
            }
            else
            {
                button.Content = $"Level Up ({Math.Min(characterClass.Level + 1, Constants.MaxLevel)})";
                if (!CanLevelUp(characterClass))
                {
                    DisplayUtility.SetControlEnableState(button, false);
                }
            }
        }

        private static void LevelUpClass(ConciergeComboBox hitDice, IntegerUpDownControl modifier, Class characterClass)
        {
            var character = Program.CcsFile.Character;
            var hitDieEnum = hitDice.Text.ToEnum<Dice>();

            // TODO
            // character.LevelUp(hitDieEnum, characterClass.ClassNumber, modifier.Value);
        }

        private static bool CanLevelUp(Class characterClass)
        {
            return characterClass.Level < Constants.MaxLevel && Program.CcsFile.Character.Disposition.Level < Constants.MaxLevel;
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
                character.Disposition.Class1);
            FillClass(
                this.Class2NameTextBox,
                this.Class2NameTextBoxBackground,
                this.Class2DiceComboBox,
                this.Class2ModIntegerUpDown,
                this.Class2LevelUpButton,
                character.Disposition.Class2);
            FillClass(
                this.Class3NameTextBox,
                this.Class3NameTextBoxBackground,
                this.Class3DiceComboBox,
                this.Class3ModIntegerUpDown,
                this.Class3LevelUpButton,
                character.Disposition.Class3);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
            this.Result = ConciergeResult.Exit;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
            this.Result = ConciergeResult.Cancel;
        }

        private void LevelUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeTextButton button)
            {
                this.CloseConciergeWindow();
                return;
            }

            if (button.Name.Contains('1') && CanLevelUp(Program.CcsFile.Character.Disposition.Class1))
            {
                LevelUpClass(this.Class1DiceComboBox, this.Class1ModIntegerUpDown, Program.CcsFile.Character.Disposition.Class1);
            }
            else if (button.Name.Contains('2') && CanLevelUp(Program.CcsFile.Character.Disposition.Class2))
            {
                LevelUpClass(this.Class2DiceComboBox, this.Class2ModIntegerUpDown, Program.CcsFile.Character.Disposition.Class2);
            }
            else if (CanLevelUp(Program.CcsFile.Character.Disposition.Class3))
            {
                LevelUpClass(this.Class3DiceComboBox, this.Class3ModIntegerUpDown, Program.CcsFile.Character.Disposition.Class3);
            }

            this.FillFields();
            this.InvokeApplyChanges();
        }
    }
}
