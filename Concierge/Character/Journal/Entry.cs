// <copyright file="Entry.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Journal
{
    using System;
    using System.Windows.Media;

    using Concierge.Utility;
    using MaterialDesignThemes.Wpf;

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

        public Guid Id { get; set; }

        public bool IsExpanded { get; set; }

        public string Name { get; set; }

        public (PackIconKind IconKind, Brush Brush, string Name) GetCategory()
        {
            throw new NotImplementedException();
        }
    }
}
