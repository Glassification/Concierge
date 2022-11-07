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
        private string _name;

        public CustomColor()
            : this("White", "#FFFFFF")
        {
        }

        public CustomColor(string name, string hex)
        {
            var rgb = HexToRgb(hex);

            this._name = name;
            this.IsValid = true;
            this.Hex = hex;
            this.R = rgb[0];
            this.G = rgb[1];
            this.B = rgb[2];
        }

        public CustomColor(string name, byte r, byte g, byte b)
        {
            this._name = name;
            this.IsValid = true;
            this.Hex = RgbToHex(new byte[] { r, g, b });
            this.R = r;
            this.G = g;
            this.B = b;
        }

        private CustomColor(bool isValid)
            : this("White", "#FFFFFF")
        {
            this.IsValid = isValid;
        }

        [JsonIgnore]
        public static CustomColor Empty => new (false);

        [JsonIgnore]
        public Color Color => Color.FromArgb(255, this.R, this.G, this.B);

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public string Hex { get; set; }

        public bool IsValid { get; init; }

        public string Name
        {
            get
            {
                return this._name.IsNullOrWhiteSpace() ? this.Color.GetName() : this._name;
            }

            set
            {
                this._name = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public CustomColor DeepCopy()
        {
            return new CustomColor()
            {
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
            var cleanHex = Regex.Replace(hex.Strip("#"), @"(.{2})", "$1 ").Trim();
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
