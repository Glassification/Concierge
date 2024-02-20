// <copyright file="Status.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class Status : ICopyable<Status>
    {
        public Status()
        {
            this.Blinded = new Condition(ConditionDescriptions.Blinded, ConditionTypes.Blinded);
            this.Charmed = new Condition(ConditionDescriptions.Charmed, ConditionTypes.Charmed);
            this.Death = new Condition(ConditionDescriptions.Death, ConditionTypes.Dead);
            this.Deafened = new Condition(ConditionDescriptions.Deafened, ConditionTypes.Deafened);
            this.Frightened = new Condition(ConditionDescriptions.Frightened, ConditionTypes.Frightened);
            this.Grappled = new Condition(ConditionDescriptions.Grappled, ConditionTypes.Grappled);
            this.Incapacitated = new Condition(ConditionDescriptions.Incapacitated, ConditionTypes.Incapacitated);
            this.Invisible = new Condition(ConditionDescriptions.Invisible, ConditionTypes.Invisible);
            this.Paralyzed = new Condition(ConditionDescriptions.Paralyzed, ConditionTypes.Paralyzed);
            this.Petrified = new Condition(ConditionDescriptions.Petrified, ConditionTypes.Petrified);
            this.Poisoned = new Condition(ConditionDescriptions.Poisoned, ConditionTypes.Poisoned);
            this.Prone = new Condition(ConditionDescriptions.Prone, ConditionTypes.Prone);
            this.Restrained = new Condition(ConditionDescriptions.Restrained, ConditionTypes.Restrained);
            this.Stunned = new Condition(ConditionDescriptions.Stunned, ConditionTypes.Stunned);
            this.Unconscious = new Condition(ConditionDescriptions.Unconscious, ConditionTypes.Unconscious);
            this.Exhaustion = new Exhaustion();
            this.StatusEffects = [];
        }

        public Condition Blinded { get; set; }

        public Condition Charmed { get; set; }

        public Condition Death { get; set; }

        public Condition Deafened { get; set; }

        public Exhaustion Exhaustion { get; set; }

        public Condition Frightened { get; set; }

        public Condition Grappled { get; set; }

        public Condition Incapacitated { get; set; }

        public Condition Invisible { get; set; }

        public Condition Paralyzed { get; set; }

        public Condition Petrified { get; set; }

        public Condition Poisoned { get; set; }

        public Condition Prone { get; set; }

        public Condition Restrained { get; set; }

        public Condition Stunned { get; set; }

        public Condition Unconscious { get; set; }

        public List<StatusEffect> StatusEffects { get; set; }

        public List<Condition> ActiveConditions()
        {
            var conditions = new List<Condition>();
            AddIfExists(conditions, this.Blinded);
            AddIfExists(conditions, this.Charmed);
            AddIfExists(conditions, this.Death);
            AddIfExists(conditions, this.Deafened);
            AddIfExists(conditions, this.Frightened);
            AddIfExists(conditions, this.Grappled);
            AddIfExists(conditions, this.Incapacitated);
            AddIfExists(conditions, this.Invisible);
            AddIfExists(conditions, this.Paralyzed);
            AddIfExists(conditions, this.Petrified);
            AddIfExists(conditions, this.Poisoned);
            AddIfExists(conditions, this.Prone);
            AddIfExists(conditions, this.Restrained);
            AddIfExists(conditions, this.Stunned);
            AddIfExists(conditions, this.Unconscious);

            return conditions;
        }

        public Status DeepCopy()
        {
            return new Status()
            {
                Blinded = this.Blinded.DeepCopy(),
                Charmed = this.Charmed.DeepCopy(),
                Death = this.Death.DeepCopy(),
                Deafened = this.Deafened.DeepCopy(),
                Exhaustion = this.Exhaustion.DeepCopy(),
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
                StatusEffects = [.. this.StatusEffects.DeepCopy()],
            };
        }

        private static void AddIfExists(List<Condition> conditions, Condition condition)
        {
            if (condition.IsAfflicted())
            {
                conditions.Add(condition);
            }
        }
    }
}
