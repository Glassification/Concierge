// <copyright file="DetailsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Character.Statuses.ConditionStatus;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Services;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for DetailsPage.xaml.
    /// </summary>
    public partial class DetailsPage : Page, IConciergePage
    {
        public DetailsPage()
        {
            this.InitializeComponent();
        }

        public ConciergePage ConciergePage => ConciergePage.Details;

        public bool HasEditableDataGrid => true;

        public void Draw()
        {
            this.DrawAppearance();
            this.DrawPersonality();
            this.DrawArmor();
            this.DrawProficiencies();
            this.DrawResources();
            this.DrawLanguages();
            this.DrawConditions();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Proficiency proficiency)
            {
                var selectedDataGrid = this.GetSelectedProficencyDataGrid();
                if (selectedDataGrid is null)
                {
                    return;
                }

                var index = selectedDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Proficiency>(
                    proficiency,
                    typeof(ProficiencyWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Details);
                this.DrawProficiencies();
                selectedDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Language language)
            {
                var index = this.LanguagesDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Language>(
                    language,
                    typeof(LanguagesWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Details);
                this.DrawLanguages();
                this.LanguagesDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is ClassResource resource)
            {
                var index = this.ResourcesDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<ClassResource>(
                    resource,
                    typeof(ClassResourceWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Details);
                this.DrawResources();
                this.ResourcesDataGrid.SetSelectedIndex(index);
            }
        }

        public void DrawAppearance()
        {
            this.AppearanceDisplay.SetAppearance(Program.CcsFile.Character.Appearance);
        }

        public void DrawPersonality()
        {
            this.PersonalityDisplay.SetPersonality(Program.CcsFile.Character.Personality);
        }

        public void DrawArmor()
        {
            this.ArmorDisplay.SetArmorDetails(Program.CcsFile.Character.Armor);
        }

        public void DrawProficiencies()
        {
            var character = Program.CcsFile.Character;

            DrawProficiency(this.WeaponProficiencyDataGrid, character.Proficiencies.Where(x => x.ProficiencyType == ProficiencyTypes.Weapon).ToList());
            DrawProficiency(this.ArmorProficiencyDataGrid, character.Proficiencies.Where(x => x.ProficiencyType == ProficiencyTypes.Armor).ToList());
            DrawProficiency(this.ToolProficiencyDataGrid, character.Proficiencies.Where(x => x.ProficiencyType == ProficiencyTypes.Tool).ToList());

            this.ProficiencyBonusField.Text = $"  Bonus: {Program.CcsFile.Character.ProficiencyBonus}  ";
        }

        public void DrawResources()
        {
            this.ResourcesDataGrid.Items.Clear();

            foreach (var resource in Program.CcsFile.Character.ClassResources)
            {
                this.ResourcesDataGrid.Items.Add(resource);
            }
        }

        public void DrawLanguages()
        {
            this.LanguagesDataGrid.Items.Clear();

            foreach (var language in Program.CcsFile.Character.Languages)
            {
                this.LanguagesDataGrid.Items.Add(language);
            }
        }

        public void DrawConditions()
        {
            this.ConditionsDataGrid.Items.Clear();

            foreach (var condition in Program.CcsFile.Character.Vitality.Conditions.ToList())
            {
                this.ConditionsDataGrid.Items.Add(condition);
            }
        }

        private static void DrawProficiency(ConciergeDataGrid conciergeDataGrid, List<Proficiency> proficiencies)
        {
            conciergeDataGrid.Items.Clear();

            foreach (var item in proficiencies)
            {
                conciergeDataGrid.Items.Add(item);
            }
        }

        private static void AddSortedToList(List<Proficiency> proficiencies, ConciergeDataGrid dataGrid)
        {
            foreach (var item in dataGrid.Items)
            {
                if (item is Proficiency proficiency)
                {
                    proficiencies.Add(proficiency);
                }
            }
        }

        private void SortProficiencyItems(List<Proficiency> proficiency)
        {
            AddSortedToList(proficiency, this.WeaponProficiencyDataGrid);
            AddSortedToList(proficiency, this.ArmorProficiencyDataGrid);
            AddSortedToList(proficiency, this.ToolProficiencyDataGrid);
        }

        private ConciergeDataGrid? GetSelectedProficencyDataGrid()
        {
            if (this.WeaponProficiencyDataGrid.SelectedItem != null)
            {
                return this.WeaponProficiencyDataGrid;
            }
            else if (this.ArmorProficiencyDataGrid.SelectedItem != null)
            {
                return this.ArmorProficiencyDataGrid;
            }
            else if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                return this.ToolProficiencyDataGrid;
            }

            return null;
        }

        private void DeleteProficiency(ConciergeDataGrid dataGrid)
        {
            if (dataGrid.SelectedItem is not Proficiency proficiency)
            {
                return;
            }

            var index = dataGrid.SelectedIndex;
            Program.UndoRedoService.AddCommand(new DeleteCommand<Proficiency>(Program.CcsFile.Character.Proficiencies, proficiency, index, this.ConciergePage));
            Program.CcsFile.Character.Proficiencies.Remove(proficiency);

            this.DrawProficiencies();
            dataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void DeleteProficencyButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGrid = this.GetSelectedProficencyDataGrid();

            if (selectedGrid is not null)
            {
                this.DeleteProficiency(selectedGrid);
            }
        }

        private void EditProficencyButton_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = this.GetSelectedProficencyDataGrid();

            if (dataGrid != null)
            {
                this.Edit(dataGrid.SelectedItem);
            }
        }

        private void AddProficencyButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Proficiency>>(
                Program.CcsFile.Character.Proficiencies,
                typeof(ProficiencyWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);

            this.DrawProficiencies();
        }

        private void ClearProficencyButton_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponProficiencyDataGrid.UnselectAll();
            this.ArmorProficiencyDataGrid.UnselectAll();
            this.ToolProficiencyDataGrid.UnselectAll();
        }

        private void WeaponProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.WeaponProficiencyDataGrid.SelectedItem != null)
            {
                this.ArmorProficiencyDataGrid.UnselectAll();
                this.ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ArmorProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ArmorProficiencyDataGrid.SelectedItem != null)
            {
                this.WeaponProficiencyDataGrid.UnselectAll();
                this.ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ToolProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                this.WeaponProficiencyDataGrid.UnselectAll();
                this.ArmorProficiencyDataGrid.UnselectAll();
            }
        }

        private void ProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            var oldList = new List<Proficiency>(Program.CcsFile.Character.Proficiencies);
            Program.CcsFile.Character.Proficiencies.Clear();
            this.SortProficiencyItems(Program.CcsFile.Character.Proficiencies);

            Program.UndoRedoService.AddCommand(
                new ListOrderCommand<Proficiency>(
                    Program.CcsFile.Character.Proficiencies,
                    oldList,
                    new List<Proficiency>(Program.CcsFile.Character.Proficiencies),
                    this.ConciergePage));
            Program.Modify();
        }

        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Conditions>(
                Program.CcsFile.Character.Vitality.Conditions,
                typeof(ConditionsWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawConditions();
        }

        private void EditLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.LanguagesDataGrid.SelectedItem != null)
            {
                this.Edit(this.LanguagesDataGrid.SelectedItem);
            }
        }

        private void EditResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourcesDataGrid.SelectedItem != null)
            {
                if (this.ResourcesDataGrid.SelectedItem != null)
                {
                    this.Edit(this.ResourcesDataGrid.SelectedItem);
                }
            }
        }

        private void AddLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Language>>(
                Program.CcsFile.Character.Languages,
                typeof(LanguagesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawLanguages();

            if (added)
            {
                this.LanguagesDataGrid.SetSelectedIndex(this.LanguagesDataGrid.LastIndex);
            }
        }

        private void AddResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<ClassResource>>(
                Program.CcsFile.Character.ClassResources,
                typeof(ClassResourceWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawResources();

            if (added)
            {
                this.ResourcesDataGrid.SetSelectedIndex(this.ResourcesDataGrid.LastIndex);
            }
        }

        private void DeleteLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.LanguagesDataGrid.SelectedItem is not Language language)
            {
                return;
            }

            var index = this.LanguagesDataGrid.SelectedIndex;
            Program.UndoRedoService.AddCommand(new DeleteCommand<Language>(Program.CcsFile.Character.Languages, language, index, this.ConciergePage));
            Program.CcsFile.Character.Languages.Remove(language);
            this.DrawLanguages();
            this.LanguagesDataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void DeleteResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourcesDataGrid.SelectedItem is not ClassResource resource)
            {
                return;
            }

            var index = this.ResourcesDataGrid.SelectedIndex;
            Program.UndoRedoService.AddCommand(new DeleteCommand<ClassResource>(Program.CcsFile.Character.ClassResources, resource, index, this.ConciergePage));
            Program.CcsFile.Character.ClassResources.Remove(resource);
            this.DrawResources();
            this.ResourcesDataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void ClearLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            this.LanguagesDataGrid.UnselectAll();
        }

        private void ClearResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            this.ResourcesDataGrid.UnselectAll();
        }

        private void ClearConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConditionsDataGrid.UnselectAll();
        }

        private void LanguagesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.LanguagesDataGrid, Program.CcsFile.Character.Languages, this.ConciergePage);
        }

        private void ResourcesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.ResourcesDataGrid, Program.CcsFile.Character.ClassResources, this.ConciergePage);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ArmorWindow):
                    this.DrawArmor();
                    break;
                case nameof(AppearanceWindow):
                    this.DrawAppearance();
                    break;
                case nameof(ClassResourceWindow):
                    this.DrawResources();
                    break;
                case nameof(ConditionsWindow):
                    this.DrawConditions();
                    break;
                case nameof(LanguagesWindow):
                    this.DrawLanguages();
                    break;
                case nameof(PersonalityWindow):
                    this.DrawPersonality();
                    break;
                case nameof(ProficiencyWindow):
                    this.DrawProficiencies();
                    break;
            }
        }
    }
}
