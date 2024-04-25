// <copyright file="MagicClassWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Magic;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for MagicClassWindow.xaml.
    /// </summary>
    public partial class MagicClassWindow : ConciergeWindow
    {
        public MagicClassWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ClassNameComboBox.ItemsSource = DefaultItems;
            this.AbilityComboBox.ItemsSource = ComboBoxGenerator.AbilitiesComboBox();
            this.ConciergePage = ConciergePage.None;
            this.SelectedClass = new MagicalClass();
            this.MagicClasses = [];
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.ClassNameComboBox);
            this.SetMouseOverEvents(this.AbilityComboBox);
            this.SetMouseOverEvents(this.CantripsUpDown);
            this.SetMouseOverEvents(this.SpellsUpDown);
            this.SetMouseOverEvents(this.LevelUpDown);
            this.SetMouseOverEvents(this.AttackBonusTextBox, this.AttackBonusTextBackground);
            this.SetMouseOverEvents(this.SpellSaveTextBox, this.SpellSaveTextBackground);
            this.SetMouseOverEvents(this.PreparedSpellsTextBox, this.PreparedSpellsTextBackground);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Magical Class";

        public override string WindowName => nameof(MagicClassWindow);

        public bool ItemsAdded { get; private set; }

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Defaults.MagicClasses, Program.CustomItemService.GetItems<MagicalClass>());

        private bool Editing { get; set; }

        private bool SettingValues { get; set; }

        private MagicalClass SelectedClass { get; set; }

        private List<MagicalClass> MagicClasses { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.MagicClasses = Program.CcsFile.Character.SpellCasting.MagicalClasses;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T magicClasses)
        {
            if (magicClasses is not List<MagicalClass> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.MagicClasses = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T magicClass)
        {
            if (magicClass is not MagicalClass castItem)
            {
                return;
            }

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedClass = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            if (this.Editing)
            {
                this.UpdateMagicClass(this.SelectedClass);
            }
            else
            {
                this.MagicClasses.Add(this.ToMagicClass());
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(MagicalClass magicClass)
        {
            this.SettingValues = true;
            this.ClassNameComboBox.Text = magicClass.Name;
            this.AbilityComboBox.Text = magicClass.Ability.ToString();
            this.AttackBonusTextBox.Text = magicClass.Attack.ToString();
            this.SpellSaveTextBox.Text = magicClass.Save.ToString();
            this.LevelUpDown.Value = magicClass.Level;
            this.CantripsUpDown.Value = magicClass.KnownCantrips;
            this.SpellsUpDown.Value = magicClass.KnownSpells;
            this.PreparedSpellsTextBox.Text = magicClass.PreparedSpells.ToString();
            this.SettingValues = false;
        }

        private void ClearFields(string name = "")
        {
            this.SettingValues = true;
            this.ClassNameComboBox.Text = name;
            this.AbilityComboBox.Text = Abilities.NONE.ToString();
            this.AttackBonusTextBox.Text = "0";
            this.SpellSaveTextBox.Text = "0";
            this.LevelUpDown.Value = 0;
            this.CantripsUpDown.Value = 0;
            this.SpellsUpDown.Value = 0;
            this.PreparedSpellsTextBox.Text = "0";
            this.SettingValues = false;
        }

        private void UpdateMagicClass(MagicalClass magicClass)
        {
            var oldItem = magicClass.DeepCopy();

            magicClass.Name = this.ClassNameComboBox.Text;
            magicClass.Ability = this.AbilityComboBox.Text.ToEnum<Abilities>();
            magicClass.Level = this.LevelUpDown.Value;
            magicClass.KnownCantrips = this.CantripsUpDown.Value;
            magicClass.KnownSpells = this.SpellsUpDown.Value;

            if (!magicClass.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<MagicalClass>(magicClass, oldItem, this.ConciergePage));
            }
        }

        private MagicalClass Create()
        {
            return new MagicalClass(Program.CcsFile.CharacterService)
            {
                Name = this.ClassNameComboBox.Text,
                Ability = this.AbilityComboBox.Text.ToEnum<Abilities>(),
                Level = this.LevelUpDown.Value,
                KnownSpells = this.SpellsUpDown.Value,
                KnownCantrips = this.CantripsUpDown.Value,
            };
        }

        private MagicalClass ToMagicClass()
        {
            this.ItemsAdded = true;
            var magicClass = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<MagicalClass>(this.MagicClasses, magicClass, this.ConciergePage));

            return magicClass;
        }

        private void RefreshFields()
        {
            string ability = this.AbilityComboBox.SelectedItem.ToString() ?? Abilities.NONE.ToString();
            this.AttackBonusTextBox.Text = Program.CcsFile.CharacterService.CalculateBonusWithProficiency(ability.ToEnum<Abilities>()).ToString();
            this.SpellSaveTextBox.Text = (Program.CcsFile.CharacterService.CalculateBonusWithProficiency(ability.ToEnum<Abilities>()) + Constants.BaseDC).ToString();
            this.PreparedSpellsTextBox.Text = Program.CcsFile.Character.SpellCasting.Spells.Where(x => (x.Class?.Equals(this.ClassNameComboBox.SelectedItem.ToString()) ?? false) && x.Prepared)?.ToList()?.Count.ToString() ?? "0";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.MagicClasses.Add(this.ToMagicClass());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void AbilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.SettingValues)
            {
                this.RefreshFields();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ClassNameComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is MagicalClass magicClass)
            {
                this.FillFields(magicClass);
            }
            else
            {
                this.ClearFields(this.ClassNameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ClassNameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.Show(
                    "Could not save the Spellcasting Class.\nA name is required before saving a custom item.",
                    "Warning",
                    ConciergeButtons.Ok,
                    ConciergeIcons.Alert);
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.ClassNameComboBox.ItemsSource = DefaultItems;
        }
    }
}
