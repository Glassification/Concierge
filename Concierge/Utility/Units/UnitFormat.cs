// <copyright file="UnitFormat.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Units
{
    using System;

    using Concierge.Primitives;
    using Concierge.Utility.Enums;

    public static class UnitFormat
    {
        public static string HeightPostfix => ConciergeSettings.UnitOfMeasurement == UnitTypes.Imperial ? "Feet/Inches" : "Centimetres";

        public static string WeightPostfix => ConciergeSettings.UnitOfMeasurement == UnitTypes.Imperial ? "Pounds" : "Kilograms";

        public static string ToString(ConciergeDouble value)
        {
            return value.Measurement switch
            {
                Measurements.Height => FormatHeight(ConciergeSettings.UnitOfMeasurement, value.Value),
                Measurements.Weight => FormatWeight(ConciergeSettings.UnitOfMeasurement, value.Value),
                _ => value.Value.ToString(),
            };
        }

        public static string FormatHeight(UnitTypes unitType, double value)
        {
            var feetAndInches = Utilities.GetSeperateFeetAndInches(value);

            return unitType switch
            {
                UnitTypes.Imperial => $"{(feetAndInches.Feet > 0 ? $"{feetAndInches.Feet}'" : string.Empty)} {(feetAndInches.Inches > 0 ? $"{(int)feetAndInches.Inches}\"" : string.Empty)}",
                UnitTypes.Metric => $"{Math.Round(value, Constants.SignificantDigits)} cm",
                _ => value.ToString()
            };
        }

        public static string FormatWeight(UnitTypes unitType, double value, bool reduceDigits = false)
        {
            var significantDigits = reduceDigits ? GetSignificantDigits(value) : Constants.SignificantDigits;

            return unitType switch
            {
                UnitTypes.Imperial => $"{Math.Round(value, Constants.SignificantDigits)} lbs",
                UnitTypes.Metric => $"{Math.Round(value, significantDigits)} kg",
                _ => Math.Round(value, Constants.SignificantDigits).ToString(),
            };
        }

        private static int GetSignificantDigits(double value)
        {
            var digits = ((int)value).ToString().Length;
            return digits > Constants.SignificantDigits ? Constants.SignificantDigits - 1 : Constants.SignificantDigits;
        }
    }
}
