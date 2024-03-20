// <copyright file="Skill.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Aspects
{
    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Common;

    /// <summary>
    /// Represents a skill in Dungeons &amp; Dragons 5th Edition.
    /// </summary>
    public sealed class Skill : ICopyable<Skill>
    {
        /// <summary>
        /// The total number of skills available.
        /// </summary>
        public const int Count = 18;

        /// <summary>
        /// Initializes a new instance of the <see cref="Skill"/> class with default values.
        /// </summary>
        public Skill()
            : this(SkillType.None, false, false, StatusChecks.Auto)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skill"/> class with the specified skill type.
        /// </summary>
        /// <param name="skillType">The type of skill.</param>
        public Skill(SkillType skillType)
            : this(skillType, false, false, StatusChecks.Auto)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skill"/> class with the specified parameters.
        /// </summary>
        /// <param name="skillType">The type of skill.</param>
        /// <param name="proficiency">Indicates whether the character has proficiency in the skill.</param>
        /// <param name="expertise">Indicates whether the character has expertise in the skill.</param>
        /// <param name="skillOverride">Specifies if the skill check has a special status override.</param>
        public Skill(SkillType skillType, bool proficiency, bool expertise, StatusChecks skillOverride)
        {
            this.Type = skillType;
            this.Proficiency = proficiency;
            this.Expertise = expertise;
            this.SkillOverride = skillOverride;
        }

        /// <summary>
        /// Gets the default instance of the <see cref="Skill"/> class.
        /// </summary>
        public static Skill Default => new ();

        /// <summary>
        /// Gets or sets a value indicating whether the character has expertise in the skill.
        /// </summary>
        public bool Expertise { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the character has proficiency in the skill.
        /// </summary>
        public bool Proficiency { get; set; }

        /// <summary>
        /// Gets the type of the skill.
        /// </summary>
        public SkillType Type { get; private set; }

        /// <summary>
        /// Gets or sets the status override for the skill check.
        /// </summary>
        public StatusChecks SkillOverride { get; set; }

        /// <summary>
        /// Creates a deep copy of the skill instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Skill"/> instance.</returns>
        public Skill DeepCopy()
        {
            return new Skill(this.Type, this.Proficiency, this.Expertise, this.SkillOverride);
        }

        /// <summary>
        /// Calculates the bonus for the skill based on the character's attributes and proficiency.
        /// </summary>
        /// <param name="attribute">The attribute associated with the skill.</param>
        /// <param name="proficiency">The character's proficiency bonus.</param>
        /// <returns>The total bonus for the skill.</returns>
        public int GetBonus(int attribute, int proficiency)
        {
            var bonus = 0;

            if (this.Proficiency)
            {
                bonus += proficiency;
            }

            if (this.Expertise)
            {
                bonus += proficiency;
            }

            bonus += ConciergeMath.Bonus(attribute);

            return bonus;
        }

        /// <summary>
        /// Gets the status of the skill check based on the character's vitality.
        /// </summary>
        /// <param name="vitality">The character's vitality.</param>
        /// <returns>The status of the skill check.</returns>
        public StatusChecks GetStatus(Vitality vitality)
        {
            if (this.SkillOverride != StatusChecks.Auto)
            {
                return this.SkillOverride;
            }

            var frightened = vitality.Status.Frightened;
            var poisoned = vitality.Status.Poisoned;
            if (
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion1 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion2 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion3 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion4 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion5 ||
                frightened.Status == ConditionStatus.Afflicted ||
                poisoned.Status == ConditionStatus.Afflicted)
            {
                return StatusChecks.Disadvantage;
            }

            if (vitality.Status.Blinded.Status == ConditionStatus.Afflicted)
            {
                return StatusChecks.Fail;
            }

            return StatusChecks.Normal;
        }
    }
}
