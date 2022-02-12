// <copyright file="ModifySpellClassWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for ModifySpellClassWindow.xaml.
    /// </summary>
    public partial class ModifySpellClassWindow : ConciergeWindow
    {
        public ModifySpellClassWindow()
        {
            this.InitializeComponent();
            this.ClassNameComboBox.ItemsSource = Constants.Classes;
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
            this.ConciergePage = ConciergePage.None;
            this.SelectedClass = new MagicClass();
            this.MagicClasses = new List<MagicClass>();
        }

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Magic Class";

        private bool SettingValues { get; set; }

        private MagicClass SelectedClass { get; set; }

        private List<MagicClass> MagicClasses { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.MagicClasses = Program.CcsFile.Character.MagicClasses;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T magicClasses)
        {
            if (magicClasses is not List<MagicClass> castItem)
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
            if (magicClass is not MagicClass castItem)
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
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateMagicClass(this.SelectedClass);
            }
            else
            {
                this.MagicClasses.Add(this.ToMagicClass());
            }

            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields(MagicClass magicClass)
        {
            this.SettingValues = true;

            this.ClassNameComboBox.Text = magicClass.Name;
            this.AbilityComboBox.Text = magicClass.Ability.ToString();
            this.AttackBonusTextBlock.Text = magicClass.Attack.ToString();
            this.SpellSaveTextBlock.Text = magicClass.Save.ToString();
            this.LevelUpDown.Value = magicClass.Level;
            this.CantripsUpDown.Value = magicClass.KnownCantrips;
            this.SpellsUpDown.Value = magicClass.KnownSpells;
            this.PreparedSpellsTextBlock.Text = magicClass.PreparedSpells.ToString();

            this.SettingValues = false;
        }

        private void ClearFields()
        {
            this.SettingValues = true;

            this.ClassNameComboBox.Text = string.Empty;
            this.AbilityComboBox.Text = Abilities.NONE.ToString();
            this.AttackBonusTextBlock.Text = "0";
            this.SpellSaveTextBlock.Text = "0";
            this.LevelUpDown.Value = 0;
            this.CantripsUpDown.Value = 0;
            this.SpellsUpDown.Value = 0;
            this.PreparedSpellsTextBlock.Text = "0";

            this.SettingValues = false;
        }

        private void UpdateMagicClass(MagicClass magicClass)
        {
            var oldItem = magicClass.DeepCopy();

            magicClass.Name = this.ClassNameComboBox.Text;
            magicClass.Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text);
            magicClass.Level = this.LevelUpDown.Value;
            magicClass.KnownCantrips = this.CantripsUpDown.Value;
            magicClass.KnownSpells = this.SpellsUpDown.Value;

            Program.UndoRedoService.AddCommand(new EditCommand<MagicClass>(magicClass, oldItem, this.ConciergePage));
        }

        private MagicClass ToMagicClass()
        {
            this.ItemsAdded = true;

            var magicClass = new MagicClass()
            {
                Name = this.ClassNameComboBox.Text,
                Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text),
                Level = this.LevelUpDown.Value,
                KnownSpells = this.SpellsUpDown.Value,
                KnownCantrips = this.CantripsUpDown.Value,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<MagicClass>(this.MagicClasses, magicClass, this.ConciergePage));

            return magicClass;
        }

        private void RefreshFields()
        {
            string ability = this.AbilityComboBox.SelectedItem.ToString() ?? Abilities.NONE.ToString();
            this.AttackBonusTextBlock.Text = CharacterUtility.CalculateBonusFromAbility((Abilities)Enum.Parse(typeof(Abilities), ability), Program.CcsFile.Character).ToString();
            this.SpellSaveTextBlock.Text = (CharacterUtility.CalculateBonusFromAbility((Abilities)Enum.Parse(typeof(Abilities), ability), Program.CcsFile.Character) + Constants.BaseDC).ToString();
            this.PreparedSpellsTextBlock.Text = Program.CcsFile.Character.Spells.Where(x => (x.Class?.Equals(this.ClassNameComboBox.SelectedItem.ToString()) ?? false) && x.Prepared).ToList().Count.ToString();
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

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.MagicClasses.Add(this.ToMagicClass());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!this.SettingValues)
            {
                this.RefreshFields();
            }
        }
    }
}
