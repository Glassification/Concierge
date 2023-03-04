// <copyright file="CategoryDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Dtos
{
    using System.Windows.Media;

    using MaterialDesignThemes.Wpf;

    public sealed class CategoryDto
    {
        public CategoryDto()
            : this(PackIconKind.Error, Brushes.Red, string.Empty)
        {
        }

        public CategoryDto(PackIconKind iconKind, Brush brush, string name)
        {
            this.Brush = brush;
            this.IconKind = iconKind;
            this.Name = name;
        }

        public Brush Brush { get; set; }

        public PackIconKind IconKind { get; set; }

        public string Name { get; set; }
    }
}
