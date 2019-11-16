using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.OverviewPageUi
{
    /// <summary>
    /// Interaction logic for ModifyAttributesWindow.xaml
    /// </summary>
    public partial class ModifyAttributesWindow : Window
    {
        public ModifyAttributesWindow()
        {
            InitializeComponent();
        }

        public void EditAttributes()
        {
            FillFields();
            ShowDialog();
        }

        private void FillFields()
        {
            StrengthUpDown.Value = Program.Character.Attributes.Strength;
            DexterityUpDown.Value = Program.Character.Attributes.Dexterity;
            ConstitutionUpDown.Value = Program.Character.Attributes.Constitution;
            IntelligenceUpDown.Value = Program.Character.Attributes.Intelligence;
            WisdomUpDown.Value = Program.Character.Attributes.Wisdom;
            CharismaUpDown.Value = Program.Character.Attributes.Charisma;
        }

        private void UpdateAttributes()
        {
            Program.Character.Attributes.Strength = StrengthUpDown.Value ?? 0;
            Program.Character.Attributes.Dexterity = DexterityUpDown.Value ?? 0;
            Program.Character.Attributes.Constitution = ConstitutionUpDown.Value ?? 0;
            Program.Character.Attributes.Intelligence = IntelligenceUpDown.Value ?? 0;
            Program.Character.Attributes.Wisdom = WisdomUpDown.Value ?? 0;
            Program.Character.Attributes.Charisma = CharismaUpDown.Value ?? 0;
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAttributes();
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateAttributes();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
