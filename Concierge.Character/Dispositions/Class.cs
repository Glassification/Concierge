// <copyright file="Class.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Dispositions
{
    using Concierge.Common;

    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a character class.
    /// </summary>
    public sealed class Class
    {
        private int level;

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class with default values.
        /// </summary>
        public Class()
        {
            this.level = 0;
            this.Name = string.Empty;
            this.Subclass = string.Empty;
            this.ClassNumber = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Class"/> class with the specified class number.
        /// </summary>
        /// <param name="classNumber">The class number.</param>
        public Class(int classNumber)
            : this()
        {
            this.ClassNumber = classNumber;
        }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subclass of the class.
        /// </summary>
        public string Subclass { get; set; }

        /// <summary>
        /// Gets or sets the class number. Should be a value between 1 and 3.
        /// </summary>
        public int ClassNumber { get; set; }

        /// <summary>
        /// Gets or sets the level of the class.
        /// </summary>
        public int Level
        {
            get => this.level;
            set
            {
                if (value is <= Constants.MaxLevel and >= 0)
                {
                    this.level = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the class is valid (i.e., has a level greater than zero).
        /// </summary>
        [JsonIgnore]
        public bool IsValid => this.Level > 0;

        /// <summary>
        /// Returns a string representation of the class.
        /// </summary>
        /// <returns>A string representation of the class.</returns>
        public override string ToString()
        {
            return $"{this.Name}{(this.Subclass.IsNullOrWhiteSpace() ? string.Empty : $" ({this.Subclass})")}";
        }

        /// <summary>
        /// Creates a deep copy of the class.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Class"/>.</returns>
        public Class DeepCopy()
        {
            return new Class()
            {
                Name = this.Name,
                Subclass = this.Subclass,
                Level = this.Level,
                ClassNumber = this.ClassNumber,
            };
        }
    }
}
