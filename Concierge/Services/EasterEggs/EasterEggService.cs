// <copyright file="EasterEggService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.EasterEggs
{
    using System.Windows.Input;

    using Concierge.Configuration;

    /// <summary>
    /// Service responsible for evaluating and handling Easter eggs.
    /// </summary>
    public sealed class EasterEggService
    {
        private readonly IEasterEgg[] easterEggs = [new KonamiCode()];

        /// <summary>
        /// Initializes a new instance of the <see cref="EasterEggService"/> class.
        /// </summary>
        public EasterEggService()
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
