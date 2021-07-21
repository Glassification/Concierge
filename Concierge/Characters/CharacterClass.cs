// <copyright file="CharacterClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    using System;

    using Concierge.Utility;

    public class CharacterClass
    {
        private int level;

        public CharacterClass()
        {
            this.level = 0;
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public Guid Id { get; private set; }

        public int Level
        {
            get => this.level;
            set
            {
                if (value <= Constants.MaxLevel && value >= 0)
                {
                    if (Utilities.ValidateClassLevel(Program.CcsFile.Character, this.Id, value))
                    {
                        this.level = value;
                    }
                }
            }
        }
    }
}
