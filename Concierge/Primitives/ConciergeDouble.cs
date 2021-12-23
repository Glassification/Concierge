// <copyright file="ConciergeDouble.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Primitives
{
    using System;

    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Utility;
    using Concierge.Utility.Units;
    using Concierge.Utility.Units.Enums;

    public class ConciergeDouble : ICopyable<ConciergeDouble>
    {
        public ConciergeDouble(double value, UnitTypes unitType, Measurements measurement)
        {
            this.Value = value;
            this.UnitType = unitType;
            this.Measurement = measurement;

            AppSettingsManager.UnitsChanged += this.ConciergeDouble_UnitsChanged;
        }

        public static ConciergeDouble Empty => new (0.0, UnitTypes.Imperial, Measurements.Weight);

        public double Value { get; set; }

        public UnitTypes UnitType { get; set; }

        public Measurements Measurement { get; init; }

        public static bool operator ==(ConciergeDouble a, ConciergeDouble b)
        {
            return a.Value == b.Value && a.UnitType == b.UnitType && a.Measurement == b.Measurement;
        }

        public static bool operator !=(ConciergeDouble a, ConciergeDouble b)
        {
            return a.Value != b.Value || a.UnitType != b.UnitType || a.Measurement != b.Measurement;
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

        public ConciergeDouble DeepCopy()
        {
            return new ConciergeDouble(this.Value, this.UnitType, this.Measurement);
        }

        private void ConciergeDouble_UnitsChanged(object sender, EventArgs e)
        {
            var conciergeSettings = sender as SettingsDto;

            this.Value = this.UnitType != conciergeSettings.UnitOfMeasurement
                    ? UnitConvertion.Convert(this.Measurement, conciergeSettings.UnitOfMeasurement, this.Value)
                    : this.Value;
            this.UnitType = conciergeSettings.UnitOfMeasurement;
        }
    }
}
