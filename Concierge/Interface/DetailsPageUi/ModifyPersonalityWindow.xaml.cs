// <copyright file="ModifyPersonalityWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.DetailsPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Characteristics;

    /// <summary>
    /// Interaction logic for ModifyPersonalityWindow.xaml.
    /// </summary>
    public partial class ModifyPersonalityWindow : Window
    {
        public ModifyPersonalityWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Personality Personality { get; set; }

        public void ShowEdit(Personality personality)
        {
            this.Personality = personality;
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.Trait1TextBox.Text = this.Personality.Trait1;
            this.Trait2TextBox.Text = this.Personality.Trait2;
            this.IdealTextBox.Text = this.Personality.Ideal;
            this.BondTextBox.Text = this.Personality.Bond;
            this.FlawTextBox.Text = this.Personality.Flaw;
            this.BackgroundTextBox.Text = this.Personality.Background;
            this.NotesTextBox.Text = this.Personality.Notes;
        }

        private void UpdatePersonality()
        {
            this.Personality.Trait1 = this.Trait1TextBox.Text;
            this.Personality.Trait2 = this.Trait2TextBox.Text;
            this.Personality.Ideal = this.IdealTextBox.Text;
            this.Personality.Bond = this.BondTextBox.Text;
            this.Personality.Flaw = this.FlawTextBox.Text;
            this.Personality.Background = this.BackgroundTextBox.Text;
            this.Personality.Notes = this.NotesTextBox.Text;
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
            Program.Modify();

            this.UpdatePersonality();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdatePersonality();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
