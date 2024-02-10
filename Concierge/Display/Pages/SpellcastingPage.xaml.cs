// <copyright file="SpellcastingPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for SpellcastingPage.xaml.
    /// </summary>
    public partial class SpellcastingPage : Page, IConciergePage
    {
        public SpellcastingPage()
        {
            this.InitializeComponent();
        }

        private delegate void DrawList();

        public ConciergePage ConciergePage => ConciergePage.Spellcasting;

        public bool HasEditableDataGrid => true;

        private List<MagicClass> MagicClassDisplayList => Program.CcsFile.Character.Magic.MagicClasses.Filter(this.SearchFilter.FilterText).ToList();

        private List<Spell> SpellDisplayList => Program.CcsFile.Character.Magic.Spells.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawMagicClasses();
            this.DrawSpellList();
            this.DrawSpellSlots();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Spell spell)
            {
                var index = this.SpellListDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit(
                    spell,
                    typeof(SpellWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Spellcasting);
                this.DrawSpellList();
                this.DrawMagicClasses();
                this.SpellListDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is MagicClass magicClass)
            {
                var index = this.MagicClassDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit(
                    magicClass,
                    typeof(MagicClassWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Spellcasting);
                this.DrawMagicClasses();
                this.MagicClassDataGrid.SetSelectedIndex(index);
            }
        }

        public void DrawSpellList()
        {
            this.SpellListDataGrid.Items.Clear();
            this.SpellDisplayList.ForEach(spell => this.SpellListDataGrid.Items.Add(spell));
            this.SetSpellListDataGridDataGridControlState();
        }

        public void DrawMagicClasses()
        {
            this.MagicClassDataGrid.Items.Clear();
            this.MagicClassDisplayList.ForEach(magicClass => this.MagicClassDataGrid.Items.Add(magicClass));
            this.CasterLevelField.Text = $"(Caster Level {Program.CcsFile.Character.Magic.CasterLevel})";
            this.SetMagicClassDataGridControlState();
        }

        public void DrawSpellSlots()
        {
            this.SpellSlotsDisplay.FillSpellSlots(Program.CcsFile.Character.Magic.SpellSlots);
        }

        private bool NextItem<T>(ConciergeDataGrid dataGrid, DrawList drawList, List<T> list, int limit, int increment)
        {
            var index = dataGrid.NextItem(list, limit, increment, this.ConciergePage);

            if (index != -1)
            {
                drawList();
                dataGrid.SetSelectedIndex(index);

                return true;
            }

            return false;
        }

        private void SetMagicClassDataGridControlState()
        {
            this.MagicClassDataGrid.SetButtonControlsEnableState(
                this.MagicClassUpButton,
                this.MagicClassDownButton,
                this.MagicClassEditButton,
                this.MagicClassDeleteButton);
        }

        private void SetSpellListDataGridDataGridControlState()
        {
            this.SpellListDataGrid.SetButtonControlsEnableState(
                this.SpellUpButton,
                this.SpellDownButton,
                this.SpellEditButton,
                this.SpellUseButton,
                this.SpellDeleteButton);
        }

        private void MagicClassUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.Magic.MagicClasses, 0, -1);
        }

        private void SpellUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Magic.Spells, 0, -1);
        }

        private void MagicClassDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.Magic.MagicClasses, Program.CcsFile.Character.Magic.MagicClasses.Count - 1, 1);
        }

        private void SpellDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Magic.Spells, Program.CcsFile.Character.Magic.Spells.Count - 1, 1);
        }

        private void MagicClassClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.MagicClassDataGrid.UnselectAll();
        }

        private void SpellClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.SpellListDataGrid.UnselectAll();
        }

        private void MagicClassAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Magic.MagicClasses,
                typeof(MagicClassWindow),
                this.Window_ApplyChanges,
                ConciergePage.Spellcasting);

            this.DrawMagicClasses();
            if (added)
            {
                this.MagicClassDataGrid.SetSelectedIndex(this.MagicClassDataGrid.LastIndex);
            }
        }

        private void SpellAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Magic.Spells,
                typeof(SpellWindow),
                this.Window_ApplyChanges,
                ConciergePage.Spellcasting);

            this.DrawSpellList();
            this.DrawMagicClasses();
            if (added)
            {
                this.SpellListDataGrid.SetSelectedIndex(this.SpellListDataGrid.LastIndex);
            }
        }

        private void MagicClassEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.Edit(this.MagicClassDataGrid.SelectedItem);
        }

        private void SpellEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.Edit(this.SpellListDataGrid.SelectedItem);
        }

        private void MagicClassDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                var index = this.MagicClassDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<MagicClass>(Program.CcsFile.Character.Magic.MagicClasses, magicClass, index, this.ConciergePage));
                Program.CcsFile.Character.Magic.MagicClasses.Remove(magicClass);
                this.DrawMagicClasses();
                this.MagicClassDataGrid.SetSelectedIndex(index);
            }
        }

        private void SpellDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SpellListDataGrid.SelectedItem != null)
            {
                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = this.SpellListDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Spell>(Program.CcsFile.Character.Magic.Spells, spell, index, this.ConciergePage));
                Program.CcsFile.Character.Magic.Spells.Remove(spell);
                this.DrawSpellList();
                this.SpellListDataGrid.SetSelectedIndex(index);
            }
        }

        private void MagicClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetMagicClassDataGridControlState();
        }

        private void SpellListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetSpellListDataGridDataGridControlState();
        }

        private void SpellListDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.SpellListDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Magic.Spells, this.ConciergePage);
        }

        private void MagicClassDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.MagicClassDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Magic.MagicClasses, this.ConciergePage);
        }

        private void SpellSlotsDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawSpellSlots();
        }

        private void SpellListDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.SpellUpButton);
            this.SearchFilter.SetButtonEnableState(this.SpellDownButton);

            this.DrawMagicClasses();
            this.DrawSpellList();
        }

        private void SpellSlotsDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            ConciergeSoundService.TapNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Magic.SpellSlots,
                typeof(SpellSlotsWindow),
                this.Window_ApplyChanges,
                ConciergePage.Spellcasting);
            this.DrawSpellSlots();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(MagicClassWindow):
                    this.DrawMagicClasses();
                    break;
                case nameof(SpellWindow):
                    this.DrawSpellList();
                    this.DrawMagicClasses();
                    break;
                case nameof(SpellSlotsWindow):
                    this.DrawSpellSlots();
                    break;
            }
        }

        private void SpellUseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SpellListDataGrid.SelectedItem is null)
            {
                return;
            }

            var magic = Program.CcsFile.Character.Magic;
            var commands = new List<Command>();
            var spell = (Spell)this.SpellListDataGrid.SelectedItem;
            var index = this.SpellListDataGrid.SelectedIndex;
            var concentratedSpell = magic.ConcentratedSpell;

            if (concentratedSpell is not null && concentratedSpell.Id != spell.Id && spell.Concentration)
            {
                var messageResult = ConciergeMessageBox.Show(
                    Regex.Unescape($"You are currently concentrating on {concentratedSpell.Name}. Using another concentration spell will drop the current one.\nDo you wish to use {spell.Name}?"),
                    "Concentration",
                    ConciergeButtons.Yes | ConciergeButtons.No,
                    ConciergeIcons.Question);

                if (messageResult != ConciergeResult.Yes)
                {
                    return;
                }
            }

            if (!spell.Class.IsNullOrWhiteSpace() && spell.Level > 0)
            {
                var spellSlots = magic.SpellSlots.DeepCopy();
                var name = spell.Class.Equals("Warlock", StringComparison.InvariantCultureIgnoreCase) ? "pact" : spell.Level.ToSpellSlot();
                var (used, total) = magic.SpellSlots.Increment(name);
                if (used == 0 && total == 0)
                {
                    ConciergeMessageBox.Show(
                        $"You have no remaining spell slots to cast {spell.Name}.",
                        "Warning",
                        ConciergeButtons.Ok,
                        ConciergeIcons.Warning);
                    return;
                }

                commands.Add(new EditCommand<SpellSlots>(magic.SpellSlots, spellSlots, this.ConciergePage));
            }

            var result = spell.Use();
            if (spell.Concentration)
            {
                magic.SetConcentration(spell);
                commands.Add(new ConcentrationCommand(spell, concentratedSpell));
            }

            Program.UndoRedoService.AddCommand(new CompositeCommand(this.ConciergePage, [.. commands]));
            ConciergeWindowService.ShowUseItemWindow(typeof(UseItemWindow), result);
            this.DrawSpellList();
            this.DrawSpellSlots();
            this.SpellListDataGrid.SetSelectedIndex(index);
        }

        private void SpellClearConcentrationButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.SpellListDataGrid.SelectedIndex;
            var concentratedSpell = Program.CcsFile.Character.Magic.ConcentratedSpell;
            if (concentratedSpell is null)
            {
                return;
            }

            Program.UndoRedoService.AddCommand(new ConcentrationCommand(concentratedSpell, null));

            Program.CcsFile.Character.Magic.ClearConcentration();
            this.DrawSpellList();
            this.SpellListDataGrid.SetSelectedIndex(index);
        }
    }
}
