// <copyright file="EditCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public class EditCommand<T> : Command
    {
        private readonly T oldItem;
        private readonly T newItem;

        public EditCommand(T originalItem, T oldItem, ConciergePage conciergePage)
        {
            this.ConciergePage = conciergePage;
            this.OriginalItem = originalItem;
            this.oldItem = oldItem;
            this.newItem = (originalItem as ICopyable<T>).DeepCopy();
        }

        private T OriginalItem { get; set; }

        public override void Redo()
        {
            this.OriginalItem.SetProperties<T>(this.newItem);
        }

        public override void Undo()
        {
            this.OriginalItem.SetProperties<T>(this.oldItem);
        }
    }
}
