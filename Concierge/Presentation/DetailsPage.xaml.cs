using Concierge.Characters.Collections;
using Concierge.Presentation.DialogBoxes;
using Concierge.Presentation.Popups;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Concierge.Presentation
{
    /// <summary>
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {

        #region Constructor

        public DetailsPage()
        {
            InitializeComponent();

            ModifyWealthWindow = new ModifyWealthWindow();
            ProficiencyPopupWindow = new ProficiencyPopupWindow();
            ModifyProficiencyWindow = new ModifyProficiencyWindow();
            MondifyConditionsWindow = new MondifyConditionsWindow();
            ModifyLanguagesWindow = new ModifyLanguagesWindow();
        }

        #endregion

        #region Methods

        #region Drawing

        public void Draw()
        {
            DrawWealth();
            DrawWeight();
            DrawAppearance();
            DrawPersonality();
            DrawProficiencies();
            DrawLanguages();
            DrawConditions();
        }

        private void DrawWealth()
        {
            TotalWealthField.Text = "¤ " + string.Format("{0:0.00}", Program.Character.Wealth.TotalValue);

            CopperField.Text = Program.Character.Wealth.Copper.ToString();
            SilverField.Text = Program.Character.Wealth.Silver.ToString();
            ElectrumField.Text = Program.Character.Wealth.Electrum.ToString();
            GoldField.Text = Program.Character.Wealth.Gold.ToString();
            PlatinumField.Text = Program.Character.Wealth.Platinum.ToString();
        }

        private void DrawWeight()
        {
            WeightCarriedField.Text = Program.Character.CarryWeight.ToString();
            LightWeightField.Text = Program.Character.LightCarryCapacity.ToString();
            MediumWeightField.Text = Program.Character.MediumCarryCapacity.ToString();
            HeavyWeightField.Text = Program.Character.HeavyCarryCapacity.ToString();

            FormatCarryWeight();
        }

        private void DrawAppearance()
        {
            GenderField.Text = Program.Character.Appearance.Gender;
            AgeField.Text = Program.Character.Appearance.Age;
            HeightField.Text = Program.Character.Appearance.Height;
            WeightField.Text = Program.Character.Appearance.Weight;
            HairColourField.Text = Program.Character.Appearance.HairColour;
            SkinColourField.Text = Program.Character.Appearance.SkinColour;
            EyeColourField.Text = Program.Character.Appearance.EyeColour;
            MarksField.Text = Program.Character.Appearance.DistinguishingMarks;
        }

        private void DrawPersonality()
        {
            TraitField1.Text = Program.Character.Personality.Trait1;
            TraitField2.Text = Program.Character.Personality.Trait2;
            IdealField.Text = Program.Character.Personality.Ideal;
            BondField.Text = Program.Character.Personality.Bond;
            FlawField.Text = Program.Character.Personality.Flaw;
            BackgroundField.Text = Program.Character.Personality.Background;
            NotesField.Text = Program.Character.Personality.Notes;
        }

        private void DrawProficiencies()
        {
            WeaponProficiencyDataGrid.Items.Clear();
            ArmorProficiencyDataGrid.Items.Clear();
            ShieldProficiencyDataGrid.Items.Clear();
            ToolProficiencyDataGrid.Items.Clear();

            foreach (var weapon in Program.Character.Proficiency.Weapons)
            {
                WeaponProficiencyDataGrid.Items.Add(weapon);
            }

            foreach (var armor in Program.Character.Proficiency.Armors)
            {
                ArmorProficiencyDataGrid.Items.Add(armor);
            }

            foreach (var shield in Program.Character.Proficiency.Shields)
            {
                ShieldProficiencyDataGrid.Items.Add(shield);
            }

            foreach (var tool in Program.Character.Proficiency.Tools)
            {
                ToolProficiencyDataGrid.Items.Add(tool);
            }

            ProficiencyBonusField.Text = "  Bonus: " + Program.Character.ProficiencyBonus + "  ";
        }

        private void DrawLanguages()
        {
            LanguagesDataGrid.Items.Clear();

            foreach (var language in Program.Character.Details.Languages)
            {
                LanguagesDataGrid.Items.Add(language);
            }
        }

        private void DrawConditions()
        {
            ConditionsDataGrid.Items.Clear();

            foreach (var condition in Program.Character.Vitality.Conditions.ToArray())
            {
                ConditionsDataGrid.Items.Add(condition);
            }
        }

        #endregion

        #region Helpers

        private void FormatCarryWeight()
        {
            double weight = Program.Character.CarryWeight;

            if (weight <= Program.Character.LightCarryCapacity)
            {
                WeightCarriedField.Foreground = Brushes.Black;
                WeightCarriedBox.Background = new SolidColorBrush(Colours.LightGreen);
            }
            else if (weight > Program.Character.LightCarryCapacity && weight <= Program.Character.MediumCarryCapacity)
            {
                WeightCarriedField.Foreground = new SolidColorBrush(Colours.MediumRed);
                WeightCarriedBox.Background = new SolidColorBrush(Colours.LightYellow);
            }
            else if (weight > Program.Character.MediumCarryCapacity && weight <= Program.Character.HeavyCarryCapacity)
            {
                WeightCarriedField.Foreground = Brushes.DarkRed;
                WeightCarriedBox.Background = Brushes.Pink;
            }
            else
            {
                WeightCarriedField.Foreground = Brushes.Red;
                WeightCarriedBox.Background = Brushes.DimGray;
            }
        }

        #endregion

        #endregion

        #region Accessors

        public double ProficiencyGridSize
        {
            get
            {
                return WeaponGrid.RenderSize.Height;
            }
        }

        private ModifyWealthWindow ModifyWealthWindow { get; }
        private ProficiencyPopupWindow ProficiencyPopupWindow { get; }
        private ModifyProficiencyWindow ModifyProficiencyWindow { get; }
        private MondifyConditionsWindow MondifyConditionsWindow { get; }
        private ModifyLanguagesWindow ModifyLanguagesWindow { get; }

        #endregion

        #region Events

        private void EditWealthButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyWealthWindow.ShowWindow();
            DrawWealth();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (WeaponProficiencyDataGrid.SelectedItem != null)
            {
                var weapon = (KeyValuePair<Guid, string>)WeaponProficiencyDataGrid.SelectedItem;
                Program.Character.Proficiency.Weapons.Remove(weapon.Key);
                DrawProficiencies();
            }
            else if (ArmorProficiencyDataGrid.SelectedItem != null)
            {
                var armor = (KeyValuePair<Guid, string>)ArmorProficiencyDataGrid.SelectedItem;
                Program.Character.Proficiency.Armors.Remove(armor.Key);
                DrawProficiencies();
            }
            else if (ShieldProficiencyDataGrid.SelectedItem != null)
            {
                var shield = (KeyValuePair<Guid, string>)ShieldProficiencyDataGrid.SelectedItem;
                Program.Character.Proficiency.Shields.Remove(shield.Key);
                DrawProficiencies();
            }
            else if (ToolProficiencyDataGrid.SelectedItem != null)
            {
                var tool = (KeyValuePair<Guid, string>)ToolProficiencyDataGrid.SelectedItem;
                Program.Character.Proficiency.Tools.Remove(tool.Key);
                DrawProficiencies();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<Guid, string> proficiency;

            if (WeaponProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)WeaponProficiencyDataGrid.SelectedItem;
                ModifyProficiencyWindow.ShowEdit(Constants.PopupButtons.WeaponProficiency, proficiency.Key);
                DrawProficiencies();
            }
            else if (ArmorProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)ArmorProficiencyDataGrid.SelectedItem;
                ModifyProficiencyWindow.ShowEdit(Constants.PopupButtons.ArmorProficiency, proficiency.Key);
                DrawProficiencies();
            }
            else if (ShieldProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)ShieldProficiencyDataGrid.SelectedItem;
                ModifyProficiencyWindow.ShowEdit(Constants.PopupButtons.ShieldProficiency, proficiency.Key);
                DrawProficiencies();
            }
            else if (ToolProficiencyDataGrid.SelectedItem != null)
            {
                proficiency = (KeyValuePair<Guid, string>)ToolProficiencyDataGrid.SelectedItem;
                ModifyProficiencyWindow.ShowEdit(Constants.PopupButtons.ToolProficiency, proficiency.Key);
                DrawProficiencies();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Constants.PopupButtons popupButtons;

            popupButtons = ProficiencyPopupWindow.ShowPopup();

            ModifyProficiencyWindow.ShowAdd(popupButtons);

            DrawProficiencies();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            WeaponProficiencyDataGrid.UnselectAll();
            ArmorProficiencyDataGrid.UnselectAll();
            ShieldProficiencyDataGrid.UnselectAll();
            ToolProficiencyDataGrid.UnselectAll();
        }

        private void WeaponProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeaponProficiencyDataGrid.SelectedItem != null)
            {
                ArmorProficiencyDataGrid.UnselectAll();
                ShieldProficiencyDataGrid.UnselectAll();
                ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ArmorProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArmorProficiencyDataGrid.SelectedItem != null)
            {
                WeaponProficiencyDataGrid.UnselectAll();
                ShieldProficiencyDataGrid.UnselectAll();
                ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ShieldProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShieldProficiencyDataGrid.SelectedItem != null)
            {
                WeaponProficiencyDataGrid.UnselectAll();
                ArmorProficiencyDataGrid.UnselectAll();
                ToolProficiencyDataGrid.UnselectAll();
            }
        }

        private void ToolProficiencyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ToolProficiencyDataGrid.SelectedItem != null)
            {
                WeaponProficiencyDataGrid.UnselectAll();
                ArmorProficiencyDataGrid.UnselectAll();
                ShieldProficiencyDataGrid.UnselectAll();
            }
        }

        #endregion

        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            MondifyConditionsWindow.ShowEdit();
            DrawConditions();
        }

        private void EditLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (LanguagesDataGrid.SelectedItem != null)
            {
                ModifyLanguagesWindow.ShowEdit(LanguagesDataGrid.SelectedItem as Language);
                DrawLanguages();
            }
        }

        private void AddLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyLanguagesWindow.ShowAdd();
            DrawLanguages();
        }

        private void DeleteLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (LanguagesDataGrid.SelectedItem != null)
            {
                Program.Character.Details.Languages.Remove(LanguagesDataGrid.SelectedItem as Language);
                DrawLanguages();
            }
        }

        private void ClearLanguagesButton_Click(object sender, RoutedEventArgs e)
        {
            LanguagesDataGrid.UnselectAll();
        }

        private void ClearConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            ConditionsDataGrid.UnselectAll();
        }
    }
}
