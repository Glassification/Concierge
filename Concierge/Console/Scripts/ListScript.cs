// <copyright file="ListScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;

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

            if (command.Action.Equals("AddCategory", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.AddCategory(command);
            }

            if (command.Action.Equals("GetId", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.GetId(command);
            }

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.NotImplemented);
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

        private ConsoleResult AddCategory(ConsoleCommand command)
        {
            try
            {
                if (command.Argument.IsNullOrWhiteSpace())
                {
                    this.CharacterList.AddRange(this.DefaultList.DistinctBy(x => x.GetCategory().Name));
                    return new ConsoleResult($"All default item categories added to {command.Name}", ResultType.Success);
                }

                var items = this.DefaultList.Where(x => x.GetCategory().Name.Equals(command.Argument, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (items.Count > 0)
                {
                    this.CharacterList.AddRange(items);
                    return new ConsoleResult($"All default items with a category of {command.Argument} added to {command.Name}", ResultType.Success);
                }

                return new ConsoleResult($"Specified category is not implemented.", ResultType.Error);
            }
            catch (Exception ex)
            {
                Program.Logger.Warning(ex.Message);
                return new ConsoleResult($"Specified list does not have a category implemented.", ResultType.Warning);
            }
        }

        private ConsoleResult GetId(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                var builder = new StringBuilder();
                builder.AppendLine($"Listing Id for all {command.Name}:");
                this.CharacterList.ForEach(x => builder.AppendLine($"Id for '{x.Name}' is [{x.Id}]."));

                return new ConsoleResult(builder.ToString(), ResultType.Success);
            }

            var item = this.GetCharacterItem(command.Argument);
            if (item is null)
            {
                return new ConsoleResult($"Item '{command.Argument}' could not be found.", ResultType.Error);
            }

            return new ConsoleResult($"Id for '{command.Argument}' is [{item.Id}].", ResultType.Success);
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
