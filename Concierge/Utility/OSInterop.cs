// <copyright file="OSInterop.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Runtime.InteropServices;

    public static class OSInterop
    {
        public const int SM_CMONITORS = 80;

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo(int nAction, int nParam, ref RECT rc, int nUpdate);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX info);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int width
            {
                get { return this.right - this.left; }
            }

            public int height
            {
                get { return this.bottom - this.top; }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "The way she goes.")]
        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
        public class MONITORINFOEX
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            public RECT rcMonitor = default;
            public RECT rcWork = default;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szDevice = new char[32];
            public int dwFlags;
        }
    }
}
