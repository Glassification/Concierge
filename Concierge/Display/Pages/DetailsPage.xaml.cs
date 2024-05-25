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

    using Concierge.Character.Details;
    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Services;

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

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawAppearance();
            this.DrawPersonality();
            this.DrawDefense();
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
                ConciergeWindowService.ShowEdit(
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
                ConciergeWindowService.ShowEdit(
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
                ConciergeWindowService.ShowEdit(
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
            this.AppearanceDisplay.SetAppearance(Program.CcsFile.Character.Detail.Appearance);
        }

        public void DrawPersonality()
        {
            this.PersonalityDisplay.SetPersonality(Program.CcsFile.Character.Detail.Personality);
        }

        public void DrawDefense()
        {
            this.ArmorDisplay.SetDefenseDetails(Program.CcsFile.Character.Equipment.Defense);
        }

        public void DrawProficiencies()
        {
            var character = Program.CcsFile.Character;

            DrawProficiency(this.WeaponProficiencyDataGrid, character.Detail.Proficiencies.Where(x => x.ProficiencyType == ProficiencyTypes.Weapon).ToList());
            DrawProficiency(this.ArmorProficiencyDataGrid, character.Detail.Proficiencies.Where(x => x.ProficiencyType == ProficiencyTypes.Armor).ToList());
            DrawProficiency(this.ToolProficiencyDataGrid, character.Detail.Proficiencies.Where(x => x.ProficiencyType == ProficiencyTypes.Tool).ToList());

            this.SetProficiencyDataGridControlState(this.WeaponProficiencyDataGrid);
            this.SetProficiencyDataGridControlState(this.ArmorProficiencyDataGrid);
            this.SetProficiencyDataGridControlState(this.ToolProficiencyDataGrid);

            this.ProficiencyBonusField.Text = $"  Bonus: {Program.CcsFile.Character.ProficiencyBonus}  ";
        }

        public void DrawResources()
        {
            this.ResourcesDataGrid.Items.Clear();
            Program.CcsFile.Character.Vitality.ClassResources.ForEach(resource => this.ResourcesDataGrid.Items.Add(resource));
            this.SetResourceDataGridControlState();
        }

        public void DrawLanguages()
        {
            this.LanguagesDataGrid.Items.Clear();
            Program.CcsFile.Character.Detail.Languages.ForEach(language => this.LanguagesDataGrid.Items.Add(language));
            this.SetLanguageDataGridControlState();
        }

        public void DrawConditions()
        {
            var character = Program.CcsFile.Character;

            this.ConditionsDataGrid.Items.Clear();
            var conditions = character.Vitality.Status.ActiveConditions();
            foreach (var condition in conditions)
            {
                this.ConditionsDataGrid.Items.Add(condition);
            }

            if (character.Vitality.Status.Exhaustion.IsAfflicted())
            {
                this.ConditionsDataGrid.Items.Add(character.Vitality.Status.Exhaustion);
            }

            if (character.Encumbrance.Status != ConditionStatus.Normal)
            {
                this.ConditionsDataGrid.Items.Add(character.Encumbrance);
            }
        }

        private static void DrawProficiency(ConciergeDataGrid conciergeDataGrid, List<Proficiency> proficiencies)
        {
            conciergeDataGrid.Items.Clear();
            proficiencies.ForEach(item => conciergeDataGrid.Items.Add(item));
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
            Program.UndoRedoService.AddCommand(new DeleteCommand<Proficiency>(Program.CcsFile.Character.Detail.Proficiencies, proficiency, index, this.ConciergePage));
            Program.CcsFile.Character.Detail.Proficiencies.Remove(proficiency);

            this.DrawProficiencies();
            dataGrid.SetSelectedIndex(index);
        }

        private void SetProficiencyDataGridControlState(ConciergeDataGrid dataGrid)
        {
            dataGrid.SetButtonControlsEnableState(this.EditProficencyButton, this.DeleteProficencyButton);
        }

        private void SetResourceDataGridControlState()
        {
            this.ResourcesDataGrid.SetButtonControlsEnableState(this.EditResourcesButton, this.DeleteResourcesButton);
        }

        private void SetLanguageDataGridControlState()
        {
            this.LanguagesDataGrid.SetButtonControlsEnableState(this.EditLanguagesButton, this.DeleteLanguagesButton);
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
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Detail.Proficiencies,
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
            if (this.WeaponProficiencyDataGrid.SelectedItem is not null)
            {
                this.ArmorProficiencyDataGrid.UnselectAll();
                this.ToolProficiencyDataGrid.UnselectAll();
            }

            this.SetProficiencyDataGridControlState(this.WeaponProficiencyDataGrid);
        }

        private void ArmorProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ArmorProficiencyDataGrid.SelectedItem is not null)
            {
                this.WeaponProficiencyDataGrid.UnselectAll();
                this.ToolProficiencyDataGrid.UnselectAll();
            }

            this.SetProficiencyDataGridControlState(this.ArmorProficiencyDataGrid);
        }

        private void ToolProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ToolProficiencyDataGrid.SelectedItem is not null)
            {
                this.WeaponProficiencyDataGrid.UnselectAll();
                this.ArmorProficiencyDataGrid.UnselectAll();
            }

            this.SetProficiencyDataGridControlState(this.ToolProficiencyDataGrid);
        }

        private void ProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            var characteristic = Program.CcsFile.Character.Detail;
            var oldList = new List<Proficiency>(characteristic.Proficiencies);
            characteristic.Proficiencies.Clear();
            this.SortProficiencyItems(characteristic.Proficiencies);

            Program.UndoRedoService.AddCommand(
                new ListOrderCommand<Proficiency>(
                    characteristic.Proficiencies,
                    oldList,
                    new List<Proficiency>(characteristic.Proficiencies),
                    this.ConciergePage));
        }

        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Vitality.Status,
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
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Detail.Languages,
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
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Vitality.ClassResources,
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
            Program.UndoRedoService.AddCommand(new DeleteCommand<Language>(Program.CcsFile.Character.Detail.Languages, language, index, this.ConciergePage));
            Program.CcsFile.Character.Detail.Languages.Remove(language);
            this.DrawLanguages();
            this.LanguagesDataGrid.SetSelectedIndex(index);
        }

        private void DeleteResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourcesDataGrid.SelectedItem is not ClassResource resource)
            {
                return;
            }

            var index = this.ResourcesDataGrid.SelectedIndex;
            Program.UndoRedoService.AddCommand(new DeleteCommand<ClassResource>(Program.CcsFile.Character.Vitality.ClassResources, resource, index, this.ConciergePage));
            Program.CcsFile.Character.Vitality.ClassResources.Remove(resource);
            this.DrawResources();
            this.ResourcesDataGrid.SetSelectedIndex(index);
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
            this.LanguagesDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Detail.Languages, this.ConciergePage);
        }

        private void ResourcesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.ResourcesDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Vitality.ClassResources, this.ConciergePage);
        }

        private void AppearanceDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Detail.Appearance,
                typeof(AppearanceWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawAppearance();
        }

        private void PersonalityDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Detail.Personality,
                typeof(PersonalityWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawPersonality();
        }

        private void ArmorDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            ConciergeWindowService.ShowEdit(
                Program.CcsFile.Character.Equipment.Defense,
                typeof(ArmorWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawDefense();
            this.DrawConditions();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ArmorWindow):
                    this.DrawDefense();
                    this.DrawConditions();
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

        private void ResourcesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetResourceDataGridControlState();
        }

        private void LanguagesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetLanguageDataGridControlState();
        }
    }
}
