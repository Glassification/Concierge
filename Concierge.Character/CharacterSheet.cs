// <copyright file="CharacterSheet.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Collections.Generic;

    using Concierge.Character.Aspects;
    using Concierge.Character.Companions;
    using Concierge.Character.Details;
    using Concierge.Character.Dispositions;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Character.Journals;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Data.Units;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a master character sheet containing various character-related information.
    /// </summary>
    public sealed class CharacterSheet : ICopyable<CharacterSheet>
    {
        private const int LightStrength = 5;
        private const int MediumStrength = 10;
        private const int HeavyStrength = 15;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSheet"/> class.
        /// </summary>
        public CharacterSheet()
        {
            this.Adventurers = [];
            this.Attributes = new Attributes();
            this.Companion = new Companion();
            this.Detail = new Detail();
            this.Disposition = new Disposition();
            this.Equipment = new Equipment();
            this.Journal = new Journal();
            this.SpellCasting = new SpellCasting();
            this.Vitality = new Vitality();
            this.Wealth = new Wealth();
        }

        /// <summary>
        /// Gets an empty character sheet instance.
        /// </summary>
        public static CharacterSheet Empty => new ();

        /// <summary>
        /// Gets or sets the adventuring party of the character.
        /// </summary>
        public List<Adventurer> Adventurers { get; set; }

        /// <summary>
        /// Gets or sets the attributes of the character.
        /// </summary>
        public Attributes Attributes { get; set; }

        /// <summary>
        /// Gets or sets the companion associated with the character.
        /// </summary>
        public Companion Companion { get; set; }

        /// <summary>
        /// Gets or sets the detail information of the character.
        /// </summary>
        public Detail Detail { get; set; }

        /// <summary>
        /// Gets or sets the disposition of the character.
        /// </summary>
        public Disposition Disposition { get; set; }

        /// <summary>
        /// Gets or sets the equipment carried by the character.
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// Gets or sets the journal entries related to the character.
        /// </summary>
        public Journal Journal { get; set; }

        /// <summary>
        /// Gets or sets the spellcasting abilities of the character.
        /// </summary>
        public SpellCasting SpellCasting { get; set; }

        /// <summary>
        /// Gets or sets the vitality status of the character.
        /// </summary>
        public Vitality Vitality { get; set; }

        /// <summary>
        /// Gets or sets the wealth and possessions of the character.
        /// </summary>
        public Wealth Wealth { get; set; }

        /// <summary>
        /// Gets the total carry weight of the character.
        /// </summary>
        [JsonIgnore]
        public double CarryWeight
        {
            get
            {
                var weight = 0.0;

                this.Equipment.Inventory.ForEach(x => weight += x.IgnoreWeight ? 0 : x.Weight.Value * x.Amount);
                this.Equipment.Weapons.ForEach(y => weight += y.IgnoreWeight ? 0 : y.Weight.Value);

                weight += this.Equipment.Defense.TotalWeight;

                if (AppSettingsManager.UserSettings.UseCoinWeight)
                {
                    weight += UnitConversion.Weight(AppSettingsManager.UserSettings.UnitOfMeasurement, this.Wealth.TotalWeight);
                }

                return weight;
            }
        }

        /// <summary>
        /// Gets the encumbrance status of the character.
        /// </summary>
        [JsonIgnore]
        public Encumbrance Encumbrance
        {
            get
            {
                var encumbrance = ConditionStatus.Normal;
                if (this.Equipment.Defense.Armor.Strength > this.Attributes.Strength.Score)
                {
                    encumbrance = ConditionStatus.ArmorEncumbered;
                }

                if (AppSettingsManager.UserSettings.UseEncumbrance)
                {
                    if (ConciergeMath.Between(this.CarryWeight, this.LightCapacity, this.MediumCapacity, Inclusivity.RightInclusive))
                    {
                        encumbrance = ConditionStatus.Encumbered;
                    }
                    else if (this.CarryWeight > this.MediumCapacity)
                    {
                        encumbrance = ConditionStatus.HeavilyEncumbered;
                    }
                }

                return new Encumbrance(encumbrance);
            }
        }

        /// <summary>
        /// Gets the initiative of the character.
        /// </summary>
        [JsonIgnore]
        public int Initiative => this.Attributes.Dexterity.Bonus + this.Detail.Senses.InitiativeBonus;

        /// <summary>
        /// Gets the passive perception of the character.
        /// </summary>
        [JsonIgnore]
        public int PassivePerception => Constants.BasePerception + this.Attributes.GetSkillBonus(this.Attributes.Perception, this.ProficiencyBonus) + this.Detail.Senses.PerceptionBonus;

        /// <summary>
        /// Gets the proficiency bonus of the character.
        /// </summary>
        [JsonIgnore]
        public int ProficiencyBonus => ProficiencyLevel.Get(this.Disposition.Level);

        /// <summary>
        /// Gets the light carrying capacity of the character.
        /// </summary>
        [JsonIgnore]
        public double LightCapacity => UnitConversion.GetStrength(this.Attributes.Strength.Score * LightStrength);

        /// <summary>
        /// Gets the medium carrying capacity of the character.
        /// </summary>
        [JsonIgnore]
        public double MediumCapacity => UnitConversion.GetStrength(this.Attributes.Strength.Score * MediumStrength);

        /// <summary>
        /// Gets the heavy carrying capacity of the character.
        /// </summary>
        [JsonIgnore]
        public double HeavyCapacity => UnitConversion.GetStrength(this.Attributes.Strength.Score * HeavyStrength);

        /// <summary>
        /// Gets the movement speed of the character.
        /// </summary>
        /// <param name="baseMovement">The base movement speed.</param>
        /// <returns>The movement speed of the character.</returns>
        public int GetMovement(int baseMovement = -1)
        {
            if (this.HasNoMovement())
            {
                return 0;
            }

            if (baseMovement < 0)
            {
                baseMovement = this.Detail.Senses.BaseMovement + this.Detail.Senses.MovementBonus;
            }

            if (this.Encumbrance.Status == ConditionStatus.Encumbered || this.Encumbrance.Status == ConditionStatus.ArmorEncumbered)
            {
                baseMovement -= 10;
            }
            else if (this.Encumbrance.Status == ConditionStatus.HeavilyEncumbered)
            {
                baseMovement -= 20;
            }

            if (this.IsExhausted())
            {
                baseMovement /= 2;
            }

            return baseMovement;
        }

        /// <summary>
        /// Retrieves the Warlock level of the character based on their class disposition.
        /// </summary>
        /// <returns>
        /// The Warlock level of the character, or 0 if the character does not have a Warlock class.
        /// </returns>
        public int GetWarlockLevel()
        {
            if (this.Disposition.Class1.Name.Contains("Warlock", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Disposition.Class1.Level;
            }
            else if (this.Disposition.Class2.Name.Contains("Warlock", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Disposition.Class2.Level;
            }
            else if (this.Disposition.Class3.Name.Contains("Warlock", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Disposition.Class3.Level;
            }

            return 0;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not CharacterSheet)
            {
                return false;
            }

            var character1 = JsonConvert.SerializeObject(obj as CharacterSheet);
            var character2 = JsonConvert.SerializeObject(this);

            return character1.Equals(character2);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Creates a deep copy of the character sheet.
        /// </summary>
        /// <returns>A deep copy of the <see cref="CharacterSheet"/>.</returns>
        public CharacterSheet DeepCopy()
        {
            return new CharacterSheet()
            {
                Adventurers = [.. this.Adventurers.DeepCopy()],
                Attributes = this.Attributes.DeepCopy(),
                Companion = this.Companion.DeepCopy(),
                Detail = this.Detail.DeepCopy(),
                Disposition = this.Disposition.DeepCopy(),
                Equipment = this.Equipment.DeepCopy(),
                Journal = this.Journal.DeepCopy(),
                SpellCasting = this.SpellCasting.DeepCopy(),
                Vitality = this.Vitality.DeepCopy(),
                Wealth = this.Wealth.DeepCopy(),
            };
        }

        private bool HasNoMovement()
        {
            return
                this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion5 ||
                this.Vitality.Status.Grappled.Status == ConditionStatus.Afflicted ||
                this.Vitality.Status.Restrained.Status == ConditionStatus.Afflicted;
        }

        private bool IsExhausted()
        {
            return
                this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion2 ||
                this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion3 ||
                this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion4;
        }
    }
}
