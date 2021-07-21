// <copyright file="ClassResource.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Status
{
    using System;

    public class ClassResource
    {
        public ClassResource()
        {
        }

        public string Type { get; set; }

        public int Total { get; set; }

        public int Spent { get; set; }

        public Guid Id { get; private set; }
    }
}
