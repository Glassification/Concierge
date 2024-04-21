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

    /// <summary>
    /// Represents an abstract attribute in Dungeons &amp; Dragons 5th Edition.
    /// </summary>
    public abstract class Attribute : ICopyable<Attribute>
    {
        /// <summary>
        /// The count of attributes in D&amp;D, which is 6.
        /// </summary>
        public const int Count = 6;

        /// <summary>
        /// The default score for an attribute.
        /// </summary>
        public const int DefaultScore = 10;

        private int score;

        /// <summary>
        /// Gets the default attribute, which is Strength.
        /// </summary>
        public static Attribute Default => new Strength();

        /// <summary>
        /// Gets the bonus associated with the attribute score.
        /// </summary>
        [JsonIgnore]
        public int Bonus => ConciergeMath.Bonus(this.Score);

        /// <summary>
        /// Gets or sets a value indicating whether the character is proficient in this attribute.
        /// </summary>
        public bool Proficiency { get; set; }

        /// <summary>
        /// Gets or sets the override for saving throws related to this attribute.
        /// </summary>
        public StatusChecks SaveOverride { get; set; }

        /// <summary>
        /// Gets or sets the score of the attribute.
        /// </summary>
        public int Score
        {
            get => this.score;
            set => this.score = Math.Clamp(value, Constants.MinScore, Constants.MaxScore);
        }

        /// <summary>
        /// Gets or sets the type of the attribute.
        /// </summary>
        public AttributeType Type { get; set; }

        /// <summary>
        /// Gets the saving throw status based on the vitality.
        /// </summary>
        /// <param name="vitality">The vitality of the character.</param>
        /// <returns>The status of the saving throw.</returns>
        public abstract StatusChecks GetSaveStatus(Vitality vitality);

        /// <summary>
        /// Gets the total saving throw bonus for this attribute.
        /// </summary>
        /// <param name="proficiency">The proficiency bonus of the character.</param>
        /// <returns>The total saving throw bonus.</returns>
        public int GetSaveBonus(int proficiency)
        {
            var bonus = 0;

            if (this.Proficiency)
            {
                bonus += proficiency;
            }

            bonus += ConciergeMath.Bonus(this.Score);

            return bonus;
        }

        /// <summary>
        /// Creates a deep copy of the attribute.
        /// </summary>
        /// <returns>A deep copy of the attribute.</returns>
        public abstract Attribute DeepCopy();
    }
}
