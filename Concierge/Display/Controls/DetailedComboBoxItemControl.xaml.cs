// <copyright file="DetailedComboBoxItemControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for DetailedComboBoxItemControl.xaml.
    /// </summary>
    public partial class DetailedComboBoxItemControl : UserControl
    {
        public DetailedComboBoxItemControl()
        {
            this.InitializeComponent();
        }

        public DetailedComboBoxItemControl(IUnique item)
            : this()
        {
            var category = item.GetCategory();

            this.ItemIcon.Kind = category.IconKind;
            this.ItemIcon.Foreground = category.Brush;

            this.ItemName.Text = item.Name;
            this.ItemName.Foreground = item.IsCustom ? Brushes.PowderBlue : Brushes.White;

            this.ItemInfo.Text = item.Information;
            this.ItemInfo.Foreground = Brushes.White;

            this.Item = item;
        }

        public DetailedComboBoxItemControl(PackIconKind icon, Brush iconColor, string name, string info)
            : this()
        {
            this.ItemIcon.Kind = icon;
            this.ItemIcon.Foreground = iconColor;

            this.ItemName.Text = name;
            this.ItemName.Foreground = Brushes.White;

            this.ItemInfo.Text = info;
            this.ItemInfo.Foreground = Brushes.White;
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
