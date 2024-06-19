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

    /// <summary>
    /// Represents an entry.
    /// </summary>
    public abstract class Entry : IUnique
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class with default values.
        /// </summary>
        public Entry()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the entry.</param>
        public Entry(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Created = DateTime.Now;
        }

        /// <summary>
        /// Gets an empty <see cref="Chapter"/>.
        /// </summary>
        public static Chapter Empty => new (string.Empty);

        public DateTime Created { get; set; }

        /// <summary>
        /// Gets the custom type of the entry.
        /// </summary>
        [JsonIgnore]
        [SearchIgnore]
        public string CustomType => nameof(Entry);

        /// <summary>
        /// Gets the custom type color of the entry.
        /// </summary>
        [JsonIgnore]
        [SearchIgnore]
        public Brush CustomTypeColor => Brushes.White;

        /// <summary>
        /// Gets the custom type icon of the entry.
        /// </summary>
        [JsonIgnore]
        [SearchIgnore]
        public PackIconKind CustomTypeIcon => PackIconKind.None;

        /// <summary>
        /// Gets or sets the unique identifier of the entry.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entry is expanded in a tree view.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entry is custom item.
        /// </summary>
        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the information of the entry.
        /// </summary>
        [JsonIgnore]
        [SearchIgnore]
        public string Information => $"";

        /// <summary>
        /// Gets the category of the entry.
        /// </summary>
        /// <returns>A <see cref="CategoryDto"/>.</returns>
        public CategoryDto GetCategory()
        {
            throw new NotImplementedException();
        }
    }
}
