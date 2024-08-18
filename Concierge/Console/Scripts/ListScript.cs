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
        private readonly List<T> defaultList;
        private readonly List<T> characterList;

        public ListScript(List<T> defaultList, List<T> characterList)
        {
            this.defaultList = defaultList;
            this.characterList = characterList;
        }

        public ConsoleResult Evaluate(ConsoleCommand command)
        {
            if (command.Action.EqualsIgnoreCase("AddItem"))
            {
                return this.AddItem(command);
            }

            if (command.Action.EqualsIgnoreCase("RemoveItem"))
            {
                return this.RemoveItem(command);
            }

            if (command.Action.EqualsIgnoreCase("Count"))
            {
                return this.Count(command);
            }

            if (command.Action.EqualsIgnoreCase("AddCategory"))
            {
                return this.AddCategory(command);
            }

            if (command.Action.EqualsIgnoreCase("GetId"))
            {
                return this.GetId(command);
            }

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.NotImplemented);
        }

        private ConsoleResult AddItem(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                this.characterList.AddRange(this.defaultList);
                return new ConsoleResult($"All default items added to {command.Name}", ResultType.Success);
            }

            var item = this.GetDefaultItem(command.Argument);
            if (item is null)
            {
                this.characterList.Add(new T()
                {
                    Name = command.Argument,
                });
                return new ConsoleResult($"Default item '{command.Argument}' could not be found. Added new item to {command.Name}.", ResultType.Warning);
            }

            this.characterList.Add(item);
            return new ConsoleResult($"Added '{command.Argument}' to {command.Name}.", ResultType.Success);
        }

        private ConsoleResult RemoveItem(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                this.characterList.Clear();
                return new ConsoleResult($"All items removed from {command.Name}.", ResultType.Success);
            }

            var items = this.GetCharacterItem(command.Argument);
            if (items.IsEmpty())
            {
                return new ConsoleResult($"Item '{command.Argument}' could not be found.", ResultType.Error);
            }

            this.characterList.RemoveAll(x => items.Contains(x));
            return new ConsoleResult($"Removed all '{command.Argument}' from {command.Name}.", ResultType.Success);
        }

        private ConsoleResult Count(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"{this.characterList.Count} items in {command.Name}.", ResultType.Success);
            }

            return new ConsoleResult($"Counting specific items is not implemented.", ResultType.Error);
        }

        private ConsoleResult AddCategory(ConsoleCommand command)
        {
            try
            {
                if (command.Argument.IsNullOrWhiteSpace())
                {
                    this.characterList.AddRange(this.defaultList.DistinctBy(x => x.GetCategory().Name));
                    return new ConsoleResult($"All default item categories added to {command.Name}", ResultType.Success);
                }

                var items = this.defaultList.Where(x => x.GetCategory().Name.EqualsIgnoreCase(command.Argument)).ToList();
                if (items.Count > 0)
                {
                    this.characterList.AddRange(items);
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
            var builder = new StringBuilder();
            if (command.Argument.IsNullOrWhiteSpace())
            {
                builder.AppendLine($"Listing Id for all {command.Name}:");
                this.characterList.ForEach(x => builder.AppendLine($"Id for '{x.Name}' is [{x.Id}]."));

                return new ConsoleResult(builder.ToString(), ResultType.Success);
            }

            var items = this.GetCharacterItem(command.Argument);
            if (items.IsEmpty())
            {
                return new ConsoleResult($"Item '{command.Argument}' could not be found.", ResultType.Error);
            }

            builder.AppendLine($"Listing Id for all {command.Argument}:");
            items.ForEach(x => builder.AppendLine($"Id for '{x.Name}' is [{x.Id}]."));
            return new ConsoleResult(builder.ToString(), ResultType.Success);
        }

        private T? GetDefaultItem(string name)
        {
            var item = this.defaultList.Where(x => x.Name.EqualsIgnoreCase(name)).FirstOrDefault();
            if (item is null)
            {
                return item;
            }

            var newItem = item.DeepCopy();
            newItem.Id = Guid.NewGuid();

            return newItem;
        }

        private List<T> GetCharacterItem(string name) => this.characterList.Where(x => x.Name.EqualsIgnoreCase(name)).ToList();
    }
}
