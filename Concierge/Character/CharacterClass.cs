// <copyright file="CharacterClass.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Common;

    using Concierge.Common.Extensions;

    public sealed class CharacterClass : ICopyable<CharacterClass>
    {
        private int level;

        public CharacterClass()
        {
            this.level = 0;
            this.Name = string.Empty;
            this.Subclass = string.Empty;
            this.ClassNumber = 0;
        }

        public CharacterClass(int classNumber)
            : this()
        {
            this.ClassNumber = classNumber;
        }

        public string Name { get; set; }

        public string Subclass { get; set; }

        public int ClassNumber { get; set; }

        public int Level
        {
            get => this.level;
            set
            {
                if (value is <= Constants.MaxLevel and >= 0)
                {
                    var oldLevel = this.level;
                    this.level = value;
                    if (!Program.CcsFile.Character.ValidateClassLevel(this.ClassNumber))
                    {
                        this.level = oldLevel;
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"{this.Name}{(this.Subclass.IsNullOrWhiteSpace() ? string.Empty : $" ({this.Subclass})")}";
        }

        public CharacterClass DeepCopy()
        {
            return new CharacterClass()
            {
                Name = this.Name,
                Subclass = this.Subclass,
                Level = this.Level,
                ClassNumber = this.ClassNumber,
            };
        }
    }
}
