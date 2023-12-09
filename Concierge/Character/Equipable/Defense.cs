// <copyright file="Defense.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Data;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Defense : ICopyable<Defense>
    {
        public Defense()
        {
            this.Armor = new Armor();
            this.ArmorStatus = ArmorStatus.Doffed;
            this.Shield = string.Empty;
            this.ShieldWeight = UnitDouble.Empty;
        }

        public Defense(Armor armor)
        {
            this.Armor = armor;
            this.ArmorStatus = ArmorStatus.Doffed;
            this.Shield = string.Empty;
            this.ShieldWeight = UnitDouble.Empty;
        }

        public Armor Armor { get; set; }

        public ArmorStatus ArmorStatus { get; set; }

        [SearchIgnore]
        [JsonIgnore]
        public PackIconKind ArmorStatusIcon => this.ArmorStatus == ArmorStatus.Doffed ? PackIconKind.ShieldOff : PackIconKind.Shield;

        [SearchIgnore]
        [JsonIgnore]
        public Brush ArmorStatusBrush => this.ArmorStatus == ArmorStatus.Doffed ? Brushes.IndianRed : ConciergeBrushes.Mint;

        public int MagicAc { get; set; }

        public int MiscAc { get; set; }

        public string Shield { get; set; }

        public int ShieldAc { get; set; }

        public UnitDouble ShieldWeight { get; set; }

        [JsonIgnore]
        public int TotalAc => this.MagicAc + this.MiscAc + this.ShieldAc + this.ArmorStatus == ArmorStatus.Donned ? this.Armor.TotalAc : GetDoffedAc();

        [JsonIgnore]
        public double TotalWeight => this.Armor.Weight.Value + this.ShieldWeight.Value;

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
                ShieldWeight = this.ShieldWeight.DeepCopy(),
            };
        }

        private static int GetDoffedAc()
        {
            return Common.Constants.BaseAC + Common.Constants.Bonus(Program.CcsFile.Character.Characteristic.Attributes.Dexterity);
        }
    }
}
