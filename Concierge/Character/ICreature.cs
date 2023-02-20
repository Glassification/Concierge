// <copyright file="ICreature.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Statuses;

    public interface ICreature
    {
        Attributes Attributes { get; set; }

        Vitality Vitality { get; set; }

        bool IsWeaponProficient(Weapon weapon);
    }
}
