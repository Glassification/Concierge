// <copyright file="CharacterClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System;

    using Concierge.Utility;
    using Concierge.Utility.Utilities;

    public sealed class CharacterClass : ICopyable<CharacterClass>
    {
        private int level;

        public CharacterClass()
        {
            this.level = 0;
            this.Name = string.Empty;
            this.ClassNumber = 0;
        }

        public CharacterClass(int classNumber)
            : this()
        {
            this.ClassNumber = classNumber;
        }

        public string Name { get; set; }

        public int ClassNumber { get; set; }

        public int Level
        {
            get => this.level;
            set
            {
                if (value is <= Constants.MaxLevel and >= 0)
                {
                    if (CharacterUtility.ValidateClassLevel(Program.CcsFile.Character, this.ClassNumber, value))
                    {
                        this.level = value;
                    }
                }
            }
        }

        public CharacterClass DeepCopy()
        {
            return new CharacterClass()
            {
                Name = this.Name,
                Level = this.Level,
                ClassNumber = this.ClassNumber,
            };
        }
    }
}
