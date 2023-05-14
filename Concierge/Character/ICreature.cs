// <copyright file="ICreature.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Character.Vitals;

    public interface ICreature
    {
        Characteristic Characteristic { get; set; }

        Vitality Vitality { get; set; }

        CreatureType CreatureType { get; }

        bool IsWeaponProficient(Weapon weapon);
    }
}
