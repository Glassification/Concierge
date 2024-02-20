// <copyright file="ConciergeLeveler.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling
{
    using System;
    using System.Linq;

    using Concierge.Character;
    using Concierge.Character.Dispositions;
    using Concierge.Character.Enums;
    using Concierge.Character.Magic;
    using Concierge.Commands;
    using Concierge.Common.Enums;
    using Concierge.Leveling.Dtos.Leveler;
    using Concierge.Tools.DiceRoller;

    public sealed class ConciergeLeveler
    {
        private readonly CharacterSheet character;

        public ConciergeLeveler(CharacterSheet character)
        {
            this.character = character;
        }

        public void LevelUp(Dice hitDie, int classNumber, int bonusHp)
        {
            var vitalityDto = this.LevelUpVitality(hitDie, bonusHp);
            var classDto = this.LevelUpCharacterClass(classNumber);
            var magicClassDto = this.LevelUpMagicClass(classDto.New);
            var spellSlotsDto = this.LevelUpSpellSlots(magicClassDto.New);
            var resourceDto = this.LevelUpClassResource(classDto.New);

            Program.UndoRedoService.AddCommand(
                new LevelUpCommand(
                    classDto,
                    resourceDto,
                    magicClassDto,
                    spellSlotsDto,
                    vitalityDto));
        }

        private CharacterClassDto LevelUpCharacterClass(int classNumber)
        {
            var levelClass = this.character.Disposition.GetClass(classNumber);
            var oldClass = levelClass.DeepCopy();

            levelClass.Level++;

            return new CharacterClassDto(oldClass, levelClass.DeepCopy());
        }

        private MagicClassDto LevelUpMagicClass(Class newClass)
        {
            var magicClass = this.character.SpellCasting.MagicalClasses.Where(x => x.Name.Equals(newClass.Name)).FirstOrDefault();
            var oldMagicClass = magicClass?.DeepCopy();

            //TODO
            //magicClass?.LevelUp(LevelingMap.GetSpellSlotIncrease(newClass.Name, newClass.Subclass, newClass.Level));

            return new MagicClassDto(oldMagicClass, magicClass?.DeepCopy());
        }

        private ClassResourcesDto LevelUpClassResource(Class newClass)
        {
            var resourceIncrease = LevelingMap.GetResourceIncrease(newClass.Name, newClass.Subclass, newClass.Level);
            var resource = this.character.Vitality.ClassResources.Where(x => x.Type.Equals(resourceIncrease.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var oldResource = resource?.DeepCopy();

            if (resource is not null)
            {
                resource.Total += resourceIncrease.Increase;
            }

            return new ClassResourcesDto(oldResource, resource?.DeepCopy());
        }

        private SpellSlotsDto LevelUpSpellSlots(MagicalClass? newClass)
        {
            var oldSpellSlots = this.character.SpellCasting.SpellSlots.DeepCopy();

            if (newClass is not null)
            {
                //TODO
                //this.character.SpellCasting.SpellSlots.LevelUp(LevelingMap.GetSpellSlotIncrease(newClass.Name, string.Empty, newClass.Level));
            }

            return new SpellSlotsDto(oldSpellSlots, this.character.SpellCasting.SpellSlots.DeepCopy());
        }

        private VitalityDto LevelUpVitality(Dice hitDie, int bonusHp)
        {
            var newHp = DiceRoll.RollHitDie(hitDie) + this.character.Attributes.Constitution.Bonus + bonusHp;
            var oldVitality = this.character.Vitality.DeepCopy();

            //TODO
            //this.character.Vitality.LevelUp(hitDie, newHp);

            return new VitalityDto(oldVitality, this.character.Vitality.DeepCopy());
        }
    }
}
