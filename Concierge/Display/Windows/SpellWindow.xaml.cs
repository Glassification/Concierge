// <copyright file="SpellWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Magic;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for SpellWindow.xaml.
    /// </summary>
    public partial class SpellWindow : ConciergeWindow
    {
        public SpellWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.SpellNameComboBox.ItemsSource = DefaultItems;
            this.SchoolComboBox.ItemsSource = ComboBoxGenerator.ArcaneSchoolsComboBox();
            this.ClassComboBox.ItemsSource = ComboBoxGenerator.ClassesComboBox();
            this.ConciergePage = ConciergePages.None;
            this.SelectedSpell = new Spell();
            this.Spells = [];
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.SpellNameComboBox);
            this.SetMouseOverEvents(this.PreparedCheckBox);
            this.SetMouseOverEvents(this.LevelUpDown);
            this.SetMouseOverEvents(this.RequireSpellSlotCheckBox);
            this.SetMouseOverEvents(this.SchoolComboBox);
            this.SetMouseOverEvents(this.RitualCheckBox);
            this.SetMouseOverEvents(this.ComponentsTextBox, this.ComponentsTextBackground);
            this.SetMouseOverEvents(this.ConcentrationCheckBox);
            this.SetMouseOverEvents(this.RangeTextBox, this.RangeTextBackground);
            this.SetMouseOverEvents(this.DurationTextBox, this.DurationTextBackground);
            this.SetMouseOverEvents(this.AreaTextBox, this.AreaTextBackground);
            this.SetMouseOverEvents(this.SaveTextBox, this.SaveTextBackground);
            this.SetMouseOverEvents(this.DamageTextBox, this.DamageTextBackground);
            this.SetMouseOverEvents(this.ClassComboBox);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Spell";

        public override string WindowName => nameof(SpellWindow);

        public bool ItemsAdded { get; private set; }

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Defaults.Spells, Program.CustomItemService.GetItems<Spell>());

        private bool Editing { get; set; }

        private Spell SelectedSpell { get; set; }

        private List<Spell> Spells { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.Spells = Program.CcsFile.Character.SpellCasting.Spells;
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
            this.Result = ConciergeResult.OK;

            if (this.Editing)
            {
                this.UpdateSpell(this.SelectedSpell);
            }
            else
            {
                this.Spells.Add(this.ToSpell());
            }

            this.CloseConciergeWindow();
        }

        private void SetRequireSpellSlotState(bool state)
        {
            DisplayUtility.SetControlEnableState(this.RequireSpellSlotLabel, state);
            DisplayUtility.SetControlEnableState(this.RequireSpellSlotCheckBox, state);
            if (!state)
            {
                this.RequireSpellSlotCheckBox.IsChecked = false;
            }
        }

        private void FillFields(Spell spell)
        {
            Program.Drawing();
            this.PreparedCheckBox.UpdatingValue();
            this.RitualCheckBox.UpdatingValue();
            this.ConcentrationCheckBox.UpdatingValue();
            this.RequireSpellSlotCheckBox.UpdatingValue();

            this.SpellNameComboBox.Text = spell.Name;
            this.PreparedCheckBox.IsChecked = spell.Prepared;
            this.LevelUpDown.Value = spell.Level;
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
            this.RequireSpellSlotCheckBox.IsChecked = spell.ExpendSpellSlot;

            this.SetRequireSpellSlotState(this.LevelUpDown.Value > 0);

            this.PreparedCheckBox.UpdatedValue();
            this.RitualCheckBox.UpdatedValue();
            this.ConcentrationCheckBox.UpdatedValue();
            this.RequireSpellSlotCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();
            this.PreparedCheckBox.UpdatingValue();
            this.RitualCheckBox.UpdatingValue();
            this.ConcentrationCheckBox.UpdatingValue();
            this.RequireSpellSlotCheckBox.UpdatingValue();

            this.SpellNameComboBox.Text = name;
            this.PreparedCheckBox.IsChecked = false;
            this.LevelUpDown.Value = 0;
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
            this.RequireSpellSlotCheckBox.IsChecked = false;

            this.PreparedCheckBox.UpdatedValue();
            this.RitualCheckBox.UpdatedValue();
            this.ConcentrationCheckBox.UpdatedValue();
            this.RequireSpellSlotCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void UpdateSpell(Spell spell)
        {
            var oldItem = spell.DeepCopy();

            spell.Name = this.SpellNameComboBox.Text;
            spell.Prepared = this.PreparedCheckBox.IsChecked ?? false;
            spell.Level = this.LevelUpDown.Value;
            spell.School = this.SchoolComboBox.Text.ToEnum<ArcaneSchools>();
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
            spell.ExpendSpellSlot = this.RequireSpellSlotCheckBox.IsChecked ?? false;

            if (!spell.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Spell>(spell, oldItem, this.ConciergePage));
            }
        }

        private Spell Create()
        {
            return new Spell()
            {
                Name = this.SpellNameComboBox.Text,
                Prepared = this.PreparedCheckBox.IsChecked ?? false,
                Level = this.LevelUpDown.Value,
                School = this.SchoolComboBox.Text.ToEnum<ArcaneSchools>(),
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
                ExpendSpellSlot = this.RequireSpellSlotCheckBox.IsChecked ?? false,
            };
        }

        private Spell ToSpell()
        {
            this.ItemsAdded = true;
            var spell = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Spell>(this.Spells, spell, this.ConciergePage));

            return spell;
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
            this.Spells.Add(this.ToSpell());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void SpellNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.SpellNameComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is Spell spell && !isLocked)
            {
                this.FillFields(spell);
            }
            else if (!isLocked)
            {
                this.ClearFields(this.SpellNameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SpellNameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Spell.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.SpellNameComboBox.ItemsSource = DefaultItems;
        }

        private void LevelUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.SetRequireSpellSlotState(this.LevelUpDown.Value > 0);
        }

        private void LockButton_Checked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.Lock;
        }

        private void LockButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.LockOpenVariant;
        }
    }
}
