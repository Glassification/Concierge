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
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for DetailsPage.xaml.
    /// </summary>
    public partial class DetailsPage : Page, IConciergePage
    {
        private readonly ModifyProficiencyWindow modifyProficiencyWindow = new ();
        private readonly MondifyConditionsWindow mondifyConditionsWindow = new ();
        private readonly ModifyLanguagesWindow modifyLanguagesWindow = new ();
        private readonly ModifyAppearanceWindow modifyAppearanceWindow = new ();
        private readonly ModifyPersonalityWindow modifyPersonalityWindow = new ();
        private readonly ModifyClassResourceWindow modifyClassResourceWindow = new ();

        public DetailsPage()
        {
            this.InitializeComponent();
            this.modifyProficiencyWindow.ApplyChanges += this.Window_ApplyChanges;
            this.mondifyConditionsWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyLanguagesWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyAppearanceWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyPersonalityWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyClassResourceWindow.ApplyChanges += this.Window_ApplyChanges;
        }

        public double ProficiencyGridSize => this.WeaponGrid.RenderSize.Height;

        public ConciergePage ConciergePage => ConciergePage.Details;

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
            if (itemToEdit is Proficiency)
            {
                var selectedDataGrid = this.GetSelectedProficencyDataGrid();
                var index = selectedDataGrid.SelectedIndex;
                this.modifyProficiencyWindow.ShowEdit(itemToEdit as Proficiency);
                this.DrawProficiencies();
                selectedDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Language)
            {
                var index = this.LanguagesDataGrid.SelectedIndex;
                this.modifyLanguagesWindow.ShowEdit(itemToEdit as Language);
                this.DrawLanguages();
                this.LanguagesDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is ClassResource)
            {
                var index = this.ResourcesDataGrid.SelectedIndex;
                this.modifyClassResourceWindow.ShowEdit(itemToEdit as ClassResource);
                this.DrawResources();
                this.ResourcesDataGrid.SetSelectedIndex(index);
            }
        }

        private static void AddSortedToList(List<Proficiency> proficiency, ConciergeDataGrid dataGrid)
        {
            foreach (var item in dataGrid.Items)
            {
                proficiency.Add(item as Proficiency);
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
            this.WeightCarriedField.Text = Program.CcsFile.Character.CarryWeight.ToString();
            this.LightWeightField.Text = Program.CcsFile.Character.LightCarryCapacity.ToString();
            this.MediumWeightField.Text = Program.CcsFile.Character.MediumCarryCapacity.ToString();
            this.HeavyWeightField.Text = Program.CcsFile.Character.HeavyCarryCapacity.ToString();

            this.FormatCarryWeight();
        }

        private void DrawAppearance()
        {
            var appearance = Program.CcsFile.Character.Appearance;

            this.GenderField.Text = appearance.Gender;
            this.AgeField.Text = appearance.Age.ToString();
            this.HeightField.Text = appearance.Height;
            this.WeightField.Text = appearance.Weight;
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

            foreach (var language in Program.CcsFile.Character.Details.Languages)
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
                this.WeightCarriedBox.Background = new SolidColorBrush(Colours.LightGreen);
            }
            else if (weight > Program.CcsFile.Character.LightCarryCapacity && weight <= Program.CcsFile.Character.MediumCarryCapacity)
            {
                this.WeightCarriedField.Foreground = new SolidColorBrush(Colours.MediumRed);
                this.WeightCarriedBox.Background = new SolidColorBrush(Colours.LightYellow);
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
            if (dataGrid?.SelectedItem is null)
            {
                return;
            }

            Program.Modify();

            var item = dataGrid.SelectedItem as Proficiency;
            var index = dataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<Proficiency>(Program.CcsFile.Character.Proficiency, item, index));
            Program.CcsFile.Character.Proficiency.Remove(item);

            this.DrawProficiencies();
            dataGrid.SetSelectedIndex(index);
        }

        private ConciergeDataGrid GetSelectedProficencyDataGrid()
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
            this.DeleteProficiency(this.GetSelectedProficencyDataGrid());
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
            this.modifyProficiencyWindow.ShowAdd(Program.CcsFile.Character.Proficiency);

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
            this.mondifyConditionsWindow.ShowEdit(Program.CcsFile.Character.Vitality.Conditions);
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
            this.modifyLanguagesWindow.ShowAdd(Program.CcsFile.Character.Details.Languages);
            this.DrawLanguages();

            if (this.modifyLanguagesWindow.ItemsAdded)
            {
                this.LanguagesDataGrid.SetSelectedIndex(this.LanguagesDataGrid.LastIndex);
            }
        }

        private void AddResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyClassResourceWindow.ShowAdd(Program.CcsFile.Character.ClassResources);
            this.DrawResources();

            if (this.modifyClassResourceWindow.ItemsAdded)
            {
                this.ResourcesDataGrid.SetSelectedIndex(this.ResourcesDataGrid.LastIndex);
            }
        }

        private void DeleteLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.LanguagesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var language = this.LanguagesDataGrid.SelectedItem as Language;
                var index = this.LanguagesDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Language>(Program.CcsFile.Character.Details.Languages, language, index));
                Program.CcsFile.Character.Details.Languages.Remove(language);
                this.DrawLanguages();
                this.LanguagesDataGrid.SetSelectedIndex(index);
            }
        }

        private void DeleteResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourcesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var resource = this.ResourcesDataGrid.SelectedItem as ClassResource;
                var index = this.ResourcesDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<ClassResource>(Program.CcsFile.Character.ClassResources, resource, index));
                Program.CcsFile.Character.ClassResources.Remove(resource);
                this.DrawResources();
                this.ResourcesDataGrid.SetSelectedIndex(index);
            }
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
            this.modifyAppearanceWindow.ShowEdit(Program.CcsFile.Character.Appearance);
            this.DrawAppearance();
        }

        private void EditPersonalityButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyPersonalityWindow.ShowEdit(Program.CcsFile.Character.Personality);
            this.DrawPersonality();
        }

        private void ProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Proficiency.Clear();
            this.SortProficiencyItems(Program.CcsFile.Character.Proficiency);
        }

        private void LanguagesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Details.Languages.Clear();

            foreach (var language in this.LanguagesDataGrid.Items)
            {
                Program.CcsFile.Character.Details.Languages.Add(language as Language);
            }
        }

        private void ResourcesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.ClassResources.Clear();

            foreach (var resource in this.ResourcesDataGrid.Items)
            {
                Program.CcsFile.Character.ClassResources.Add(resource as ClassResource);
            }
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
