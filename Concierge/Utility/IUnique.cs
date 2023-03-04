// <copyright file="IUnique.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;

    using Concierge.Utility.Dtos;

    public interface IUnique
    {
        Guid Id { get; set; }

        string Name { get; set; }

        CategoryDto GetCategory();
    }
}
