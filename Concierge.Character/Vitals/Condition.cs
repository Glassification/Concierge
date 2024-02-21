// <copyright file="Condition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    public class Condition : ICopyable<Condition>
    {
        public Condition()
            : this(string.Empty, ConditionTypes.None, ConditionStatus.Normal)
        {
        }

        public Condition(string description, ConditionTypes type)
            : this(description, type, ConditionStatus.Normal)
        {
        }

        public Condition(string description, ConditionTypes type, ConditionStatus status)
        {
            this.Description = description;
            this.Status = status;
            this.Type = type;
        }

        public ConditionStatus Status { get; set; }

        public ConditionTypes Type { get; set; }

        public string Description { get; set; }

        public string Name => this.Type.ToString();

        public string Value => $"{this.Name} - {this.Description}";

        public Condition DeepCopy()
        {
            return new Condition(this.Description, this.Type, this.Status);
        }

        public bool IsAfflicted()
        {
            return this.Status == ConditionStatus.Afflicted;
        }
    }
}
