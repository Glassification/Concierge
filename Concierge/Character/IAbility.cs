// <copyright file="IAbility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Character.Enums;

    public interface IAbility
    {
        int Bonus { get; }

        StatusChecks StatusChecks { get; }
    }
}
