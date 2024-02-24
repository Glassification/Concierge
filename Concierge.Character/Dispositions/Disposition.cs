// <copyright file="Disposition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Dispositions
{
    using Concierge.Common;
    using Concierge.Common.Exceptions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the disposition or character traits of a character.
    /// </summary>
    public sealed class Disposition : ICopyable<Disposition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Disposition"/> class with default values.
        /// </summary>
        public Disposition()
        {
            this.Name = string.Empty;
            this.Race = new Race();
            this.Background = string.Empty;
            this.Alignment = string.Empty;
            this.Experience = string.Empty;
            this.Class1 = new Class(1);
            this.Class2 = new Class(2);
            this.Class3 = new Class(3);
        }

        /// <summary>
        /// Gets or sets the alignment of the character.
        /// </summary>
        public string Alignment { get; set; }

        /// <summary>
        /// Gets or sets the background of the character.
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the primary class of the character.
        /// </summary>
        public Class Class1 { get; set; }

        /// <summary>
        /// Gets or sets the secondary class of the character.
        /// </summary>
        public Class Class2 { get; set; }

        /// <summary>
        /// Gets or sets the tertiary class of the character.
        /// </summary>
        public Class Class3 { get; set; }

        /// <summary>
        /// Gets or sets the experience of the character.
        /// </summary>
        public string Experience { get; set; }

        /// <summary>
        /// Gets the total level of the character.
        /// </summary>
        [JsonIgnore]
        public int Level => this.Class1.Level + this.Class2.Level + this.Class3.Level;

        /// <summary>
        /// Gets or sets the name of the character.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the race of the character.
        /// </summary>
        public Race Race { get; set; }

        /// <summary>
        /// Creates a deep copy of the disposition.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Disposition"/>.</returns>
        public Disposition DeepCopy()
        {
            return new Disposition()
            {
                Name = this.Name,
                Race = this.Race.DeepCopy(),
                Background = this.Background,
                Alignment = this.Alignment,
                Experience = this.Experience,
                Class1 = this.Class1.DeepCopy(),
                Class2 = this.Class2.DeepCopy(),
                Class3 = this.Class3.DeepCopy(),
            };
        }

        /// <summary>
        /// Gets the class of the character based on the specified number.
        /// </summary>
        /// <param name="num">The class number (1, 2, or 3).</param>
        /// <returns>The class of the character.</returns>
        public Class GetClass(int num)
        {
            return num switch
            {
                1 => this.Class1,
                2 => this.Class2,
                3 => this.Class3,
                _ => throw new InvalidValueException(num.ToString()),
            };
        }
    }
}
