// <copyright file="NameGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Enums;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Utility.Extensions;

    public sealed class NameGenerator
    {
        private readonly List<Name> names;
        private readonly Random randomName = new ();

        public NameGenerator()
        {
            this.names = DefaultListReadWriter.ReadJson<Name>(Properties.Resources.Names);
        }

        private int Index { get; set; }

        private bool Sync { get; set; }

        public string FirstName(Gender gender)
        {
            var sublist = gender == Gender.Other ? this.names : this.names.Where(x => x.Gender == gender).ToList();
            if (sublist.IsEmpty())
            {
                return string.Empty;
            }

            var index = this.Index = this.randomName.Next(0, sublist.Count);

            return sublist[index].FirstName;
        }

        public string LastName(Gender gender)
        {
            var sublist = gender == Gender.Other ? this.names : this.names.Where(x => x.Gender == gender).ToList();
            if (sublist.IsEmpty())
            {
                return string.Empty;
            }

            var index = this.Sync ? this.Index : this.randomName.Next(0, sublist.Count);

            return sublist[index].LastName;
        }

        public string FullName(Gender gender, bool isSync = false)
        {
            this.Sync = isSync;
            var fullName = $"{this.FirstName(gender)} {this.LastName(gender)}";
            this.Sync = false;

            return fullName.Trim();
        }
    }
}
