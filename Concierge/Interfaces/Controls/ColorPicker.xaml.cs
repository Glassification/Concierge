// <copyright file="ColorPicker.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Interfaces.Components;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for ColorPicker.xaml.
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private static readonly RoutedEvent ColorChangedEvent =
            EventManager.RegisterRoutedEvent(
            "ColorChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ColorPicker));

        private Color _selectedColor;

        public ColorPicker()
        {
            this.InitializeComponent();
            this.SelectedColor = Colors.White;
            this.DefaultColorList.ItemsSource = ColorUtility.ListDotNetColors();
            this.DefaultColorList.SelectedIndex = 0;
            this.SelectedColor = Colors.White;
        }

        public event RoutedEventHandler ColorChanged
        {
            add { this.AddHandler(ColorChangedEvent, value); }
            remove { this.RemoveHandler(ColorChangedEvent, value); }
        }

        public Color SelectedColor
        {
            get
            {
                return this._selectedColor;
            }

            set
            {
                this._selectedColor = value;
                this.PopupToggleButton.Content = this._selectedColor.GetName();
                this.PopupToggleButton.Foreground = this._selectedColor.GetForeColor();
                this.PopupToggleButton.Background = new SolidColorBrush(this._selectedColor);
                this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
            }
        }

        private void DefaultColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var color = this.DefaultColorList.SelectedItem.ToString().ToColor();
            this.SelectDefaultColorButton.Foreground = color.GetForeColor();
            this.SelectDefaultColorButton.Background = new SolidColorBrush(color);
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ConciergeColorButton;
            this.SelectedColor = button.Color;
            this.PopupToggleButton.IsChecked = false;
        }

        private void SelectDefaultColorButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = (this.SelectDefaultColorButton.Background as SolidColorBrush).Color;
            this.PopupToggleButton.IsChecked = false;
        }
    }
}
