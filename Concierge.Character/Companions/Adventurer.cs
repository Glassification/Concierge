// <copyright file="Adventurer.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Companions
{
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an adventurer character in a party.
    /// </summary>
    public sealed class Adventurer : ICopyable<Adventurer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Adventurer"/> class with default values.
        /// </summary>
        public Adventurer()
        {
            this.CharacterName = string.Empty;
            this.PlayerName = string.Empty;
            this.Race = string.Empty;
            this.Class = string.Empty;
        }

        /// <summary>
        /// Gets an empty <see cref="Adventurer"/> instance with default values.
        /// </summary>
        public static Adventurer Empty => new ();

        /// <summary>
        /// Gets or sets the name of the player controlling the adventurer.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the adventurer character.
        /// </summary>
        public string CharacterName { get; set; }

        /// <summary>
        /// Gets or sets the race of the adventurer character.
        /// </summary>
        public string Race { get; set; }

        /// <summary>
        /// Gets or sets the class of the adventurer character.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the level of the adventurer character.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the status of the adventurer character.
        /// </summary>
        public PartyStatus Status { get; set; }

        [JsonIgnore]
        public Brush IconColor
        {
            get
            {
                return this.Status switch
                {
                    PartyStatus.Alive => ConciergeBrushes.Mint,
                    PartyStatus.Missing => ConciergeBrushes.Deer,
                    PartyStatus.Dead => Brushes.IndianRed,
                    _ => Brushes.Red,
                };
            }
        }

        /// <summary>
        /// Creates a deep copy of the adventurer instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Adventurer"/> instance.</returns>
        public Adventurer DeepCopy()
        {
            return new Adventurer()
            {
                PlayerName = this.PlayerName,
                CharacterName = this.CharacterName,
                Race = this.Race,
                Class = this.Class,
                Level = this.Level,
                Status = this.Status,
            };
        }

        public override string ToString()
        {
            return $"{this.CharacterName} ({this.PlayerName}) - Level {this.Level} {this.Class}";
        }
    }
}
