// <copyright file="EditCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Exceptions;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public class EditCommand<T> : Command
    {
        private readonly T oldItem;
        private readonly T newItem;

        public EditCommand(T originalItem, T oldItem, ConciergePage conciergePage)
        {
            if (originalItem is not ICopyable<T> newItem || oldItem is null)
            {
                throw new InvalidValueException($"{originalItem} or {oldItem}");
            }

            this.ConciergePage = conciergePage;
            this.OriginalItem = originalItem;
            this.oldItem = oldItem;
            this.newItem = newItem.DeepCopy();
        }

        private T OriginalItem { get; set; }

        public override void Redo()
        {
            if (this.newItem is not null)
            {
                this.OriginalItem?.SetProperties<T>(this.newItem);
            }
        }

        public override void Undo()
        {
            if (this.oldItem is not null)
            {
                this.OriginalItem?.SetProperties<T>(this.oldItem);
            }
        }

        public override bool ShouldAdd()
        {
            var oldItemString = JsonConvert.SerializeObject(this.oldItem, Formatting.Indented);
            var newItemString = JsonConvert.SerializeObject(this.newItem, Formatting.Indented);

            return !oldItemString.Equals(newItemString);
        }
    }
}
