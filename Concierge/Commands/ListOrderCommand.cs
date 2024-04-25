// <copyright file="ListOrderCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a command that manages the order of items in a list and allows undoing the order changes.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public sealed class ListOrderCommand<T> : Command
    {
        private readonly List<T> sourceList;
        private readonly List<T> oldListOrder;
        private readonly List<T> newListOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListOrderCommand{T}"/> class.
        /// </summary>
        /// <param name="sourceList">The original list whose order is being modified.</param>
        /// <param name="oldListOrder">The original order of items in the list before the modification.</param>
        /// <param name="newListOrder">The new order of items in the list after the modification.</param>
        /// <param name="conciergePage">The ConciergePage associated with this command.</param>
        public ListOrderCommand(List<T> sourceList, List<T> oldListOrder, List<T> newListOrder, ConciergePage conciergePage)
        {
            this.ConciergePage = conciergePage;
            this.sourceList = sourceList;
            this.oldListOrder = oldListOrder;
            this.newListOrder = newListOrder;
        }

        public override void Redo()
        {
            this.sourceList.Clear();
            this.sourceList.AddRange(this.newListOrder);
        }

        public override void Undo()
        {
            this.sourceList.Clear();
            this.sourceList.AddRange(this.oldListOrder);
        }
    }
}
