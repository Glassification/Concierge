// <copyright file="NameGeneratorWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools;
    using Concierge.Tools.Enums;
    using Concierge.Tools.Generators;
    using Concierge.Tools.Generators.Names;

    /// <summary>
    /// Interaction logic for NameGeneratorWindow.xaml.
    /// </summary>
    public partial class NameGeneratorWindow : ConciergeWindow
    {
        private readonly string nameHistoryFile = Path.Combine(ConciergeFiles.HistoryDirectory, ConciergeFiles.NameGeneratorHistoryName);
        private readonly IGenerator nameGenerator;

        public NameGeneratorWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.RaceComboBox.ItemsSource = Defaults.Races;
            this.GenderComboBox.ItemsSource = Enum.GetValues(typeof(Gender)).Cast<Gender>();
            this.nameGenerator = new NameGenerator(Defaults.Names.ToList());
            this.History = new History(HistoryReadWriter.Read(this.nameHistoryFile), string.Empty);

            this.SetGenderState(false);
            this.SetRaceState(false);
        }

        public override string HeaderText => "Name Generator";

        public override string WindowName => nameof(NameGeneratorWindow);

        private History History { get; set; }

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();

            return this.Result == ConciergeWindowResult.OK ? this.NameTextBox.Text : null;
        }

        private Gender GetGenderText()
        {
            return this.GenderComboBox.Text switch
            {
                "Male" => Gender.Male,
                "Female" => Gender.Female,
                _ => Gender.Other,
            };
        }

        private void SetGenderState(bool isEnabled)
        {
            this.GenderComboBox.IsEnabled = isEnabled;
            this.GenderLabel.IsEnabled = isEnabled;
            this.GenderComboBox.Opacity = isEnabled ? 1 : 0.5;
            this.GenderLabel.Opacity = isEnabled ? 1 : 0.5;
        }

        private void SetRaceState(bool isEnabled)
        {
            this.RaceComboBox.IsEnabled = isEnabled;
            this.RaceLabel.IsEnabled = isEnabled;
            this.RaceComboBox.Opacity = isEnabled ? 1 : 0.5;
            this.RaceLabel.Opacity = isEnabled ? 1 : 0.5;
        }

        private void ScrollHistory(HistoryDirection direction)
        {
            if (!this.NameTextBox.IsFocused)
            {
                return;
            }

            switch (direction)
            {
                case HistoryDirection.Backward:
                    this.NameTextBox.Text = this.History.Backward();
                    this.NameTextBox.Select(this.NameTextBox.Text.Length, 0);
                    break;
                case HistoryDirection.Forward:
                    this.NameTextBox.Text = this.History.Forward();
                    this.NameTextBox.Select(this.NameTextBox.Text.Length, 0);
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            var settings = new NameSettings(
                this.FilterGenderCheckBox.IsChecked ?? false,
                this.GetGenderText(),
                this.FilterRaceCheckBox.IsChecked ?? false,
                this.RaceComboBox.Text);

            var result = this.nameGenerator.Generate(settings);
            if (result is NameResult nameResult)
            {
                var name = nameResult.FullName;

                this.NameTextBox.Text = name;
                HistoryReadWriter.Write(this.nameHistoryFile, name);
                this.History.Add(name);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void FilterGenderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.SetGenderState(true);
        }

        private void FilterGenderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.SetGenderState(false);
        }

        private void FilterRaceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.SetRaceState(true);
        }

        private void FilterRaceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.SetRaceState(false);
        }

        private void NameTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            this.ScrollHistory(e.Key == Key.Up ? HistoryDirection.Backward : e.Key == Key.Down ? HistoryDirection.Forward : HistoryDirection.None);
        }

        private void NameTextBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            this.ScrollHistory(e.Delta < 0 ? HistoryDirection.Forward : HistoryDirection.Backward);
        }
    }
}
