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

        #endregion

        #region Helpers

        private void FormatCarryWeight()
        {
            double weight = Program.Character.CarryWeight;

            if (weight <= Program.Character.LightCarryCapacity)
            {
                WeightCarriedField.Foreground = Brushes.Black;
                WeightCarriedBox.Background = new SolidColorBrush(Constants.LightGreen);
            }
            else if (weight > Program.Character.LightCarryCapacity && weight <= Program.Character.MediumCarryCapacity)
            {
                WeightCarriedField.Foreground = new SolidColorBrush(Constants.MediumRed);
                WeightCarriedBox.Background = new SolidColorBrush(Constants.LightYellow);
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
    }
}
