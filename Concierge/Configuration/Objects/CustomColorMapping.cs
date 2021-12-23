// <copyright file="CustomColorMapping.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using System.Collections.Generic;

    public class CustomColorMapping
    {
        public CustomColorMapping()
        {
            this.CustomColors = new Dictionary<string, string>();
        }

        public Dictionary<string, string> CustomColors { get; set; }
    }
}
