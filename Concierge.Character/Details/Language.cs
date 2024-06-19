// <copyright file="Language.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using System;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a language spoken by characters or entities.
    /// </summary>
    public sealed class Language : ICopyable<Language>, IUnique
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        public Language()
        {
            this.Name = string.Empty;
            this.Script = string.Empty;
            this.Speakers = string.Empty;
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
        }

        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the script the language is written in.
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the types of speakers of the language.
        /// </summary>
        public string Speakers { get; set; }

        public bool IsCustom { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Language);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.Orchid;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.Translate;

        public Guid Id { get; set; }

        [JsonIgnore]
        public string Description => $"{this.Name}{(IsValid(this.Script) ? $" ({this.Script})" : string.Empty)} - Spoken by: {this.Speakers}";

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

        /// <summary>
        /// Creates a deep copy of the <see cref="Language"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Language"/> object.</returns>
        public Language DeepCopy()
        {
            return new Language()
            {
                Name = this.Name,
                Script = this.Script,
                Speakers = this.Speakers,
                Id = this.Id,
                IsCustom = this.IsCustom,
                Created = this.Created,
            };
        }

        /// <summary>
        /// Returns a string representation of the language.
        /// </summary>
        /// <returns>The name of the language.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Gets the category information of the language.
        /// </summary>
        /// <returns>The category information of the language.</returns>
        public CategoryDto GetCategory()
        {
            return new CategoryDto(PackIconKind.Translate, Brushes.LightBlue, this.Name);
        }

        private static bool IsValid(string value)
        {
            return !value.IsNullOrWhiteSpace() && !value.Equals("-") && !value.Equals("--");
        }
    }
}
