// <copyright file="NameGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Names
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools.Enums;
    using Concierge.Utility.Extensions;

    public sealed class NameGenerator : IGenerator
    {
        private readonly List<Name> names;
        private readonly Random randomName = new ();

        public NameGenerator()
        {
            this.names = DefaultListReadWriter.ReadJson<Name>(Properties.Resources.Names);
        }

        public IGeneratorResult Generate(IGeneratorSettings generatorSettings)
        {
            if (generatorSettings is NameSettings nameSettings)
            {
                var firstName = this.GetName(nameSettings, NameType.First);
                var lastName = this.GetName(nameSettings, NameType.Last);

                return new NameResult(firstName, lastName);
            }

            return new NameResult();
        }

        private string GetName(NameSettings settings, NameType nameType)
        {
            var sublist = this.GetSubList(settings, nameType);
            if (sublist.IsEmpty())
            {
                return string.Empty;
            }

            return sublist[this.randomName.Next(0, sublist.Count)].Value;
        }

        private List<Name> GetSubList(NameSettings settings, NameType nameType)
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
