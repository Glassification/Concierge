// <copyright file="LevelUpCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character;
    using Concierge.Character.Statuses;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;

    public class LevelUpCommand : Command
    {
        private readonly Vitality oldVitality;
        private readonly Vitality newVitality;
        private readonly CharacterClass oldClass;
        private readonly CharacterClass newClass;

        private readonly ConciergeCharacter character;

        public LevelUpCommand(
            Vitality oldVitality,
            Vitality newVitality,
            CharacterClass oldClass,
            CharacterClass newClass)
        {
            this.oldVitality = oldVitality;
            this.newVitality = newVitality;
            this.oldClass = oldClass;
            this.newClass = newClass;
            this.ConciergePage = ConciergePage.None;
            this.character = Program.CcsFile.Character;
        }

        public override void Redo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.newVitality);
            this.character.Properties.GetClassByNumber(this.newClass.ClassNumber).SetProperties<CharacterClass>(this.newClass);
        }

        public override void Undo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.oldVitality);
            this.character.Properties.GetClassByNumber(this.oldClass.ClassNumber).SetProperties<CharacterClass>(this.oldClass);
        }
    }
}
