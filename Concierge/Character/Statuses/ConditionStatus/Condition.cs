// <copyright file="Condition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses.ConditionStatus
{
    public abstract class Condition
    {
        public Condition()
            : this(string.Empty, string.Empty)
        {
        }

        public Condition(string description, string name)
        {
            this.Description = description;
            this.Name = name;
        }

        public string Description { get; set; }

        public string Name { get; set; }

        public abstract string Value { get; }

        public abstract bool IsAfflicted();
    }
}
