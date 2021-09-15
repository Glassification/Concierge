// <copyright file="ModifySpellClassWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySpellClassWindow.xaml.
    /// </summary>
    public partial class ModifySpellClassWindow : Window, IConciergeWindow
    {
        public ModifySpellClassWindow()
        {
            this.InitializeComponent();
            this.ClassNameComboBox.ItemsSource = Constants.Classes;
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private bool SettingValues { get; set; }

        private MagicClass SelectedClass { get; set; }

        private List<MagicClass> MagicClasses { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Magic Class";
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.MagicClasses = Program.CcsFile.Character.MagicClasses;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void AddClass(List<MagicClass> magicClasses)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Magic Class";
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.MagicClasses = magicClasses;

            this.ClearFields();
            this.ShowDialog();
        }

        public void EditClass(MagicClass magicClass)
        {
            this.HeaderTextBlock.Text = "Edit Magic Class";
            this.SelectedClass = magicClass;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(magicClass);
            this.ShowDialog();
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

        private void UpdateClass(MagicClass magicClass)
        {
            magicClass.Name = this.ClassNameComboBox.Text;
            magicClass.Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text);
            magicClass.Level = this.LevelUpDown.Value ?? 0;
            magicClass.KnownCantrips = this.CantripsUpDown.Value ?? 0;
            magicClass.KnownSpells = this.SpellsUpDown.Value ?? 0;
        }

        private MagicClass ToMagicClass()
        {
            return new MagicClass()
            {
                Name = this.ClassNameComboBox.Text,
                Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text),
                Level = this.LevelUpDown.Value ?? 0,
                KnownSpells = this.SpellsUpDown.Value ?? 0,
                KnownCantrips = this.CantripsUpDown.Value ?? 0,
            };
        }

        private void RefreshFields()
        {
            this.AttackBonusTextBlock.Text = Utilities.CalculateBonusFromAbility((Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.SelectedItem.ToString()), Program.CcsFile.Character).ToString();
            this.SpellSaveTextBlock.Text = (Utilities.CalculateBonusFromAbility((Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.SelectedItem.ToString()), Program.CcsFile.Character) + Constants.BaseDC).ToString();
            this.PreparedSpellsTextBlock.Text = Program.CcsFile.Character.Spells.Where(x => (x.Class?.Equals(this.ClassNameComboBox.SelectedItem.ToString()) ?? false) && x.Prepared).ToList().Count.ToString();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateClass(this.SelectedClass);
            }
            else
            {
                this.MagicClasses.Add(this.ToMagicClass());
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.MagicClasses.Add(this.ToMagicClass());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
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
