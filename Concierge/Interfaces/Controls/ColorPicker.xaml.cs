// <copyright file="ColorPicker.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Configuration;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.UtilityInterface;
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
            this.LoadColorList();
            SetColorButtons(this.DefaultColorsStackPanel, AppSettingsManager.ColorPicker.DefaultColors);
            SetColorButtons(this.RecentColorsStackPanel, this.RecentColors);
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
                this.PickColorButton.Foreground = this._selectedColor.GetForeColor();
                this.PickColorButton.Background = new SolidColorBrush(this._selectedColor);
                this.PickColorButton.ToolTip = this._selectedColor.GetName();
                this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
            }
        }

        private List<Color> RecentColors { get; set; }

        private static void SetColorButtons(StackPanel stackPanel, List<Color> colors)
        {
            var buttons = DisplayUtility.FindVisualChildren<ConciergeColorButton>(stackPanel).ToList();

            for (int i = 0; i < colors.Count; i++)
            {
                buttons[i].Color = colors[i];
            }
        }

        private static void SetColorButtons(StackPanel stackPanel, List<string> colors)
        {
            var buttons = DisplayUtility.FindVisualChildren<ConciergeColorButton>(stackPanel).ToList();

            for (int i = 0; i < colors.Count; i++)
            {
                buttons[i].Color = colors[i].ToColor();
            }
        }

        private void UpdateRecentColors(int index)
        {
            var color = this.RecentColors[index];
            this.RecentColors.RemoveAt(index);
            this.RecentColors.Insert(0, color);

            SetColorButtons(this.RecentColorsStackPanel, this.RecentColors);
            AppSettingsManager.UpdateRecentColors(this.RecentColors);
        }

        private void AddRecentColor(Color color)
        {
            this.RecentColors.RemoveAt(this.RecentColors.Count - 1);
            this.RecentColors.Insert(0, color);

            SetColorButtons(this.RecentColorsStackPanel, this.RecentColors);
            AppSettingsManager.UpdateRecentColors(this.RecentColors);
        }

        private void LoadColorList()
        {
            this.RecentColors = new List<Color>();
            foreach (var color in AppSettingsManager.ColorPicker.RecentColors)
            {
                this.RecentColors.Add(color.ToColor());
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

            if (button.Index > 0)
            {
                this.UpdateRecentColors(button.Index);
            }
        }

        private void SelectDefaultColorButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = (this.SelectDefaultColorButton.Background as SolidColorBrush).Color;
            this.PopupToggleButton.IsChecked = false;
            this.AddRecentColor(this.SelectedColor);
        }

        private void PickColorButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
        }

        private void SelectCustomColorButton_Click(object sender, RoutedEventArgs e)
        {
            var result = new CustomColorWindow().ShowColorWindow(this.SelectedColor);
            this.PopupToggleButton.IsChecked = false;

            if (result != Colors.Transparent)
            {
                this.SelectedColor = result;
                this.AddRecentColor(this.SelectedColor);
                this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
            }
        }
    }
}
