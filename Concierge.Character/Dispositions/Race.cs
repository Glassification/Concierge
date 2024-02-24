// <copyright file="Race.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Dispositions
{
    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents a character race.
    /// </summary>
    public sealed class Race : ICopyable<Race>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Race"/> class with default values.
        /// </summary>
        public Race()
        {
            this.Name = string.Empty;
            this.Subrace = string.Empty;
        }

        /// <summary>
        /// Gets or sets the name of the race.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subrace of the race.
        /// </summary>
        public string Subrace { get; set; }

        /// <summary>
        /// Creates a deep copy of the race.
        /// </summary>
        /// <returns>A deep copy of the race.</returns>
        public Race DeepCopy()
        {
            return new Race()
            {
                Name = this.Name,
                Subrace = this.Subrace,
            };
        }

        /// <summary>
        /// Returns a string representation of the race.
        /// </summary>
        /// <returns>A string representation of the race.</returns>
        public override string ToString()
        {
            return $"{(this.Subrace.IsNullOrWhiteSpace() ? string.Empty : $"{this.Subrace} ")}{this.Name}";
        }

        /// <summary>
        /// Determines whether the current race is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <returns><c>true</c> if the current race is null, empty, or consists only of white-space characters; otherwise, <c>false</c>.</returns>
        public bool IsNullOrWhiteSpace()
        {
            return this.Subrace.IsNullOrWhiteSpace() && this.Name.IsNullOrWhiteSpace();
        }
    }
}
