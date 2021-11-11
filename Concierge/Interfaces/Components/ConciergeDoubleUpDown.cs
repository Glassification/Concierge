// <copyright file="ConciergeDoubleUpDown.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Utility;
    using Xceed.Wpf.Toolkit;

    public class ConciergeDoubleUpDown : DoubleUpDown
    {
        private bool updatingValue;

        public ConciergeDoubleUpDown()
            : base()
        {
            this.Background = ConciergeColors.ControlBackgroundBrush;
            this.BorderThickness = new Thickness(0);
            this.Foreground = Brushes.White;
            this.Margin = new Thickness(0, 0, 20, 0);
            this.Height = 40;
            this.Minimum = 0;
            this.TextAlignment = TextAlignment.Center;
            this.FontSize = 25;
            this.Watermark = "Enter Double";

            this.Spinned += this.CreateSound_Spinned;
            this.MouseMove += this.IntegerUpDown_MouseMove;
            this.MouseLeave += this.Button_MouseLeave;
        }

        public void UpdatingValue()
        {
            this.updatingValue = true;
        }

        private void CreateSound_Spinned(object sender, SpinEventArgs e)
        {
            if (!this.updatingValue)
            {
                ConciergeSound.UpdateValue();
            }

            this.updatingValue = false;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void IntegerUpDown_MouseMove(object sender, MouseEventArgs e)
        {
            if (Utilities.GetElementUnderMouse<RepeatButton>() != null)
            {
                if (Mouse.OverrideCursor != Cursors.Hand)
                {
                    Mouse.OverrideCursor = Cursors.Hand;
                }
            }
            else if (Utilities.GetElementUnderMouse<TextBox>() != null)
            {
                if (Mouse.OverrideCursor != Cursors.IBeam)
                {
                    Mouse.OverrideCursor = Cursors.IBeam;
                }
            }
            else if (Mouse.OverrideCursor != Cursors.Arrow)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
