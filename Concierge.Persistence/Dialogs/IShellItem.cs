// <copyright file="IShellItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.Dialogs
{
    using System;
    using System.Runtime.InteropServices;

    using Concierge.Persistence.Enums;

    [ComImport]
    [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        [PreserveSig]
        int BindToHandler(); // not fully defined

        [PreserveSig]
        int GetParent(); // not fully defined

        [PreserveSig]
        int GetDisplayName(SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

        [PreserveSig]
        int GetAttributes();  // not fully defined

        [PreserveSig]
        int Compare();  // not fully defined
    }
}
