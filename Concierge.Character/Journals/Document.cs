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
        private const string EmptyRtf = "{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp\\deff2{\\fonttbl{\\f0\\fcharset0 Times New Roman;}{\\f2\\fcharset0 Calibri;}}{\\colortbl\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\\loch\\hich\\dbch\\pard\\plain\\ltrpar\\itap0{\\lang1033\\fs30\\f2\\cf1 \\cf1\\ql}\r\n}";

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the document.</param>
        public Document(string name)
            : base(name)
        {
            this.Rtf = EmptyRtf;
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
