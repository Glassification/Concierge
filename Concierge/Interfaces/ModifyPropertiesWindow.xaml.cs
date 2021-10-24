// <copyright file="ModifyPropertiesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyPropertiesWindow.xaml.
    /// </summary>
    public partial class ModifyPropertiesWindow : Window, IConciergeModifyWindow
    {
        public ModifyPropertiesWindow()
        {
            this.InitializeComponent();
            this.AlignmentComboBox.ItemsSource = Constants.Alignment;
            this.RaceComboBox.ItemsSource = Constants.Races;
            this.BackgroundComboBox.ItemsSource = Constants.Backgrounds;
            this.Class1ComboBox.ItemsSource = Constants.Classes;
            this.Class2ComboBox.ItemsSource = Constants.Classes;
            this.Class3ComboBox.ItemsSource = Constants.Classes;

            Program.Logger.Info($"Initialized {nameof(ModifyPropertiesWindow)}.");
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit()
        {
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void FillFields()
        {
            var character = Program.CcsFile.Character;

            this.Level1UpDown.UpdatingValue();
            this.Level2UpDown.UpdatingValue();
            this.Level3UpDown.UpdatingValue();

            this.NameTextBox.Text = character.Details.Name;
            this.RaceComboBox.Text = character.Details.Race;
            this.BackgroundComboBox.Text = character.Details.Background;
            this.AlignmentComboBox.Text = character.Details.Alignment;
            this.Level1UpDown.Value = character.Class1.Level;
            this.Level2UpDown.Value = character.Class2.Level;
            this.Level3UpDown.Value = character.Class3.Level;
            this.Class1ComboBox.Text = character.Class1.Name;
            this.Class2ComboBox.Text = character.Class2.Name;
            this.Class3ComboBox.Text = character.Class3.Name;
        }

        private void UpdateProperties()
        {
            var character = Program.CcsFile.Character;

            character.Details.Name = this.NameTextBox.Text;
            character.Details.Race = this.RaceComboBox.Text;
            character.Details.Background = this.BackgroundComboBox.Text;
            character.Details.Alignment = this.AlignmentComboBox.Text;
            character.Class1.Level = this.Level1UpDown.Value ?? 0;
            character.Class2.Level = this.Level2UpDown.Value ?? 0;
            character.Class3.Level = this.Level3UpDown.Value ?? 0;
            character.Class1.Name = this.Class1ComboBox.Text;
            character.Class2.Name = this.Class2ComboBox.Text;
            character.Class3.Name = this.Class3ComboBox.Text;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Program.Logger.Info($"Escape key pressed.");
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateProperties();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateProperties();
            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;

            this.Hide();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;

            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
