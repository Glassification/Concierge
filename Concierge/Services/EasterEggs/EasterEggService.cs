// <copyright file="EasterEggService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.EasterEggs
{
    using System.Windows.Input;

    public sealed class EasterEggService
    {
        private readonly IEasterEgg[] easterEggs = [new KonamiCode()];

        public EasterEggService()
        {
        }

        public void Evaluate(Key key)
        {
            foreach (var egg in this.easterEggs)
            {
                egg.CheckCode(key);
            }
        }
    }
}
