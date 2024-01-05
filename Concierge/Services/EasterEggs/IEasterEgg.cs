// <copyright file="IEasterEgg.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.EasterEggs
{
    using System.Windows.Input;

    public interface IEasterEgg
    {
        int CodeIndex { get; }

        public void CheckCode(Key key);
    }
}
