// <copyright file="IUnique.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Windows.Media;

    using MaterialDesignThemes.Wpf;

    public interface IUnique
    {
        Guid Id { get; set; }

        string Name { get; set; }

        (PackIconKind IconKind, Brush Brush, string Name) GetCategory();
    }
}
