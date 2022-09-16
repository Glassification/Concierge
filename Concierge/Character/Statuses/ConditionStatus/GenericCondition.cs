// <copyright file="GenericCondition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses.ConditionStatus
{
    using Concierge.Utility;

    public sealed class GenericCondition : Condition, ICopyable<GenericCondition>
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

        public override string Value => $"{this.Name} - {this.Description}";

        public bool Afflicted { get; set; }

        public override bool IsAfflicted()
        {
            return this.Afflicted;
        }

        public GenericCondition DeepCopy()
        {
            return new GenericCondition(this.Afflicted, this.Description, this.Name);
        }
    }
}
