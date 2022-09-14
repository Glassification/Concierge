// <copyright file="Conditions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses.ConditionStatus
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Conditions : ICopyable<Conditions>
    {
        public const string BlindedDescription = "Automatically fail any ability checks. Attack rolls against you have advantage, your attacks have disadvantage.";
        public const string CharmedDescription = "You cannot attack the charmer. The charmer has advantage on ability checks when interacting socially.";
        public const string DeafenedDescription = "You cannot hear and automatically fails any ability check that requires hearing.";
        public const string EncumbranceDescription = "A carry weight exceeding 5 and 10 times Strength will reduce speed by 10 and 20 respectively.";
        public const string FatiguedDescription = "Exhaustion levels stack up to 6. A long rest reduces the level by 1.";
        public const string FrightenedDescription = "You have disadvantage on Ability Checks and Attack rolls while the source of fear is within line of sight. You can’t willingly move closer to the source.";
        public const string GrappledDescription = "Your speed becomes 0. It ends when the grappler is incapacitated or you are thrown away.";
        public const string IncapacitatedDescription = "You Cannot take Actions or reactions.";
        public const string InvisibleDescription = "You are impossible to see without the aid of magic or a Special sense. Attacks against you have disadvantage, your attacks have advantage.";
        public const string ParalyzedDescription = "You are incapacitated and automatically fail Strength and Dexterity Saving Throws. Attacks have advantage, and melee are auto crit.";
        public const string PetrifiedDescription = "You are transformed into an inanimate substance and are incapacitated. Resistant to all damage and immune to poison and disease.";
        public const string PoisonedDescription = "You have disadvantage on Attack rolls and Ability Checks";
        public const string ProneDescription = "Your only movement option is to crawl and have disadvantage on attacks. Melee attack is advantage, ranged is disadvantage.";
        public const string RestrainedDescription = "Your speed becomes 0. Your attacks have disadvantage, enemies have advantage. Dexterity Saving Throws are disadvantage.";
        public const string StunnedDescription = "You are incapacitated and speak falteringly, and automatically fail Strength and Dexterity Saving Throws. Attacks against have advantage.";
        public const string UnconsciousDescription = "You are incapacitated, drop what you're holding, and fall prone. Attacks against have advantage and hits are auto crit.";

        public Conditions()
        {
            this.Blinded = new GenericCondition(false, BlindedDescription, nameof(this.Blinded));
            this.Charmed = new GenericCondition(false, CharmedDescription, nameof(this.Charmed));
            this.Deafened = new GenericCondition(false, DeafenedDescription, nameof(this.Deafened));
            this.Encumbrance = new EncumbranceCondition(EncumbranceDescription, nameof(this.Encumbrance));
            this.Fatigued = new ExhaustionCondition(ExhaustionLevel.Normal, FatiguedDescription, nameof(this.Fatigued));
            this.Frightened = new GenericCondition(false, FrightenedDescription, nameof(this.Frightened));
            this.Grappled = new GenericCondition(false, GrappledDescription, nameof(this.Grappled));
            this.Incapacitated = new GenericCondition(false, IncapacitatedDescription, nameof(this.Incapacitated));
            this.Invisible = new GenericCondition(false, InvisibleDescription, nameof(this.Invisible));
            this.Paralyzed = new GenericCondition(false, ParalyzedDescription, nameof(this.Paralyzed));
            this.Petrified = new GenericCondition(false, PetrifiedDescription, nameof(this.Petrified));
            this.Poisoned = new GenericCondition(false, PoisonedDescription, nameof(this.Poisoned));
            this.Prone = new GenericCondition(false, ProneDescription, nameof(this.Prone));
            this.Restrained = new GenericCondition(false, RestrainedDescription, nameof(this.Restrained));
            this.Stunned = new GenericCondition(false, StunnedDescription, nameof(this.Stunned));
            this.Unconscious = new GenericCondition(false, UnconsciousDescription, nameof(this.Unconscious));
        }

        [JsonIgnore]
        public EncumbranceCondition Encumbrance { get; set; }

        public GenericCondition Blinded { get; set; }

        public GenericCondition Charmed { get; set; }

        public GenericCondition Deafened { get; set; }

        public ExhaustionCondition Fatigued { get; set; }

        public GenericCondition Frightened { get; set; }

        public GenericCondition Grappled { get; set; }

        public GenericCondition Incapacitated { get; set; }

        public GenericCondition Invisible { get; set; }

        public GenericCondition Paralyzed { get; set; }

        public GenericCondition Petrified { get; set; }

        public GenericCondition Poisoned { get; set; }

        public GenericCondition Prone { get; set; }

        public GenericCondition Restrained { get; set; }

        public GenericCondition Stunned { get; set; }

        public GenericCondition Unconscious { get; set; }

        public List<Condition> ToList()
        {
            var conditions = new List<Condition>
            {
                this.Blinded,
                this.Charmed,
                this.Deafened,
                this.Encumbrance,
                this.Fatigued,
                this.Frightened,
                this.Grappled,
                this.Incapacitated,
                this.Invisible,
                this.Paralyzed,
                this.Petrified,
                this.Poisoned,
                this.Prone,
                this.Restrained,
                this.Stunned,
                this.Unconscious,
            };

            return conditions.Where(x => x.IsAfflicted()).ToList();
        }

        public Conditions DeepCopy()
        {
            return new Conditions()
            {
                Blinded = this.Blinded.DeepCopy(),
                Charmed = this.Charmed.DeepCopy(),
                Deafened = this.Deafened.DeepCopy(),
                Fatigued = this.Fatigued.DeepCopy(),
                Frightened = this.Frightened.DeepCopy(),
                Grappled = this.Grappled.DeepCopy(),
                Incapacitated = this.Incapacitated.DeepCopy(),
                Invisible = this.Invisible.DeepCopy(),
                Paralyzed = this.Paralyzed.DeepCopy(),
                Petrified = this.Petrified.DeepCopy(),
                Poisoned = this.Poisoned.DeepCopy(),
                Prone = this.Prone.DeepCopy(),
                Restrained = this.Restrained.DeepCopy(),
                Stunned = this.Stunned.DeepCopy(),
                Unconscious = this.Unconscious.DeepCopy(),
            };
        }
    }
}
