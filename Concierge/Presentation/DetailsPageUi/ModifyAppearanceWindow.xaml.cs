using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.DetailsPageUi
{
    /// <summary>
    /// Interaction logic for ModifyAppearanceWindow.xaml
    /// </summary>
    public partial class ModifyAppearanceWindow : Window
    {
        public ModifyAppearanceWindow()
        {
            InitializeComponent();
        }

        public void ShowEdit()
        {
            FillFields();
            ShowDialog();
        }

        private void FillFields()
        {
            GenderTextBox.Text = Program.Character.Appearance.Gender;
            AgeTextBox.Text = Program.Character.Appearance.Age;
            HeightTextBox.Text = Program.Character.Appearance.Height;
            WeightTextBox.Text = Program.Character.Appearance.Weight;
            SkinColourTextBox.Text = Program.Character.Appearance.SkinColour;
            EyeColourTextBox.Text = Program.Character.Appearance.EyeColour;
            HairColourTextBox.Text = Program.Character.Appearance.HairColour;
            DistinguishingMarksTextBox.Text = Program.Character.Appearance.DistinguishingMarks;
        }

        private void UpdateAppearance()
        {
            Program.Character.Appearance.Gender = GenderTextBox.Text;
            Program.Character.Appearance.Age = AgeTextBox.Text;
            Program.Character.Appearance.Height = HeightTextBox.Text;
            Program.Character.Appearance.Weight = WeightTextBox.Text;
            Program.Character.Appearance.SkinColour = SkinColourTextBox.Text;
            Program.Character.Appearance.EyeColour = EyeColourTextBox.Text;
            Program.Character.Appearance.HairColour = HairColourTextBox.Text;
            Program.Character.Appearance.DistinguishingMarks = DistinguishingMarksTextBox.Text;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAppearance();
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAppearance();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
