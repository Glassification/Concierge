// <copyright file="CustomColor.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a custom color with RGBA values and hexadecimal representation.
    /// Implements <see cref="ICopyable{CustomColor}"/> and <see cref="IUnique"/>.
    /// </summary>
    public sealed class CustomColor : ICopyable<CustomColor>, IUnique
    {
        private const byte MaxColor = 255;
        private const byte MinColor = 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.", Justification = "We don't do that in Canada.")]
        private static readonly Regex formatHex = new (@"(.{2})", RegexOptions.Compiled);
        private string? name;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColor"/> class with a color name.
        /// </summary>
        /// <param name="name">The name of the color.</param>
        /// <param name="isHex">A value indicating whether the provided name is a hexadecimal color representation.</param>
        public CustomColor(string name, bool isHex = false)
        {
            var color = (Color)ColorConverter.ConvertFromString(name);
            this.IsValid = true;
            this.Hex = RgbToHex([color.R, color.G, color.B]);
            this.A = MaxColor;
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
            this.Name = isHex ? this.Color.GetName() : name.FormatColorName();
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColor"/> class with a color name and hexadecimal representation.
        /// </summary>
        /// <param name="name">The name of the color.</param>
        /// <param name="hex">The hexadecimal representation of the color.</param>
        public CustomColor(string name, string hex)
        {
            var rgb = HexToRgb(hex);
            var alphaOffset = rgb.Length - 3;

            this.A = rgb.Length == 4 ? rgb[0] : MaxColor;
            this.Name = name;
            this.IsValid = true;
            this.Hex = hex;
            this.R = rgb[0 + alphaOffset];
            this.G = rgb[1 + alphaOffset];
            this.B = rgb[2 + alphaOffset];
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColor"/> class with RGB values.
        /// </summary>
        /// <param name="name">The name of the color.</param>
        /// <param name="r">The red component of the color.</param>
        /// <param name="g">The green component of the color.</param>
        /// <param name="b">The blue component of the color.</param>
        /// <param name="a">The alpha component of the color. (Optional)</param>
        public CustomColor(string name, byte r, byte g, byte b, byte a = MaxColor)
        {
            this.Name = name;
            this.IsValid = true;
            this.Hex = RgbToHex([r, g, b]);
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColor"/> class with a color name and <see cref="Color"/> object.
        /// </summary>
        /// <param name="name">The name of the color.</param>
        /// <param name="color">The <see cref="Color"/> object representing the color.</param>
        public CustomColor(string name, Color color)
            : this(name, color.R, color.G, color.B)
        {
        }

        private CustomColor()
            : this("Color Uninitialized", "#FF0000")
        {
        }

        private CustomColor(bool isValid)
            : this("Not Set", "#FF0000")
        {
            this.IsValid = isValid;
            this.A = 85;
        }

        /// <summary>
        /// Gets an invalid color instance.
        /// </summary>
        [JsonIgnore]
        public static CustomColor Invalid => new (false);

        /// <summary>
        /// Gets a white color instance.
        /// </summary>
        [JsonIgnore]
        public static CustomColor White => new ("White", MaxColor, MaxColor, MaxColor);

        /// <summary>
        /// Gets a black color instance.
        /// </summary>
        [JsonIgnore]
        public static CustomColor Black => new ("Black", MinColor, MinColor, MinColor);

        /// <summary>
        /// Gets the <see cref="Color"/> object representation of the custom color.
        /// </summary>
        [JsonIgnore]
        public Color Color => Color.FromArgb(this.A, this.R, this.G, this.B);

        /// <summary>
        /// Gets the <see cref="Brush"/> object representation of the custom color.
        /// </summary>
        [JsonIgnore]
        public Brush Brush => new SolidColorBrush(this.Color);

        /// <summary>
        /// Gets or sets the alpha component of the color.
        /// </summary>
        public byte A { get; set; }

        /// <summary>
        /// Gets or sets the red component of the color.
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Gets or sets the green component of the color.
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Gets or sets the blue component of the color.
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Gets or sets the hexadecimal representation of the color.
        /// </summary>
        public string Hex { get; set; }

        /// <summary>
        /// Gets a value indicating whether the color is valid.
        /// </summary>
        public bool IsValid { get; init; }

        public bool IsCustom { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(CustomColor);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => new SolidColorBrush(this.Color);

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.Palette;

        /// <summary>
        /// Gets or sets the name of the color.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name.IsNullOrWhiteSpace() ? this.Color.GetName() : (this.name ?? this.Color.GetName());
            }

            set
            {
                this.name = value;
            }
        }

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"{this.Hex} [{this.R},{this.G},{this.B}]";

        /// <summary>
        /// Gets or sets the unique identifier of the color.
        /// </summary>
        public Guid Id { get; set; }

        public static bool operator ==(CustomColor left, CustomColor right)
        {
            return left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
        }

        public static bool operator !=(CustomColor left, CustomColor right)
        {
            return left.A != right.A || left.R != right.R || left.G != right.G || left.B != right.B;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CustomColor color)
            {
                return color == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Creates a new <see cref="CustomColor"/> that is saturated by the specified percentage.
        /// </summary>
        /// <param name="percent">The percentage to saturate the color by.</param>
        /// <returns>A new <see cref="CustomColor"/> that is saturated by the specified percentage.</returns>
        public CustomColor Saturate(double percent)
        {
            return new CustomColor(this.Name, this.Color.Saturate(percent));
        }

        /// <summary>
        /// Creates a new <see cref="CustomColor"/> that is desaturated by the specified percentage.
        /// </summary>
        /// <param name="percent">The percentage to desaturate the color by.</param>
        /// <returns>A new <see cref="CustomColor"/> that is desaturated by the specified percentage.</returns>
        public CustomColor Desaturate(double percent)
        {
            return new CustomColor(this.Name, this.Color.Desaturate(percent));
        }

        /// <summary>
        /// Creates a deep copy of the current <see cref="CustomColor"/>.
        /// </summary>
        /// <returns>A new <see cref="CustomColor"/> object that is a deep copy of the current instance.</returns>
        public CustomColor DeepCopy()
        {
            return new CustomColor()
            {
                A = this.A,
                R = this.R,
                G = this.G,
                B = this.B,
                Hex = this.Hex,
                Name = this.Name,
                IsValid = this.IsValid,
                Id = this.Id,
                IsCustom = this.IsCustom,
            };
        }

        /// <summary>
        /// Copies the properties of another <see cref="CustomColor"/> object into the current instance.
        /// </summary>
        /// <param name="color">The <see cref="CustomColor"/> object from which to copy the properties.</param>
        public void ShallowCopy(CustomColor color)
        {
            this.A = color.A;
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
            this.Hex = color.Hex;
            this.Name = color.Name;
            this.IsCustom = color.IsCustom;
        }

        /// <summary>
        /// Gets the category of the color.
        /// </summary>
        /// <returns>The category of the color.</returns>
        public CategoryDto GetCategory()
        {
            return new CategoryDto(PackIconKind.Palette, this.Brush, this.Name);
        }

        private static byte[] HexToRgb(string hex)
        {
            var cleanHex = formatHex.Replace(hex.Strip("#"), "$1 ").Trim();
            var result = cleanHex
                .Split(' ')
                .Select(item => Convert.ToByte(item, 16))
                .ToArray();

            return result;
        }

        private static string RgbToHex(byte[] rgb)
        {
            return BitConverter.ToString(rgb).Replace("-", string.Empty).Insert(0, "#");
        }
    }
}
