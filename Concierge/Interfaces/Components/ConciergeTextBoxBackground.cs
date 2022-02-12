// <copyright file="ConciergeTextBoxBackground.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Utility;

    public class ConciergeTextBoxBackground : Grid
    {
        public ConciergeTextBoxBackground()
        {
            this.Background = ConciergeColors.ControlBackgroundBrush;
            this.Margin = new Thickness(0, 10, 20, 10);
        }
    }
}
