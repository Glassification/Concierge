﻿// <copyright file="Charisma.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Aspects
{
    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;

    /// <summary>
    /// Represents the Charisma attribute in Dungeons &amp; Dragons 5th Edition.
    /// </summary>
    public sealed class Charisma : Attribute
    {
        public Charisma()
        {
            this.Score = DefaultScore;
            this.Type = AttributeType.Charisma;
        }

        public override Attribute DeepCopy()
        {
            return new Charisma()
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

            if (
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion3 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion4 ||
                vitality.Status.Exhaustion.Status == ConditionStatus.Exhaustion5)
            {
                return StatusChecks.Disadvantage;
            }

            var paraliyzed = vitality.Status.Paralyzed;
            var stunned = vitality.Status.Stunned;
            if (paraliyzed.Status == ConditionStatus.Afflicted || stunned.Status == ConditionStatus.Afflicted)
            {
                return StatusChecks.Fail;
            }

            return StatusChecks.Normal;
        }
    }
}
