// <copyright file="EditCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Common;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a command that edits an item in a list, allowing undoing the edit operation.
    /// </summary>
    /// <typeparam name="T">The type of item being edited.</typeparam>
    public sealed class EditCommand<T> : Command
    {
        private readonly T oldItem;
        private readonly T newItem;
        private readonly T originalItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommand{T}"/> class.
        /// </summary>
        /// <param name="originalItem">The original item before the edit operation.</param>
        /// <param name="oldItem">The item's previous state before the edit operation.</param>
        /// <param name="conciergePage">The ConciergePage associated with this command.</param>
        public EditCommand(T originalItem, T oldItem, ConciergePage conciergePage)
        {
            if (originalItem is not ICopyable<T> newItem || oldItem is null)
            {
                throw new InvalidValueException($"{originalItem} or {oldItem}");
            }

            this.ConciergePage = conciergePage;
            this.originalItem = originalItem;
            this.oldItem = oldItem;
            this.newItem = newItem.DeepCopy();
        }

        public override void Redo()
        {
            if (this.newItem is not null)
            {
                this.originalItem?.SetProperties<T>(this.newItem);
            }
        }

        public override void Undo()
        {
            if (this.oldItem is not null)
            {
                this.originalItem?.SetProperties<T>(this.oldItem);
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
