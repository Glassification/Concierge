// <copyright file="ModifySpellClassWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySpellClassWindow.xaml.
    /// </summary>
    public partial class ModifySpellClassWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifySpellClassWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.ClassNameComboBox.ItemsSource = Constants.Classes;
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
            this.conciergePage = conciergePage;
        }

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Magic Class";

        private bool SettingValues { get; set; }

        private MagicClass SelectedClass { get; set; }

        private List<MagicClass> MagicClasses { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.MagicClasses = Program.CcsFile.Character.MagicClasses;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void AddClass(List<MagicClass> magicClasses)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.MagicClasses = magicClasses;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();
        }

        public void EditClass(MagicClass magicClass)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedClass = magicClass;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(magicClass);
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields(MagicClass magicClass)
        {
            this.SettingValues = true;

            this.LevelUpDown.UpdatingValue();
            this.CantripsUpDown.UpdatingValue();
            this.SpellsUpDown.UpdatingValue();

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

            this.LevelUpDown.UpdatingValue();
            this.CantripsUpDown.UpdatingValue();
            this.SpellsUpDown.UpdatingValue();

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
            var oldItem = magicClass.DeepCopy() as MagicClass;

            magicClass.Name = this.ClassNameComboBox.Text;
            magicClass.Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text);
            magicClass.Level = this.LevelUpDown.Value ?? 0;
            magicClass.KnownCantrips = this.CantripsUpDown.Value ?? 0;
            magicClass.KnownSpells = this.SpellsUpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<MagicClass>(magicClass, oldItem, this.conciergePage));
        }

        private MagicClass ToMagicClass()
        {
            this.ItemsAdded = true;

            var magicClass = new MagicClass()
            {
                Name = this.ClassNameComboBox.Text,
                Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text),
                Level = this.LevelUpDown.Value ?? 0,
                KnownSpells = this.SpellsUpDown.Value ?? 0,
                KnownCantrips = this.CantripsUpDown.Value ?? 0,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<MagicClass>(this.MagicClasses, magicClass, this.conciergePage));

            return magicClass;
        }

        private void RefreshFields()
        {
            this.AttackBonusTextBlock.Text = Utilities.CalculateBonusFromAbility((Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.SelectedItem.ToString()), Program.CcsFile.Character).ToString();
            this.SpellSaveTextBlock.Text = (Utilities.CalculateBonusFromAbility((Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.SelectedItem.ToString()), Program.CcsFile.Character) + Constants.BaseDC).ToString();
            this.PreparedSpellsTextBlock.Text = Program.CcsFile.Character.Spells.Where(x => (x.Class?.Equals(this.ClassNameComboBox.SelectedItem.ToString()) ?? false) && x.Prepared).ToList().Count.ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateMagicClass(this.SelectedClass);
            }
            else
            {
                this.MagicClasses.Add(this.ToMagicClass());
            }

            this.HideConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.MagicClasses.Add(this.ToMagicClass());
            this.ClearFields();

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
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
