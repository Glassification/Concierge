// <copyright file="DeleteCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Interfaces.Enums;

    public sealed class DeleteCommand<T> : Command
    {
        private readonly List<T> list;
        private readonly T item;
        private readonly int index;

        public DeleteCommand(List<T> list, T item, int index, ConciergePage conciergePage)
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
