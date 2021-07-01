// <copyright file="SavingThrows.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.SavingThrowsNamespace
{
    using Concierge.Characters.Enums;

    public abstract class SavingThrows
    {
        public bool Proficiency
        {
            get;
            set;
        }

        public abstract StatusChecks StatusChecks
        {
            get;
        }

        public abstract int Bonus
        {
            get;
        }
    }
}
