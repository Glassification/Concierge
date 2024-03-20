// <copyright file="UnitFormat.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>
namespace Concierge.Data.Units
{
    using System;

    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Configuration;

    /// <summary>
    /// Provides formatting methods for units of measurement.
    /// </summary>
    public static class UnitFormat
    {
        /// <summary>
        /// Gets the postfix string for height based on the current unit of measurement.
        /// </summary>
        public static string HeightPostfix => AppSettingsManager.UserSettings.UnitOfMeasurement == UnitTypes.Imperial ? "Feet/Inches" : "Centimetres";

        /// <summary>
        /// Gets the postfix string for weight based on the current unit of measurement.
        /// </summary>
        public static string WeightPostfix => AppSettingsManager.UserSettings.UnitOfMeasurement == UnitTypes.Imperial ? "Pounds" : "Kilograms";

        /// <summary>
        /// Converts a <see cref="UnitDouble"/> value to its string representation.
        /// </summary>
        /// <param name="value">The <see cref="UnitDouble"/> value to convert.</param>
        /// <returns>The string representation of the <see cref="UnitDouble"/> value.</returns>
        public static string ToString(UnitDouble value)
        {
            return value.Measurement switch
            {
                Measurements.Height => FormatHeight(AppSettingsManager.UserSettings.UnitOfMeasurement, value.Value),
                Measurements.Weight => FormatWeight(AppSettingsManager.UserSettings.UnitOfMeasurement, value.Value),
                _ => value.Value.ToString(),
            };
        }

        /// <summary>
        /// Formats a height value based on the specified unit type.
        /// </summary>
        /// <param name="unitType">The unit type to format the height value.</param>
        /// <param name="value">The height value to format.</param>
        /// <returns>The formatted height string.</returns>
        public static string FormatHeight(UnitTypes unitType, double value)
        {
            var feetAndInches = GetSeperateFeetAndInches(value);

            return unitType switch
            {
                UnitTypes.Imperial => $"{(feetAndInches.Feet > 0 ? $"{feetAndInches.Feet}'" : string.Empty)} {(feetAndInches.Inches > 0 ? $"{(int)feetAndInches.Inches}\"" : string.Empty)}",
                UnitTypes.Metric => $"{Math.Round(value, ConciergeMath.SignificantDigits)} cm",
                _ => value.ToString()
            };
        }

        /// <summary>
        /// Formats a weight value based on the specified unit type.
        /// </summary>
        /// <param name="unitType">The unit type to format the weight value.</param>
        /// <param name="value">The weight value to format.</param>
        /// <param name="reduceDigits">Optional. Specifies whether to reduce the number of significant digits. Default is false.</param>
        /// <returns>The formatted weight string.</returns>
        public static string FormatWeight(UnitTypes unitType, double value, bool reduceDigits = false)
        {
            var significantDigits = reduceDigits ? GetSignificantDigits(value) : ConciergeMath.SignificantDigits;

            return unitType switch
            {
                UnitTypes.Imperial => $"{Math.Round(value, ConciergeMath.SignificantDigits)} lbs",
                UnitTypes.Metric => $"{Math.Round(value, significantDigits)} kg",
                _ => Math.Round(value, ConciergeMath.SignificantDigits).ToString(),
            };
        }

        /// <summary>
        /// Combines the feet and inches values into a single total inches value.
        /// </summary>
        /// <param name="feet">The feet value.</param>
        /// <param name="inches">The inches value.</param>
        /// <returns>The total inches value.</returns>
        public static double CombineFeetAndInches(double feet, double inches)
        {
            return (feet * 12) + inches;
        }

        /// <summary>
        /// Separates the total inches value into feet and inches.
        /// </summary>
        /// <param name="totalInches">The total inches value.</param>
        /// <returns>A <see cref="FormattedFeetInches"/> object containing the feet and inches values.</returns>
        public static FormattedFeetInches GetSeperateFeetAndInches(double totalInches)
        {
            var feet = (int)(totalInches / 12);
            var inches = totalInches - (feet * 12);

            return new FormattedFeetInches(feet, inches);
        }

        private static int GetSignificantDigits(double value)
        {
            var digits = ((int)value).ToString().Length;
            return digits > ConciergeMath.SignificantDigits ? ConciergeMath.SignificantDigits - 1 : ConciergeMath.SignificantDigits;
        }
    }
}
