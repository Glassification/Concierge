// <copyright file="ColorPickerWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for ColorPickerWindow.xaml.
    /// </summary>
    public partial class ColorPickerWindow : ConciergeWindow
    {
        private readonly CustomColorService customColorService = Program.CustomColorService;

        private CustomColor selectedColor = CustomColor.Invalid;

        public ColorPickerWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            InitializeColorList(this.customColorService.DotNetColors, this.DefaultColorList);
            InitializeColorList(this.customColorService.CustomColors, this.CustomColorList);
            SetColorButtons(this.DefaultColorsStackPanel, this.customColorService.DefaultColors);
            SetColorButtons(this.RecentColorsStackPanel, this.customColorService.RecentColors);
        }

        public override string HeaderText => "Colour Picker";

        public override string WindowName => nameof(ColorPickerWindow);

        public override CustomColor ShowColorWindow(CustomColor color)
        {
            this.selectedColor = color;
            this.ShowConciergeWindow();

            return this.selectedColor.DeepCopy();
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
                comboBox.Items.Add(new ComboBoxItemControl(customColor));
            }

            comboBox.SelectedIndex = 0;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeColorButton button)
            {
                return;
            }

            if (button.Index >= 0)
            {
                this.selectedColor = button.Color;
                this.customColorService.UpdateRecentColors(button.Index);
                this.CloseConciergeWindow();
            }
            else
            {
                this.selectedColor = button.Color;
                this.CloseConciergeWindow();
            }
        }

        private void ColorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }

            if (e.AddedItems[0] is not ComboBoxItemControl item)
            {
                return;
            }

            if (item.Item is not CustomColor customColor)
            {
                return;
            }

            this.SelectDefaultColorButton.Color = customColor;
        }

        private void SelectDefaultColorButton_Click(object sender, RoutedEventArgs e)
        {
            this.selectedColor = this.SelectDefaultColorButton.Color;
            this.customColorService.AddRecentColor(this.selectedColor);
            this.CloseConciergeWindow();
        }

        private void SelectCustomColorButton_Click(object sender, RoutedEventArgs e)
        {
            var result = WindowService.ShowColorWindow(typeof(CustomColorWindow), this.selectedColor);

            if (result.IsValid)
            {
                this.selectedColor = result;
                this.customColorService.AddRecentColor(this.selectedColor);
                this.customColorService.AddCustomColor(this.selectedColor);
                this.CloseConciergeWindow();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.selectedColor = CustomColor.Invalid;
            this.CloseConciergeWindow();
        }
    }
}
