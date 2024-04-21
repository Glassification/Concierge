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
        public ComboBoxItemControl()
        {
            this.InitializeComponent();
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

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, Enum name)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name.ToString().Strip(" ").FormatFromPascalCase();
            this.ItemName.Foreground = Brushes.White;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, Enum name, object tag)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name.ToString().Strip(" ").FormatFromPascalCase();
            this.ItemName.Foreground = Brushes.White;
            this.Tag = tag;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, string name, object tag)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name;
            this.ItemName.Foreground = Brushes.White;
            this.Tag = tag;
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
            this.MainGrid.Background = ConciergeBrushes.ControlForeBlue;
            this.MainBorder.BorderBrush = ConciergeBrushes.ControlForeBlue;
        }
    }
}
