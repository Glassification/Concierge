// <copyright file="IEquipable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using Concierge.Character.Enums;

    public interface IEquipable
    {
        int Amount { get; set; }

        bool Attuned { get; set; }

        EquipmentSlot EquipmentSlot { get; set; }

        bool IsEquipped { get; set; }

        string Name { get; set; }
    }
}
