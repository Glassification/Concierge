// <copyright file="LevelUpCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Linq;

    using Concierge.Character;
    using Concierge.Character.Dispositions;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;
    using Concierge.Leveling.Dtos.Leveler;

    /// <summary>
    /// Represents a command that handles leveling up a character, including changes to vitality, classes, magic classes, spell slots, and class resources.
    /// </summary>
    public sealed class LevelUpCommand : Command
    {
        private readonly Vitality oldVitality;
        private readonly Vitality newVitality;
        private readonly Class oldClass;
        private readonly Class newClass;
        private readonly MagicalClass? oldMagicClass;
        private readonly MagicalClass? newMagicClass;
        private readonly SpellSlots oldSpellSlots;
        private readonly SpellSlots newSpellSlots;
        private readonly ClassResource? oldClassResource;
        private readonly ClassResource? newClassResource;

        private readonly CharacterSheet character;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelUpCommand"/> class.
        /// </summary>
        /// <param name="characterClass">The character class data including old and new class information.</param>
        /// <param name="classResource">The class resource data including old and new resource information.</param>
        /// <param name="magicClass">The magic class data including old and new magic class information.</param>
        /// <param name="spellSlots">The spell slots data including old and new spell slot information.</param>
        /// <param name="vitality">The vitality data including old and new vitality information.</param>
        public LevelUpCommand(
            CharacterClassDto characterClass,
            ClassResourcesDto classResource,
            MagicClassDto magicClass,
            SpellSlotsDto spellSlots,
            VitalityDto vitality)
        {
            this.oldVitality = vitality.Old;
            this.newVitality = vitality.New;
            this.oldClass = characterClass.Old;
            this.newClass = characterClass.New;
            this.oldMagicClass = magicClass.Old;
            this.newMagicClass = magicClass.New;
            this.oldSpellSlots = spellSlots.Old;
            this.newSpellSlots = spellSlots.New;
            this.oldClassResource = classResource.Old;
            this.newClassResource = classResource.New;

            this.ConciergePage = ConciergePages.None;
            this.character = Program.CcsFile.Character;
        }

        public override void Redo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.newVitality);
            this.character.Disposition.GetClass(this.newClass.ClassNumber).SetProperties<Class>(this.newClass);
            if (this.newMagicClass is not null)
            {
                this.character.SpellCasting.SpellSlots.SetProperties<SpellSlots>(this.newSpellSlots);
                this.character.SpellCasting.MagicalClasses.Where(x => x.Name.Equals(this.newMagicClass.Name)).First().SetProperties<MagicalClass>(this.newMagicClass);
            }

            if (this.newClassResource is not null)
            {
                this.character.Vitality.ClassResources
                    .Where(x => x.Type.EqualsIgnoreCase(this.newClassResource.Type))
                    .First()
                    .SetProperties<ClassResource>(this.newClassResource);
            }
        }

        public override void Undo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.oldVitality);
            this.character.Disposition.GetClass(this.oldClass.ClassNumber).SetProperties<Class>(this.oldClass);
            if (this.oldMagicClass is not null)
            {
                this.character.SpellCasting.SpellSlots.SetProperties<SpellSlots>(this.oldSpellSlots);
                this.character.SpellCasting.MagicalClasses.Where(x => x.Name.Equals(this.oldMagicClass.Name)).First().SetProperties<MagicalClass>(this.oldMagicClass);
            }

            if (this.oldClassResource is not null)
            {
                this.character.Vitality.ClassResources
                    .Where(x => x.Type.EqualsIgnoreCase(this.oldClassResource.Type))
                    .First()
                    .SetProperties<ClassResource>(this.oldClassResource);
            }
        }
    }
}
