// <copyright file="Weapon.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Primitives;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;
    using Newtonsoft.Json;

    public class Weapon : ICopyable<Weapon>
    {
        public Weapon()
        {
            this.Name = string.Empty;
            this.Damage = string.Empty;
            this.Misc = string.Empty;
            this.Range = string.Empty;
            this.Note = string.Empty;
            this.Weight = UnitDouble.Empty;
            this.Id = Guid.NewGuid();
            this.ProficiencyOverride = false;
        }

        public string Name { get; set; }

        [JsonIgnore]
        public int Attack
        {
            get
            {
                var bonus = 0;
                var character = Program.CcsFile.Character;

                if (character.IsWeaponProficient(this))
                {
                    bonus = character.ProficiencyBonus;
                }

                return this.Ability switch
                {
                    Abilities.STR => CharacterUtility.CalculateBonus(character.Attributes.Strength) + bonus,
                    Abilities.DEX => CharacterUtility.CalculateBonus(character.Attributes.Dexterity) + bonus,
                    Abilities.CON => CharacterUtility.CalculateBonus(character.Attributes.Constitution) + bonus,
                    Abilities.INT => CharacterUtility.CalculateBonus(character.Attributes.Intelligence) + bonus,
                    Abilities.WIS => CharacterUtility.CalculateBonus(character.Attributes.Wisdom) + bonus,
                    Abilities.CHA => CharacterUtility.CalculateBonus(character.Attributes.Charisma) + bonus,
                    Abilities.NONE => bonus,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        public Abilities Ability { get; set; }

        public string Damage { get; set; }

        public string Misc { get; set; }

        public DamageTypes DamageType { get; set; }

        public string Range { get; set; }

        public string Note { get; set; }

        public UnitDouble Weight { get; set; }

        public WeaponTypes WeaponType { get; set; }

        public bool ProficiencyOverride { get; set; }

        public bool IsInBagOfHolding { get; set; }

        public Guid Id { get; init; }

        public Weapon DeepCopy()
        {
            return new Weapon()
            {
                Name = this.Name,
                Ability = this.Ability,
                Damage = this.Damage,
                Misc = this.Misc,
                DamageType = this.DamageType,
                Range = this.Range,
                Note = this.Note,
                Weight = this.Weight.DeepCopy(),
                WeaponType = this.WeaponType,
                ProficiencyOverride = this.ProficiencyOverride,
                IsInBagOfHolding = this.IsInBagOfHolding,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
