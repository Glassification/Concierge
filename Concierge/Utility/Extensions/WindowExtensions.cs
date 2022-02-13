// <copyright file="WindowExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public static class WindowExtensions
    {
        public static Point GetAbsolutePosition(this Window w)
        {
            if (w.WindowState != WindowState.Maximized)
            {
                return new Point(w.Left, w.Top);
            }

            Int32Rect r;
            bool multimonSupported = OSInterop.GetSystemMetrics(OSInterop.SM_CMONITORS) != 0;
            if (!multimonSupported)
            {
                OSInterop.RECT rc = new ();
                OSInterop.SystemParametersInfo(48, 0, ref rc, 0);
                r = new Int32Rect(rc.left, rc.top, rc.Width, rc.Height);
            }
            else
            {
                var helper = new WindowInteropHelper(w);
                IntPtr hmonitor = OSInterop.MonitorFromWindow(new HandleRef(null, helper.EnsureHandle()), 2);
                var info = new OSInterop.MONITORINFOEX();
                OSInterop.GetMonitorInfo(new HandleRef(null, hmonitor), info);
                r = new Int32Rect(info.rcWork.left, info.rcWork.top, info.rcWork.Width, info.rcWork.Height);
            }

            return new Point(r.X, r.Y);
        }
    }
}
