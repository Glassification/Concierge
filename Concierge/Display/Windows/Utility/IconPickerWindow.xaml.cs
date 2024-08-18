// <copyright file="IconPickerWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common.Extensions;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for IconPickerWindow.xaml.
    /// </summary>
    public partial class IconPickerWindow : ConciergeWindow
    {
        private readonly int maxIcons;
        private readonly List<ConciergeDesignButton> iconList = [];
        private readonly ResourceDictionary resourceDictionary = new ()
        {
            Source = new Uri("Display/Dictionaries/ButtonDictionary.xaml", UriKind.RelativeOrAbsolute),
        };

        private CustomColor customColor = CustomColor.Invalid;

        public IconPickerWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.maxIcons = Enum.GetValues(typeof(PackIconKind))
                .Cast<PackIconKind>()
                .DistinctBy(x => x.ToString())
                .Count();
        }

        public override string HeaderText => "Icon Picker";

        public override string WindowName => nameof(IconPickerWindow);

        public override CustomIcon ShowIconWindow(CustomIcon icon)
        {
            this.SelectedIcon.Kind = icon.Kind;
            this.SelectedIcon.Foreground = icon.Color.Brush;
            this.IconLabel.Text = icon.Name;
            this.ColorPicker.SelectedColor = icon.Color;
            this.AmountLabel.Text = $"0 / {this.maxIcons}";

            this.ShowConciergeWindow();

            return this.Result == ConciergeResult.OK ? new CustomIcon(this.customColor, this.SelectedIcon.Kind) : icon;
        }

        private void FillFields()
        {
            var iconWrapPanel = this.BuildIconList(this.FilterTextBox.Text);
            this.IconScrollViewer.Content = iconWrapPanel;
            this.LoadButton.Content = "Refresh";
            this.AmountLabel.Text = $"{iconWrapPanel.Children.Count} / {this.maxIcons}";
        }

        private WrapPanel BuildIconList(string filter)
        {
            var icons = Enum.GetValues(typeof(PackIconKind))
                .Cast<PackIconKind>()
                .Where(x => filter.IsNullOrWhiteSpace() || x.ToString().ContainsIgnoreCase(filter))
                .DistinctBy(x => x.ToString());

            var iconWrapPanel = new WrapPanel();
            foreach (var icon in icons)
            {
                var button = new ConciergeDesignButton()
                {
                    Style = this.resourceDictionary["ConciergeDesignButtonStyle"] as Style,
                    Foreground = Brushes.White,
                    Width = 30,
                    Height = 30,
                    Content = new PackIcon()
                    {
                        Width = 25,
                        Height = 25,
                        Kind = icon,
                    },
                    Tag = icon,
                    ToolTip = icon.ToString(),
                };

                button.Click += this.IconButton_Click;
                iconWrapPanel.Children.Add(button);
            }

            return iconWrapPanel;
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ConciergeDesignButton button && button.Tag is PackIconKind iconKind)
            {
                this.SelectedIcon.Kind = iconKind;
                this.IconLabel.Text = iconKind.PascalCase();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
            this.Result = ConciergeResult.Cancel;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            this.FillFields();
        }

        private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            this.customColor = this.ColorPicker.SelectedColor;
            this.SelectedIcon.Foreground = this.ColorPicker.SelectedColor.Brush;
        }

        private void FilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.FillFields();
                    break;
            }
        }
    }
}
