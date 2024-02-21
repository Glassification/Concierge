// <copyright file="Attribute.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Aspects
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Newtonsoft.Json;

    public abstract class Attribute : ICopyable<Attribute>
    {
        public const int Count = 6;
        public const int DefaultScore = 10;

        private int score;

        public static Attribute Default => new Strength();

        [JsonIgnore]
        public int Bonus => Constants.Bonus(this.Score);

        public bool Proficiency { get; set; }

        public StatusChecks SaveOverride { get; set; }

        public int Score
        {
            get => this.score;
            set => this.score = Truncate(value);
        }

        public AttributeType Type { get; set; }

        public abstract StatusChecks GetSaveStatus(Vitality vitality);

        public int GetSaveBonus(int proficiency)
        {
            var bonus = 0;

            if (this.Proficiency)
            {
                bonus += proficiency;
            }

            bonus += Constants.Bonus(this.Score);

            return bonus;
        }

        public abstract Attribute DeepCopy();

        protected static int Truncate(int value)
        {
            return Math.Min(Constants.MaxScore, Math.Max(Constants.MinScore, value));
        }
    }
}
