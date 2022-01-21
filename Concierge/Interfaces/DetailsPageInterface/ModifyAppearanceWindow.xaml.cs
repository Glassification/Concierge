// <copyright file="ModifyAppearanceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Units;
    using Concierge.Utility.Units.Enums;

    /// <summary>
    /// Interaction logic for ModifyAppearanceWindow.xaml.
    /// </summary>
    public partial class ModifyAppearanceWindow : ConciergeWindow
    {
        public ModifyAppearanceWindow()
        {
            this.InitializeComponent();
            this.GenderComboBox.ItemsSource = Enum.GetValues(typeof(Gender)).Cast<Gender>();
            this.ConciergePage = ConciergePage.None;
            this.Appearance = new Appearance();
        }

        private Appearance Appearance { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Appearance = Program.CcsFile.Character.Appearance;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T appearance)
        {
            if (appearance is not Appearance castItem)
            {
                return;
            }

            this.Appearance = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        private void FillHeightFields()
        {
            if (this.Appearance.Height.UnitType == UnitTypes.Imperial)
            {
                var feedAndInches = UnitFormat.GetSeperateFeetAndInches(this.Appearance.Height.Value);

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
                return UnitFormat.CombineFeetAndInches(this.FeetUpDown.Value, this.InchesUpDown.Value);
            }
            else
            {
                return this.HeightUpDown.Value;
            }
        }

        private void FillFields()
        {
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
            var oldItem = this.Appearance.DeepCopy();

            this.Appearance.Gender = this.GenderComboBox.Text;
            this.Appearance.Age = this.AgeUpDown.Value;
            this.Appearance.Height.Value = this.UpdateHeight();
            this.Appearance.Weight.Value = this.WeightUpDown.Value;
            this.Appearance.SkinColour = this.SkinColourTextBox.Text;
            this.Appearance.EyeColour = this.EyeColourTextBox.Text;
            this.Appearance.HairColour = this.HairColourTextBox.Text;
            this.Appearance.DistinguishingMarks = this.DistinguishingMarksTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Appearance>(this.Appearance, oldItem, this.ConciergePage));
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
