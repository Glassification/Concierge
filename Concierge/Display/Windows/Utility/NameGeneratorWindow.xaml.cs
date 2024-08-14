// <copyright file="NameGeneratorWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools;
    using Concierge.Tools.Enums;
    using Concierge.Tools.Generators.Names;

    /// <summary>
    /// Interaction logic for NameGeneratorWindow.xaml.
    /// </summary>
    public partial class NameGeneratorWindow : ConciergeWindow
    {
        private readonly string nameHistoryFile = Path.Combine(ConciergeFiles.HistoryDirectory, ConciergeFiles.NameGeneratorHistoryName);
        private readonly NameGenerator nameGenerator = new ([.. Defaults.Names]);
        private readonly HistoryReadWriter historyReadWriter = new (Program.ErrorService);
        private readonly History history;

        public NameGeneratorWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.RaceComboBox.ItemsSource = ComboBoxGenerator.RacesComboBox();
            this.GenderComboBox.ItemsSource = ComboBoxGenerator.GenderComboBox();
            this.history = new History(this.historyReadWriter.ReadList<string>(this.nameHistoryFile), string.Empty);

            this.SetGenderState(false);
            this.SetRaceState(false);
        }

        public override string HeaderText => "Name Generator";

        public override string WindowName => nameof(NameGeneratorWindow);

        public override object? ShowWindow()
        {
            this.RaceComboBox.SelectedIndex = 0;
            this.GenderComboBox.SelectedIndex = 0;

            this.ShowConciergeWindow();

            return this.Result == ConciergeResult.OK ? this.NameTextBox.Text : null;
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
            DisplayUtility.SetControlEnableState(this.GenderComboBox, isEnabled);
            DisplayUtility.SetControlEnableState(this.GenderLabel, isEnabled);
        }

        private void SetRaceState(bool isEnabled)
        {
            DisplayUtility.SetControlEnableState(this.RaceComboBox, isEnabled);
            DisplayUtility.SetControlEnableState(this.RaceLabel, isEnabled);
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
                    this.NameTextBox.Text = this.history.Backward();
                    this.NameTextBox.Select(this.NameTextBox.Text.Length, 0);
                    break;
                case HistoryDirection.Forward:
                    this.NameTextBox.Text = this.history.Forward();
                    this.NameTextBox.Select(this.NameTextBox.Text.Length, 0);
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
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
                this.historyReadWriter.Append(this.nameHistoryFile, name);
                this.history.Add(name);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void FilterGenderCheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (sender is ConciergeCheckBox conciergeCheckBox)
            {
                this.SetGenderState(conciergeCheckBox.IsChecked ?? false);
            }
        }

        private void FilterRaceCheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (sender is ConciergeCheckBox conciergeCheckBox)
            {
                this.SetRaceState(conciergeCheckBox.IsChecked ?? false);
            }
        }

        private void NameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.ScrollHistory(e.Key == Key.Up ? HistoryDirection.Backward : e.Key == Key.Down ? HistoryDirection.Forward : HistoryDirection.None);
        }

        private void NameTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.ScrollHistory(e.Delta < 0 ? HistoryDirection.Forward : HistoryDirection.Backward);
        }
    }
}
