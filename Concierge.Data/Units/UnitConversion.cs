// <copyright file="UnitConversion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data.Units
{
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Configuration;

    /// <summary>
    /// Provides conversion methods for various units of measurement.
    /// </summary>
    public static class UnitConversion
    {
        private const double InchesToCentimetres = 2.54;
        private const double KiliogramToPounds = 0.45359237;

        /// <summary>
        /// Converts a height value from one unit type to another.
        /// </summary>
        /// <param name="convertTo">The unit type to convert the value to.</param>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>The converted height value.</returns>
        public static double Height(UnitTypes convertTo, double valueToConvert)
        {
            return convertTo switch
            {
                UnitTypes.Imperial => valueToConvert / InchesToCentimetres,
                UnitTypes.Metric => valueToConvert * InchesToCentimetres,
                _ => valueToConvert,
            };
        }

        /// <summary>
        /// Converts a weight value from one unit type to another.
        /// </summary>
        /// <param name="convertTo">The unit type to convert the value to.</param>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>The converted weight value.</returns>
        public static double Weight(UnitTypes convertTo, double valueToConvert)
        {
            return convertTo switch
            {
                UnitTypes.Imperial => valueToConvert / KiliogramToPounds,
                UnitTypes.Metric => valueToConvert * KiliogramToPounds,
                _ => valueToConvert,
            };
        }

        /// <summary>
        /// Converts a value of a specific measurement type from one unit type to another.
        /// </summary>
        /// <param name="measurements">The measurement type.</param>
        /// <param name="convertTo">The unit type to convert the value to.</param>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static double Convert(Measurements measurements, UnitTypes convertTo, double valueToConvert)
        {
            return measurements switch
            {
                Measurements.Height => Height(convertTo, valueToConvert),
                Measurements.Weight => Weight(convertTo, valueToConvert),
                _ => valueToConvert,
            };
        }

        /// <summary>
        /// Calculates the strength value based on the unit of measurement set in the application settings.
        /// </summary>
        /// <param name="baseStrength">The base strength value in Imperial units.</param>
        /// <returns>
        /// The strength value, adjusted for the unit of measurement. If the unit of measurement is set to Metric,
        /// the strength value is converted from pounds to kilograms and rounded so that the last digit is a multiple of 5.
        /// </returns>
        public static double GetStrength(int baseStrength)
        {
            return AppSettingsManager.UserSettings.UnitOfMeasurement switch
            {
                UnitTypes.Imperial => baseStrength,
                UnitTypes.Metric => ConciergeMath.RoundLastDigitTo5(baseStrength * KiliogramToPounds),
                _ => baseStrength,
            };
        }
    }
}
