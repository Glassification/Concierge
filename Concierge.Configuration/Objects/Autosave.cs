// <copyright file="Autosave.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    /// <summary>
    /// Represents autosave settings, including whether autosave is enabled and the autosave interval.
    /// </summary>
    public class Autosave
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Autosave"/> class with default settings.
        /// </summary>
        public Autosave()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether autosave is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the interval (in minutes) for autosave.
        /// </summary>
        public int Interval { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Autosave autosave)
            {
                return false;
            }

            return autosave.Enabled == this.Enabled && autosave.Interval == this.Interval;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Sets the properties of the current <see cref="Autosave"/> instance with the values of another <see cref="Autosave"/> instance.
        /// </summary>
        /// <param name="autosave">The <see cref="Autosave"/> object from which to copy the properties.</param>
        public void Set(Autosave autosave)
        {
            this.Enabled = autosave.Enabled;
            this.Interval = autosave.Interval;
        }
    }
}
