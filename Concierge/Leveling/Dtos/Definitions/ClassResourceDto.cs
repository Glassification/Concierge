// <copyright file="ClassResourceDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Definitions
{
    public sealed class ClassResourceDto
    {
        public ClassResourceDto()
            : this(0, string.Empty)
        {
        }

        public ClassResourceDto(int increase, string name)
        {
            this.Increase = increase;
            this.Name = name;
        }

        public int Increase { get; set; }

        public string Name { get; set; }
    }
}
