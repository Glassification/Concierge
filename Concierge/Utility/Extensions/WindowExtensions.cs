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
                r = new Int32Rect(rc.left, rc.top, rc.width, rc.height);
            }
            else
            {
                WindowInteropHelper helper = new WindowInteropHelper(w);
                IntPtr hmonitor = OSInterop.MonitorFromWindow(new HandleRef((object)null, helper.EnsureHandle()), 2);
                OSInterop.MONITORINFOEX info = new OSInterop.MONITORINFOEX();
                OSInterop.GetMonitorInfo(new HandleRef((object)null, hmonitor), info);
                r = new Int32Rect(info.rcWork.left, info.rcWork.top, info.rcWork.width, info.rcWork.height);
            }

            return new Point(r.X, r.Y);
        }
    }
}
