// <copyright file="CompanionProperties.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    /// <summary>
    /// Represents the properties of a companion character.
    /// </summary>
    public sealed class CompanionProperties : ICopyable<CompanionProperties>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanionProperties"/> class with default properties.
        /// </summary>
        public CompanionProperties()
        {
            this.Name = string.Empty;
            this.ArmorClass = 0;
            this.Movement = 0;
            this.Perception = 0;
            this.CreatureSize = CreatureSizes.Medium;
            this.Vision = VisionTypes.Normal;
            this.Initiative = 0;
        }

        /// <summary>
        /// Gets or sets the name of the companion.
        /// </summary>
        public int ArmorClass { get; set; }

        /// <summary>
        /// Gets or sets the armor class of the companion.
        /// </summary>
        public CreatureSizes CreatureSize { get; set; }

        /// <summary>
        /// Gets or sets the movement speed of the companion.
        /// </summary>
        public int Initiative { get; set; }

        /// <summary>
        /// Gets or sets the perception score of the companion.
        /// </summary>
        public int Movement { get; set; }

        /// <summary>
        /// Gets or sets the size of the companion creature.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the vision type of the companion.
        /// </summary>
        public int Perception { get; set; }

        /// <summary>
        /// Gets or sets the initiative score of the companion.
        /// </summary>
        public VisionTypes Vision { get; set; }

        /// <summary>
        /// Creates a deep copy of the companion properties instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="CompanionProperties"/> instance.</returns>
        public CompanionProperties DeepCopy()
        {
            return new CompanionProperties()
            {
                Name = this.Name,
                ArmorClass = this.ArmorClass,
                Movement = this.Movement,
                Perception = this.Perception,
                CreatureSize = this.CreatureSize,
                Vision = this.Vision,
                Initiative = this.Initiative,
            };
        }
    }
}
