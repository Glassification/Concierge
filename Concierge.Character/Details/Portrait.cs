// <copyright file="Portrait.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Windows.Media;

    using Concierge.Common;

    /// <summary>
    /// Represents a portrait image for a character.
    /// </summary>
    public sealed class Portrait : ICopyable<Portrait>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Portrait"/> class with default values.
        /// </summary>
        public Portrait()
        {
            this.Encoded = string.Empty;
            this.Name = string.Empty;
            this.Stretch = Stretch.UniformToFill;
            this.UseCustomImage = false;
        }

        /// <summary>
        /// Gets an empty instance of a Portrait.
        /// </summary>
        public static Portrait Empty => new ();

        /// <summary>
        /// Gets or sets the base64-encoded representation of the image.
        /// </summary>
        public string Encoded { get; set; }

        /// <summary>
        /// Gets or sets the name of the image file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the stretch mode for displaying the image.
        /// </summary>
        public Stretch Stretch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a custom image should be rendered.
        /// </summary>
        public bool UseCustomImage { get; set; }

        /// <summary>
        /// Creates a deep copy of the portrait.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Portrait"/>.</returns>
        public Portrait DeepCopy()
        {
            return new Portrait()
            {
                Encoded = this.Encoded,
                Name = this.Name,
                Stretch = this.Stretch,
                UseCustomImage = this.UseCustomImage,
            };
        }
    }
}
