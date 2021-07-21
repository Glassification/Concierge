// <copyright file="Class.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    using Concierge.Utility;

    public class Class
    {
        private int _level;

        public Class()
        {
            this._level = 0;
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public Guid Id { get; private set; }

        public int Level
        {
            get => this._level;
            set
            {
                if (value <= Constants.MaxLevel && value >= 0)
                {
                    if (Utilities.ValidateClassLevel(Program.CcsFile.Character.Classess, this.Id))
                    {
                        this._level = value;
                    }
                }
            }
        }
    }
}
