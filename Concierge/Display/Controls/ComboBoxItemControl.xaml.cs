// <copyright file="ComboBoxItemControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for ComboBoxItemControl.xaml.
    /// </summary>
    public partial class ComboBoxItemControl : UserControl
    {
        private Brush originalBackground;

        public ComboBoxItemControl()
        {
            this.InitializeComponent();

            this.MainBorder.BorderBrush = ConciergeBrushes.ControlForeBlue;
            this.MainGrid.Background = ConciergeBrushes.ControlForeBlue;
            this.originalBackground = ConciergeBrushes.ControlForeBlue;
        }

        public ComboBoxItemControl(IUnique item)
            : this()
        {
            var category = item.GetCategory();

            this.ItemIcon.Kind = category.IconKind;
            this.ItemIcon.Foreground = category.Brush;

            this.ItemName.Text = item.Name;
            this.ItemName.Foreground = item.IsCustom ? Brushes.PowderBlue : Brushes.White;

            this.Item = item;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, string name)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name;
            this.ItemName.Foreground = Brushes.White;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, string name, Brush background)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name;
            this.ItemName.Foreground = Brushes.White;

            this.MainBorder.BorderBrush = background;
            this.MainGrid.Background = background;
            this.originalBackground = background;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, Enum name)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name.ToString().Strip(" ").PascalCase();
            this.ItemName.Foreground = Brushes.White;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, Enum name, Brush background)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name.ToString().Strip(" ").PascalCase();
            this.ItemName.Foreground = Brushes.White;

            this.MainBorder.BorderBrush = background;
            this.MainGrid.Background = background;
            this.originalBackground = background;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, Enum name, object tag)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name.ToString().Strip(" ").PascalCase();
            this.ItemName.Foreground = Brushes.White;
            this.Tag = tag;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, string name, object tag, Brush background)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name;
            this.ItemName.Foreground = Brushes.White;
            this.Tag = tag;

            this.MainBorder.BorderBrush = background;
            this.MainGrid.Background = background;
            this.originalBackground = background;
        }

        public IUnique? Item { get; private set; }

        public string Text => this.ItemName.Text;

        public override string ToString()
        {
            return this.ItemName.Text;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MainGrid.Background = ConciergeBrushes.Highlight;
            this.MainBorder.BorderBrush = ConciergeBrushes.BorderHighlight;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.MainGrid.Background = this.originalBackground;
            this.MainBorder.BorderBrush = this.originalBackground;
        }
    }
}
