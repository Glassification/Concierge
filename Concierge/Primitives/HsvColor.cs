// <copyright file="HsvColor.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Primitives
{
    using System;

    using Concierge.Utility;

    public sealed class HsvColor : ICopyable<HsvColor>, IComparable
    {
        public HsvColor(double hue, double saturation, double value)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
        }

        public static HsvColor Empty => new (0, 0, 0);

        public double Hue { get; set; }

        public double Saturation { get; set; }

        public double Value { get; set; }

        public static bool operator ==(HsvColor a, HsvColor b)
        {
            return a.Value == b.Value && a.Saturation == b.Saturation && a.Hue == b.Hue;
        }

        public static bool operator !=(HsvColor a, HsvColor b)
        {
            return a.Value != b.Value || a.Saturation != b.Saturation || a.Hue != b.Hue;
        }

        public override string ToString()
        {
            return $"[Hue={this.Hue}, Saturation={this.Saturation}, Value={this.Value}]";
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is null ? false : throw new System.NotImplementedException());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Invert()
        {
            this.Hue = (this.Hue + 180) % 360;
        }

        public HsvColor DeepCopy()
        {
            return new HsvColor(this.Hue, this.Saturation, this.Value);
        }

        public int CompareTo(object? obj)
        {
            if (obj is not HsvColor color)
            {
                return -1;
            }

            return this.Value < color.Value ? -1 : this.Value == color.Value ? 0 : 1;
        }
    }
}
