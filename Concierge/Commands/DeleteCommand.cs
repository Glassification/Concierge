// <copyright file="DeleteCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a command that deletes an item from a list and allows undoing the deletion.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public sealed class DeleteCommand<T> : Command
    {
        private readonly List<T> list;
        private readonly T item;
        private readonly int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommand{T}"/> class.
        /// </summary>
        /// <param name="list">The list from which the item will be deleted.</param>
        /// <param name="item">The item to be deleted from the list.</param>
        /// <param name="index">The index at which the item was located before deletion.</param>
        /// <param name="conciergePage">The ConciergePage associated with this command.</param>
        public DeleteCommand(List<T> list, T item, int index, ConciergePages conciergePage)
        {
            this.ConciergePage = conciergePage;
            this.list = list;
            this.item = item;
            this.index = index;
        }

        public override void Redo()
        {
            this.list.Remove(this.item);
        }

        public override void Undo()
        {
            this.list.Insert(this.index, this.item);
        }
    }
}
