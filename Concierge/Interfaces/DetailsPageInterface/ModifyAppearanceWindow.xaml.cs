// <copyright file="ModifyAppearanceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.DetailsPageInterface
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.UtilityInterface;
    using Concierge.Primitives;
    using Concierge.Services;
    using Concierge.Utility.Units;
    using Concierge.Utility.Units.Enums;

    /// <summary>
    /// Interaction logic for ModifyAppearanceWindow.xaml.
    /// </summary>
    public partial class ModifyAppearanceWindow : ConciergeWindow
    {
        private CustomColor? _eyeColor;
        private CustomColor? _hairColor;
        private CustomColor? _skinColor;

        public ModifyAppearanceWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.GenderComboBox.ItemsSource = Enum.GetValues(typeof(Gender)).Cast<Gender>();
            this.ConciergePage = ConciergePage.None;
            this.Appearance = new Appearance();
            this.EyeColor = CustomColor.Invalid;
            this.HairColor = CustomColor.Invalid;
            this.SkinColor = CustomColor.Invalid;
        }

        public override string HeaderText => "Edit Appearance";

        private Appearance Appearance { get; set; }

        private CustomColor EyeColor
        {
            get
            {
                return this._eyeColor ?? CustomColor.Invalid;
            }

            set
            {
                this._eyeColor = value;
                this.EyeColourTextBox.Text = this._eyeColor.Name;
                this.EyeColorPreview.Color = this._eyeColor;
            }
        }

        private CustomColor HairColor
        {
            get
            {
                return this._hairColor ?? CustomColor.Invalid;
            }

            set
            {
                this._hairColor = value;
                this.HairColourTextBox.Text = this._hairColor.Name;
                this.HairColorPreview.Color = this._hairColor;
            }
        }

        private CustomColor SkinColor
        {
            get
            {
                return this._skinColor ?? CustomColor.Invalid;
            }

            set
            {
                this._skinColor = value;
                this.SkinColourTextBox.Text = this._skinColor.Name;
                this.SkinColorPreview.Color = this._skinColor;
            }
        }

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

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateAppearance();
            this.CloseConciergeWindow();

            Program.Modify();
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
            this.Result = ConciergeWindowResult.Exit;
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

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
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
