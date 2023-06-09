// <copyright file="UnitDouble.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using System;

    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Data.Units;

    /// <summary>
    /// Represents a double value with associated unit type and measurement.
    /// </summary>
    public sealed class UnitDouble : ICopyable<UnitDouble>, IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitDouble"/> class with the specified value, unit type, and measurement.
        /// </summary>
        /// <param name="value">The numeric value.</param>
        /// <param name="unitType">The unit type.</param>
        /// <param name="measurement">The measurement type.</param>
        public UnitDouble(double value, UnitTypes unitType, Measurements measurement)
        {
            this.Value = value;
            this.UnitType = unitType;
            this.Measurement = measurement;

            AppSettingsManager.UnitsChanged += this.ConciergeDouble_UnitsChanged;
        }

        /// <summary>
        /// Gets an empty <see cref="UnitDouble"/> instance with default values.
        /// </summary>
        public static UnitDouble Empty => new (0.0, UnitTypes.Imperial, Measurements.Weight);

        /// <summary>
        /// Gets or sets the numeric value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the unit type.
        /// </summary>
        public UnitTypes UnitType { get; set; }

        /// <summary>
        /// Gets the measurement type.
        /// </summary>
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

        /// <summary>
        /// Creates a deep copy of the <see cref="UnitDouble"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="UnitDouble"/> object.</returns>
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
                    ? UnitConversion.Convert(this.Measurement, conciergeSettings.UnitOfMeasurement, this.Value)
                    : this.Value;
            this.UnitType = conciergeSettings.UnitOfMeasurement;
        }
    }
}