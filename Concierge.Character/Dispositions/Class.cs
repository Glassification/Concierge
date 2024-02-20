// <copyright file="Class.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Dispositions
{
    using Concierge.Common;

    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class Class
    {
        private int level;

        public Class()
        {
            this.level = 0;
            this.Name = string.Empty;
            this.Subclass = string.Empty;
            this.ClassNumber = 0;
        }

        public Class(int classNumber)
            : this()
        {
            this.ClassNumber = classNumber;
        }

        public string Name { get; set; }

        public string Subclass { get; set; }

        public int ClassNumber { get; set; }

        public int Level
        {
            get => this.level;
            set
            {
                if (value is <= Constants.MaxLevel and >= 0)
                {
                    this.level = value;
                }
            }
        }

        [JsonIgnore]
        public bool IsValid => this.Level > 0;

        public override string ToString()
        {
            return $"{this.Name}{(this.Subclass.IsNullOrWhiteSpace() ? string.Empty : $" ({this.Subclass})")}";
        }

        public Class DeepCopy()
        {
            return new Class()
            {
                Name = this.Name,
                Subclass = this.Subclass,
                Level = this.Level,
                ClassNumber = this.ClassNumber,
            };
        }
    }
}
