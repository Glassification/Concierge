// <copyright file="Race.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System.Text.RegularExpressions;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public class Race : ICopyable<Race>
    {
        public Race()
        {
            this.Name = string.Empty;
            this.Subrace = string.Empty;
        }

        public string Name { get; set; }

        public string Subrace { get; set; }

        public static string FormatSubRace(string subrace)
        {
            return Regex.Replace(subrace, @"\((.*?)\)", string.Empty).Trim(new char[] { '-', ' ' });
        }

        public Race DeepCopy()
        {
            return new Race()
            {
                Name = this.Name,
                Subrace = this.Subrace,
            };
        }

        public override string ToString()
        {
            return $"{this.Name}{(this.Subrace.IsNullOrWhiteSpace() ? string.Empty : $" ({this.Subrace})")}";
        }
    }
}
