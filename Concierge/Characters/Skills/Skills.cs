// <copyright file="Skills.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.SkillsNamespace
{
    using Concierge.Characters.Enums;

    public abstract class Skills
    {
        public bool Proficiency
        {
            get;
            set;
        }

        public bool Expertise
        {
            get;
            set;
        }

        public abstract StatusChecks Checks
        {
            get;
        }

        public abstract int Bonus
        {
            get;
        }
    }
}
