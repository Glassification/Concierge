// <copyright file="Name.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using Concierge.Character.Enums;

    public sealed class Name
    {
        public Name()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
        }

        public Name(string firstName, string lastName, Gender gender)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }
    }
}
