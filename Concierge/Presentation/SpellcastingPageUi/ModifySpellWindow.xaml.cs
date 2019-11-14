using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Concierge.Presentation.SpellcastingPageUi
{
    /// <summary>
    /// Interaction logic for ModifySpellWindow.xaml
    /// </summary>
    public partial class ModifySpellWindow : Window
    {
        public ModifySpellWindow()
        {
            InitializeComponent();
            SpellNameComboBox.ItemsSource = Constants.Spells;
            SchoolComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ArcaneSchools)).Cast<Constants.ArcaneSchools>();
            ClassComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ClassTypes)).Cast<Constants.ClassTypes>();
        }

        public void AddSpell()
        {
            HeaderTextBlock.Text = "Add Spell";
            Editing = false;
            ApplyButton.Visibility = Visibility.Visible;
            ClearFields();

            ShowDialog();
        }

        public void EditSpell(Spell spell)
        {
            HeaderTextBlock.Text = "Edit Spell";
            SelectedSpellId = spell.ID;
            Editing = true;
            ApplyButton.Visibility = Visibility.Collapsed;
            FillFields(spell);

            ShowDialog();
        }

        private void FillFields(Spell spell)
        {
            SpellNameComboBox.Text = spell.Name;
            PreparedCheckBox.IsChecked = spell.Prepared;
            LevelUpDown.Value = spell.Level;
            PageUpDown.Value = spell.Page;
            SchoolComboBox.Text = spell.School.ToString();
            RitualCheckBox.IsChecked = spell.Ritual;
            ComponentsTextBox.Text = spell.Components;
            ConcentrationCheckBox.IsChecked = spell.Concentration;
            RangeTextBox.Text = spell.Range;
            DurationTextBox.Text = spell.Duration;
            AreaTextBox.Text = spell.Area;
            SaveTextBox.Text = spell.Save;
            DamageTextBox.Text = spell.Damage;
            NotesTextBox.Text = spell.Description;
            ClassComboBox.Text = spell.Class;
        }

        private void ClearFields()
        {
            SpellNameComboBox.Text = string.Empty;
            PreparedCheckBox.IsChecked = false;
            LevelUpDown.Value = 0;
            PageUpDown.Value = 0;
            SchoolComboBox.Text = Constants.ArcaneSchools.Universal.ToString();
            RitualCheckBox.IsChecked = false;
            ComponentsTextBox.Text = string.Empty;
            ConcentrationCheckBox.IsChecked = false;
            RangeTextBox.Text = string.Empty;
            DurationTextBox.Text = string.Empty;
            AreaTextBox.Text = string.Empty;
            SaveTextBox.Text = string.Empty;
            ClassComboBox.Text = string.Empty;
            DamageTextBox.Text = string.Empty;
            NotesTextBox.Text = string.Empty;
        }

        private void UpdateSpell(Spell spell)
        {
            spell.Name = SpellNameComboBox.Text;
            spell.Prepared = PreparedCheckBox.IsChecked ?? false;
            spell.Level = LevelUpDown.Value ?? 0;
            spell.Page = PageUpDown.Value ?? 0;
            spell.School = (Constants.ArcaneSchools)Enum.Parse(typeof(Constants.ArcaneSchools), SchoolComboBox.Text);
            spell.Ritual = RitualCheckBox.IsChecked ?? false;
            spell.Components = ComponentsTextBox.Text;
            spell.Concentration = ConcentrationCheckBox.IsChecked ?? false;
            spell.Range = RangeTextBox.Text;
            spell.Duration = DurationTextBox.Text;
            spell.Area = AreaTextBox.Text;
            spell.Save = SaveTextBox.Text;
            spell.Damage = DamageTextBox.Text;
            spell.Description = NotesTextBox.Text;
            spell.Class = ClassComboBox.Text;
        }

        private Spell ToSpell()
        {
            Spell spell = new Spell()
            {
                Name = SpellNameComboBox.Text,
                Prepared = PreparedCheckBox.IsChecked ?? false,
                Level = LevelUpDown.Value ?? 0,
                Page = PageUpDown.Value ?? 0,
                School = (Constants.ArcaneSchools)Enum.Parse(typeof(Constants.ArcaneSchools), SchoolComboBox.Text),
                Ritual = RitualCheckBox.IsChecked ?? false,
                Components = ComponentsTextBox.Text,
                Concentration = ConcentrationCheckBox.IsChecked ?? false,
                Range = RangeTextBox.Text,
                Duration = DurationTextBox.Text,
                Area = AreaTextBox.Text,
                Save = SaveTextBox.Text,
                Damage = DamageTextBox.Text,
                Description = NotesTextBox.Text,
                Class = ClassComboBox.Text
            };

            return spell;
        }

        private bool Editing { get; set; }
        private Guid SelectedSpellId { get; set; }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                UpdateSpell(Program.Character.GetSpellById(SelectedSpellId));
            }
            else
            {
                Program.Character.Spells.Add(ToSpell());
            }

            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Spells.Add(ToSpell());
            ClearFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void SpellNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpellNameComboBox.SelectedItem != null)
            {
                FillFields(SpellNameComboBox.SelectedItem as Spell);
            }
        }
    }
}
