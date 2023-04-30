// <copyright file="CategoryItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    public sealed class CategoryItem
    {
        public CategoryItem()
            : this(string.Empty, string.Empty)
        {
        }

        public CategoryItem(string name, string category)
        {
            this.Name = name;
            this.Category = category;
        }

        public string Name { get; set; }

        public string Category { get; set; }

        public static bool operator ==(CategoryItem a, CategoryItem b)
        {
            return a.Name == b.Name && a.Category == b.Category;
        }

        public static bool operator !=(CategoryItem a, CategoryItem b)
        {
            return b.Name != a.Name || b.Category != a.Category;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CategoryItem item)
            {
                return this == item;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"({this.Name}) - {this.Category}";
        }
    }
}
