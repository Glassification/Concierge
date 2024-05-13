// <copyright file="ConciergeLeveler.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling
{
    using System;
    using System.Linq;

    using Concierge.Character;
    using Concierge.Character.Dispositions;
    using Concierge.Character.Magic;
    using Concierge.Commands;
    using Concierge.Common.Enums;
    using Concierge.Leveling.Dtos.Leveler;
    using Concierge.Tools.DiceRoller;

    /// <summary>
    /// Provides functionality for leveling up a character.
    /// </summary>
    public sealed class ConciergeLeveler
    {
        private readonly CharacterSheet character;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConciergeLeveler"/> class with the specified character.
        /// </summary>
        /// <param name="character">The character sheet to level up.</param>
        public ConciergeLeveler(CharacterSheet character)
        {
            this.character = character;
        }

        /// <summary>
        /// Levels up the character with the specified hit die, class number, and bonus HP.
        /// </summary>
        /// <param name="hitDie">The hit die to use for leveling up.</param>
        /// <param name="classNumber">The number representing the character's class.</param>
        /// <param name="bonusHp">The bonus hit points gained at each level.</param>
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
            if (magicClass is not null)
            {
                var spellSlotDto = LevelingMap.GetSpellSlotIncrease(newClass.Name, newClass.Subclass, newClass.Level);

                magicClass.Level++;
                magicClass.KnownSpells += spellSlotDto.Known;
                magicClass.KnownCantrips += spellSlotDto.Cantrip;
                magicClass.SpellSlots += spellSlotDto.Slots;
            }

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
            var spellSlots = this.character.SpellCasting.SpellSlots;
            var oldSpellSlots = spellSlots.DeepCopy();

            if (newClass is not null)
            {
                var spellSlotDto = LevelingMap.GetSpellSlotIncrease(newClass.Name, string.Empty, newClass.Level);

                spellSlots.FirstTotal += spellSlotDto.First;
                spellSlots.SecondTotal += spellSlotDto.Second;
                spellSlots.ThirdTotal += spellSlotDto.Third;
                spellSlots.FourthTotal += spellSlotDto.Fourth;
                spellSlots.FifthTotal += spellSlotDto.Fifth;
                spellSlots.SixthTotal += spellSlotDto.Sixth;
                spellSlots.SeventhTotal += spellSlotDto.Seventh;
                spellSlots.EighthTotal += spellSlotDto.Eighth;
                spellSlots.NinethTotal += spellSlotDto.Nineth;

                spellSlots.Reset();
            }

            return new SpellSlotsDto(oldSpellSlots, this.character.SpellCasting.SpellSlots.DeepCopy());
        }

        private VitalityDto LevelUpVitality(Dice hitDie, int bonusHp)
        {
            var newHp = DiceRoll.RollHitDie(hitDie) + this.character.Attributes.Constitution.Bonus + bonusHp;
            var oldVitality = this.character.Vitality.DeepCopy();

            this.character.Vitality.Health.MaxHealth += newHp;
            this.character.Vitality.Health.ResetHealth();
            this.character.Vitality.HitDice.RegainHitDice();
            switch (hitDie)
            {
                case Dice.D6:
                    this.character.Vitality.HitDice.TotalD6++;
                    break;
                case Dice.D8:
                    this.character.Vitality.HitDice.TotalD8++;
                    break;
                case Dice.D10:
                    this.character.Vitality.HitDice.TotalD10++;
                    break;
                case Dice.D12:
                    this.character.Vitality.HitDice.TotalD12++;
                    break;
            }

            return new VitalityDto(oldVitality, this.character.Vitality.DeepCopy());
        }
    }
}
