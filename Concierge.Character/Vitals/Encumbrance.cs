// <copyright file="Encumbrance.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using Concierge.Character.Enums;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents the encumbrance status of a character, including its description.
    /// </summary>
    public sealed class Encumbrance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Encumbrance"/> class with the specified condition status.
        /// </summary>
        /// <param name="status">The condition status related to encumbrance.</param>
        public Encumbrance(ConditionStatus status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets the condition status related to encumbrance.
        /// </summary>
        public ConditionStatus Status { get; set; }

        /// <summary>
        /// Gets the value that combines the PascalCase status and its description.
        /// </summary>
        /// <remarks>
        /// The value is formatted as "Status - Description".
        /// </remarks>
        public string Value => $"{this.Status.PascalCase()} - {this.GetDescription()}";

        private string GetDescription()
        {
            return this.Status switch
            {
                ConditionStatus.Encumbered => "A carry weight exceeding 5 times Strength will reduce movement by 10.",
                ConditionStatus.ArmorEncumbered => "Wearing armor with a Strength score exceeding your own will reduce movement by 10.",
                ConditionStatus.HeavilyEncumbered => "A carry weight exceeding 10 times Strength will reduce movement by 20.",
                _ => string.Empty,
            };
        }
    }
}
