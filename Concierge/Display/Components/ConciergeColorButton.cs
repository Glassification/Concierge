// <copyright file="ConciergeColorButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Data;
    using Concierge.Services;

    public sealed class ConciergeColorButton : Button
    {
        private CustomColor color;

        public ConciergeColorButton()
            : base()
        {
            this.Width = 40;
            this.Height = 40;
            this.SnapsToDevicePixels = true;
            this.Margin = new Thickness(5, 0, 5, 0);
            this.Index = -1;
            this.color = CustomColor.Invalid;

            this.Click += this.Button_Click;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        public CustomColor Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                this.Background = new SolidColorBrush(this.color.Color);
                this.ToolTip = this.color.Name;
            }
        }

        public int Index { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            Program.Logger.Info($"{this.Name} clicked.");
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
