// <copyright file="ListScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Concierge.Console.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public sealed class ListScript<T> : IScript
        where T : IUnique, ICopyable<T>, new()
    {
        private readonly Regex textInParentheses = new (@"\(.*?\)", RegexOptions.Compiled);

        public ListScript(List<T> defaultList, List<T> characterList, string listName)
        {
            this.DefaultList = defaultList;
            this.CharacterList = characterList;
            this.ListName = listName;
        }

        private List<T> DefaultList { get; set; }

        private List<T> CharacterList { get; set; }

        private string ListName { get; set; }

        public ConsoleResult Evaluate(string command)
        {
            if (command.Contains("AddItem", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.AddItem(this.textInParentheses.Match(command).Value.Strip("(").Strip(")"));
            }

            if (command.Contains("RemoveItem", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.RemoveItem(this.textInParentheses.Match(command).Value.Strip("(").Strip(")"));
            }

            if (command.Contains("Count", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Count(this.textInParentheses.Match(command).Value.Strip("(").Strip(")"));
            }

            return new ConsoleResult($"Implementation for '{command}' not found.", ResultType.Error);
        }

        private ConsoleResult AddItem(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                this.CharacterList.AddRange(this.DefaultList);
                return new ConsoleResult($"All default items added to {this.ListName}", ResultType.Success);
            }

            var item = this.GetDefaultItem(name);
            if (item is null)
            {
                this.CharacterList.Add(new T()
                {
                    Name = name,
                });
                return new ConsoleResult($"Default item '{name}' could not be found. Added new item to {this.ListName}.", ResultType.Warning);
            }

            this.CharacterList.Add(item);
            return new ConsoleResult($"Added '{name}' to {this.ListName}.", ResultType.Success);
        }

        private ConsoleResult RemoveItem(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                this.CharacterList.Clear();
                return new ConsoleResult($"All items removed from {this.ListName}.", ResultType.Success);
            }

            var item = this.GetCharacterItem(name);
            if (item is null)
            {
                return new ConsoleResult($"Item '{name}' could not be found.", ResultType.Error);
            }

            this.CharacterList.Remove(item);
            return new ConsoleResult($"Removed '{name}' from {this.ListName}.", ResultType.Success);
        }

        private ConsoleResult Count(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"{this.CharacterList.Count} items in {this.ListName}.", ResultType.Success);
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
