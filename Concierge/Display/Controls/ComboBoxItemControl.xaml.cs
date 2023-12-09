// <copyright file="ComboBoxItemControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows.Controls;
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

            this.ItemName.Text = name.Strip(" ").FormatFromEnum();
            this.ItemName.Foreground = Brushes.White;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, string name, object tag)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name.Strip(" ").FormatFromEnum();
            this.ItemName.Foreground = Brushes.White;
            this.Tag = tag;
        }

        public ComboBoxItemControl(PackIconKind icon, Brush iconColor, string name, Brush nameColor)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name.Strip(" ").FormatFromEnum();
            this.ItemName.Foreground = nameColor;
        }

        public IUnique? Item { get; private set; }

        public string Text => this.ItemName.Text;

        public override string ToString()
        {
            return this.ItemName.Text;
        }

        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.MainGrid.Background = ConciergeBrushes.Highlight;
            this.MainBorder.BorderBrush = ConciergeBrushes.BorderHighlight;
        }

        private void Grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.MainGrid.Background = ConciergeBrushes.TotalLightBox;
            this.MainBorder.BorderBrush = ConciergeBrushes.TotalLightBox;
        }
    }
}
