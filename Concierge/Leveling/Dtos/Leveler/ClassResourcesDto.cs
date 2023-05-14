// <copyright file="ClassResourcesDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Leveler
{
    using Concierge.Character.Vitals;

    public sealed class ClassResourcesDto
    {
        public ClassResourcesDto(ClassResource? oldResource, ClassResource? newResource)
        {
            this.Old = oldResource;
            this.New = newResource;
        }

        public ClassResource? Old { get; set; }

        public ClassResource? New { get; set; }
    }
}
