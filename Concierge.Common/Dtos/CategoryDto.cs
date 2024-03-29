﻿// <copyright file="CategoryDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Dtos
{
    using System.Windows.Media;

    using MaterialDesignThemes.Wpf;

    public sealed class CategoryDto
    {
        public CategoryDto(PackIconKind iconKind, Brush brush, string name)
        {
            this.Brush = brush;
            this.IconKind = iconKind;
            this.Name = name;
        }

        private CategoryDto()
            : this(PackIconKind.Error, Brushes.Red, string.Empty)
        {
        }

        public static CategoryDto Empty => new ();

        public Brush Brush { get; set; }

        public PackIconKind IconKind { get; set; }

        public string Name { get; set; }
    }
}
