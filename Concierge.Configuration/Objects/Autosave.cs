// <copyright file="Autosave.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    public class Autosave
    {
        public Autosave()
        {
        }

        public bool Enabled { get; set; }

        public int Interval { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Autosave autosave)
            {
                return false;
            }

            return autosave.Enabled == this.Enabled && autosave.Interval == this.Interval;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Set(Autosave autosave)
        {
            this.Enabled = autosave.Enabled;
            this.Interval = autosave.Interval;
        }
    }
}
