// <copyright file="RecentColors.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;

    public class RecentColors
    {
        private Queue<Color> _colorsQueue = new Queue<Color>();

        public RecentColors()
        {

        }

        public List<Color> Colors => this._colorsQueue.ToList();
    }
}
