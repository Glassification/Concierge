// <copyright file="ColorPickerControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for ColorPickerControl.xaml.
    /// </summary>
    public partial class ColorPickerControl : UserControl
    {
        private static readonly RoutedEvent ColorChangedEvent =
            EventManager.RegisterRoutedEvent(
            "ColorChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ColorPickerControl));

        private CustomColor? selectedColor;

        public ColorPickerControl()
        {
            this.InitializeComponent();

            this.CustomColorService = Program.CustomColorService;
            this.DefaultColorList.SelectedIndex = 0;
            this.SelectedColor = CustomColor.White;

            InitializeColorList(this.CustomColorService.DotNetColors, this.DefaultColorList);
            InitializeColorList(this.CustomColorService.CustomColors, this.CustomColorList);
            SetColorButtons(this.DefaultColorsStackPanel, this.CustomColorService.DefaultColors);
            SetColorButtons(this.RecentColorsStackPanel, this.CustomColorService.RecentColors);
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
                return this.selectedColor ?? CustomColor.Invalid;
            }

            set
            {
                this.selectedColor = value;
                this.PickColorButton.Foreground = this.selectedColor.Color.GetForeColor();
                this.PickColorButton.Background = new SolidColorBrush(this.selectedColor.Color);
                this.PickColorButton.ToolTip = this.selectedColor.Name;
                this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
            }
        }

        private CustomColorService CustomColorService { get; set; }

        private static void SetColorButtons(StackPanel stackPanel, List<CustomColor> colors)
        {
            var buttons = DisplayUtility.FindVisualChildren<ConciergeColorButton>(stackPanel).ToList();

            for (int i = 0; i < colors.Count; i++)
            {
                buttons[i].Color = colors[i];
            }
        }

        private static void InitializeColorList(List<CustomColor> colors, ConciergeComboBox comboBox)
        {
            colors.ForEach(x => comboBox.Items.Add(new ComboBoxItemControl(x)));
            comboBox.SelectedIndex = 0;
        }

        private void ColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItemControl item && item.Item is CustomColor customColor)
            {
                this.SelectDefaultColorButton.Color = customColor;
            }
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
                this.CustomColorService.UpdateRecentColors(button.Index);
                SetColorButtons(this.RecentColorsStackPanel, this.CustomColorService.RecentColors);
            }
        }

        private void SelectDefaultColorButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = this.SelectDefaultColorButton.Color;
            this.PopupToggleButton.IsChecked = false;
            this.CustomColorService.AddRecentColor(this.SelectedColor);
            SetColorButtons(this.RecentColorsStackPanel, this.CustomColorService.RecentColors);
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
                this.CustomColorService.AddRecentColor(this.SelectedColor);
                this.CustomColorService.AddCustomColor(this.SelectedColor);

                this.CustomColorList.Items.Insert(0, new ComboBoxItem()
                {
                    Content = result.Name,
                    Foreground = new SolidColorBrush(result.Color),
                    Tag = result,
                });

                SetColorButtons(this.RecentColorsStackPanel, this.CustomColorService.RecentColors);
                this.RaiseEvent(new RoutedEventArgs(ColorChangedEvent));
            }
        }

        private void PopupToggleButton_Click(object sender, RoutedEventArgs e)
        {
            SetColorButtons(this.RecentColorsStackPanel, this.CustomColorService.RecentColors);
            this.CustomColorList.Items.Clear();
            InitializeColorList(this.CustomColorService.CustomColors, this.CustomColorList);
        }
    }
}
