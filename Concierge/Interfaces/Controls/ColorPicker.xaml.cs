// <copyright file="ColorPicker.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Configuration;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.UtilityInterface;
    using Concierge.Primitives;
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

        private CustomColor? _selectedColor;

        public ColorPicker()
        {
            this.InitializeComponent();
            this.SelectedColor = CustomColor.White;
            this.InitializeColorList();
            this.DefaultColorList.SelectedIndex = 0;
            this.SelectedColor = CustomColor.White;
            this.RecentColors = new List<CustomColor>();
            foreach (var color in AppSettingsManager.ColorPicker.RecentColors)
            {
                this.RecentColors.Add(color);
            }

            SetColorButtons(this.DefaultColorsStackPanel, AppSettingsManager.ColorPicker.DefaultColors);
            SetColorButtons(this.RecentColorsStackPanel, this.RecentColors);
        }

        public event RoutedEventHandler ColorChanged
        {
            add { this.AddHandler(ColorChangedEvent, value); }
            remove { this.RemoveHandler(ColorChangedEvent, value); }
        }

        public CustomColor SelectedColor
        {
            get
            {
                return this._selectedColor ?? CustomColor.Invalid;
            }

            set
            {
                this._selectedColor = value;
                this.PickColorButton.Foreground = this._selectedColor.Color.GetForeColor();
                this.PickColorButton.Background = new SolidColorBrush(this._selectedColor.Color);
                this.PickColorButton.ToolTip = this._selectedColor.Name;
                this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
            }
        }

        private List<CustomColor> RecentColors { get; set; }

        private static void SetColorButtons(StackPanel stackPanel, List<CustomColor> colors)
        {
            var buttons = DisplayUtility.FindVisualChildren<ConciergeColorButton>(stackPanel).ToList();

            for (int i = 0; i < colors.Count; i++)
            {
                buttons[i].Color = colors[i];
            }
        }

        private void InitializeColorList()
        {
            foreach (var color in ColorUtility.ListDotNetColors())
            {
                this.DefaultColorList.Items.Add(new ComboBoxItem()
                {
                    Content = color,
                    Foreground = color.ToBrush(),
                });
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

        private void AddRecentColor(CustomColor color)
        {
            this.RecentColors.RemoveAt(this.RecentColors.Count - 1);
            this.RecentColors.Insert(0, color);

            SetColorButtons(this.RecentColorsStackPanel, this.RecentColors);
            AppSettingsManager.UpdateRecentColors(this.RecentColors);
        }

        private void DefaultColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DefaultColorList.SelectedItem is not ComboBoxItem item)
            {
                return;
            }

            var color = item.Content.ToString().ToColor();
            this.SelectDefaultColorButton.Color = new CustomColor(item.Content.ToString() ?? string.Empty, color.R, color.G, color.B);
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeColorButton button)
            {
                return;
            }

            this.SelectedColor = button.Color;
            this.PopupToggleButton.IsChecked = false;

            if (button.Index > 0)
            {
                this.UpdateRecentColors(button.Index);
            }
        }

        private void SelectDefaultColorButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = this.SelectDefaultColorButton.Color;
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

            if (result.IsValid)
            {
                this.SelectedColor = result;
                this.AddRecentColor(this.SelectedColor);
                this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
            }
        }
    }
}
