// <copyright file="SpellDetails.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    /// <summary>
    /// Represents details about a spell, consisting of a header and a corresponding value.
    /// </summary>
    public sealed class SpellDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellDetails"/> class with the specified header and value.
        /// </summary>
        /// <param name="header">The header of the spell detail.</param>
        /// <param name="value">The value of the spell detail.</param>
        public SpellDetails(string header, string value)
        {
            this.Header = header;
            this.Value = value;
        }

        private SpellDetails()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Gets an empty <see cref="SpellDetails"/> instance.
        /// </summary>
        public static SpellDetails Empty => new ();

        /// <summary>
        /// Gets the header of the spell detail.
        /// </summary>
        public string Header { get; private set; }

        /// <summary>
        /// Gets the value of the spell detail.
        /// </summary>
        public string Value { get; private set; }

        public override string ToString()
        {
            return $"{this.Header} {this.Value}";
        }
    }
}
