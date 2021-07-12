// <copyright file="DetailsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for DetailsPage.xaml.
    /// </summary>
    public partial class DetailsPage : Page
    {
        public DetailsPage()
        {
            this.InitializeComponent();

            this.ModifyWealthWindow = new ModifyWealthWindow();
            this.ProficiencyPopupWindow = new ProficiencyPopupWindow();
            this.ModifyProficiencyWindow = new ModifyProficiencyWindow();
            this.MondifyConditionsWindow = new MondifyConditionsWindow();
            this.ModifyLanguagesWindow = new ModifyLanguagesWindow();
            this.ModifyAppearanceWindow = new ModifyAppearanceWindow();
            this.ModifyPersonalityWindow = new ModifyPersonalityWindow();
        }

        public double ProficiencyGridSize => this.WeaponGrid.RenderSize.Height;

        private ModifyWealthWindow ModifyWealthWindow { get; }

        private ProficiencyPopupWindow ProficiencyPopupWindow { get; }

        private ModifyProficiencyWindow ModifyProficiencyWindow { get; }

        private MondifyConditionsWindow MondifyConditionsWindow { get; }

        private ModifyLanguagesWindow ModifyLanguagesWindow { get; }

        private ModifyAppearanceWindow ModifyAppearanceWindow { get; }

        private ModifyPersonalityWindow ModifyPersonalityWindow { get; }

        public void Draw()
        {
            this.DrawWealth();
            this.DrawWeight();
            this.DrawAppearance();
            this.DrawPersonality();
            this.DrawProficiencies();
            this.DrawLanguages();
            this.DrawConditions();
        }

        private void DrawWealth()
        {
            this.TotalWealthField.Text = "¤ " + string.Format("{0:0.00}", Program.CcsFile.Character.Wealth.TotalValue);

            this.CopperField.Text = Program.CcsFile.Character.Wealth.Copper.ToString();
            this.SilverField.Text = Program.CcsFile.Character.Wealth.Silver.ToString();
            this.ElectrumField.Text = Program.CcsFile.Character.Wealth.Electrum.ToString();
            this.GoldField.Text = Program.CcsFile.Character.Wealth.Gold.ToString();
            this.PlatinumField.Text = Program.CcsFile.Character.Wealth.Platinum.ToString();
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
            this.GenderField.Text = Program.CcsFile.Character.Appearance.Gender;
            this.AgeField.Text = Program.CcsFile.Character.Appearance.Age;
            this.HeightField.Text = Program.CcsFile.Character.Appearance.Height;
            this.WeightField.Text = Program.CcsFile.Character.Appearance.Weight;
            this.HairColourField.Text = Program.CcsFile.Character.Appearance.HairColour;
            this.SkinColourField.Text = Program.CcsFile.Character.Appearance.SkinColour;
            this.EyeColourField.Text = Program.CcsFile.Character.Appearance.EyeColour;
            this.MarksField.Text = Program.CcsFile.Character.Appearance.DistinguishingMarks;
        }

        private void DrawPersonality()
        {
            this.TraitField1.Text = Program.CcsFile.Character.Personality.Trait1;
            this.TraitField2.Text = Program.CcsFile.Character.Personality.Trait2;
            this.IdealField.Text = Program.CcsFile.Character.Personality.Ideal;
            this.BondField.Text = Program.CcsFile.Character.Personality.Bond;
            this.FlawField.Text = Program.CcsFile.Character.Personality.Flaw;
            this.BackgroundField.Text = Program.CcsFile.Character.Personality.Background;
            this.NotesField.Text = Program.CcsFile.Character.Personality.Notes;
        }

        private void DrawProficiencies()
        {
            this.WeaponProficiencyDataGrid.Items.Clear();
            this.ArmorProficiencyDataGrid.Items.Clear();
            this.ShieldProficiencyDataGrid.Items.Clear();
            this.ToolProficiencyDataGrid.Items.Clear();

            foreach (var weapon in Program.CcsFile.Character.Proficiency.Weapons)
            {
                this.WeaponProficiencyDataGrid.Items.Add(weapon);
            }

            foreach (var armor in Program.CcsFile.Character.Proficiency.Armors)
            {
                this.ArmorProficiencyDataGrid.Items.Add(armor);
            }

            foreach (var shield in Program.CcsFile.Character.Proficiency.Shields)
            {
                this.ShieldProficiencyDataGrid.Items.Add(shield);
            }

            foreach (var tool in Program.CcsFile.Character.Proficiency.Tools)
            {
                this.ToolProficiencyDataGrid.Items.Add(tool);
            }

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

        private void DrawConditions()
        {
            this.ConditionsDataGrid.Items.Clear();

            foreach (var condition in Program.CcsFile.Character.Vitality.Conditions.ToArray())
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

        private void EditWealthButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyWealthWindow.ShowWindow();
            this.DrawWealth();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponProficiencyDataGrid.SelectedItem != null)
            {
                var weapon = (KeyValuePair<Guid, string>)this.WeaponProficiencyDataGrid.SelectedItem;
                Program.CcsFile.Character.Proficiency.Weapons.Remove(weapon.Key);
                this.DrawProficiencies();
            }
            else if (this.ArmorProficiencyDataGrid.SelectedItem != null)
            {
                var armor = (KeyValuePair<Guid, string>)this.ArmorProficiencyDataGrid.SelectedItem;
                Program.CcsFile.Character.Proficiency.Armors.Remove(armor.Key);
                this.DrawProficiencies();
            }
            else if (this.ShieldProficiencyDataGrid.SelectedItem != null)
            {
                var shield = (KeyValuePair<Guid, string>)this.ShieldProficiencyDataGrid.SelectedItem;
                Program.CcsFile.Character.Proficiency.Shields.Remove(shield.Key);
                this.DrawProficiencies();
            }
            else if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                var tool = (KeyValuePair<Guid, string>)this.ToolProficiencyDataGrid.SelectedItem;
                Program.CcsFile.Character.Proficiency.Tools.Remove(tool.Key);
                this.DrawProficiencies();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<Guid, string> proficiency;

            if (this.WeaponProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.WeaponProficiencyDataGrid.SelectedItem;
                this.ModifyProficiencyWindow.ShowEdit(PopupButtons.WeaponProficiency, proficiency.Key);
                this.DrawProficiencies();
            }
            else if (this.ArmorProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.ArmorProficiencyDataGrid.SelectedItem;
                this.ModifyProficiencyWindow.ShowEdit(PopupButtons.ArmorProficiency, proficiency.Key);
                this.DrawProficiencies();
            }
            else if (this.ShieldProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.ShieldProficiencyDataGrid.SelectedItem;
                this.ModifyProficiencyWindow.ShowEdit(PopupButtons.ShieldProficiency, proficiency.Key);
                this.DrawProficiencies();
            }
            else if (this.ToolProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)this.ToolProficiencyDataGrid.SelectedItem;
                this.ModifyProficiencyWindow.ShowEdit(PopupButtons.ToolProficiency, proficiency.Key);
                this.DrawProficiencies();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PopupButtons popupButtons;

            popupButtons = this.ProficiencyPopupWindow.ShowPopup();

            this.ModifyProficiencyWindow.ShowAdd(popupButtons);

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
            this.MondifyConditionsWindow.ShowEdit();
            this.DrawConditions();
        }

        private void EditLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.LanguagesDataGrid.SelectedItem != null)
            {
                this.ModifyLanguagesWindow.ShowEdit(this.LanguagesDataGrid.SelectedItem as Language);
                this.DrawLanguages();
            }
        }

        private void AddLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyLanguagesWindow.ShowAdd();
            this.DrawLanguages();
        }

        private void DeleteLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.LanguagesDataGrid.SelectedItem != null)
            {
                Program.CcsFile.Character.Details.Languages.Remove(this.LanguagesDataGrid.SelectedItem as Language);
                this.DrawLanguages();
            }
        }

        private void ClearLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            this.LanguagesDataGrid.UnselectAll();
        }

        private void ClearConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConditionsDataGrid.UnselectAll();
        }

        private void EditAppearanceButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyAppearanceWindow.ShowEdit();
            this.DrawAppearance();
        }

        private void EditPersonalityButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyPersonalityWindow.ShowEdit();
            this.DrawPersonality();
        }

        private void WeaponProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Proficiency.Weapons.Clear();

            foreach (var weapon in this.WeaponProficiencyDataGrid.Items)
            {
                var keyValuePair = (KeyValuePair<Guid, string>)weapon;
                Program.CcsFile.Character.Proficiency.Weapons.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        private void ArmorProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Proficiency.Armors.Clear();

            foreach (var armor in this.ArmorProficiencyDataGrid.Items)
            {
                var keyValuePair = (KeyValuePair<Guid, string>)armor;
                Program.CcsFile.Character.Proficiency.Armors.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        private void ShieldProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Proficiency.Shields.Clear();

            foreach (var shield in this.ShieldProficiencyDataGrid.Items)
            {
                var keyValuePair = (KeyValuePair<Guid, string>)shield;
                Program.CcsFile.Character.Proficiency.Shields.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        private void ToolProficiencyDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Proficiency.Tools.Clear();

            foreach (var tool in this.ToolProficiencyDataGrid.Items)
            {
                var keyValuePair = (KeyValuePair<Guid, string>)tool;
                Program.CcsFile.Character.Proficiency.Tools.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        private void LanguagesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Details.Languages.Clear();

            foreach (var language in this.LanguagesDataGrid.Items)
            {
                Program.CcsFile.Character.Details.Languages.Add(language as Language);
            }
        }
    }
}
