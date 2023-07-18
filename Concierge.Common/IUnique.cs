// <copyright file="IUnique.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;
    using System.Windows.Media;

    using Concierge.Common.Dtos;
    using MaterialDesignThemes.Wpf;

    public interface IUnique
    {
        string CustomType { get; }

        Brush CustomTypeColor { get; }

        PackIconKind CustomTypeIcon { get; }

        Guid Id { get; set; }

        bool IsCustom { get; set; }

        string Name { get; set; }

        CategoryDto GetCategory();
    }
}
