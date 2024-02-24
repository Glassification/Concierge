// <copyright file="Chapter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journals
{
    using System.Collections.Generic;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents a chapter in a document or book.
    /// </summary>
    public sealed class Chapter : Entry, ICopyable<Chapter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Chapter"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the chapter.</param>
        public Chapter(string name)
            : base(name)
        {
            this.Documents = [];
        }

        /// <summary>
        /// Gets or sets the list of documents contained within the chapter.
        /// </summary>
        public List<Document> Documents { get; set; }

        /// <summary>
        /// Creates a deep copy of the chapter instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Chapter"/> instance.</returns>
        public Chapter DeepCopy()
        {
            return new Chapter(this.Name)
            {
                Documents = [.. this.Documents.DeepCopy()],
                IsExpanded = this.IsExpanded,
                Id = this.Id,
                Created = this.Created,
            };
        }

        /// <summary>
        /// Returns a string that represents the current chapter.
        /// </summary>
        /// <returns>The name of the chapter.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
