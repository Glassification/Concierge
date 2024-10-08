﻿// <copyright file="CustomColorWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Display.Components;
    using MaterialDesignThemes.Wpf;

    using Constants = Concierge.Common.Constants;

    /// <summary>
    /// Interaction logic for CustomColorWindow.xaml.
    /// </summary>
    public partial class CustomColorWindow : ConciergeWindow
    {
        private Color color;
        private bool rgbValueLock;
        private bool rgbSliderLock;

        public CustomColorWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ColorPickerImage.LoadFromByteArray(Properties.Resources.ColorPickerBackground);
        }

        public override string HeaderText => "Custom Colour";

        public override string WindowName => nameof(CustomColorWindow);

        public Color SelectedColor
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                this.ColorPreviewFill.Background = new SolidColorBrush(this.color);
            }
        }

        public override CustomColor ShowColorWindow(CustomColor startingColor)
        {
            this.NameTextBox.Text = startingColor.IsValid ? startingColor.Name : string.Empty;
            this.UpdateRgbSpinner(startingColor.Color);
            this.UpdateRgbSlider(startingColor.Color);
            this.ShowConciergeWindow();

            if (this.SelectedColor == Colors.Transparent)
            {
                return CustomColor.Invalid;
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
                var color = (Color)ColorConverter.ConvertFromString(ColorUtility.FormatHex(this.HexTextBox.Text));
                this.UpdateRgbSpinner(color);
                this.UpdateRgbSlider(color);
            }
            catch (Exception)
            {
                this.HexTextBox.Text = this.SelectedColor.ToHexWithoutAlpha();
            }
        }

        private void UpdateBrightness(Color color)
        {
            var brightness = color.GetBrightness();

            this.BrightnessTextBox.Text = brightness.ToString();
            if (brightness < 85)
            {
                this.GoodForTextIcon.Foreground = Brushes.IndianRed;
                this.GoodForTextIcon.Kind = PackIconKind.LowQuality;
            }
            else if (brightness > 170)
            {
                this.GoodForTextIcon.Foreground = ConciergeBrushes.Mint;
                this.GoodForTextIcon.Kind = PackIconKind.HighQuality;
            }
            else
            {
                this.GoodForTextIcon.Foreground = ConciergeBrushes.Deer;
                this.GoodForTextIcon.Kind = PackIconKind.MediumQuality;
            }
        }

        private void UpdateRgbSpinner(Color color)
        {
            this.rgbValueLock = true;
            this.RedUpDown.Value = color.R;
            this.GreenUpDown.Value = color.G;
            this.UpdateBrightness(color);
            this.rgbValueLock = false;
            this.BlueUpDown.Value = color.B;
        }

        private void UpdateRgbSlider(Color color)
        {
            this.rgbSliderLock = true;
            this.RedSlider.Value = color.R;
            this.GreenSlider.Value = color.G;
            this.BlueSlider.Value = color.B;
            this.UpdateBrightness(color);
            this.rgbSliderLock = false;
        }

        private void SetColorAtPoint()
        {
            if (this.ColorPickerImage.Source is not BitmapSource img)
            {
                return;
            }

            var point = Mouse.GetPosition(this.ColorPickerCanvas);
            if (point.X <= 0 || point.Y <= 0 || point.X >= img.PixelWidth || point.Y >= img.PixelHeight)
            {
                return;
            }

            var cb = new CroppedBitmap(this.ColorPickerImage.Source as BitmapSource, new Int32Rect((int)point.X, (int)point.Y, 1, 1));
            var pixels = new byte[4];

            cb.CopyPixels(pixels, 4, 0);
            this.ColorPickerCanvas.InvalidateVisual();

            var color = Color.FromArgb(Constants.ColorSpace, pixels[2], pixels[1], pixels[0]);
            this.UpdateRgbSpinner(color);
            this.UpdateRgbSlider(color);
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
            if (this.rgbValueLock)
            {
                return;
            }

            this.SelectedColor = Color.FromArgb(
                Constants.ColorSpace,
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

            this.SetColorAtPoint();
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
            Mouse.Capture(this.ColorPickerCanvas);
            this.SetColorAtPoint();
        }

        private void ColorPickerImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.rgbSliderLock)
            {
                return;
            }

            this.SelectedColor = Color.FromArgb(
                Constants.ColorSpace,
                (byte)this.RedSlider.Value,
                (byte)this.GreenSlider.Value,
                (byte)this.BlueSlider.Value);
            this.HexTextBox.Text = this.SelectedColor.ToHexWithoutAlpha();
            this.UpdateRgbSpinner(this.SelectedColor);
        }
    }
}
