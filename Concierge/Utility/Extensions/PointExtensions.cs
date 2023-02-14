// <copyright file="PointExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System.Windows;

    public static class PointExtensions
    {
        public static Point Multiply(this Point p, double factor)
        {
            return new Point(p.X * factor, p.Y * factor);
        }
    }
}
