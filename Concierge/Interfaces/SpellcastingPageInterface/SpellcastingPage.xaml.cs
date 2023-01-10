// <copyright file="SpellcastingPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for SpellcastingPage.xaml.
    /// </summary>
    public partial class SpellcastingPage : Page, IConciergePage
    {
        public SpellcastingPage()
        {
            this.InitializeComponent();
            this.SpellSlotsDisplay.InitializeUsedSlot();
            this.SearchFilter.FilterChanged += this.MagicClassSpellsDataGrid_Filtered;
        }

        private delegate void DrawList();

        public ConciergePage ConciergePage => ConciergePage.Spellcasting;

        public bool HasEditableDataGrid => true;

        private List<MagicClass> MagicClassDisplayList => Program.CcsFile.Character.MagicClasses.Filter(this.SearchFilter.FilterText).ToList();

        private List<Spell> SpellDisplayList => Program.CcsFile.Character.Spells.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw()
        {
            this.DrawSpellList();
            this.DrawMagicClasses();
            this.DrawSpellSlots();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Spell spell)
            {
                var index = this.SpellListDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Spell>(
                    spell,
                    typeof(ModifySpellWindow),
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
                    typeof(ModifySpellClassWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Spellcasting);
                this.DrawMagicClasses();
                this.MagicClassDataGrid.SetSelectedIndex(index);
            }
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

        private void DrawSpellList()
        {
            this.SpellListDataGrid.Items.Clear();

            foreach (var spell in this.SpellDisplayList)
            {
                this.SpellListDataGrid.Items.Add(spell);
            }
        }

        private void DrawMagicClasses()
        {
            this.MagicClassDataGrid.Items.Clear();

            foreach (var magicClass in this.MagicClassDisplayList)
            {
                this.MagicClassDataGrid.Items.Add(magicClass);
            }

            this.CasterLevelField.Text = Program.CcsFile.Character.CasterLevel.ToString();
        }

        private void DrawSpellSlots()
        {
            this.SpellSlotsDisplay.FillTotalSpellSlot(Program.CcsFile.Character.SpellSlots);
            this.SpellSlotsDisplay.FillUsedSpellSlot(Program.CcsFile.Character.SpellSlots);
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (!this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, 0, -1))
            {
                this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, 0, -1);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (!this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, Program.CcsFile.Character.MagicClasses.Count - 1, 1))
            {
                this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, Program.CcsFile.Character.Spells.Count - 1, 1);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.MagicClassDataGrid.UnselectAll();
            this.SpellListDataGrid.UnselectAll();
        }

        private void AddMagicClassButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<MagicClass>>(
                Program.CcsFile.Character.MagicClasses,
                typeof(ModifySpellClassWindow),
                this.Window_ApplyChanges,
                ConciergePage.Spellcasting);

            this.DrawMagicClasses();
            if (added)
            {
                this.MagicClassDataGrid.SetSelectedIndex(this.MagicClassDataGrid.LastIndex);
            }
        }

        private void AddSpellButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Spell>>(
                Program.CcsFile.Character.Spells,
                typeof(ModifySpellWindow),
                this.Window_ApplyChanges,
                ConciergePage.Spellcasting);

            this.DrawSpellList();
            this.DrawMagicClasses();
            if (added)
            {
                this.SpellListDataGrid.SetSelectedIndex(this.SpellListDataGrid.LastIndex);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                this.Edit(this.MagicClassDataGrid.SelectedItem);
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                this.Edit(this.SpellListDataGrid.SelectedItem);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
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
            else if (this.SpellListDataGrid.SelectedItem != null)
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

        private void LevelEditButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<SpellSlots>(
                    Program.CcsFile.Character.SpellSlots,
                    typeof(ModifySpellSlotsWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Spellcasting);
            this.DrawSpellSlots();
        }

        private void SpellListDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.SpellListDataGrid, Program.CcsFile.Character.Spells, this.ConciergePage);
        }

        private void MagicClassDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.MagicClassDataGrid, Program.CcsFile.Character.MagicClasses, this.ConciergePage);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ModifySpellClassWindow):
                    this.DrawMagicClasses();
                    break;
                case nameof(ModifySpellWindow):
                    this.DrawSpellList();
                    this.DrawMagicClasses();
                    break;
                case nameof(ModifySpellSlotsWindow):
                    this.DrawSpellSlots();
                    break;
            }
        }

        private void SpellSlotsDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawSpellSlots();
        }

        private void MagicClassSpellsDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.ButtonUp);
            this.SearchFilter.SetButtonEnableState(this.ButtonDown);

            this.DrawMagicClasses();
            this.DrawSpellList();
        }
    }
}
