// <copyright file="CategoryItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    /// <summary>
    /// Represents an item in a category.
    /// </summary>
    public sealed class CategoryItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryItem"/> class with empty values for name and category.
        /// </summary>
        public CategoryItem()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryItem"/> class with the specified name and category.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="category">The category of the item.</param>
        public CategoryItem(string name, string category)
        {
            this.Name = name;
            this.Category = category;
        }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category of the item.
        /// </summary>
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
