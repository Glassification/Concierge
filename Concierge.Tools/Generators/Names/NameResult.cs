// <copyright file="NameResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Names
{
    public sealed class NameResult : IGeneratorResult
    {
        public NameResult()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
        }

        public NameResult(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}".Trim();
    }
}
