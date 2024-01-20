// <copyright file="AppearanceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Common.Enums;
    using Concierge.Data;
    using Concierge.Data.Units;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for AppearanceWindow.xaml.
    /// </summary>
    public partial class AppearanceWindow : ConciergeWindow
    {
        private CustomColor? eyeColor;
        private CustomColor? hairColor;
        private CustomColor? skinColor;

        public AppearanceWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.GenderComboBox.ItemsSource = ComboBoxGenerator.GenderComboBox();
            this.ConciergePage = ConciergePage.None;
            this.Appearance = new Appearance();
            this.EyeColor = CustomColor.Invalid;
            this.HairColor = CustomColor.Invalid;
            this.SkinColor = CustomColor.Invalid;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.GenderComboBox);
            this.SetMouseOverEvents(this.AgeUpDown);
            this.SetMouseOverEvents(this.FeetUpDown);
            this.SetMouseOverEvents(this.InchesUpDown);
            this.SetMouseOverEvents(this.WeightUpDown);
            this.SetMouseOverEvents(this.SkinColourTextBox, this.SkinColourTextBackground);
            this.SetMouseOverEvents(this.EyeColourTextBox, this.EyeColourTextBackground);
            this.SetMouseOverEvents(this.HairColourTextBox, this.HairColourTextBackground);
            this.SetMouseOverEvents(this.DistinguishingMarksTextBox, this.DistinguishingMarksTextBackground);
        }

        public override string HeaderText => "Edit Appearance";

        public override string WindowName => nameof(AppearanceWindow);

        private Appearance Appearance { get; set; }

        private CustomColor EyeColor
        {
            get
            {
                return this.eyeColor ?? CustomColor.Invalid;
            }

            set
            {
                this.eyeColor = value;
                this.EyeColourTextBox.Text = this.eyeColor.Name;
                this.EyeColorPreview.Color = this.eyeColor;
            }
        }

        private CustomColor HairColor
        {
            get
            {
                return this.hairColor ?? CustomColor.Invalid;
            }

            set
            {
                this.hairColor = value;
                this.HairColourTextBox.Text = this.hairColor.Name;
                this.HairColorPreview.Color = this.hairColor;
            }
        }

        private CustomColor SkinColor
        {
            get
            {
                return this.skinColor ?? CustomColor.Invalid;
            }

            set
            {
                this.skinColor = value;
                this.SkinColourTextBox.Text = this.skinColor.Name;
                this.SkinColorPreview.Color = this.skinColor;
            }
        }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.Appearance = Program.CcsFile.Character.Characteristic.Appearance;
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

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            this.UpdateAppearance();
            this.CloseConciergeWindow();
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
            this.SkinColor = this.Appearance.SkinColour;
            this.EyeColor = this.Appearance.EyeColour;
            this.HairColor = this.Appearance.HairColour;
            this.DistinguishingMarksTextBox.Text = this.Appearance.DistinguishingMarks;

            this.HeightUnits.Text = $"({UnitFormat.HeightPostfix})";
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";

            this.FillHeightFields();
        }

        private void UpdateAppearance()
        {
            var oldItem = this.Appearance.DeepCopy();

            this.Appearance.Gender = this.GenderComboBox.Text;
            this.Appearance.Age = this.AgeUpDown.Value;
            this.Appearance.Height.Value = this.UpdateHeight();
            this.Appearance.Weight.Value = this.WeightUpDown.Value;
            this.Appearance.SkinColour = this.SkinColor;
            this.Appearance.EyeColour = this.EyeColor;
            this.Appearance.HairColour = this.HairColor;
            this.Appearance.DistinguishingMarks = this.DistinguishingMarksTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Appearance>(this.Appearance, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateAppearance();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeColorButton button)
            {
                return;
            }

            switch (button.Name)
            {
                case "SkinColorPreview":
                    var skinColor = ConciergeWindowService.ShowColorWindow(typeof(ColorPickerWindow), this.SkinColor);
                    this.SkinColor = skinColor.IsValid ? skinColor : this.SkinColor;
                    break;
                case "EyeColorPreview":
                    var eyeColor = ConciergeWindowService.ShowColorWindow(typeof(ColorPickerWindow), this.EyeColor);
                    this.EyeColor = eyeColor.IsValid ? eyeColor : this.EyeColor;
                    break;
                case "HairColorPreview":
                    var hairColor = ConciergeWindowService.ShowColorWindow(typeof(ColorPickerWindow), this.HairColor);
                    this.HairColor = hairColor.IsValid ? hairColor : this.HairColor;
                    break;
            }
        }
    }
}
