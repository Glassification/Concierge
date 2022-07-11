// <copyright file="CompanionProperties.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class CompanionProperties : ICopyable<CompanionProperties>
    {
        public CompanionProperties()
        {
            this.Name = string.Empty;
            this.ArmorClass = 0;
            this.Movement = 0;
            this.Perception = 0;
            this.CreatureSize = CreatureSizes.Medium;
            this.Vision = VisionTypes.Normal;
        }

        public int ArmorClass { get; set; }

        public CreatureSizes CreatureSize { get; set; }

        public int Movement { get; set; }

        public string Name { get; set; }

        public int Perception { get; set; }

        public VisionTypes Vision { get; set; }

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
            };
        }
    }
}
