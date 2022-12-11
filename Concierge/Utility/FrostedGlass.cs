// <copyright file="FrostedGlass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

    using Concierge.Utility.Structs;

    public partial class FrostedGlass
    {
        public FrostedGlass()
        {
        }

        [LibraryImport("dwmapi.dll")]
        public static partial int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1401:P/Invokes should not be visible", Justification = "Not supported yet.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "Not supported yet.")]
        public static extern bool DwmIsCompositionEnabled();

        public static bool ExtendGlassFrame(Window window, Thickness margin)
        {
            if (!DwmIsCompositionEnabled())
            {
                return false;
            }

            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
            {
                throw new InvalidOperationException("The Window must be shown before extending glass.");
            }

            window.Background = Brushes.Transparent;
            HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

            MARGINS margins = new (margin);
            DwmExtendFrameIntoClientArea(hwnd, ref margins);

            return true;
        }
    }
}
