// <copyright file="Proficiency.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a proficiency in a particular armor, tool, or weapon.
    /// </summary>
    public sealed class Proficiency : ICopyable<Proficiency>, IUnique
    {
        public const string MartialMelee = "Martial Melee Weapons";
        public const string MartialRanged = "Martial Ranged Weapons";
        public const string SimpleMelee = "Simple Melee Weapons";
        public const string SimpleRanged = "Simple Ranged Weapons";

        /// <summary>
        /// Initializes a new instance of the <see cref="Proficiency"/> class.
        /// </summary>
        public Proficiency()
        {
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Proficiency"/> class with the specified name and type.
        /// </summary>
        /// <param name="name">The name of the proficiency.</param>
        /// <param name="type">The type of proficiency.</param>
        public Proficiency(string name, ProficiencyTypes type)
        {
            this.Name = name;
            this.ProficiencyType = type;
            this.Created = DateTime.Now;
        }

        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the name of the proficiency.
        /// </summary>
        public string Name { get; set; }

        public bool IsCustom { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Proficiency);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.LightSeaGreen;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.School;

        public ProficiencyTypes ProficiencyType { get; set; }

        public Guid Id { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

        /// <summary>
        /// Creates a deep copy of the <see cref="Proficiency"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Proficiency"/> object.</returns>
        public Proficiency DeepCopy()
        {
            return new Proficiency()
            {
                Name = this.Name,
                ProficiencyType = this.ProficiencyType,
                Id = this.Id,
                IsCustom = this.IsCustom,
                Created = this.Created,
            };
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current proficiency.
        /// </summary>
        /// <param name="obj">The object to compare with the current proficiency.</param>
        /// <returns>True if the specified object is equal to the current proficiency; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Proficiency proficiency)
            {
                return false;
            }

            return proficiency.Name.Equals(this.Name) && proficiency.ProficiencyType == this.ProficiencyType;
        }

        /// <summary>
        /// Gets the category of the proficiency.
        /// </summary>
        /// <returns>The category of the proficiency.</returns>
        public CategoryDto GetCategory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the hash code for the current proficiency.
        /// </summary>
        /// <returns>A hash code for the current proficiency.</returns>
        public override int GetHashCode()
        {
            return $"{this.Name}|{this.ProficiencyType}".GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current proficiency.
        /// </summary>
        /// <returns>A string that represents the current proficiency.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
