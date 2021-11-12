// <copyright file="EditCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

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

        private int Depth { get; set; }

        public override void Redo()
        {
            this.Depth = 0;
            this.SetProperties(this.newItem, this.OriginalItem);
        }

        public override void Undo()
        {
            this.Depth = 0;
            this.SetProperties(this.oldItem, this.OriginalItem);
        }

        private void SetProperties(object item, object originalItem)
        {
            this.Depth++;

            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite && property.CanRead)
                {
                    var propertyValue = property.GetValue(item);

                    if (propertyValue is ICopyable<T> && this.Depth < Constants.MaxDepth)
                    {
                        this.SetProperties(propertyValue, property.GetValue(originalItem));
                    }
                    else
                    {
                        property.SetValue(originalItem, propertyValue);
                    }
                }
            }

            this.Depth--;
        }
    }
}
