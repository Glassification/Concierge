// <copyright file="AddCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a command that adds an item to a list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public sealed class AddCommand<T> : Command
    {
        private readonly List<T> list;
        private readonly T item;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCommand{T}"/> class.
        /// </summary>
        /// <param name="list">The list to which the item will be added.</param>
        /// <param name="item">The item to add to the list.</param>
        /// <param name="conciergePage">The ConciergePage associated with this command.</param>
        public AddCommand(List<T> list, T item, ConciergePages conciergePage)
        {
            this.ConciergePage = conciergePage;
            this.list = list;
            this.item = item;
        }

        public override void Redo()
        {
            this.list.Add(this.item);
        }

        public override void Undo()
        {
            this.list.Remove(this.item);
        }
    }
}
