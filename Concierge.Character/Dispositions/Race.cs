// <copyright file="Race.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Dispositions
{
    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class Race : ICopyable<Race>
    {
        public Race()
        {
            this.Name = string.Empty;
            this.Subrace = string.Empty;
        }

        public string Name { get; set; }

        public string Subrace { get; set; }

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
            return $"{(this.Subrace.IsNullOrWhiteSpace() ? string.Empty : $"{this.Subrace} ")}{this.Name}";
        }

        public bool IsNullOrWhiteSpace()
        {
            return this.Subrace.IsNullOrWhiteSpace() && this.Name.IsNullOrWhiteSpace();
        }
    }
}
