// <copyright file="Skill.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Aspects
{
    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Common;

    public sealed class Skill : ICopyable<Skill>
    {
        public const int Count = 18;

        public Skill()
            : this(SkillType.None, false, false, StatusChecks.Auto)
        {
        }

        public Skill(SkillType skillType)
            : this(skillType, false, false, StatusChecks.Auto)
        {
        }

        public Skill(SkillType skillType, bool proficiency, bool expertise, StatusChecks skillOverride)
        {
            this.Type = skillType;
            this.Proficiency = proficiency;
            this.Expertise = expertise;
            this.SkillOverride = skillOverride;
        }

        public static Skill Default => new ();

        public bool Expertise { get; set; }

        public bool Proficiency { get; set; }

        public SkillType Type { get; private set; }

        public StatusChecks SkillOverride { get; set; }

        public Skill DeepCopy()
        {
            return new Skill(this.Type, this.Proficiency, this.Expertise, this.SkillOverride);
        }

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

            bonus += Constants.Bonus(attribute);

            return bonus;
        }

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
