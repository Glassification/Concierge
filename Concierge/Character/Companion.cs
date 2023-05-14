// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Newtonsoft.Json;

    public sealed class Companion : ICopyable<Companion>, ICreature
    {
        public Companion()
        {
            this.Vitality = new Vitality();
            this.Characteristic = new Characteristic();
            this.Equipment = new Equipment();
            this.Properties = new CompanionProperties();
            this.CompanionImage = new CharacterImage();
        }

        public CompanionProperties Properties { get; set; }

        public CharacterImage CompanionImage { get; set; }

        public Vitality Vitality { get; set; }

        [JsonIgnore]
        public CreatureType CreatureType => CreatureType.Companion;

        public Characteristic Characteristic { get; set; }

        public Equipment Equipment { get; set; }

        public Companion DeepCopy()
        {
            return new Companion()
            {
                Vitality = this.Vitality.DeepCopy(),
                Equipment = this.Equipment.DeepCopy(),
                Characteristic = this.Characteristic.DeepCopy(),
                Properties = this.Properties.DeepCopy(),
                CompanionImage = this.CompanionImage.DeepCopy(),
            };
        }

        public bool IsWeaponProficient(Weapon weapon)
        {
            return weapon.ProficiencyOverride;
        }
    }
}
