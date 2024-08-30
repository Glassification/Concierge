// <copyright file="FrameworkElementExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System.Windows;

    /// <summary>
    /// Provides extension methods for framework elements.
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Sets the enable state of a control element and adjusts its opacity.
        /// </summary>
        /// <param name="element">The control element.</param>
        /// <param name="isEnabled">The state indicating whether the control is enabled.</param>
        public static void SetEnableState(this FrameworkElement element, bool isEnabled)
        {
            element.IsEnabled = isEnabled;
            element.Opacity = isEnabled ? 1 : 0.5;
        }
    }
}
