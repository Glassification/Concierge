// <copyright file="Document.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journals
{
    using Concierge.Common;

    /// <summary>
    /// Represents a document.
    /// </summary>
    public sealed class Document : Entry, ICopyable<Document>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the document.</param>
        public Document(string name)
            : base(name)
        {
            this.Rtf = string.Empty;
        }

        /// <summary>
        /// Gets or sets the Rich Text Format (RTF) content of the document.
        /// </summary>
        public string Rtf { get; set; }

        /// <summary>
        /// Creates a deep copy of the document instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Document"/> instance.</returns>
        public Document DeepCopy()
        {
            return new Document(this.Name)
            {
                IsExpanded = this.IsExpanded,
                Rtf = this.Rtf,
                Id = this.Id,
                Created = this.Created,
            };
        }

        /// <summary>
        /// Returns a string that represents the current document.
        /// </summary>
        /// <returns>The name of the document.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
