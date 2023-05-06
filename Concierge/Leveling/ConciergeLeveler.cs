// <copyright file="ConciergeLeveler.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling
{
    using System;
    using System.Linq;

    using Concierge.Character;
    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Leveling.Dtos.Leveler;
    using Concierge.Tools.DiceRoller;

    public sealed class ConciergeLeveler
    {
        private readonly ConciergeCharacter character;

        public ConciergeLeveler(ConciergeCharacter character)
        {
            this.character = character;
        }

        public void LevelUp(HitDie hitDie, int classNumber, int bonusHp)
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
            var levelClass = this.character.Properties.GetClassByNumber(classNumber);
            var oldClass = levelClass.DeepCopy();

            levelClass.Level++;

            return new CharacterClassDto(oldClass, levelClass.DeepCopy());
        }

        private MagicClassDto LevelUpMagicClass(CharacterClass newClass)
        {
            var magicClass = this.character.Magic.MagicClasses.Where(x => x.Name.Equals(newClass.Name)).FirstOrDefault();
            var oldMagicClass = magicClass?.DeepCopy();

            magicClass?.LevelUp(LevelingMap.GetSpellSlotIncrease(newClass.Name, newClass.Subclass, newClass.Level));

            return new MagicClassDto(oldMagicClass, magicClass?.DeepCopy());
        }

        private ClassResourcesDto LevelUpClassResource(CharacterClass newClass)
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

        private SpellSlotsDto LevelUpSpellSlots(MagicClass? newClass)
        {
            var oldSpellSlots = this.character.Magic.SpellSlots.DeepCopy();

            if (newClass is not null)
            {
                this.character.Magic.SpellSlots.LevelUp(LevelingMap.GetSpellSlotIncrease(newClass.Name, string.Empty, newClass.Level));
            }

            return new SpellSlotsDto(oldSpellSlots, this.character.Magic.SpellSlots.DeepCopy());
        }

        private VitalityDto LevelUpVitality(HitDie hitDie, int bonusHp)
        {
            var newHp = DiceRoll.RollHitDie(hitDie) + Constants.CalculateBonus(this.character.Characteristic.Attributes.Constitution) + bonusHp;
            var oldVitality = this.character.Vitality.DeepCopy();

            this.character.Vitality.LevelUp(hitDie, newHp);

            return new VitalityDto(oldVitality, this.character.Vitality.DeepCopy());
        }
    }
}
