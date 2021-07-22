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
            this.Id = Guid.NewGuid();
        }

        public string Type { get; set; }

        public int Total { get; set; }

        public int Spent { get; set; }

        public string Description => $"{this.Type} - {this.Spent}/{this.Total} Used";

        public Guid Id { get; private set; }
    }
}
