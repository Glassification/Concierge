// <copyright file="EditCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character;

    public class EditCommand<T> : Command
    {
        private readonly T oldItem;
        private readonly T newItem;

        public EditCommand(T originalItem, T oldItem)
        {
            this.OriginalItem = originalItem;
            this.oldItem = oldItem;
            this.newItem = (T)(originalItem as ICopyable).DeepCopy();
        }

        private T OriginalItem { get; set; }

        public override void Redo()
        {
            this.SetProperties(this.newItem);
        }

        public override void Undo()
        {
            this.SetProperties(this.oldItem);
        }

        private void SetProperties(T item)
        {
            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    var propertyValue = property.GetValue(item);
                    property.SetValue(this.OriginalItem, propertyValue);
                }
            }
        }
    }
}
