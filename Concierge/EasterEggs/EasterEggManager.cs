// <copyright file="EasterEggManager.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.EasterEggs
{
    using System.Windows.Input;

    using Concierge.Configuration;

    /// <summary>
    /// Service responsible for evaluating and handling Easter eggs.
    /// </summary>
    public sealed class EasterEggManager
    {
        private readonly IEasterEgg[] easterEggs = [new KonamiCode()];

        /// <summary>
        /// Initializes a new instance of the <see cref="EasterEggManager"/> class.
        /// </summary>
        public EasterEggManager()
        {
        }

        /// <summary>
        /// Evaluates the input key against registered Easter eggs.
        /// </summary>
        /// <param name="key">The input key.</param>
        public void Evaluate(Key key)
        {
            if (!AppSettingsManager.StartUp.WildWasteland)
            {
                return;
            }

            foreach (var egg in this.easterEggs)
            {
                egg.CheckCode(key);
            }
        }
    }
}
