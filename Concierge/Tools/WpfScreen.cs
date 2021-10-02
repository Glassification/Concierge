// <copyright file="WpfScreen.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Interop;

    public class WpfScreen
    {
        private readonly Screen screen;

        internal WpfScreen(Screen screen)
        {
            this.screen = screen;
        }

        public static WpfScreen Primary => new (Screen.PrimaryScreen);

        public bool IsPrimary => this.screen.Primary;

        public string DeviceName => this.screen.DeviceName;

        public Rect DeviceBounds => GetRect(this.screen.Bounds);

        public Rect WorkingArea => GetRect(this.screen.WorkingArea);

        public static IEnumerable<WpfScreen> AllScreens()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                yield return new WpfScreen(screen);
            }
        }

        public static WpfScreen GetScreenFrom(Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);
            var screen = Screen.FromHandle(windowInteropHelper.Handle);
            var wpfScreen = new WpfScreen(screen);

            return wpfScreen;
        }

        public static WpfScreen GetScreenFrom(System.Windows.Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            var drawingPoint = new System.Drawing.Point(x, y);
            var screen = Screen.FromPoint(drawingPoint);
            var wpfScreen = new WpfScreen(screen);

            return wpfScreen;
        }

        private static Rect GetRect(Rectangle value)
        {
            return new Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height,
            };
        }
    }
}
