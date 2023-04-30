// <copyright file="UnitConversion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data.Units
{
    using Concierge.Common.Enums;
    using Concierge.Configuration;

    public static class UnitConversion
    {
        private const double InchesToCentimetres = 2.54;
        private const double KiliogramToPounds = 0.45359237;

        private const int LightStrength = 5;
        private const int MediumStrength = 10;
        private const int HeavyStrength = 15;

        public static double LightMultiplier => GetStrengthMultiplier(LightStrength);

        public static double MediumMultiplier => GetStrengthMultiplier(MediumStrength);

        public static double HeavyMultiplier => GetStrengthMultiplier(HeavyStrength);

        public static double Height(UnitTypes convertTo, double valueToConvert)
        {
            return convertTo switch
            {
                UnitTypes.Imperial => valueToConvert / InchesToCentimetres,
                UnitTypes.Metric => valueToConvert * InchesToCentimetres,
                _ => valueToConvert,
            };
        }

        public static double Weight(UnitTypes convertTo, double valueToConvert)
        {
            return convertTo switch
            {
                UnitTypes.Imperial => valueToConvert / KiliogramToPounds,
                UnitTypes.Metric => valueToConvert * KiliogramToPounds,
                _ => valueToConvert,
            };
        }

        public static double Convert(Measurements measurements, UnitTypes convertTo, double valueToConvert)
        {
            return measurements switch
            {
                Measurements.Height => Height(convertTo, valueToConvert),
                Measurements.Weight => Weight(convertTo, valueToConvert),
                _ => valueToConvert,
            };
        }

        private static double GetStrengthMultiplier(int baseStrength)
        {
            return AppSettingsManager.UserSettings.UnitOfMeasurement switch
            {
                UnitTypes.Imperial => baseStrength,
                UnitTypes.Metric => (baseStrength / 2) + (baseStrength * 0.05),
                _ => baseStrength,
            };
        }
    }
}
