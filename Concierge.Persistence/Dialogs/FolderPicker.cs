// <copyright file="FolderPicker.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Windows;
    using System.Windows.Interop;

    using Concierge.Persistence.Enums;

    public class FolderPicker
    {
        private const int ErrorCanceled = unchecked((int)0x800704C7);

        private readonly List<string> resultPaths = new ();
        private readonly List<string> resultNames = new ();

        public FolderPicker()
        {
            this.InputPath = string.Empty;
            this.Title = string.Empty;
            this.OkButtonLabel = "Select Folder";
            this.FileNameLabel = string.Empty;
        }

        public IReadOnlyList<string> ResultPaths => this.resultPaths;

        public IReadOnlyList<string> ResultNames => this.resultNames;

        public string ResultPath => this.ResultPaths.ToList().FirstOrDefault() ?? string.Empty;

        public string ResultName => this.ResultNames.ToList().FirstOrDefault() ?? string.Empty;

        public virtual string InputPath { get; set; }

        public virtual bool ForceFileSystem { get; set; }

        public virtual bool MultiSelect { get; set; }

        public virtual string Title { get; set; }

        public virtual string OkButtonLabel { get; set; }

        public virtual string FileNameLabel { get; set; }

        // for WPF support
        public bool? ShowDialog(Window? owner = null, bool throwOnError = false)
        {
            owner ??= Application.Current?.MainWindow;
            return this.ShowDialog(owner != null ? new WindowInteropHelper(owner).Handle : IntPtr.Zero, throwOnError);
        }

        // for all .NET
        public virtual bool? ShowDialog(IntPtr owner, bool throwOnError = false)
        {
            var dialog = (IFileOpenDialog)new FileOpenDialog();
            if (!string.IsNullOrEmpty(this.InputPath))
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                if (CheckHr(SHCreateItemFromParsingName(this.InputPath, null, typeof(IShellItem).GUID, out var item), throwOnError) != 0)
                {
                    return null;
                }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                dialog.SetFolder(item);
            }

            var options = FOS.FOS_PICKFOLDERS;
            options = (FOS)this.SetOptions((int)options);
            dialog.SetOptions(options);

            if (this.Title != null)
            {
                dialog.SetTitle(this.Title);
            }

            if (this.OkButtonLabel != null)
            {
                dialog.SetOkButtonLabel(this.OkButtonLabel);
            }

            if (this.FileNameLabel != null)
            {
                dialog.SetFileName(this.FileNameLabel);
            }

            if (owner == IntPtr.Zero)
            {
                owner = Process.GetCurrentProcess().MainWindowHandle;
                if (owner == IntPtr.Zero)
                {
                    owner = GetDesktopWindow();
                }
            }

            var hr = dialog.Show(owner);
            if (hr == ErrorCanceled)
            {
                return null;
            }

            if (CheckHr(hr, throwOnError) != 0)
            {
                return null;
            }

            if (CheckHr(dialog.GetResults(out var items), throwOnError) != 0)
            {
                return null;
            }

            items.GetCount(out var count);
            for (var i = 0; i < count; i++)
            {
                items.GetItemAt(i, out var item);
                CheckHr(item.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEPARSING, out var path), throwOnError);
                CheckHr(item.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEEDITING, out var name), throwOnError);
                if (path is not null && name is not null)
                {
                    this.resultPaths.Add(path);
                    this.resultNames.Add(name);
                }
            }

            return true;
        }

        protected virtual int SetOptions(int options)
        {
            if (this.ForceFileSystem)
            {
                options |= (int)FOS.FOS_FORCEFILESYSTEM;
            }

            if (this.MultiSelect)
            {
                options |= (int)FOS.FOS_ALLOWMULTISELECT;
            }

            return options;
        }

        private static int CheckHr(int hr, bool throwOnError)
        {
            if (hr != 0 && throwOnError)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            return hr;
        }

        [DllImport("shell32")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "Not supported.")]
        private static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IShellItem ppv);

        [DllImport("user32")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time", Justification = "Not supported.")]
        private static extern IntPtr GetDesktopWindow();
    }
}
