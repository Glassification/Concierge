// <copyright file="ConciergeSeparator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows.Controls;

    using Concierge.Common;

    public sealed class ConciergeSeparator : Separator
    {
        public ConciergeSeparator()
            : base()
        {
            this.Background = ConciergeBrushes.Separator;
        }
    }
}
