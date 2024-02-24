// <copyright file="ArmorType.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Enum representing different classes of armor.
    /// </summary>
    public enum ArmorType
    {
        [Description("None")]
        None,
        [Description("Light Armor")]
        Light,
        [Description("Medium Armor")]
        Medium,
        [Description("Heavy Armor")]
        Heavy,
        [Description("Massive Armor")]
        Massive,
    }
}
