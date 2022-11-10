// <copyright file="ColorPicker.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using System.Collections.Generic;

    using Concierge.Primitives;

    public sealed class ColorPicker
    {
        public ColorPicker()
        {
            this.DefaultColors = new List<CustomColor>();
            this.RecentColors = new List<CustomColor>();
        }

        public List<CustomColor> DefaultColors { get; set; }

        public List<CustomColor> RecentColors { get; set; }
    }
}
