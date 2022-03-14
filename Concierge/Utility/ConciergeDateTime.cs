// <copyright file="ConciergeDateTime.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;

    public static class ConciergeDateTime
    {
        public static string OriginalCreationNow => DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:ss tt");

        public static string StatusMenuNow => DateTime.Now.ToString("h:mm tt   yyyy-MM-d");

        public static string ToolTipNow => DateTime.Now.ToString("MMMM d, yyyy");
    }
}
