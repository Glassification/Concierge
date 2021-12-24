// <copyright file="ModifyAppearanceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Units;
    using Concierge.Utility.Units.Enums;

    /// <summary>
    /// Interaction logic for ModifyAppearanceWindow.xaml.
    /// </summary>
    public partial class ModifyAppearanceWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyAppearanceWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.GenderComboBox.ItemsSource = Enum.GetValues(typeof(Gender)).Cast<Gender>();
            this.conciergePage = conciergePage;
        }

        private Appearance Appearance { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Appearance = Program.CcsFile.Character.Appearance;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void ShowEdit(Appearance appearance)
        {
            this.Appearance = appearance;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillHeightFields()
        {
            if (this.Appearance.Height.UnitType == UnitTypes.Imperial)
            {
                var feedAndInches = Utilities.GetSeperateFeetAndInches(this.Appearance.Height.Value);

                this.MetricGrid.Visibility = Visibility.Collapsed;
                this.ImperialGrid.Visibility = Visibility.Visible;
                this.FeetUpDown.Value = feedAndInches.Feet;
                this.InchesUpDown.Value = feedAndInches.Inches;
            }
            else
            {
                this.MetricGrid.Visibility = Visibility.Visible;
                this.ImperialGrid.Visibility = Visibility.Collapsed;
                this.HeightUpDown.Value = this.Appearance.Height.Value;
            }
        }

        private double UpdateHeight()
        {
            if (this.Appearance.Height.UnitType == UnitTypes.Imperial)
            {
                return Utilities.CombineFeetAndInches(this.FeetUpDown.Value ?? 0, this.InchesUpDown.Value ?? 0);
            }
            else
            {
                return this.HeightUpDown.Value ?? 0;
            }
        }

        private void FillFields()
        {
            this.AgeUpDown.UpdatingValue();

            this.GenderComboBox.Text = this.Appearance.Gender;
            this.AgeUpDown.Value = this.Appearance.Age;
            this.WeightUpDown.Value = this.Appearance.Weight.Value;
            this.SkinColourTextBox.Text = this.Appearance.SkinColour;
            this.EyeColourTextBox.Text = this.Appearance.EyeColour;
            this.HairColourTextBox.Text = this.Appearance.HairColour;
            this.DistinguishingMarksTextBox.Text = this.Appearance.DistinguishingMarks;

            this.HeightUnits.Text = $"({UnitFormat.HeightPostfix})";
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";

            this.UpdateColorPreviews();
            this.FillHeightFields();
        }

        private void UpdateAppearance()
        {
            var oldItem = this.Appearance.DeepCopy() as Appearance;

            this.Appearance.Gender = this.GenderComboBox.Text;
            this.Appearance.Age = this.AgeUpDown.Value ?? 0;
            this.Appearance.Height.Value = this.UpdateHeight();
            this.Appearance.Weight.Value = this.WeightUpDown.Value ?? 0;
            this.Appearance.SkinColour = this.SkinColourTextBox.Text;
            this.Appearance.EyeColour = this.EyeColourTextBox.Text;
            this.Appearance.HairColour = this.HairColourTextBox.Text;
            this.Appearance.DistinguishingMarks = this.DistinguishingMarksTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Appearance>(this.Appearance, oldItem, this.conciergePage));
        }

        private void UpdateColorPreviews()
        {
            this.SkinColorPreview.Background = this.SkinColourTextBox.Text.ToBrush();
            this.EyeColorPreview.Background = this.EyeColourTextBox.Text.ToBrush();
            this.HairColorPreview.Background = this.HairColourTextBox.Text.ToBrush();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateAppearance();
            this.HideConciergeWindow();

            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateAppearance();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }

        private void ColourTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.UpdateColorPreviews();
        }

        private void ColourTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.UpdateColorPreviews();
                    break;
            }
        }
    }
}
