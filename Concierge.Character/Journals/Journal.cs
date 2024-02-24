// <copyright file="Journal.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents a journal.
    /// </summary>
    public sealed class Journal : ICopyable<Journal>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Journal"/> class.
        /// </summary>
        public Journal()
        {
            this.Chapters = [];
        }

        /// <summary>
        /// Gets or sets the chapters in the journal.
        /// </summary>
        public List<Chapter> Chapters { get; set; }

        /// <summary>
        /// Gets the chapter associated with the specified document identifier.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <returns>The chapter containing the specified document.</returns>
        public Chapter GetChapter(Guid documentId)
        {
            return this.Chapters.Single(x => x.Documents.Any(y => y.Id.Equals(documentId)));
        }

        /// <summary>
        /// Creates a deep copy of the journal.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Journal"/>.</returns>
        public Journal DeepCopy()
        {
            return new Journal()
            {
                Chapters = [.. this.Chapters.DeepCopy()],
            };
        }
    }
}
