// <copyright file="UnitFormat.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Units
{
    using System;

    using Concierge.Configuration;
    using Concierge.Primitives;
    using Concierge.Utility.Units.Dtos;
    using Concierge.Utility.Units.Enums;

    public static class UnitFormat
    {
        public static string HeightPostfix => AppSettingsManager.Settings.UnitOfMeasurement == UnitTypes.Imperial ? "Feet/Inches" : "Centimetres";

        public static string WeightPostfix => AppSettingsManager.Settings.UnitOfMeasurement == UnitTypes.Imperial ? "Pounds" : "Kilograms";

        public static string ToString(UnitDouble value)
        {
            return value.Measurement switch
            {
                Measurements.Height => FormatHeight(AppSettingsManager.Settings.UnitOfMeasurement, value.Value),
                Measurements.Weight => FormatWeight(AppSettingsManager.Settings.UnitOfMeasurement, value.Value),
                _ => value.Value.ToString(),
            };
        }

        public static string FormatHeight(UnitTypes unitType, double value)
        {
            var feetAndInches = GetSeperateFeetAndInches(value);

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

        public static double CombineFeetAndInches(double feet, double inches)
        {
            return (feet * 12) + inches;
        }

        public static FormattedFeetInchesDto GetSeperateFeetAndInches(double totalInches)
        {
            var feet = (int)(totalInches / 12);
            var inches = totalInches - (feet * 12);

            return new FormattedFeetInchesDto(feet, inches);
        }

        private static int GetSignificantDigits(double value)
        {
            var digits = ((int)value).ToString().Length;
            return digits > Constants.SignificantDigits ? Constants.SignificantDigits - 1 : Constants.SignificantDigits;
        }
    }
}
