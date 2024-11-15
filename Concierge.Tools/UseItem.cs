// <copyright file="UseItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an item that can be used, with an optional bonus.
    /// </summary>
    public sealed class UseItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UseItem"/> class with a specified bonus.
        /// </summary>
        /// <param name="bonus">The bonus associated with the item.</param>
        public UseItem(int bonus)
            : this(bonus, [])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UseItem"/> class with a specified item and bonus.
        /// </summary>
        /// <param name="item">The item to be used.</param>
        /// <param name="bonus">The bonus associated with the item.</param>
        public UseItem(int bonus, params IUsable[] items)
        {
            this.Items = [.. items];
            this.Bonus = bonus;
        }

        /// <summary>
        /// Gets an empty instance of <see cref="UseItem"/>.
        /// </summary>
        public static UseItem Empty => new (0);

        /// <summary>
        /// Gets the item to be used.
        /// </summary>
        public IUsable? Item => this.Items.FirstOrDefault();

        /// <summary>
        /// Gets or sets the items to be used.
        /// </summary>
        public List<IUsable> Items { get; set; }

        /// <summary>
        /// Gets or sets the bonus associated with the item.
        /// </summary>
        public int Bonus { get; set; }
    }
}
