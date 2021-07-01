// <copyright file="ModifySpellWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.SpellcastingPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifySpellWindow.xaml.
    /// </summary>
    public partial class ModifySpellWindow : Window
    {
        public ModifySpellWindow()
        {
            this.InitializeComponent();
            this.SpellNameComboBox.ItemsSource = Constants.Spells;
            this.SchoolComboBox.ItemsSource = Enum.GetValues(typeof(ArcaneSchools)).Cast<ArcaneSchools>();
            this.ClassComboBox.ItemsSource = Enum.GetValues(typeof(ClassType)).Cast<ClassType>();
        }

        private bool Editing { get; set; }

        private Guid SelectedSpellId { get; set; }

        public void AddSpell()
        {
            this.HeaderTextBlock.Text = "Add Spell";
            this.Editing = false;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.ClearFields();

            this.ShowDialog();
        }

        public void EditSpell(Spell spell)
        {
            this.HeaderTextBlock.Text = "Edit Spell";
            this.SelectedSpellId = spell.ID;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields(spell);

            this.ShowDialog();
        }

        private void FillFields(Spell spell)
        {
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
        }

        private void ClearFields()
        {
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

            Program.Modified = true;
        }

        private Spell ToSpell()
        {
            var spell = new Spell()
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

            return spell;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.UpdateSpell(Program.Character.GetSpellById(this.SelectedSpellId));
            }
            else
            {
                Program.Character.Spells.Add(this.ToSpell());
                Program.Modified = true;
            }

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Spells.Add(this.ToSpell());
            Program.Modified = true;
            this.ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
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
