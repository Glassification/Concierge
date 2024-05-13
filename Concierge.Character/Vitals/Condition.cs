// <copyright file="Condition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a condition that can affect a character or creature.
    /// </summary>
    public class Condition : ICopyable<Condition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class with default values.
        /// </summary>
        public Condition()
            : this(ConditionTypes.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class with the specified description and type, and default status.
        /// </summary>
        /// <param name="type">The type of the condition.</param>
        public Condition(ConditionTypes type)
        {
            this.Description = ConditionDescriptions.Get(type);
            this.Status = ConditionStatus.Normal;
            this.Type = type;
        }

        private Condition(string description, ConditionTypes type, ConditionStatus status)
        {
            this.Description = description;
            this.Status = status;
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the status of the condition.
        /// </summary>
        public ConditionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the type of the condition.
        /// </summary>
        public ConditionTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the description of the condition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the name of the condition.
        /// </summary>
        [JsonIgnore]
        public string Name => this.Type.ToString();

        /// <summary>
        /// Gets the value of the condition.
        /// </summary>
        [JsonIgnore]
        public string Value => $"{this.Name} - {this.Description}";

        /// <summary>
        /// Creates a deep copy of the condition.
        /// </summary>
        /// <returns>A new instance of the <see cref="Condition"/> class with the same property values.</returns>
        public Condition DeepCopy()
        {
            return new Condition(this.Description, this.Type, this.Status);
        }

        /// <summary>
        /// Determines whether the condition is afflicted.
        /// </summary>
        /// <returns><c>true</c> if the condition status is <see cref="ConditionStatus.Afflicted"/>, otherwise <c>false</c>.</returns>
        public bool IsAfflicted()
        {
            return this.Status == ConditionStatus.Afflicted;
        }
    }
}
