using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.DetailsPageUi
{
    /// <summary>
    /// Interaction logic for ModifyPersonalityWindow.xaml
    /// </summary>
    public partial class ModifyPersonalityWindow : Window
    {
        public ModifyPersonalityWindow()
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
            Trait1TextBox.Text = Program.Character.Personality.Trait1;
            Trait2TextBox.Text = Program.Character.Personality.Trait2;
            IdealTextBox.Text = Program.Character.Personality.Ideal;
            BondTextBox.Text = Program.Character.Personality.Bond;
            FlawTextBox.Text = Program.Character.Personality.Flaw;
            BackgroundTextBox.Text = Program.Character.Personality.Background;
            NotesTextBox.Text = Program.Character.Personality.Notes;
        }

        private void UpdatePersonality()
        {
            Program.Character.Personality.Trait1 = Trait1TextBox.Text;
            Program.Character.Personality.Trait2 = Trait2TextBox.Text;
            Program.Character.Personality.Ideal = IdealTextBox.Text;
            Program.Character.Personality.Bond = BondTextBox.Text;
            Program.Character.Personality.Flaw = FlawTextBox.Text;
            Program.Character.Personality.Background = BackgroundTextBox.Text;
            Program.Character.Personality.Notes = NotesTextBox.Text;
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
            UpdatePersonality();
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePersonality();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
