// <copyright file="Document.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Notes
{
    using System;
    using System.Windows.Media;

    using Concierge.Utility;
    using MaterialDesignThemes.Wpf;

    public sealed class Document : ICopyable<Document>, IUnique
    {
        public Document(string name)
        {
            this.Name = name;
            this.Id = Guid.NewGuid();
            this.Rtf = string.Empty;
        }

        public bool IsExpanded { get; set; }

        public string Name { get; set; }

        public string Rtf { get; set; }

        public Guid Id { get; set; }

        public Document DeepCopy()
        {
            return new Document(this.Name)
            {
                IsExpanded = this.IsExpanded,
                Rtf = this.Rtf,
                Id = this.Id,
            };
        }

        public (PackIconKind IconKind, Brush Brush, string Name) GetCategory()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
