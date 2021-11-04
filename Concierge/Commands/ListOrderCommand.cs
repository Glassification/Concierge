// <copyright file="ListOrderCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Interfaces.Enums;

    public class ListOrderCommand<T> : Command
    {
        private readonly List<T> sourceList;
        private readonly List<T> oldListOrder;
        private readonly List<T> newListOrder;

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
