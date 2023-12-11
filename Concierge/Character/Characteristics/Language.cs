// <copyright file="Language.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public sealed class Language : ICopyable<Language>, IUnique
    {
        public Language()
        {
            this.Name = string.Empty;
            this.Script = string.Empty;
            this.Speakers = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string Script { get; set; }

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
        public string Description => $"{this.Name}{(IsValid(this.Script) ? $" ({this.Script})" : string.Empty)}, Spoken by: {this.Speakers}";

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

        public Language DeepCopy()
        {
            return new Language()
            {
                Name = this.Name,
                Script = this.Script,
                Speakers = this.Speakers,
                Id = this.Id,
                IsCustom = this.IsCustom,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }

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
