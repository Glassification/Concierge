// <copyright file="SkillCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character.AbilitySkills.SkillTypes;
    using Concierge.Commands.Enums;
    using Concierge.Interfaces.Enums;

    public class SkillCommand : Command
    {
        private readonly Skills skills;
        private readonly BonusType bonusType;
        private readonly bool oldValue;
        private readonly bool newValue;

        public SkillCommand(Skills skills, BonusType bonusType, bool oldValue, bool newValue, ConciergePage conciergePage)
        {
            this.ConciergePage = conciergePage;
            this.skills = skills;
            this.bonusType = bonusType;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public override void Redo()
        {
            this.SetSkill(this.newValue);
        }

        public override void Undo()
        {
            this.SetSkill(this.oldValue);
        }

        private void SetSkill(bool value)
        {
            switch (this.bonusType)
            {
                case BonusType.Expertise:
                    this.skills.Expertise = value;
                    break;
                case BonusType.Proficiency:
                    this.skills.Proficiency = value;
                    break;
            }
        }
    }
}
