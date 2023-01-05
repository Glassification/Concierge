// <copyright file="ConciergeLeveler.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling
{
    using System;
    using System.Linq;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Leveling.Dtos.Leveler;
    using Concierge.Tools.DiceRolling.Dice;
    using Concierge.Utility.Utilities;

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
            var magicClass = this.character.MagicClasses.Where(x => x.Name.Equals(newClass.Name)).FirstOrDefault();
            var oldMagicClass = magicClass?.DeepCopy();

            magicClass?.LevelUp(CharacterUtility.GetSpellSlotIncrease(newClass.Name, newClass.Subclass, newClass.Level));

            return new MagicClassDto(oldMagicClass, magicClass?.DeepCopy());
        }

        private ClassResourcesDto LevelUpClassResource(CharacterClass newClass)
        {
            var resourceIncrease = CharacterUtility.GetResourceIncrease(newClass.Name, newClass.Subclass, newClass.Level);
            var resource = this.character.ClassResources.Where(x => x.Type.Equals(resourceIncrease.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var oldResource = resource?.DeepCopy();

            if (resource is not null)
            {
                resource.Total += resourceIncrease.Increase;
            }

            return new ClassResourcesDto(oldResource, resource?.DeepCopy());
        }

        private SpellSlotsDto LevelUpSpellSlots(MagicClass? newClass)
        {
            var oldSpellSlots = this.character.SpellSlots.DeepCopy();

            if (newClass is not null)
            {
                this.character.SpellSlots.LevelUp(CharacterUtility.GetSpellSlotIncrease(newClass.Name, string.Empty, newClass.Level));
            }

            return new SpellSlotsDto(oldSpellSlots, this.character.SpellSlots.DeepCopy());
        }

        private VitalityDto LevelUpVitality(HitDie hitDie, int bonusHp)
        {
            var newHp = DiceRoll.RollHitDie(hitDie) + CharacterUtility.CalculateBonus(this.character.Attributes.Constitution) + bonusHp;
            var oldVitality = this.character.Vitality.DeepCopy();

            this.character.Vitality.LevelUp(hitDie, newHp);

            return new VitalityDto(oldVitality, this.character.Vitality.DeepCopy());
        }
    }
}
