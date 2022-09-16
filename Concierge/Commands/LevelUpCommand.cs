// <copyright file="LevelUpCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Linq;

    using Concierge.Character;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;

    public sealed class LevelUpCommand : Command
    {
        private readonly Vitality oldVitality;
        private readonly Vitality newVitality;
        private readonly CharacterClass oldClass;
        private readonly CharacterClass newClass;
        private readonly MagicClass? oldMagicClass;
        private readonly MagicClass? newMagicClass;
        private readonly SpellSlots oldSpellSlots;
        private readonly SpellSlots newSpellSlots;

        private readonly ConciergeCharacter character;

        public LevelUpCommand(
            Vitality oldVitality,
            Vitality newVitality,
            CharacterClass oldClass,
            CharacterClass newClass,
            MagicClass? oldMagicClass,
            MagicClass? newMagicClass,
            SpellSlots oldSpellSlots,
            SpellSlots newSpellSlots)
        {
            this.oldVitality = oldVitality;
            this.newVitality = newVitality;
            this.oldClass = oldClass;
            this.newClass = newClass;
            this.oldMagicClass = oldMagicClass;
            this.newMagicClass = newMagicClass;
            this.oldSpellSlots = oldSpellSlots;
            this.newSpellSlots = newSpellSlots;

            this.ConciergePage = ConciergePage.None;
            this.character = Program.CcsFile.Character;
        }

        public override void Redo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.newVitality);
            this.character.Properties.GetClassByNumber(this.newClass.ClassNumber).SetProperties<CharacterClass>(this.newClass);
            if (this.newMagicClass is not null)
            {
                this.character.SpellSlots.SetProperties<SpellSlots>(this.newSpellSlots);
                this.character.MagicClasses.Where(x => x.Name.Equals(this.newMagicClass.Name)).First().SetProperties<MagicClass>(this.newMagicClass);
            }
        }

        public override void Undo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.oldVitality);
            this.character.Properties.GetClassByNumber(this.oldClass.ClassNumber).SetProperties<CharacterClass>(this.oldClass);
            if (this.oldMagicClass is not null)
            {
                this.character.SpellSlots.SetProperties<SpellSlots>(this.oldSpellSlots);
                this.character.MagicClasses.Where(x => x.Name.Equals(this.oldMagicClass.Name)).First().SetProperties<MagicClass>(this.oldMagicClass);
            }
        }
    }
}
