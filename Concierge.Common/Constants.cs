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
        /// <summary>
        /// The maximum value for a color space component.
        /// </summary>
        public const byte ColorSpace = 255;

        /// <summary>
        /// The maximum level allowed in D&amp;D 5E.
        /// </summary>
        public const int MaxLevel = 20;

        /// <summary>
        /// The minimum total attribute value required before a re-roll.
        /// </summary>
        public const int MinAttributeTotal = 60;

        /// <summary>
        /// The base AC (Armor Class).
        /// </summary>
        public const int BaseAC = 10;

        /// <summary>
        /// The base DC (Difficulty Class).
        /// </summary>
        public const int BaseDC = 8;

        /// <summary>
        /// The base DC (Difficulty Class) for maintaining concentration.
        /// </summary>
        public const int BaseConcentrationDC = 10;

        /// <summary>
        /// The number of coins in a group.
        /// </summary>
        public const int CoinGroup = 50;

        /// <summary>
        /// The maximum attribute score value.
        /// </summary>
        public const int MaxScore = 30;

        /// <summary>
        /// The minimum attribute score value.
        /// </summary>
        public const int MinScore = 0;

        /// <summary>
        /// The base perception value.
        /// </summary>
        public const int BasePerception = 10;

        /// <summary>
        /// The maximum number of classes allowed.
        /// </summary>
        public const int MaxClasses = 3;

        /// <summary>
        /// The maximum number of attuned items allowed.
        /// </summary>
        public const int MaxAttunedItems = 3;

        /// <summary>
        /// The number of significant digits used for rounding.
        /// </summary>
        public const int SignificantDigits = 2;

        /// <summary>
        /// The number of different currencies used (Platinum, Gold, Electrum, Silver, Copper).
        /// </summary>
        public const int Currencies = 5;

        /// <summary>
        /// The maximum depth for recursion.
        /// </summary>
        public const int MaxDepth = 20;

        /// <summary>
        /// The brightness transition threshold.
        /// </summary>
        public const int BrightnessTransition = 130;

        /// <summary>
        /// Minimum allowable font size.
        /// </summary>
        public const int FontSizeLimit = 10;

        /// <summary>
        /// Length to display status text on the screen.
        /// </summary>
        public const int StatusDisplayTime = 17;

        /// <summary>
        /// The name of the designer.
        /// </summary>
        public const string Designer = "Thomas Beckett";

        /// <summary>
        /// The license text.
        /// </summary>
        public const string License = "This program is provided as is, without warranty. The end user is solely responsible for any injuries or TPKs that may result from the use of this product.";

        /// <summary>
        /// The copyright notice.
        /// </summary>
        public const string Copyright = "2018-2023 Most Rights Reserved.";

        /// <summary>
        /// The prompt text for the console.
        /// </summary>
        public const string ConsolePrompt = "CS> ";

        /// <summary>
        /// Calculates the bonus value based on a given score.
        /// Subtract 10 from the ability score and then divides the total by 2 (rounded down).
        /// </summary>
        /// <param name="score">The score to calculate the bonus for.</param>
        /// <returns>The calculated bonus value.</returns>
        public static int Bonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        /// <summary>
        /// Calculates the DC (Difficulty Class) of a concentration check.
        /// Divides the damage by 2 (rounded down), and takes the greater of the result or 10.
        /// </summary>
        /// <param name="damage">The damage to calculate the concentration check for.</param>
        /// <returns>The calculated bonus value.</returns>
        public static int Concentration(int damage)
        {
            return Math.Max((int)Math.Floor(damage / 2.0), 10);
        }

        /// <summary>
        /// Calculates the amount of an item that is recoverable.
        /// Divides the amount by 2 (rounded down).
        /// </summary>
        /// <param name="amount">The amount to calculate the recovery for.</param>
        /// <returns>The recovered amount.</returns>
        public static int Recover(int amount)
        {
            return (int)Math.Floor(amount / 2.0);
        }

        /// <summary>
        /// Calculates the amount of spent resources to regain.
        /// Divides the amount by 2 (rounded down) and takes the greater of the result or 1.
        /// </summary>
        /// <param name="spent">The amount to calculate the regain for.</param>
        /// <returns>The recovered amount.</returns>
        public static int Regain(int spent)
        {
            return Math.Max(spent -= Math.Max(spent / 2, 1), 0);
        }
    }
}
