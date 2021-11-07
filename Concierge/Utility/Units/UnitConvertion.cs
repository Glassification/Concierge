// <copyright file="UnitConvertion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Units
{
    using Concierge.Utility.Enums;

    public static class UnitConvertion
    {
        private const double InchesToCentimetres = 2.54;
        private const double KiliogramToPounds = 0.45359237;

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
    }
}
