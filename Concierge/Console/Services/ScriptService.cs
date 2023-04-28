// <copyright file="ScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System;
    using System.Linq;
    using System.Text;

    using Concierge.Common.Extensions;

    public abstract class ScriptService
    {
        protected const string Indent = "   ";

        public abstract string[] Names { get; }

        public abstract string[] Actions { get; }

        protected bool IsFirstList { get; set; }

        public abstract ConsoleResult Run(ConsoleCommand command);

        public virtual string List()
        {
            var builder = new StringBuilder();

            builder.Append($"{(this.IsFirstList ? string.Empty : Indent)}|--");
            foreach (var action in this.Actions)
            {
                builder.Append($"{action}, ");
            }

            builder.Remove(builder.Length - 2, 2);
            builder.AppendLine();
            foreach (var name in this.Names)
            {
                builder.Append($"{Indent}|   |--{name}\n");
            }

            var builderString = builder.ToString();
            builderString = builderString.ReplaceLast("|", "\\");

            return builderString;
        }

        public bool Contains(string name)
        {
            return this.Names.Contains(name, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
