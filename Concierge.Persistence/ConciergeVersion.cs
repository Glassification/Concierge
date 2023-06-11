// <copyright file="ConciergeVersion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;

    using Concierge.Common.Exceptions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a version number in the format "Major.Minor.Patch".
    /// </summary>
    public sealed class ConciergeVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConciergeVersion"/> class with default values.
        /// </summary>
        public ConciergeVersion()
        {
            this.Major = 0;
            this.Minor = 0;
            this.Patch = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciergeVersion"/> class
        /// with the specified version number.
        /// </summary>
        /// <param name="version">The version number in the format "Major.Minor.Patch".</param>
        /// <exception cref="InvalidValueException">Thrown when the version number is invalid.</exception>
        public ConciergeVersion(string version)
        {
            var tokens = version.Split('.');
            if (tokens.Length < 3)
            {
                throw new InvalidValueException(nameof(version));
            }

            this.Major = int.Parse(tokens[0]);
            this.Minor = int.Parse(tokens[1]);
            this.Patch = int.Parse(tokens[2]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciergeVersion"/> class
        /// with the specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="version">The <see cref="Version"/> object representing the version number.</param>
        public ConciergeVersion(Version version)
        {
            this.Major = version.Major;
            this.Minor = version.Minor;
            this.Patch = version.Build;
        }

        /// <summary>
        /// Gets or sets the major version number.
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// Gets or sets the minor version number.
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// Gets or sets the patch version number.
        /// </summary>
        public int Patch { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ConciergeVersion"/> is empty.
        /// </summary>
        /// <value><c>true</c> if the current <see cref="ConciergeVersion"/> is empty; otherwise, <c>false</c>.</value>
        [JsonIgnore]
        public bool IsEmpty => this.Major == 0 && this.Minor == 0 && this.Patch == 0;

        public override string ToString()
        {
            return $"{this.Major}.{this.Minor}.{this.Patch}";
        }
    }
}
