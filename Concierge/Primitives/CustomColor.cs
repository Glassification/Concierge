// <copyright file="CustomColor.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Primitives
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Media;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public sealed class CustomColor : ICopyable<CustomColor>
    {
        private const byte MaxColor = 255;
        private const byte MinColor = 0;

        private static readonly Regex _formatHex = new (@"(.{2})", RegexOptions.Compiled);
        private string? _name;

        public CustomColor(string name, bool isHex = false)
        {
            var color = (Color)ColorConverter.ConvertFromString(name);
            this.IsValid = true;
            this.Hex = RgbToHex(new byte[] { color.R, color.G, color.B });
            this.A = MaxColor;
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
            this.Name = isHex ? this.Color.GetName() : name.FormatColorName();
        }

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
        }

        public CustomColor(string name, byte r, byte g, byte b, byte a = MaxColor)
        {
            this.Name = name;
            this.IsValid = true;
            this.Hex = RgbToHex(new byte[] { r, g, b });
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
        }

        private CustomColor()
            : this("Color Uninitialized", "#FF0000")
        {
        }

        private CustomColor(bool isValid)
            : this("Invalid", "#FF0000")
        {
            this.IsValid = isValid;
            this.A = 85;
        }

        [JsonIgnore]
        public static CustomColor Invalid => new (false);

        [JsonIgnore]
        public static CustomColor White => new ("White", MaxColor, MaxColor, MaxColor);

        [JsonIgnore]
        public static CustomColor Black => new ("Black", MinColor, MinColor, MinColor);

        [JsonIgnore]
        public Color Color => Color.FromArgb(this.A, this.R, this.G, this.B);

        public byte A { get; set; }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public string Hex { get; set; }

        public bool IsValid { get; init; }

        public string Name
        {
            get
            {
                return this._name.IsNullOrWhiteSpace() ? this.Color.GetName() : (this._name ?? this.Color.GetName());
            }

            set
            {
                this._name = value;
            }
        }

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
            };
        }

        private static byte[] HexToRgb(string hex)
        {
            var cleanHex = _formatHex.Replace(hex.Strip("#"), "$1 ").Trim();
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
