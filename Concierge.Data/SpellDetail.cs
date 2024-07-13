// <copyright file="SpellDetail.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using System.Windows.Media;

    using Concierge.Common.Dtos;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Represents details about a spell, consisting of a header and a corresponding value.
    /// </summary>
    public sealed class SpellDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellDetail"/> class with the specified header, value, icon, and icon color.
        /// </summary>
        /// <param name="header">The header of the spell detail.</param>
        /// <param name="value">The value of the spell detail.</param>
        /// <param name="icon">The icon of the spell detail.</param>
        /// <param name="iconColor">The icon color of the spell detail.</param>
        public SpellDetail(string header, string value, PackIconKind icon, Brush iconColor)
        {
            this.Header = header;
            this.Value = value;
            this.Icon = icon;
            this.IconColor = iconColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellDetail"/> class with the specified header, value, and category.
        /// </summary>
        /// <param name="header">The header text of the spell detail.</param>
        /// <param name="value">The value or description of the spell detail.</param>
        /// <param name="category">The category to which the spell detail belongs, providing the icon and icon color.</param>
        public SpellDetail(string header, string value, CategoryDto category)
        {
            this.Header = header;
            this.Value = value;
            this.Icon = category.IconKind;
            this.IconColor = category.Brush;
        }

        private SpellDetail()
            : this(string.Empty, string.Empty, PackIconKind.Error, Brushes.Red)
        {
        }

        /// <summary>
        /// Gets an empty <see cref="SpellDetail"/> instance.
        /// </summary>
        public static SpellDetail Empty => new ();

        /// <summary>
        /// Gets the header of the spell detail.
        /// </summary>
        public string Header { get; private set; }

        /// <summary>
        /// Gets the icon of the spell detail.
        /// </summary>
        public PackIconKind Icon { get; private set; }

        /// <summary>
        /// Gets the icon color of the spell detail.
        /// </summary>
        public Brush IconColor { get; private set; }

        /// <summary>
        /// Gets the value of the spell detail.
        /// </summary>
        public string Value { get; private set; }

        public override string ToString()
        {
            return $"{this.Header} {this.Value}";
        }
    }
}
