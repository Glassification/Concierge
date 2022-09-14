// <copyright file="ExhaustionCondition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses.ConditionStatus
{
    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class ExhaustionCondition : Condition, ICopyable<ExhaustionCondition>
    {
        public const string Exausted1 = "Disadvantage on Ability Checks";
        public const string Exausted2 = "Speed halved";
        public const string Exausted3 = "Disadvantage on Attack rolls and Saving Throws";
        public const string Exausted4 = "Hit point maximum halved";
        public const string Exausted5 = "Speed reduced to 0";
        public const string Exausted6 = "Death.";

        public ExhaustionCondition()
            : base(string.Empty, string.Empty)
        {
        }

        public ExhaustionCondition(ExhaustionLevel exhaustionLevel, string description, string name)
            : base(description, name)
        {
            this.ExhaustionLevel = exhaustionLevel;
        }

        public ExhaustionLevel ExhaustionLevel { get; set; }

        public override string ToString()
        {
            var exhaustion = this.ExhaustionLevel switch
            {
                ExhaustionLevel.One => "Exhaustion 1",
                ExhaustionLevel.Two => "Exhaustion 2",
                ExhaustionLevel.Three => "Exhaustion 3",
                ExhaustionLevel.Four => "Exhaustion 4",
                ExhaustionLevel.Five => "Exhaustion 5",
                ExhaustionLevel.Six => "Exhaustion 6",
                _ => string.Empty,
            };

            var description = this.ExhaustionLevel switch
            {
                ExhaustionLevel.One => Exausted1,
                ExhaustionLevel.Two => $"{Exausted1}, {Exausted2}",
                ExhaustionLevel.Three => $"{Exausted1}, {Exausted2}, {Exausted3}",
                ExhaustionLevel.Four => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}",
                ExhaustionLevel.Five => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}, {Exausted5}",
                ExhaustionLevel.Six => $"{Exausted1}, {Exausted2}, {Exausted3}, {Exausted4}, {Exausted5}, {Exausted6}",
                _ => string.Empty,
            };

            return $"{exhaustion} - {this.Description} {description}.";
        }

        public ExhaustionCondition DeepCopy()
        {
            return new ExhaustionCondition(this.ExhaustionLevel, this.Description, this.Name);
        }

        public override bool IsAfflicted()
        {
            return this.ExhaustionLevel != ExhaustionLevel.Normal;
        }
    }
}
