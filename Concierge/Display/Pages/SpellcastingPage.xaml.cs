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

    using Concierge.Character.Magic;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;
    using Concierge.Tools;

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

        private List<MagicalClass> MagicalClassDisplayList => Program.CcsFile.Character.SpellCasting.MagicalClasses.Filter(this.SearchFilter.FilterText).ToList();

        private List<Spell> SpellDisplayList => Program.CcsFile.Character.SpellCasting.Spells.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawMagicalClasses();
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
                this.DrawMagicalClasses();
                this.SpellListDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is MagicalClass magicClass)
            {
                var index = this.MagicalClassDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit(
                    magicClass,
                    typeof(MagicClassWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Spellcasting);
                this.DrawMagicalClasses();
                this.MagicalClassDataGrid.SetSelectedIndex(index);
            }
        }

        public void DrawSpellList()
        {
            this.SpellListDataGrid.Items.Clear();
            this.SpellDisplayList.ForEach(spell => this.SpellListDataGrid.Items.Add(spell));
            this.SetSpellListDataGridDataGridControlState();
        }

        public void DrawMagicalClasses()
        {
            this.MagicalClassDataGrid.Items.Clear();
            this.MagicalClassDisplayList.ForEach(magicClass => this.MagicalClassDataGrid.Items.Add(magicClass));
            this.CasterLevelField.Text = $"(Caster Level {Program.CcsFile.Character.SpellCasting.CasterLevel})";
            this.SetMagicalClassDataGridControlState();
        }

        public void DrawSpellSlots()
        {
            this.SpellSlotsDisplay.FillSpellSlots(Program.CcsFile.Character.SpellCasting.SpellSlots);
        }

        private static UseItem GetUseItem(Spell spell, SpellCasting magic)
        {
            var attack = magic.GetSpellAttack(spell.Class);
            return attack == 0 ? UseItem.Empty : new UseItem(attack);
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

        private void SetMagicalClassDataGridControlState()
        {
            this.MagicalClassDataGrid.SetButtonControlsEnableState(
                this.MagicalClassUpButton,
                this.MagicalClassDownButton,
                this.MagicalClassEditButton,
                this.MagicalClassDeleteButton);
        }

        private void SetSpellListDataGridDataGridControlState()
        {
            DisplayUtility.SetControlEnableState(this.SpellInformationButton, this.SpellListDataGrid.Items.Count > 0);
            this.SpellListDataGrid.SetButtonControlsEnableState(
                this.SpellUpButton,
                this.SpellDownButton,
                this.SpellEditButton,
                this.SpellUseButton,
                this.SpellDeleteButton);
        }

        private void MagicalClassUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.MagicalClassDataGrid, this.DrawMagicalClasses, Program.CcsFile.Character.SpellCasting.MagicalClasses, 0, -1);
        }

        private void SpellUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.SpellCasting.Spells, 0, -1);
        }

        private void MagicalClassDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.MagicalClassDataGrid, this.DrawMagicalClasses, Program.CcsFile.Character.SpellCasting.MagicalClasses, Program.CcsFile.Character.SpellCasting.MagicalClasses.Count - 1, 1);
        }

        private void SpellDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.SpellCasting.Spells, Program.CcsFile.Character.SpellCasting.Spells.Count - 1, 1);
        }

        private void MagicalClassClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.MagicalClassDataGrid.UnselectAll();
        }

        private void SpellClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.SpellListDataGrid.UnselectAll();
        }

        private void MagicalClassAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.SpellCasting.MagicalClasses,
                typeof(MagicClassWindow),
                this.Window_ApplyChanges,
                ConciergePage.Spellcasting);

            this.DrawMagicalClasses();
            if (added)
            {
                this.MagicalClassDataGrid.SetSelectedIndex(this.MagicalClassDataGrid.LastIndex);
            }
        }

        private void SpellAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.SpellCasting.Spells,
                typeof(SpellWindow),
                this.Window_ApplyChanges,
                ConciergePage.Spellcasting);

            this.DrawSpellList();
            this.DrawMagicalClasses();
            if (added)
            {
                this.SpellListDataGrid.SetSelectedIndex(this.SpellListDataGrid.LastIndex);
            }
        }

        private void MagicalClassEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.Edit(this.MagicalClassDataGrid.SelectedItem);
        }

        private void SpellEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.Edit(this.SpellListDataGrid.SelectedItem);
        }

        private void MagicalClassDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicalClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicalClass)this.MagicalClassDataGrid.SelectedItem;
                var index = this.MagicalClassDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<MagicalClass>(Program.CcsFile.Character.SpellCasting.MagicalClasses, magicClass, index, this.ConciergePage));
                Program.CcsFile.Character.SpellCasting.MagicalClasses.Remove(magicClass);
                this.DrawMagicalClasses();
                this.MagicalClassDataGrid.SetSelectedIndex(index);
            }
        }

        private void SpellDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SpellListDataGrid.SelectedItem != null)
            {
                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = this.SpellListDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Spell>(Program.CcsFile.Character.SpellCasting.Spells, spell, index, this.ConciergePage));
                Program.CcsFile.Character.SpellCasting.Spells.Remove(spell);
                this.DrawSpellList();
                this.SpellListDataGrid.SetSelectedIndex(index);
            }
        }

        private void MagicalClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetMagicalClassDataGridControlState();
        }

        private void SpellListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetSpellListDataGridDataGridControlState();
        }

        private void SpellListDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.SpellListDataGrid.SortListFromDataGrid(Program.CcsFile.Character.SpellCasting.Spells, this.ConciergePage);
        }

        private void MagicalClassDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.MagicalClassDataGrid.SortListFromDataGrid(Program.CcsFile.Character.SpellCasting.MagicalClasses, this.ConciergePage);
        }

        private void SpellSlotsDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawSpellSlots();
        }

        private void SpellListDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.SpellUpButton);
            this.SearchFilter.SetButtonEnableState(this.SpellDownButton);

            this.DrawMagicalClasses();
            this.DrawSpellList();
        }

        private void SpellSlotsDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.SpellCasting.SpellSlots,
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
                    this.DrawMagicalClasses();
                    break;
                case nameof(SpellWindow):
                    this.DrawSpellList();
                    this.DrawMagicalClasses();
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

            var magic = Program.CcsFile.Character.SpellCasting;
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

            if (spell.ExpendSpellSlot)
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

            var result = spell.Use(GetUseItem(spell, magic));
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
            var concentratedSpell = Program.CcsFile.Character.SpellCasting.ConcentratedSpell;
            if (concentratedSpell is null)
            {
                return;
            }

            Program.UndoRedoService.AddCommand(new ConcentrationCommand(concentratedSpell, null));

            Program.CcsFile.Character.SpellCasting.ClearConcentration();
            this.DrawSpellList();
            this.SpellListDataGrid.SetSelectedIndex(index);
        }

        private void SpellInformationButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowWindow(typeof(SpellDetailsWindow));
        }
    }
}
