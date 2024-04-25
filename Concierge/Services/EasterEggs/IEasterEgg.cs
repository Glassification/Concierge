// <copyright file="IEasterEgg.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.EasterEggs
{
    using System.Windows.Input;

    /// <summary>
    /// Represents an interface for handling Easter egg functionality.
    /// </summary>
    public interface IEasterEgg
    {
        /// <summary>
        /// Gets the index of the Easter egg code.
        /// </summary>
        int CodeIndex { get; }

        /// <summary>
        /// Checks the input key against the Easter egg code.
        /// </summary>
        /// <param name="key">The key to check.</param>
        public void CheckCode(Key key);
    }
}
