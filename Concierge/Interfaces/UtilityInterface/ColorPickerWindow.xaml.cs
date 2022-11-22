// <copyright file="ColorPickerWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Interfaces.Components;
    using Concierge.Primitives;
    using Concierge.Services;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for ColorPickerWindow.xaml.
    /// </summary>
    public partial class ColorPickerWindow : ConciergeWindow
    {
        public ColorPickerWindow()
        {
            this.InitializeComponent();

            this.CustomColorService = Program.CustomColorService;
            this.SelectedColor = CustomColor.Invalid;

            InitializeColorList(this.CustomColorService.DotNetColors, this.DefaultColorList);
            InitializeColorList(this.CustomColorService.GetLimitedCustomColors(), this.CustomColorList);
            SetColorButtons(this.DefaultColorsStackPanel, this.CustomColorService.DefaultColors);
            SetColorButtons(this.RecentColorsStackPanel, this.CustomColorService.RecentColors);
        }

        public override string HeaderText => "Colour Picker";

        private CustomColor SelectedColor { get; set; }

        private CustomColorService CustomColorService { get; set; }

        public override CustomColor ShowColorWindow(CustomColor color)
        {
            this.SelectedColor = color;
            this.ShowConciergeWindow();

            return this.SelectedColor.DeepCopy();
        }

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
            foreach (var customColor in colors)
            {
                comboBox.Items.Add(new ComboBoxItem()
                {
                    Content = customColor.Name,
                    Foreground = new SolidColorBrush(customColor.Color),
                    Tag = customColor,
                });
            }

            comboBox.SelectedIndex = 0;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeColorButton button)
            {
                return;
            }

            if (button.Index > 0)
            {
                this.SelectedColor = button.Color;
                this.CustomColorService.UpdateRecentColors(button.Index);
                this.CloseConciergeWindow();
            }
        }

        private void ColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }

            if (e.AddedItems[0] is not ComboBoxItem item)
            {
                return;
            }

            if (item.Tag is not CustomColor customColor)
            {
                return;
            }

            this.SelectDefaultColorButton.Color = customColor;
        }

        private void SelectDefaultColorButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = this.SelectDefaultColorButton.Color;
            this.CustomColorService.AddRecentColor(this.SelectedColor);
            this.CloseConciergeWindow();
        }

        private void SelectCustomColorButton_Click(object sender, RoutedEventArgs e)
        {
            var result = new CustomColorWindow().ShowColorWindow(this.SelectedColor);

            if (result.IsValid)
            {
                this.SelectedColor = result;
                this.CustomColorService.AddRecentColor(this.SelectedColor);
                this.CustomColorService.AddCustomColor(this.SelectedColor);
                this.CloseConciergeWindow();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedColor = CustomColor.Invalid;
            this.CloseConciergeWindow();
        }
    }
}
