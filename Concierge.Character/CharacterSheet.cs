// <copyright file="CharacterSheet.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Character.Aspects;
    using Concierge.Character.Details;
    using Concierge.Character.Dispositions;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Character.Journals;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Configuration;
    using Concierge.Data.Units;
    using Newtonsoft.Json;

    public sealed class CharacterSheet : ICopyable<CharacterSheet>
    {
        public CharacterSheet()
        {
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

        public static CharacterSheet Empty => new ();

        public Attributes Attributes { get; set; }

        public Companion Companion { get; set; }

        public Detail Detail { get; set; }

        public Disposition Disposition { get; set; }

        public Equipment Equipment { get; set; }

        public Journal Journal { get; set; }

        public SpellCasting SpellCasting { get; set; }

        public Vitality Vitality { get; set; }

        public Wealth Wealth { get; set; }

        [JsonIgnore]
        public double CarryWeight
        {
            get
            {
                var weight = 0.0;

                foreach (var item in this.Equipment.Inventory)
                {
                    if (!item.IgnoreWeight)
                    {
                        weight += item.Weight.Value * item.Amount;
                    }
                }

                foreach (var weapon in this.Equipment.Weapons)
                {
                    if (!weapon.IgnoreWeight)
                    {
                        weight += weapon.Weight.Value;
                    }
                }

                weight += this.Equipment.Defense.TotalWeight;

                if (AppSettingsManager.UserSettings.UseCoinWeight)
                {
                    weight += UnitConversion.Weight(AppSettingsManager.UserSettings.UnitOfMeasurement, this.Wealth.TotalCoins / Constants.CoinGroup);
                }

                return weight;
            }
        }

        [JsonIgnore]
        public ConditionStatus Encumbrance
        {
            get
            {
                var encumbrance = ConditionStatus.Normal;

                if (this.Equipment.Defense.Armor.Strength > this.Attributes.Strength.Score)
                {
                    encumbrance = ConditionStatus.Encumbered;
                }

                if (AppSettingsManager.UserSettings.UseEncumbrance)
                {
                    if (
                        this.CarryWeight > this.LightCapacity && this.CarryWeight <= this.MediumCapacity)
                    {
                        encumbrance = ConditionStatus.Encumbered;
                    }
                    else if (this.CarryWeight > this.MediumCapacity)
                    {
                        encumbrance = ConditionStatus.HeavilyEncumbered;
                    }
                }

                return encumbrance;
            }
        }

        [JsonIgnore]
        public int Initiative => this.Attributes.Dexterity.Bonus + this.Detail.Senses.InitiativeBonus;

        [JsonIgnore]
        public int PassivePerception => Constants.BasePerception + this.Attributes.GetSkillBonus(this.Attributes.Perception, this.ProficiencyBonus) + this.Detail.Senses.PerceptionBonus;

        [JsonIgnore]
        public int ProficiencyBonus => ProficiencyLevel.Get(this.Disposition.Level);

        [JsonIgnore]
        public double LightCapacity => this.Attributes.Strength.Score * UnitConversion.LightMultiplier;

        [JsonIgnore]
        public double MediumCapacity => this.Attributes.Strength.Score * UnitConversion.MediumMultiplier;

        [JsonIgnore]
        public double HeavyCapacity => this.Attributes.Strength.Score * UnitConversion.HeavyMultiplier;

        public int GetMovement(int baseMovement = -1)
        {
            if (baseMovement < 0)
            {
                baseMovement = this.Detail.Senses.BaseMovement + this.Detail.Senses.MovementBonus;
            }

            var grappled = this.Vitality.Status.Grappled;
            var restrained = this.Vitality.Status.Restrained;
            if (this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion5 ||
                grappled.Status == ConditionStatus.Afflicted ||
                restrained.Status == ConditionStatus.Afflicted)
            {
                return 0;
            }
            else
            {
                if (this.Encumbrance == ConditionStatus.Encumbered)
                {
                    baseMovement -= 10;
                }
                else if (this.Encumbrance == ConditionStatus.HeavilyEncumbered)
                {
                    baseMovement -= 20;
                }

                if (this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion2 ||
                    this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion3 ||
                    this.Vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion4)
                {
                    baseMovement /= 2;
                }

                return baseMovement;
            }
        }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public CharacterSheet DeepCopy()
        {
            return new CharacterSheet()
            {
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
    }
}
