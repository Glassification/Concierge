// <copyright file="ConciergeDefaultScale.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Interface
{
    public class ConciergeDefaultScale
    {
        public ConciergeDefaultScale()
        {
            this.ScaleInitialized = 0;
            this.FullScreenHeight = 1;
            this.FullScreenWidth = 1;
        }

        public double FullScreenHeight { get; set; }

        public double FullScreenWidth { get; set; }

        private int ScaleInitialized { get; set; }

        public void Initialize(double height, double width)
        {
            if (this.ScaleInitialized >= 2)
            {
                return;
            }

            this.FullScreenHeight = height;
            this.FullScreenWidth = width;

            this.ScaleInitialized++;
        }
    }
}
