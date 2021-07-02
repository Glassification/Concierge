// <copyright file="ModifyPersonalityWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModifyPersonalityWindow.xaml.
    /// </summary>
    public partial class ModifyPersonalityWindow : Window
    {
        public ModifyPersonalityWindow()
        {
            this.InitializeComponent();
        }

        public void ShowEdit()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.Trait1TextBox.Text = Program.Character.Personality.Trait1;
            this.Trait2TextBox.Text = Program.Character.Personality.Trait2;
            this.IdealTextBox.Text = Program.Character.Personality.Ideal;
            this.BondTextBox.Text = Program.Character.Personality.Bond;
            this.FlawTextBox.Text = Program.Character.Personality.Flaw;
            this.BackgroundTextBox.Text = Program.Character.Personality.Background;
            this.NotesTextBox.Text = Program.Character.Personality.Notes;
        }

        private void UpdatePersonality()
        {
            Program.Character.Personality.Trait1 = this.Trait1TextBox.Text;
            Program.Character.Personality.Trait2 = this.Trait2TextBox.Text;
            Program.Character.Personality.Ideal = this.IdealTextBox.Text;
            Program.Character.Personality.Bond = this.BondTextBox.Text;
            Program.Character.Personality.Flaw = this.FlawTextBox.Text;
            Program.Character.Personality.Background = this.BackgroundTextBox.Text;
            Program.Character.Personality.Notes = this.NotesTextBox.Text;

            Program.Modified = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdatePersonality();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdatePersonality();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
