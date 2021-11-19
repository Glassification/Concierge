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

        public static string WeightUnits => ConciergeSettings.UnitOfMeasurement == UnitTypes.Imperial ? "lbs" : "kg";

        public static string ToString(ConciergeDouble value)
        {
            return value.Measurement switch
            {
                Measurements.Height => FormatHeight(ConciergeSettings.UnitOfMeasurement, value.Value),
                Measurements.Weight => FormatWeight(ConciergeSettings.UnitOfMeasurement, value.Value),
                _ => value.Value.ToString(),
            };
        }

        private static string FormatHeight(UnitTypes unitType, double value)
        {
            var feetAndInches = Utilities.GetSeperateFeetAndInches(value);

            return unitType switch
            {
                UnitTypes.Imperial => $"{(feetAndInches.Feet > 0 ? $"{feetAndInches.Feet}'" : string.Empty)} {(feetAndInches.Inches > 0 ? $"{(int)feetAndInches.Inches}\"" : string.Empty)}",
                UnitTypes.Metric => $"{Math.Round(value, Constants.SignificantDigits)} cm",
                _ => value.ToString()
            };
        }

        private static string FormatWeight(UnitTypes unitType, double value)
        {
            return unitType switch
            {
                UnitTypes.Imperial => $"{Math.Round(value, Constants.SignificantDigits)} lbs",
                UnitTypes.Metric => $"{Math.Round(value, Constants.SignificantDigits)} kg",
                _ => Math.Round(value, Constants.SignificantDigits).ToString(),
            };
        }
    }
}
