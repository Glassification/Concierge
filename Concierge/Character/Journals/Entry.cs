// <copyright file="Entry.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journals
{
    using System;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Attributes;
    using Concierge.Common.Dtos;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public abstract class Entry : IUnique
    {
        public Entry()
            : this(string.Empty)
        {
        }

        public Entry(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Created = ConciergeDateTime.OriginalCreationNow;
        }

        public static Chapter Empty => new (string.Empty);

        public string Created { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Entry);

        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.White;

        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.None;

        public Guid Id { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsCustom { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

        public CategoryDto GetCategory()
        {
            throw new NotImplementedException();
        }
    }
}
