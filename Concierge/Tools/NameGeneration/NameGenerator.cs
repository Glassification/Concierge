// <copyright file="NameGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.NameGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Enums;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools.Enums;
    using Concierge.Utility.Extensions;

    public sealed class NameGenerator
    {
        private readonly List<Name> names;
        private readonly Random randomName = new ();

        public NameGenerator()
        {
            this.names = DefaultListReadWriter.ReadJson<Name>(Properties.Resources.Names);
        }

        public string FirstName(GeneratorSettings settings)
        {
            var sublist = this.GetSubList(settings, NameType.First);
            if (sublist.IsEmpty())
            {
                return string.Empty;
            }

            return sublist[this.randomName.Next(0, sublist.Count)].Value;
        }

        public string LastName(GeneratorSettings settings)
        {
            var sublist = this.GetSubList(settings, NameType.Last);
            if (sublist.IsEmpty())
            {
                return string.Empty;
            }

            return sublist[this.randomName.Next(0, sublist.Count)].Value;
        }

        public string FullName(GeneratorSettings settings)
        {
            return $"{this.FirstName(settings)} {this.LastName(settings)}".Trim();
        }

        private List<Name> GetSubList(GeneratorSettings settings, NameType nameType)
        {
            return nameType == NameType.Last ?
                settings.FilterRace ?
                    this.names.Where(x => x.Race.Equals(settings.Race) && x.NameType == nameType).ToList() :
                    this.names.Where(x => x.NameType == nameType).ToList() :
                settings.FilterRace ?
                    settings.FilterGender ?
                        this.names.Where(x => x.Race.Equals(settings.Race) && x.Gender == settings.Gender && x.NameType == nameType).ToList() :
                        this.names.Where(x => x.Race.Equals(settings.Race) && x.NameType == nameType).ToList() :
                    settings.FilterGender ?
                        this.names.Where(x => x.Gender == settings.Gender && x.NameType == nameType).ToList() :
                        this.names.Where(x => x.NameType == nameType).ToList();
        }
    }
}
