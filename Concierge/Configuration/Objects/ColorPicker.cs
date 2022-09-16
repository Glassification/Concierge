// <copyright file="ColorPicker.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using System.Collections.Generic;

    public sealed class ColorPicker
    {
        public ColorPicker()
        {
            this.DefaultColors = new List<string>();
            this.RecentColors = new List<string>();
        }

        public List<string> DefaultColors { get; set; }

        public List<string> RecentColors { get; set; }
    }
}
