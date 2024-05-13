// <copyright file="Status.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents the status conditions and effects applied to a character.
    /// </summary>
    public sealed class Status : ICopyable<Status>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Status"/> class with default values.
        /// </summary>
        public Status()
        {
            this.Blinded = new Condition(ConditionTypes.Blinded);
            this.Charmed = new Condition(ConditionTypes.Charmed);
            this.Death = new Condition(ConditionTypes.Dead);
            this.Deafened = new Condition(ConditionTypes.Deafened);
            this.Frightened = new Condition(ConditionTypes.Frightened);
            this.Grappled = new Condition(ConditionTypes.Grappled);
            this.Incapacitated = new Condition(ConditionTypes.Incapacitated);
            this.Invisible = new Condition(ConditionTypes.Invisible);
            this.Paralyzed = new Condition(ConditionTypes.Paralyzed);
            this.Petrified = new Condition(ConditionTypes.Petrified);
            this.Poisoned = new Condition(ConditionTypes.Poisoned);
            this.Prone = new Condition(ConditionTypes.Prone);
            this.Restrained = new Condition(ConditionTypes.Restrained);
            this.Stunned = new Condition(ConditionTypes.Stunned);
            this.Unconscious = new Condition(ConditionTypes.Unconscious);
            this.Exhaustion = new Exhaustion();
            this.StatusEffects = [];
        }

        /// <summary>
        /// Gets or sets the blinded condition.
        /// </summary>
        public Condition Blinded { get; set; }

        /// <summary>
        /// Gets or sets the charmed condition.
        /// </summary>
        public Condition Charmed { get; set; }

        /// <summary>
        /// Gets or sets the death condition.
        /// </summary>
        public Condition Death { get; set; }

        /// <summary>
        /// Gets or sets the deafened condition.
        /// </summary>
        public Condition Deafened { get; set; }

        /// <summary>
        /// Gets or sets the exhaustion condition.
        /// </summary>
        public Exhaustion Exhaustion { get; set; }

        /// <summary>
        /// Gets or sets the frightened condition.
        /// </summary>
        public Condition Frightened { get; set; }

        /// <summary>
        /// Gets or sets the grappled condition.
        /// </summary>
        public Condition Grappled { get; set; }

        /// <summary>
        /// Gets or sets the incapacitated condition.
        /// </summary>
        public Condition Incapacitated { get; set; }

        /// <summary>
        /// Gets or sets the invisible condition.
        /// </summary>
        public Condition Invisible { get; set; }

        /// <summary>
        /// Gets or sets the paralyzed condition.
        /// </summary>
        public Condition Paralyzed { get; set; }

        /// <summary>
        /// Gets or sets the petrified condition.
        /// </summary>
        public Condition Petrified { get; set; }

        /// <summary>
        /// Gets or sets the poisoned condition.
        /// </summary>
        public Condition Poisoned { get; set; }

        /// <summary>
        /// Gets or sets the prone condition.
        /// </summary>
        public Condition Prone { get; set; }

        /// <summary>
        /// Gets or sets the restrained condition.
        /// </summary>
        public Condition Restrained { get; set; }

        /// <summary>
        /// Gets or sets the stunned condition.
        /// </summary>
        public Condition Stunned { get; set; }

        /// <summary>
        /// Gets or sets the unconscious condition.
        /// </summary>
        public Condition Unconscious { get; set; }

        /// <summary>
        /// Gets or sets the list of status effects.
        /// </summary>
        public List<StatusEffect> StatusEffects { get; set; }

        /// <summary>
        /// Retrieves a list of active conditions.
        /// </summary>
        /// <returns>A list of active conditions.</returns>
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

        /// <summary>
        /// Creates a deep copy of the status object.
        /// </summary>
        /// <returns>A new instance of the <see cref="Status"/> class with the same property values as the original.</returns>
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
