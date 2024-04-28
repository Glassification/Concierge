// <copyright file="Exhaustion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents the exhaustion condition and its effects.
    /// </summary>
    public sealed class Exhaustion : ICopyable<Exhaustion>
    {
        public const string Exausted1 = "Disadvantage on Ability Checks";
        public const string Exausted2 = "Speed halved";
        public const string Exausted3 = "Disadvantage on Attack rolls and Saving Throws";
        public const string Exausted4 = "Hit point maximum halved";
        public const string Exausted5 = "Speed reduced to 0";
        public const string Exausted6 = "Death";

        /// <summary>
        /// Initializes a new instance of the <see cref="Exhaustion"/> class.
        /// </summary>
        public Exhaustion()
            : this(string.Empty, ConditionStatus.Normal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exhaustion"/> class with the specified description.
        /// </summary>
        /// <param name="description">The description of the exhaustion condition.</param>
        public Exhaustion(string description)
            : this(description, ConditionStatus.Normal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exhaustion"/> class with the specified description and status.
        /// </summary>
        /// <param name="description">The description of the exhaustion condition.</param>
        /// <param name="status">The status of the exhaustion condition.</param>
        public Exhaustion(string description, ConditionStatus status)
        {
            this.Description = description;
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets the description of the exhaustion condition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status of the exhaustion condition.
        /// </summary>
        public ConditionStatus Status { get; set; }

        /// <summary>
        /// Gets the value of the exhaustion condition.
        /// </summary>
        public string Value
        {
            get
            {
                var description = this.Status switch
                {
                    ConditionStatus.Exhaustion1 => Exausted1,
                    ConditionStatus.Exhaustion2 => $"{Exausted1}, {Exausted2}",
                    ConditionStatus.Exhaustion3 => $"{Exausted1}, {Exausted2}, {Exausted3}",
                    ConditionStatus.Exhaustion4 => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}",
                    ConditionStatus.Exhaustion5 => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}, {Exausted5}",
                    ConditionStatus.Exhaustion6 => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}, {Exausted5}, {Exausted6}",
                    _ => string.Empty,
                };

                return $"{this.Status.PascalCase()} - {this.Description} {description}.";
            }
        }

        /// <summary>
        /// Checks if the creature is afflicted with exhaustion.
        /// </summary>
        /// <returns><c>true</c> if the creature is afflicted with exhaustion; otherwise, <c>false</c>.</returns>
        public bool IsAfflicted()
        {
            return this.Status != ConditionStatus.Normal;
        }

        /// <summary>
        /// Creates a deep copy of the exhaustion condition.
        /// </summary>
        /// <returns>A new instance of the <see cref="Exhaustion"/> class with the same property values as the original.</returns>
        public Exhaustion DeepCopy()
        {
            return new Exhaustion(this.Description, this.Status);
        }
    }
}
