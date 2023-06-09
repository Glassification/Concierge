// <copyright file="Constants.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;

    /// <summary>
    /// Provides utility methods and constants for the Concierge application.
    /// </summary>
    public static class Constants
    {
        public const byte ColorSpace = 255;

        public const int MaxLevel = 20;
        public const int MinAttributeTotal = 60;
        public const int BaseDC = 8;
        public const int CoinGroup = 50;
        public const int MaxScore = 30;
        public const int MinScore = 0;
        public const int BasePerception = 10;
        public const int MaxClasses = 3;
        public const int MaxAttunedItems = 3;
        public const int SignificantDigits = 2;
        public const int Currencies = 5;
        public const int MaxDepth = 20;
        public const int BrightnessTransition = 130;

        public const string Designer = "Thomas Beckett";
        public const string License = "This program is provided as is, without warranty. The end user is solely responsible for any injuries or TPKs that may result from the use of this product.";
        public const string Copyright = "2018-2023 Most Rights Reserved.";
        public const string ConsolePrompt = "CS> ";

        /// <summary>
        /// Calculates the bonus value based on a given score.
        /// </summary>
        /// <param name="score">The score to calculate the bonus for.</param>
        /// <returns>The calculated bonus value.</returns>
        public static int Bonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }
    }
}
