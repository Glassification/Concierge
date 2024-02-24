// <copyright file="Constitution.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Aspects
{
    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;

    /// <summary>
    /// Represents the Constitution attribute in Dungeons &amp; Dragons 5th Edition.
    /// </summary>
    public sealed class Constitution : Attribute
    {
        public Constitution()
        {
            this.Score = DefaultScore;
            this.Type = AttributeType.Constitution;
        }

        public override Attribute DeepCopy()
        {
            return new Constitution()
            {
                Proficiency = this.Proficiency,
                Score = this.Score,
                Type = this.Type,
            };
        }

        public override StatusChecks GetSaveStatus(Vitality vitality)
        {
            if (this.SaveOverride != StatusChecks.Auto)
            {
                return this.SaveOverride;
            }

            if (vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion3 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion4 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion5)
            {
                return StatusChecks.Disadvantage;
            }

            return StatusChecks.Normal;
        }
    }
}
