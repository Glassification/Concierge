// <copyright file="Defense.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Aspects;
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    using Constants = Concierge.Common.Constants;

    /// <summary>
    /// Represents the defense attributes of a character, including armor, shield, and total armor class calculation.
    /// </summary>
    public sealed class Defense : ICopyable<Defense>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Defense"/> class with default values.
        /// </summary>
        public Defense()
        {
            this.Armor = new Armor();
            this.ArmorStatus = ArmorStatus.Doffed;
            this.Shield = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Defense"/> class with the specified armor.
        /// </summary>
        /// <param name="armor">The armor to use for defense.</param>
        public Defense(Armor armor)
        {
            this.Armor = armor;
            this.ArmorStatus = ArmorStatus.Doffed;
            this.Shield = string.Empty;
        }

        /// <summary>
        /// Gets or sets the armor used for defense.
        /// </summary>
        public Armor Armor { get; set; }

        /// <summary>
        /// Gets or sets the status of the armor (donned or doffed).
        /// </summary>
        public ArmorStatus ArmorStatus { get; set; }

        /// <summary>
        /// Gets or sets the magical armor class bonus.
        /// </summary>
        public int MagicAc { get; set; }

        /// <summary>
        /// Gets or sets the miscellaneous armor class bonus.
        /// </summary>
        public int MiscAc { get; set; }

        /// <summary>
        /// Gets or sets the shield used for defense.
        /// </summary>
        public string Shield { get; set; }

        /// <summary>
        /// Gets or sets the armor class bonus provided by the shield.
        /// </summary>
        public int ShieldAc { get; set; }

        [SearchIgnore]
        [JsonIgnore]
        public PackIconKind ArmorStatusIcon => this.ArmorStatus == ArmorStatus.Doffed ? PackIconKind.ShieldOff : PackIconKind.Shield;

        [SearchIgnore]
        [JsonIgnore]
        public Brush ArmorStatusBrush => this.ArmorStatus == ArmorStatus.Doffed ? Brushes.IndianRed : ConciergeBrushes.Mint;

        [JsonIgnore]
        public double TotalWeight => this.Armor.Weight.Value;

        /// <summary>
        /// Creates a deep copy of the <see cref="Defense"/> object.
        /// </summary>
        /// <returns>A new instance of <see cref="Defense"/> with the same property values as this instance.</returns>
        public Defense DeepCopy()
        {
            return new Defense()
            {
                Armor = this.Armor.DeepCopy(),
                ArmorStatus = this.ArmorStatus,
                MagicAc = this.MagicAc,
                MiscAc = this.MiscAc,
                Shield = this.Shield,
                ShieldAc = this.ShieldAc,
            };
        }

        /// <summary>
        /// Calculates the total armor class based on the defense attributes and dexterity bonus.
        /// </summary>
        /// <param name="dexterity">The dexterity attribute.</param>
        /// <returns>The total armor class value.</returns>
        public int GetTotalArmorClass(Dexterity dexterity)
        {
            var armorClass = this.MagicAc + this.MiscAc + this.ShieldAc;
            var baseAC = Constants.BaseAC + dexterity.Bonus;
            var armorAC = this.Armor.Type switch
            {
                ArmorType.None => dexterity.Bonus,
                ArmorType.Light => dexterity.Bonus,
                ArmorType.Medium => Math.Min(2, dexterity.Bonus),
                _ => 0,
            };

            armorAC = this.Armor.FullDex ? dexterity.Bonus : armorAC;
            return armorClass + (this.ArmorStatus == ArmorStatus.Donned ? armorAC + this.Armor.Ac : baseAC);
        }
    }
}
