﻿// <copyright file="ConciergeTreeView.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Utility;

    public sealed class ConciergeTreeView : TreeView
    {
        public ConciergeTreeView()
            : base()
        {
            var scaling = ResolutionScaling.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);
        }
    }
}