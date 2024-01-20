// <copyright file="ConciergeDateTime.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides utility methods for working with date and time related operations in the Concierge application.
    /// </summary>
    public static class ConciergeDateTime
    {
        /// <summary>
        /// Gets the current date and time formatted for logging purposes.
        /// The format is "dd/MM/yyyy HH:mm:ss".
        /// </summary>
        public static string LoggingNow => DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets the current date formatted for log rotation purposes.
        /// The format is "yyyy-MM-dd".
        /// </summary>
        public static string RotateLog => DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets the current date and time formatted for original creation purposes.
        /// The format is "dddd, dd MMMM yyyy hh:mm:ss tt".
        /// </summary>
        public static string OriginalCreationNow => DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:ss tt");

        /// <summary>
        /// Gets the current date and time formatted for status menu display purposes.
        /// The format is "h:mm tt   yyyy-MM-d".
        /// </summary>
        public static string StatusMenuNow => DateTime.Now.ToString("h:mm tt   yyyy-MM-d");

        /// <summary>
        /// Gets the current date formatted for tooltip display purposes.
        /// The format is "dddd MMMM d, yyyy".
        /// </summary>
        public static string ToolTipNow => DateTime.Now.ToString("dddd MMMM d, yyyy");

        /// <summary>
        /// Gets the current time formatted for message history display purposes.
        /// The format is "hh:mm:ss tt".
        /// </summary>
        public static string MessageTime => DateTime.Now.ToString("hh:mm:ss tt");
    }
}
