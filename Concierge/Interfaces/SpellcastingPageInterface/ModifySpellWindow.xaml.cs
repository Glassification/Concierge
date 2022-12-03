// <copyright file="ModifySpellWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySpellWindow.xaml.
    /// </summary>
    public partial class ModifySpellWindow : ConciergeWindow
    {
        public ModifySpellWindow()
        {
            this.InitializeComponent();
            this.ForceRoundedCorners();

            this.SpellNameComboBox.ItemsSource = Constants.Spells;
            this.SchoolComboBox.ItemsSource = Enum.GetValues(typeof(ArcaneSchools)).Cast<ArcaneSchools>();
            this.ClassComboBox.ItemsSource = Constants.Classes;
            this.ConciergePage = ConciergePage.None;
            this.SelectedSpell = new Spell();
            this.Spells = new List<Spell>();
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Spell";

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private Spell SelectedSpell { get; set; }

        private List<Spell> Spells { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.Spells = Program.CcsFile.Character.Spells;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T spells)
        {
            if (spells is not List<Spell> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Spells = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T spell)
        {
            if (spell is not Spell castItem)
            {
                return;
            }

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedSpell = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateSpell(this.SelectedSpell);
            }
            else
            {
                this.Spells.Add(this.ToSpell());
            }

            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields(Spell spell)
        {
            this.PreparedCheckBox.UpdatingValue();
            this.RitualCheckBox.UpdatingValue();
            this.ConcentrationCheckBox.UpdatingValue();

            this.SpellNameComboBox.Text = spell.Name;
            this.PreparedCheckBox.IsChecked = spell.Prepared;
            this.LevelUpDown.Value = spell.Level;
            this.PageUpDown.Value = spell.Page;
            this.SchoolComboBox.Text = spell.School.ToString();
            this.RitualCheckBox.IsChecked = spell.Ritual;
            this.ComponentsTextBox.Text = spell.Components;
            this.ConcentrationCheckBox.IsChecked = spell.Concentration;
            this.RangeTextBox.Text = spell.Range;
            this.DurationTextBox.Text = spell.Duration;
            this.AreaTextBox.Text = spell.Area;
            this.SaveTextBox.Text = spell.Save;
            this.DamageTextBox.Text = spell.Damage;
            this.NotesTextBox.Text = spell.Description;
            this.ClassComboBox.Text = spell.Class;

            this.PreparedCheckBox.UpdatedValue();
            this.RitualCheckBox.UpdatedValue();
            this.ConcentrationCheckBox.UpdatedValue();
        }

        private void ClearFields()
        {
            this.PreparedCheckBox.UpdatingValue();
            this.RitualCheckBox.UpdatingValue();
            this.ConcentrationCheckBox.UpdatingValue();

            this.SpellNameComboBox.Text = string.Empty;
            this.PreparedCheckBox.IsChecked = false;
            this.LevelUpDown.Value = 0;
            this.PageUpDown.Value = 0;
            this.SchoolComboBox.Text = ArcaneSchools.Universal.ToString();
            this.RitualCheckBox.IsChecked = false;
            this.ComponentsTextBox.Text = string.Empty;
            this.ConcentrationCheckBox.IsChecked = false;
            this.RangeTextBox.Text = string.Empty;
            this.DurationTextBox.Text = string.Empty;
            this.AreaTextBox.Text = string.Empty;
            this.SaveTextBox.Text = string.Empty;
            this.ClassComboBox.Text = string.Empty;
            this.DamageTextBox.Text = string.Empty;
            this.NotesTextBox.Text = string.Empty;

            this.PreparedCheckBox.UpdatedValue();
            this.RitualCheckBox.UpdatedValue();
            this.ConcentrationCheckBox.UpdatedValue();
        }

        private void UpdateSpell(Spell spell)
        {
            var oldItem = spell.DeepCopy();

            spell.Name = this.SpellNameComboBox.Text;
            spell.Prepared = this.PreparedCheckBox.IsChecked ?? false;
            spell.Level = this.LevelUpDown.Value;
            spell.Page = this.PageUpDown.Value;
            spell.School = (ArcaneSchools)Enum.Parse(typeof(ArcaneSchools), this.SchoolComboBox.Text);
            spell.Ritual = this.RitualCheckBox.IsChecked ?? false;
            spell.Components = this.ComponentsTextBox.Text;
            spell.Concentration = this.ConcentrationCheckBox.IsChecked ?? false;
            spell.Range = this.RangeTextBox.Text;
            spell.Duration = this.DurationTextBox.Text;
            spell.Area = this.AreaTextBox.Text;
            spell.Save = this.SaveTextBox.Text;
            spell.Damage = this.DamageTextBox.Text;
            spell.Description = this.NotesTextBox.Text;
            spell.Class = this.ClassComboBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Spell>(spell, oldItem, this.ConciergePage));
        }

        private Spell ToSpell()
        {
            this.ItemsAdded = true;

            var spell = new Spell()
            {
                Name = this.SpellNameComboBox.Text,
                Prepared = this.PreparedCheckBox.IsChecked ?? false,
                Level = this.LevelUpDown.Value,
                Page = this.PageUpDown.Value,
                School = (ArcaneSchools)Enum.Parse(typeof(ArcaneSchools), this.SchoolComboBox.Text),
                Ritual = this.RitualCheckBox.IsChecked ?? false,
                Components = this.ComponentsTextBox.Text,
                Concentration = this.ConcentrationCheckBox.IsChecked ?? false,
                Range = this.RangeTextBox.Text,
                Duration = this.DurationTextBox.Text,
                Area = this.AreaTextBox.Text,
                Save = this.SaveTextBox.Text,
                Damage = this.DamageTextBox.Text,
                Description = this.NotesTextBox.Text,
                Class = this.ClassComboBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Spell>(this.Spells, spell, this.ConciergePage));

            return spell;
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
            this.Spells.Add(this.ToSpell());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void SpellNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SpellNameComboBox.SelectedItem is Spell spell)
            {
                this.FillFields(spell);
            }
        }
    }
}
