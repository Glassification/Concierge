// <copyright file="ConciergeComboBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Services;

    public sealed class ConciergeComboBox : ComboBox
    {
        public ConciergeComboBox()
            : base()
        {
            this.Height = 40;
            this.IsEditable = true;
            this.FontSize = 15;
            this.Margin = new Thickness(0, 0, 20, 0);

            this.DropDownOpened += this.ComboBox_DropDownOpened;
            this.MouseEnter += this.ComboBox_MouseEnter;
            this.MouseLeave += this.ComboBox_MouseLeave;
        }

        public List<T> ToList<T>()
        {
            var items = new List<T>();
            foreach (var item in this.Items)
            {
                if (item is ComboBoxItem comboBoxItem && comboBoxItem.Tag is T genericItem)
                {
                    items.Add(genericItem);
                }
            }

            return items;
        }

        private void ComboBox_DropDownOpened(object? sender, EventArgs e)
        {
            ConciergeSoundService.UpdateValue();
        }

        private void ComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ComboBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
