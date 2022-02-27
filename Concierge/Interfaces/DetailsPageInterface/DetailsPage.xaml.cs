// <copyright file="DetailsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Configuration;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility;
    using Concierge.Utility.Units;
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

        public double ProficiencyGridSize => this.WeaponGrid.RenderSize.Height;

        public ConciergePage ConciergePage => ConciergePage.Details;

        public bool HasEditableDataGrid => true;

        public void Draw()
        {
            this.DrawWeight();
            this.DrawAppearance();
            this.DrawPersonality();
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
                    typeof(ModifyProficiencyWindow),
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
                    typeof(ModifyLanguagesWindow),
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
                    typeof(ModifyClassResourceWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Details);
                this.DrawResources();
                this.ResourcesDataGrid.SetSelectedIndex(index);
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

        private static void DrawProficiency(ConciergeDataGrid conciergeDataGrid, List<Proficiency> proficiencies)
        {
            conciergeDataGrid.Items.Clear();

            foreach (var item in proficiencies)
            {
                conciergeDataGrid.Items.Add(item);
            }
        }

        private void SortProficiencyItems(List<Proficiency> proficiency)
        {
            AddSortedToList(proficiency, this.WeaponProficiencyDataGrid);
            AddSortedToList(proficiency, this.ArmorProficiencyDataGrid);
            AddSortedToList(proficiency, this.ShieldProficiencyDataGrid);
            AddSortedToList(proficiency, this.ToolProficiencyDataGrid);
        }

        private void DrawWeight()
        {
            var character = Program.CcsFile.Character;

            this.WeightCarriedField.Text = UnitFormat.FormatWeight(AppSettingsManager.UserSettings.UnitOfMeasurement, character.CarryWeight);
            this.LightWeightField.Text = UnitFormat.FormatWeight(AppSettingsManager.UserSettings.UnitOfMeasurement, character.LightCarryCapacity, true);
            this.MediumWeightField.Text = UnitFormat.FormatWeight(AppSettingsManager.UserSettings.UnitOfMeasurement, character.MediumCarryCapacity, true);
            this.HeavyWeightField.Text = UnitFormat.FormatWeight(AppSettingsManager.UserSettings.UnitOfMeasurement, character.HeavyCarryCapacity, true);

            this.FormatCarryWeight();
        }

        private void DrawAppearance()
        {
            var appearance = Program.CcsFile.Character.Appearance;

            this.GenderField.Text = appearance.Gender;
            this.AgeField.Text = appearance.Age.ToString();
            this.HeightField.Text = appearance.Height.ToString();
            this.WeightField.Text = appearance.Weight.ToString();
            this.HairColourField.Text = appearance.HairColour;
            this.SkinColourField.Text = appearance.SkinColour;
            this.EyeColourField.Text = appearance.EyeColour;
            this.MarksField.Text = appearance.DistinguishingMarks;
        }

        private void DrawPersonality()
        {
            var personality = Program.CcsFile.Character.Personality;

            this.TraitField1.Text = personality.Trait1;
            this.TraitField2.Text = personality.Trait2;
            this.IdealField.Text = personality.Ideal;
            this.BondField.Text = personality.Bond;
            this.FlawField.Text = personality.Flaw;
            this.BackgroundField.Text = personality.Background;
            this.NotesField.Text = personality.Notes;
        }

        private void DrawProficiencies()
        {
            var character = Program.CcsFile.Character;

            DrawProficiency(this.WeaponProficiencyDataGrid, character.Proficiency.Where(x => x.ProficiencyType == ProficiencyTypes.Weapon).ToList());
            DrawProficiency(this.ArmorProficiencyDataGrid, character.Proficiency.Where(x => x.ProficiencyType == ProficiencyTypes.Armor).ToList());
            DrawProficiency(this.ShieldProficiencyDataGrid, character.Proficiency.Where(x => x.ProficiencyType == ProficiencyTypes.Shield).ToList());
            DrawProficiency(this.ToolProficiencyDataGrid, character.Proficiency.Where(x => x.ProficiencyType == ProficiencyTypes.Tool).ToList());

            this.ProficiencyBonusField.Text = $"  Bonus: {Program.CcsFile.Character.ProficiencyBonus}  ";
        }

        private void DrawLanguages()
        {
            this.LanguagesDataGrid.Items.Clear();

            foreach (var language in Program.CcsFile.Character.Languages)
            {
                this.LanguagesDataGrid.Items.Add(language);
            }
        }

        private void DrawResources()
        {
            this.ResourcesDataGrid.Items.Clear();

            foreach (var resource in Program.CcsFile.Character.ClassResources)
            {
                this.ResourcesDataGrid.Items.Add(resource);
            }
        }

        private void DrawConditions()
        {
            this.ConditionsDataGrid.Items.Clear();

            foreach (var condition in Program.CcsFile.Character.Vitality.Conditions.ToList())
            {
                this.ConditionsDataGrid.Items.Add(condition);
            }
        }

        private void FormatCarryWeight()
        {
            var weight = Program.CcsFile.Character.CarryWeight;

            if (weight <= Program.CcsFile.Character.LightCarryCapacity)
            {
                this.WeightCarriedField.Foreground = Brushes.Black;
                this.WeightCarriedBox.Background = ConciergeColors.LightCarryCapacity;
            }
            else if (weight > Program.CcsFile.Character.LightCarryCapacity && weight <= Program.CcsFile.Character.MediumCarryCapacity)
            {
                this.WeightCarriedField.Foreground = ConciergeColors.HeavyCarryCapacity;
                this.WeightCarriedBox.Background = ConciergeColors.MediumCarryCapacity;
            }
            else if (weight > Program.CcsFile.Character.MediumCarryCapacity && weight <= Program.CcsFile.Character.HeavyCarryCapacity)
            {
                this.WeightCarriedField.Foreground = Brushes.DarkRed;
                this.WeightCarriedBox.Background = Brushes.Pink;
            }
            else
            {
                this.WeightCarriedField.Foreground = Brushes.Red;
                this.WeightCarriedBox.Background = Brushes.DimGray;
            }
        }

        private void DeleteProficiency(ConciergeDataGrid dataGrid)
        {
            if (dataGrid.SelectedItem is not Proficiency proficiency)
            {
                return;
            }

            var index = dataGrid.SelectedIndex;
            Program.UndoRedoService.AddCommand(new DeleteCommand<Proficiency>(Program.CcsFile.Character.Proficiency, proficiency, index, this.ConciergePage));
            Program.CcsFile.Character.Proficiency.Remove(proficiency);

            this.DrawProficiencies();
            dataGrid.SetSelectedIndex(index);

            Program.Modify();
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
            else if (this.ShieldProficiencyDataGrid.SelectedItem != null)
            {
                return this.ShieldProficiencyDataGrid;
            }
            else if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                return this.ToolProficiencyDataGrid;
            }

            return null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGrid = this.GetSelectedProficencyDataGrid();

            if (selectedGrid is not null)
            {
                this.DeleteProficiency(selectedGrid);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = this.GetSelectedProficencyDataGrid();

            if (dataGrid != null)
            {
                this.Edit(dataGrid.SelectedItem);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Proficiency>>(
                Program.CcsFile.Character.Proficiency,
                typeof(ModifyProficiencyWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);

            this.DrawProficiencies();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponProficiencyDataGrid.UnselectAll();
            this.ArmorProficiencyDataGrid.UnselectAll();
            this.ShieldProficiencyDataGrid.UnselectAll();
            this.ToolProficiencyDataGrid.UnselectAll();
        }

        private void WeaponProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.WeaponProficiencyDataGrid.SelectedItem != null)
            {
                this.ArmorProficiencyDataGrid.UnselectAll();
                this.ShieldProficiencyDataGrid.UnselectAll();
                this.ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ArmorProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ArmorProficiencyDataGrid.SelectedItem != null)
            {
                this.WeaponProficiencyDataGrid.UnselectAll();
                this.ShieldProficiencyDataGrid.UnselectAll();
                this.ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ShieldProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ShieldProficiencyDataGrid.SelectedItem != null)
            {
                this.WeaponProficiencyDataGrid.UnselectAll();
                this.ArmorProficiencyDataGrid.UnselectAll();
                this.ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ToolProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                this.WeaponProficiencyDataGrid.UnselectAll();
                this.ArmorProficiencyDataGrid.UnselectAll();
                this.ShieldProficiencyDataGrid.UnselectAll();
            }
        }

        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Conditions>(
                Program.CcsFile.Character.Vitality.Conditions,
                typeof(MondifyConditionsWindow),
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
                typeof(ModifyLanguagesWindow),
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
                typeof(ModifyClassResourceWindow),
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

        private void EditAppearanceButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Appearance>(
                Program.CcsFile.Character.Appearance,
                typeof(ModifyAppearanceWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawAppearance();
        }

        private void EditPersonalityButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Personality>(
                Program.CcsFile.Character.Personality,
                typeof(ModifyPersonalityWindow),
                this.Window_ApplyChanges,
                ConciergePage.Details);
            this.DrawPersonality();
        }

        private void ProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            var oldList = new List<Proficiency>(Program.CcsFile.Character.Proficiency);
            Program.CcsFile.Character.Proficiency.Clear();
            this.SortProficiencyItems(Program.CcsFile.Character.Proficiency);

            Program.UndoRedoService.AddCommand(
                new ListOrderCommand<Proficiency>(
                    Program.CcsFile.Character.Proficiency,
                    oldList,
                    new List<Proficiency>(Program.CcsFile.Character.Proficiency),
                    this.ConciergePage));
            Program.Modify();
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
                case "ModifyAppearanceWindow":
                    this.DrawAppearance();
                    break;
                case "ModifyClassResourceWindow":
                    this.DrawResources();
                    break;
                case "MondifyConditionsWindow":
                    this.DrawConditions();
                    break;
                case "ModifyLanguagesWindow":
                    this.DrawLanguages();
                    break;
                case "ModifyPersonalityWindow":
                    this.DrawPersonality();
                    break;
                case "ModifyProficiencyWindow":
                    this.DrawProficiencies();
                    break;
            }
        }
    }
}
