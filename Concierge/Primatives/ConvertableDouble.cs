// <copyright file="ConvertableDouble.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Primatives
{
    using System;

    using Concierge.Utility;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Enums;
    using Concierge.Utility.Units;

    public class ConvertableDouble : ICopyable<ConvertableDouble>
    {
        public ConvertableDouble()
            : this(0.0, UnitTypes.Imperial, Measurements.Weight)
        {
        }

        public ConvertableDouble(double value, UnitTypes unitType, Measurements measurement)
        {
            this.Value = value;
            this.UnitType = unitType;
            this.Measurement = measurement;

            ConciergeSettings.UnitsChanged += this.ConvertableDouble_UnitsChanged;
        }

        public double Value { get; set; }

        public UnitTypes UnitType { get; set; }

        public Measurements Measurement { get; init; }

        public static bool operator ==(ConvertableDouble a, ConvertableDouble b)
        {
            return a.Value == b.Value && a.UnitType == b.UnitType;
        }

        public static bool operator !=(ConvertableDouble a, ConvertableDouble b)
        {
            return a.Value != b.Value || a.UnitType != b.UnitType;
        }

        public override string ToString()
        {
            return UnitFormat.ToString(this);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || (obj is null ? false : throw new System.NotImplementedException());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public ConvertableDouble DeepCopy()
        {
            return new ConvertableDouble(this.Value, this.UnitType, this.Measurement);
        }

        private void ConvertableDouble_UnitsChanged(object sender, EventArgs e)
        {
            var conciergeSettings = sender as ConciergeSettingsDto;

            this.Value = this.UnitType != conciergeSettings.UnitOfMeasurement
                    ? UnitConvertion.Convert(this.Measurement, conciergeSettings.UnitOfMeasurement, this.Value)
                    : this.Value;
            this.UnitType = conciergeSettings.UnitOfMeasurement;
        }
    }
}
