// <copyright file="HsvColor.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Primatives
{
    using Concierge.Utility;

    public class HsvColor : ICopyable<HsvColor>
    {
        public HsvColor(double hue, double saturation, double value)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
        }

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
            return $"[{this.Hue}, {this.Saturation}, {this.Value}]";
        }

        public override bool Equals(object obj)
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
    }
}
