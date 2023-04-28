// <copyright file="SpellcastingPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
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

        private List<MagicClass> MagicClassDisplayList => Program.CcsFile.Character.MagicClasses.Filter(this.SearchFilter.FilterText).ToList();

        private List<Spell> SpellDisplayList => Program.CcsFile.Character.Spells.Filter(this.SearchFilter.FilterText).ToList();

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
                ConciergeWindowService.ShowEdit<Spell>(
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
                ConciergeWindowService.ShowEdit<MagicClass>(
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

            foreach (var spell in this.SpellDisplayList)
            {
                this.SpellListDataGrid.Items.Add(spell);
            }
        }

        public void DrawMagicClasses()
        {
            this.MagicClassDataGrid.Items.Clear();

            foreach (var magicClass in this.MagicClassDisplayList)
            {
                this.MagicClassDataGrid.Items.Add(magicClass);
            }

            this.CasterLevelField.Text = $"(Caster Level {Program.CcsFile.Character.CasterLevel})";
        }

        public void DrawSpellSlots()
        {
            this.SpellSlotsDisplay.FillSpellSlot(Program.CcsFile.Character.SpellSlots);
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

        private void MagicClassUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, 0, -1);
        }

        private void SpellUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, 0, -1);
        }

        private void MagicClassDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, Program.CcsFile.Character.MagicClasses.Count - 1, 1);
        }

        private void SpellDownButton_Click(object sender, RoutedEventArgs e)
        {
            this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, Program.CcsFile.Character.Spells.Count - 1, 1);
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
            var added = ConciergeWindowService.ShowAdd<List<MagicClass>>(
                Program.CcsFile.Character.MagicClasses,
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
            var added = ConciergeWindowService.ShowAdd<List<Spell>>(
                Program.CcsFile.Character.Spells,
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

                Program.UndoRedoService.AddCommand(new DeleteCommand<MagicClass>(Program.CcsFile.Character.MagicClasses, magicClass, index, this.ConciergePage));
                Program.CcsFile.Character.MagicClasses.Remove(magicClass);
                this.DrawMagicClasses();
                this.MagicClassDataGrid.SetSelectedIndex(index);

                Program.Modify();
            }
        }

        private void SpellDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SpellListDataGrid.SelectedItem != null)
            {
                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = this.SpellListDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Spell>(Program.CcsFile.Character.Spells, spell, index, this.ConciergePage));
                Program.CcsFile.Character.Spells.Remove(spell);
                this.DrawSpellList();
                this.SpellListDataGrid.SetSelectedIndex(index);

                Program.Modify();
            }
        }

        private void MagicClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                this.SpellListDataGrid.UnselectAll();
            }
        }

        private void SpellListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SpellListDataGrid.SelectedItem != null)
            {
                this.MagicClassDataGrid.UnselectAll();
            }
        }

        private void SpellListDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.SpellListDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Spells, this.ConciergePage);
        }

        private void MagicClassDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.MagicClassDataGrid.SortListFromDataGrid(Program.CcsFile.Character.MagicClasses, this.ConciergePage);
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
            ConciergeWindowService.ShowEdit<SpellSlots>(
                Program.CcsFile.Character.SpellSlots,
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
    }
}
