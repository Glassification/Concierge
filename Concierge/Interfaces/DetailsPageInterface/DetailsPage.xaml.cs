﻿// <copyright file="DetailsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Statuses;
    using Concierge.Interfaces.Components;
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

        private static void SortProficiencyItems(Dictionary<Guid, string> proficiency, ItemCollection items)
        {
            foreach (var item in items)
            {
                var keyValuePair = (KeyValuePair<Guid, string>)item;
                proficiency.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        private static void DrawProficiency(ConciergeDataGrid conciergeDataGrid, Dictionary<Guid, string> proficiencies)
        {
            conciergeDataGrid.Items.Clear();

            foreach (var item in proficiencies)
            {
                conciergeDataGrid.Items.Add(item);
            }
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
            DrawProficiency(this.WeaponProficiencyDataGrid, Program.CcsFile.Character.Proficiency.Weapons);
            DrawProficiency(this.ArmorProficiencyDataGrid, Program.CcsFile.Character.Proficiency.Armors);
            DrawProficiency(this.ShieldProficiencyDataGrid, Program.CcsFile.Character.Proficiency.Shields);
            DrawProficiency(this.ToolProficiencyDataGrid, Program.CcsFile.Character.Proficiency.Tools);

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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponProficiencyDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var weapon = (KeyValuePair<Guid, string>)this.WeaponProficiencyDataGrid.SelectedItem;
                var index = this.WeaponProficiencyDataGrid.SelectedIndex;
                Program.CcsFile.Character.Proficiency.Weapons.Remove(weapon.Key);
                this.DrawProficiencies();
                Utilities.SetDataGridSelectedIndex(this.WeaponProficiencyDataGrid, index);
            }
            else if (this.ArmorProficiencyDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var armor = (KeyValuePair<Guid, string>)this.ArmorProficiencyDataGrid.SelectedItem;
                var index = this.ArmorProficiencyDataGrid.SelectedIndex;
                Program.CcsFile.Character.Proficiency.Armors.Remove(armor.Key);
                this.DrawProficiencies();
                Utilities.SetDataGridSelectedIndex(this.ArmorProficiencyDataGrid, index);
            }
            else if (this.ShieldProficiencyDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var shield = (KeyValuePair<Guid, string>)this.ShieldProficiencyDataGrid.SelectedItem;
                var index = this.ShieldProficiencyDataGrid.SelectedIndex;
                Program.CcsFile.Character.Proficiency.Shields.Remove(shield.Key);
                this.DrawProficiencies();
                Utilities.SetDataGridSelectedIndex(this.ShieldProficiencyDataGrid, index);
            }
            else if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var tool = (KeyValuePair<Guid, string>)this.ToolProficiencyDataGrid.SelectedItem;
                var index = this.ToolProficiencyDataGrid.SelectedIndex;
                Program.CcsFile.Character.Proficiency.Tools.Remove(tool.Key);
                this.DrawProficiencies();
                Utilities.SetDataGridSelectedIndex(this.ToolProficiencyDataGrid, index);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<Guid, string> proficiency;

            if (this.WeaponProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.WeaponProficiencyDataGrid.SelectedItem;
                this.modifyProficiencyWindow.ShowEdit(proficiency.Key);
                this.DrawProficiencies();
            }
            else if (this.ArmorProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.ArmorProficiencyDataGrid.SelectedItem;
                this.modifyProficiencyWindow.ShowEdit(proficiency.Key);
                this.DrawProficiencies();
            }
            else if (this.ShieldProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.ShieldProficiencyDataGrid.SelectedItem;
                this.modifyProficiencyWindow.ShowEdit(proficiency.Key);
                this.DrawProficiencies();
            }
            else if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.ToolProficiencyDataGrid.SelectedItem;
                this.modifyProficiencyWindow.ShowEdit(proficiency.Key);
                this.DrawProficiencies();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyProficiencyWindow.ShowAdd();

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
                this.modifyLanguagesWindow.ShowEdit(this.LanguagesDataGrid.SelectedItem as Language);
                this.DrawLanguages();
            }
        }

        private void EditResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourcesDataGrid.SelectedItem != null)
            {
                this.modifyClassResourceWindow.ShowEdit(this.ResourcesDataGrid.SelectedItem as ClassResource);
                this.DrawResources();
            }
        }

        private void AddLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyLanguagesWindow.ShowAdd(Program.CcsFile.Character.Details.Languages);
            this.DrawLanguages();
        }

        private void AddResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyClassResourceWindow.ShowAdd(Program.CcsFile.Character.ClassResources);
            this.DrawResources();
        }

        private void DeleteLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.LanguagesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var index = this.LanguagesDataGrid.SelectedIndex;
                Program.CcsFile.Character.Details.Languages.Remove(this.LanguagesDataGrid.SelectedItem as Language);
                this.DrawLanguages();
                Utilities.SetDataGridSelectedIndex(this.LanguagesDataGrid, index);
            }
        }

        private void DeleteResourcesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourcesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var index = this.ResourcesDataGrid.SelectedIndex;
                Program.CcsFile.Character.ClassResources.Remove(this.ResourcesDataGrid.SelectedItem as ClassResource);
                this.DrawResources();
                Utilities.SetDataGridSelectedIndex(this.ResourcesDataGrid, index);
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

            switch ((sender as ConciergeDataGrid).Name)
            {
                case "WeaponProficiencyDataGrid":
                    Program.CcsFile.Character.Proficiency.Weapons.Clear();
                    SortProficiencyItems(Program.CcsFile.Character.Proficiency.Weapons, this.WeaponProficiencyDataGrid.Items);
                    break;
                case "ArmorProficiencyDataGrid":
                    Program.CcsFile.Character.Proficiency.Armors.Clear();
                    SortProficiencyItems(Program.CcsFile.Character.Proficiency.Armors, this.ArmorProficiencyDataGrid.Items);
                    break;
                case "ShieldProficiencyDataGrid":
                    Program.CcsFile.Character.Proficiency.Shields.Clear();
                    SortProficiencyItems(Program.CcsFile.Character.Proficiency.Shields, this.ShieldProficiencyDataGrid.Items);
                    break;
                case "ToolProficiencyDataGrid":
                    Program.CcsFile.Character.Proficiency.Tools.Clear();
                    SortProficiencyItems(Program.CcsFile.Character.Proficiency.Tools, this.ToolProficiencyDataGrid.Items);
                    break;
            }
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