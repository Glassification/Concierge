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
    using Concierge.Leveling.Dtos;
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

        public LevelUpCommand(CharacterClassDto characterClass, MagicClassDto magicClass, SpellSlotsDto spellSlots, VitalityDto vitality)
        {
            this.oldVitality = vitality.Old;
            this.newVitality = vitality.New;
            this.oldClass = characterClass.Old;
            this.newClass = characterClass.New;
            this.oldMagicClass = magicClass.Old;
            this.newMagicClass = magicClass.New;
            this.oldSpellSlots = spellSlots.Old;
            this.newSpellSlots = spellSlots.New;

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
