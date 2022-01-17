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
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;

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
            this.Lock = false;
        }

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

        private bool Lock { get; set; }

        public override Color ShowColorWindow(Color startingColor)
        {
            this.UpdateRgbValues(startingColor);
            this.ShowConciergeWindow();

            return this.SelectedColor;
        }

        private void ParseHexValue()
        {
            try
            {
                this.UpdateRgbValues((Color)ColorConverter.ConvertFromString(this.HexTextBox.Text));
            }
            catch (Exception)
            {
                this.HexTextBox.Text = this.SelectedColor.ToHexWithoutAlpha();
            }
        }

        private void UpdateRgbValues(Color color)
        {
            this.Lock = true;
            this.RedUpDown.Value = color.R;
            this.GreenUpDown.Value = color.G;
            this.Lock = false;
            this.BlueUpDown.Value = color.B;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = Colors.Transparent;
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;
            this.HideConciergeWindow();
        }

        private void UpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (this.Lock)
            {
                return;
            }

            this.SelectedColor = Color.FromArgb(
                255,
                (byte)this.RedUpDown.Value,
                (byte)this.GreenUpDown.Value,
                (byte)this.BlueUpDown.Value);
            this.HexTextBox.Text = this.SelectedColor.ToHexWithoutAlpha();
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

            var point = e.GetPosition(this.ColorPickerImage);
            var img = this.ColorPickerImage.Source as BitmapSource;

            if (point.X > 0 && point.Y > 0 && point.X < img.PixelWidth && point.Y < img.PixelHeight)
            {
                this.UpdateRgbValues(this.ColorPickerImage.GetColorFromPoint(point));
            }
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
        }

        private void ColorPickerImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }
    }
}
