// <copyright file="UnitDouble.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Primitives
{
    using System;

    using Concierge.Common;
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Primitives.Units;
    using Concierge.Primitives.Units.Enums;

    public sealed class UnitDouble : ICopyable<UnitDouble>, IComparable
    {
        public UnitDouble(double value, UnitTypes unitType, Measurements measurement)
        {
            this.Value = value;
            this.UnitType = unitType;
            this.Measurement = measurement;

            AppSettingsManager.UnitsChanged += this.ConciergeDouble_UnitsChanged;
        }

        public static UnitDouble Empty => new (0.0, UnitTypes.Imperial, Measurements.Weight);

        public double Value { get; set; }

        public UnitTypes UnitType { get; set; }

        public Measurements Measurement { get; init; }

        public static bool operator ==(UnitDouble a, UnitDouble b)
        {
            return a.Value == b.Value && a.UnitType == b.UnitType && a.Measurement == b.Measurement;
        }

        public static bool operator !=(UnitDouble a, UnitDouble b)
        {
            return a.Value != b.Value || a.UnitType != b.UnitType || a.Measurement != b.Measurement;
        }

        public override string ToString()
        {
            return UnitFormat.ToString(this);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || (obj is null ? false : throw new NotImplementedException());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public UnitDouble DeepCopy()
        {
            return new UnitDouble(this.Value, this.UnitType, this.Measurement);
        }

        public int CompareTo(object? other)
        {
            if (other is not UnitDouble unit)
            {
                return -1;
            }

            return this.Value < unit.Value ? -1 : this.Value == unit.Value ? 0 : 1;
        }

        private void ConciergeDouble_UnitsChanged(object sender, EventArgs e)
        {
            if (sender is not UserSettingsDto conciergeSettings)
            {
                return;
            }

            this.Value = this.UnitType != conciergeSettings.UnitOfMeasurement
                    ? UnitConvertion.Convert(this.Measurement, conciergeSettings.UnitOfMeasurement, this.Value)
                    : this.Value;
            this.UnitType = conciergeSettings.UnitOfMeasurement;
        }
    }
}