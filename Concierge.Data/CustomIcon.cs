// <copyright file="CustomIcon.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a custom icon with a color and a kind.
    /// </summary>
    public sealed class CustomIcon : ICopyable<CustomIcon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomIcon"/> class with the specified color and kind.
        /// </summary>
        /// <param name="color">The color of the icon.</param>
        /// <param name="kind">The kind or type of the icon.</param>
        public CustomIcon(CustomColor color, PackIconKind kind)
        {
            this.Color = color;
            this.Kind = kind;
        }

        /// <summary>
        /// Gets an empty custom icon with a default color of <see cref="CustomColor.White"/> and a default kind of <see cref="PackIconKind.Abacus"/>.
        /// </summary>
        public static CustomIcon Empty => new (CustomColor.White, PackIconKind.Abacus);

        /// <summary>
        /// Gets or sets the color of the icon.
        /// </summary>
        public CustomColor Color { get; set; }

        /// <summary>
        /// Gets or sets the kind or type of the icon.
        /// </summary>
        public PackIconKind Kind { get; set; }

        /// <summary>
        /// Gets the name of the icon derived from its kind, formatted from PascalCase.
        /// </summary>
        /// <remarks>
        /// For example, if the kind of the icon is "SomeIcon", the name returned will be "Some Icon".
        /// </remarks>
        [JsonIgnore]
        public string Name => this.Kind.ToString().FormatFromPascalCase();

        public static bool operator ==(CustomIcon left, CustomIcon right)
        {
            return left.Kind == right.Kind && left.Color == right.Color;
        }

        public static bool operator !=(CustomIcon left, CustomIcon right)
        {
            return left.Kind != right.Kind || left.Color != right.Color;
        }

        public override string ToString()
        {
            return $"{this.Color} {this.Kind}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is CustomIcon icon)
            {
                return icon == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Creates a deep copy of the current <see cref="CustomIcon"/>.
        /// </summary>
        /// <returns>A new <see cref="CustomIcon"/> object that is a deep copy of the current instance.</returns>
        public CustomIcon DeepCopy()
        {
            return new CustomIcon(
                this.Color.DeepCopy(),
                this.Kind);
        }
    }
}
