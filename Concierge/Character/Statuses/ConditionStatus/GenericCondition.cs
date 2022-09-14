// <copyright file="GenericCondition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses.ConditionStatus
{
    using Concierge.Utility;

    public class GenericCondition : Condition, ICopyable<GenericCondition>
    {
        public GenericCondition()
            : base(string.Empty, string.Empty)
        {
        }

        public GenericCondition(bool afflicted, string description, string name)
            : base(description, name)
        {
            this.Afflicted = afflicted;
        }

        public bool Afflicted { get; set; }

        public override string ToString()
        {
            return $"{this.Name} - {this.Description}";
        }

        public GenericCondition DeepCopy()
        {
            return new GenericCondition(this.Afflicted, this.Description, this.Name);
        }

        public override bool IsAfflicted()
        {
            return this.Afflicted;
        }
    }
}
