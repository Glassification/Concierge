// <copyright file="ModifySpellWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySpellWindow.xaml.
    /// </summary>
    public partial class ModifySpellWindow : Window, IConciergeWindow
    {
        public ModifySpellWindow()
        {
            this.InitializeComponent();
            this.SpellNameComboBox.ItemsSource = Constants.Spells;
            this.SchoolComboBox.ItemsSource = Enum.GetValues(typeof(ArcaneSchools)).Cast<ArcaneSchools>();
            this.ClassComboBox.ItemsSource = Constants.Classes;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private Spell SelectedSpell { get; set; }

        private List<Spell> Spells { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Spell";
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.Spells = Program.CcsFile.Character.Spells;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void AddSpell(List<Spell> spells)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Spell";
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.Spells = spells;

            this.ClearFields();
            this.ShowDialog();
        }

        public void EditSpell(Spell spell)
        {
            this.HeaderTextBlock.Text = "Edit Spell";
            this.SelectedSpell = spell;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;
            this.FillFields(spell);

            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void FillFields(Spell spell)
        {
            this.LevelUpDown.UpdatingValue();
            this.PageUpDown.UpdatingValue();
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
            this.LevelUpDown.UpdatingValue();
            this.PageUpDown.UpdatingValue();
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
            spell.Name = this.SpellNameComboBox.Text;
            spell.Prepared = this.PreparedCheckBox.IsChecked ?? false;
            spell.Level = this.LevelUpDown.Value ?? 0;
            spell.Page = this.PageUpDown.Value ?? 0;
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
        }

        private Spell ToSpell()
        {
            return new Spell()
            {
                Name = this.SpellNameComboBox.Text,
                Prepared = this.PreparedCheckBox.IsChecked ?? false,
                Level = this.LevelUpDown.Value ?? 0,
                Page = this.PageUpDown.Value ?? 0,
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
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateSpell(this.SelectedSpell);
            }
            else
            {
                this.Spells.Add(this.ToSpell());
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Spells.Add(this.ToSpell());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void SpellNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SpellNameComboBox.SelectedItem != null)
            {
                this.FillFields(this.SpellNameComboBox.SelectedItem as Spell);
            }
        }
    }
}
