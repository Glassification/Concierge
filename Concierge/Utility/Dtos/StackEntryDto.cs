// <copyright file="StackEntryDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Dtos
{
    public class StackEntryDto
    {
        public StackEntryDto(int numberOfCharactersToSkip, bool ignorable)
        {
            this.NumberOfCharactersToSkip = numberOfCharactersToSkip;
            this.Ignorable = ignorable;
        }

        public int NumberOfCharactersToSkip { get; set; }

        public bool Ignorable { get; set; }
    }
}
