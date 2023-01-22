// <copyright file="ListScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Console.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class ListScript<T> : IScript
        where T : IUnique, ICopyable<T>, new()
    {
        public ListScript(List<T> defaultList, List<T> characterList)
        {
            this.DefaultList = defaultList;
            this.CharacterList = characterList;
        }

        private List<T> DefaultList { get; set; }

        private List<T> CharacterList { get; set; }

        public ConsoleResult Evaluate(ConsoleCommand command)
        {
            if (command.Action.Equals("AddItem", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.AddItem(command);
            }

            if (command.Action.Equals("RemoveItem", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.RemoveItem(command);
            }

            if (command.Action.Equals("Count", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Count(command);
            }

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.Error);
        }

        private ConsoleResult AddItem(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                this.CharacterList.AddRange(this.DefaultList);
                return new ConsoleResult($"All default items added to {command.Name}", ResultType.Success);
            }

            var item = this.GetDefaultItem(command.Argument);
            if (item is null)
            {
                this.CharacterList.Add(new T()
                {
                    Name = command.Argument,
                });
                return new ConsoleResult($"Default item '{command.Argument}' could not be found. Added new item to {command.Name}.", ResultType.Warning);
            }

            this.CharacterList.Add(item);
            return new ConsoleResult($"Added '{command.Argument}' to {command.Name}.", ResultType.Success);
        }

        private ConsoleResult RemoveItem(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                this.CharacterList.Clear();
                return new ConsoleResult($"All items removed from {command.Name}.", ResultType.Success);
            }

            var item = this.GetCharacterItem(command.Argument);
            if (item is null)
            {
                return new ConsoleResult($"Item '{command.Argument}' could not be found.", ResultType.Error);
            }

            this.CharacterList.Remove(item);
            return new ConsoleResult($"Removed '{command.Argument}' from {command.Name}.", ResultType.Success);
        }

        private ConsoleResult Count(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"{this.CharacterList.Count} items in {command.Name}.", ResultType.Success);
            }

            return new ConsoleResult($"Counting specific items is not implemented.", ResultType.Error);
        }

        private T? GetDefaultItem(string name)
        {
            var item = this.DefaultList.Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (item is null)
            {
                return item;
            }

            var newItem = item.DeepCopy();
            newItem.Id = Guid.NewGuid();

            return newItem;
        }

        private T? GetCharacterItem(string name)
        {
            return this.CharacterList.Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
