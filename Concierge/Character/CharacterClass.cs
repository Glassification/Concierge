// <copyright file="CharacterClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System;

    using Concierge.Utility;

    public class CharacterClass : ICopyable
    {
        private int level;

        public CharacterClass()
        {
            this.level = 0;
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public Guid Id { get; init; }

        public int Level
        {
            get => this.level;
            set
            {
                if (value is <= Constants.MaxLevel and >= 0)
                {
                    if (Utilities.ValidateClassLevel(Program.CcsFile.Character, this.Id, value))
                    {
                        this.level = value;
                    }
                }
            }
        }

        public ICopyable DeepCopy()
        {
            return new CharacterClass()
            {
                Name = this.Name,
                Level = this.Level,
                Id = this.Id,
            };
        }
    }
}
