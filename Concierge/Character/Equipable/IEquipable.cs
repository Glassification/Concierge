// <copyright file="IEquipable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using Concierge.Character.Enums;

    public interface IEquipable
    {
        string Name { get; set; }

        bool Attuned { get; set; }

        bool IsEquipped { get; set; }

        int Amount { get; set; }

        EquipmentSlot EquipmentSlot { get; set; }
    }
}
