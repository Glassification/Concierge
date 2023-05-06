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

    public class Journal : ICopyable<Journal>
    {
        public Journal()
        {
            this.Chapters = new List<Chapter>();
        }

        public List<Chapter> Chapters { get; set; }

        public Chapter GetChapter(Guid documentId)
        {
            return this.Chapters.Single(x => x.Documents.Any(y => y.Id.Equals(documentId)));
        }

        public Journal DeepCopy()
        {
            return new Journal()
            {
                Chapters = this.Chapters.DeepCopy().ToList(),
            };
        }
    }
}
