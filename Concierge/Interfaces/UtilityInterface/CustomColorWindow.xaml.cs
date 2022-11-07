// <copyright file="CustomColorWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Concierge.Interfaces.Components;
    using Concierge.Primitives;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for CustomColorWindow.xaml.
    /// </summary>
    public partial class CustomColorWindow : ConciergeWindow
    {
        private Color _color;

        public CustomColorWindow()
        {
            this.InitializeComponent();
            this.ColorPickerImage.LoadFromByteArray(Properties.Resources.ColorPickerBackground);
            this.RgbValueLock = false;
            this.RgbSliderLock = false;
        }

        public override string HeaderText => "Colour Picker";

        public Color SelectedColor
        {
            get
            {
                return this._color;
            }

            set
            {
                this._color = value;
                this.ColorPreviewFill.Background = new SolidColorBrush(this._color);
            }
        }

        private bool RgbValueLock { get; set; }

        private bool RgbSliderLock { get; set; }

        public override CustomColor ShowColorWindow(CustomColor startingColor)
        {
            this.NameTextBox.Text = startingColor.Name;
            this.UpdateRgbValues(startingColor.Color);
            this.UpdateRgbSlider(startingColor.Color);
            this.ShowConciergeWindow();

            if (this.SelectedColor == Colors.Transparent)
            {
                return CustomColor.Empty;
            }
            else
            {
                return new CustomColor(this.NameTextBox.Text, this.SelectedColor.R, this.SelectedColor.G, this.SelectedColor.B);
            }
        }

        private void ParseHexValue()
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(ColorUtility.FormatHexString(this.HexTextBox.Text));
                this.UpdateRgbValues(color);
                this.UpdateRgbSlider(color);
            }
            catch (Exception)
            {
                this.HexTextBox.Text = this.SelectedColor.ToHexWithoutAlpha();
            }
        }

        private void UpdateRgbValues(Color color)
        {
            this.RgbValueLock = true;
            this.RedUpDown.Value = color.R;
            this.GreenUpDown.Value = color.G;
            this.RgbValueLock = false;
            this.BlueUpDown.Value = color.B;
        }

        private void UpdateRgbSlider(Color color)
        {
            this.RgbSliderLock = true;
            this.RedSlider.Value = color.R;
            this.GreenSlider.Value = color.G;
            this.BlueSlider.Value = color.B;
            this.RgbSliderLock = false;
        }

        private void SetColorAtPoint(Point point)
        {
            if (this.ColorPickerImage.Source is not BitmapSource img)
            {
                return;
            }

            if (point.X > 0 && point.Y > 0 && point.X < img.PixelWidth && point.Y < img.PixelHeight)
            {
                var color = this.ColorPickerImage.GetColorFromPoint(point);
                this.UpdateRgbValues(color);
                this.UpdateRgbSlider(color);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = Colors.Transparent;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void UpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (this.RgbValueLock)
            {
                return;
            }

            this.SelectedColor = Color.FromArgb(
                255,
                (byte)this.RedUpDown.Value,
                (byte)this.GreenUpDown.Value,
                (byte)this.BlueUpDown.Value);
            this.HexTextBox.Text = this.SelectedColor.ToHexWithoutAlpha();
            this.UpdateRgbSlider(this.SelectedColor);
        }

        private void HexTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.ParseHexValue();
        }

        private void HexTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.ParseHexValue();
                    break;
            }
        }

        private void ColorPickerImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            this.SetColorAtPoint(e.GetPosition(this.ColorPickerImage));
        }

        private void ColorPickerImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Cross;
        }

        private void ColorPickerImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ColorPickerImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(this.ColorPickerImage);
            this.SetColorAtPoint(e.GetPosition(this.ColorPickerImage));
        }

        private void ColorPickerImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.RgbSliderLock)
            {
                return;
            }

            this.SelectedColor = Color.FromArgb(
                255,
                (byte)this.RedSlider.Value,
                (byte)this.GreenSlider.Value,
                (byte)this.BlueSlider.Value);
            this.HexTextBox.Text = this.SelectedColor.ToHexWithoutAlpha();
            this.UpdateRgbValues(this.SelectedColor);
        }
    }
}
