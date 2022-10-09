// <copyright file="AddCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Interfaces.Enums;

    public sealed class AddCommand<T> : Command
    {
        private readonly List<T> list;
        private readonly T item;

        public AddCommand(List<T> list, T item, ConciergePage conciergePage)
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
