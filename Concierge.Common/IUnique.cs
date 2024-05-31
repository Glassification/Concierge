// <copyright file="IUnique.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;
    using System.Windows.Media;

    using Concierge.Common.Dtos;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Represents a unique item with identifiable characteristics.
    /// </summary>
    public interface IUnique
    {
        /// <summary>
        /// Gets the custom type associated with the unique item.
        /// </summary>
        string CustomType { get; }

        /// <summary>
        /// Gets the color of the custom type icon associated with the unique item.
        /// </summary>
        Brush CustomTypeColor { get; }

        /// <summary>
        /// Gets the custom type icon associated with the unique item.
        /// </summary>
        PackIconKind CustomTypeIcon { get; }

        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is custom.
        /// </summary>
        bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets the name of the unique item.
        /// </summary>
        string Name { get; set; }

        string Information { get; }

        CategoryDto GetCategory();
    }
}
