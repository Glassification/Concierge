// <copyright file="DetailedItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using System.Windows.Media;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Represents a detailed item with a name, additional information, an icon, and an icon color.
    /// </summary>
    public class DetailedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedItem"/> class with default values.
        /// </summary>
        public DetailedItem()
            : this(string.Empty, string.Empty, PackIconKind.ListStatus, Brushes.SlateGray)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedItem"/> class with specified values.
        /// </summary>
        /// <param name="name">The name of the detailed item.</param>
        /// <param name="information">Additional information about the item.</param>
        /// <param name="icon">The icon associated with the item.</param>
        /// <param name="iconColor">The color of the icon.</param>
        public DetailedItem(string name, string information, PackIconKind icon, Brush iconColor)
        {
            this.Name = name;
            this.Information = information;
            this.Icon = icon;
            this.IconColor = iconColor;
        }

        /// <summary>
        /// Gets or sets the name of the detailed item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets additional information about the detailed item.
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Gets or sets the icon associated with the detailed item.
        /// </summary>
        public PackIconKind Icon { get; set; }

        /// <summary>
        /// Gets or sets the color of the icon.
        /// </summary>
        public Brush IconColor { get; set; }
    }
}
