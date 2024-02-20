// <copyright file="Exhaustion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class Exhaustion : ICopyable<Exhaustion>
    {
        public const string Exausted1 = "Disadvantage on Ability Checks";
        public const string Exausted2 = "Speed halved";
        public const string Exausted3 = "Disadvantage on Attack rolls and Saving Throws";
        public const string Exausted4 = "Hit point maximum halved";
        public const string Exausted5 = "Speed reduced to 0";
        public const string Exausted6 = "Death";

        public Exhaustion()
            : this(string.Empty, ConditionStatus.Normal)
        {
        }

        public Exhaustion(string description)
            : this(description, ConditionStatus.Normal)
        {
        }

        public Exhaustion(string description, ConditionStatus status)
        {
            this.Description = description;
            this.Status = status;
        }

        public string Description { get; set; }

        public ConditionStatus Status { get; set; }

        public string Value
        {
            get
            {
                var description = this.Status switch
                {
                    ConditionStatus.Exhaustion1 => Exausted1,
                    ConditionStatus.Exhaustion2 => $"{Exausted1}, {Exausted2}",
                    ConditionStatus.Exhaustion3 => $"{Exausted1}, {Exausted2}, {Exausted3}",
                    ConditionStatus.Exhaustion4 => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}",
                    ConditionStatus.Exhaustion5 => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}, {Exausted5}",
                    ConditionStatus.Exhaustion6 => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}, {Exausted5}, {Exausted6}",
                    _ => string.Empty,
                };

                return $"{this.Status.ToString().FormatFromPascalCase()} - {this.Description} {description}.";
            }
        }

        public bool IsAfflicted()
        {
            return this.Status != ConditionStatus.Normal;
        }

        public Exhaustion DeepCopy()
        {
            return new Exhaustion(this.Description, this.Status);
        }
    }
}
