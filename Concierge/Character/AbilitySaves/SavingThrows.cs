﻿// <copyright file="SavingThrows.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySaves
{
    using System;

    using Concierge.Common;
    using Concierge.Leveling.Dtos.Definitions;

    public sealed class SavingThrows : ICopyable<SavingThrows>
    {
        public SavingThrows()
        {
            this.Strength = new Strength();
            this.Dexterity = new Dexterity();
            this.Constitution = new Constitution();
            this.Intelligence = new Intelligence();
            this.Wisdom = new Wisdom();
            this.Charisma = new Charisma();
        }

        public Strength Strength { get; set; }

        public Dexterity Dexterity { get; set; }

        public Constitution Constitution { get; set; }

        public Intelligence Intelligence { get; set; }

        public Wisdom Wisdom { get; set; }

        public Charisma Charisma { get; set; }

        public SavingThrows DeepCopy()
        {
            return new SavingThrows()
            {
                Strength = (Strength)this.Strength.DeepCopy(),
                Dexterity = (Dexterity)this.Dexterity.DeepCopy(),
                Constitution = (Constitution)this.Constitution.DeepCopy(),
                Intelligence = (Intelligence)this.Intelligence.DeepCopy(),
                Wisdom = (Wisdom)this.Wisdom.DeepCopy(),
                Charisma = (Charisma)this.Charisma.DeepCopy(),
            };
        }

        public void SetProficiency(SavingThrowDto savingThrowDto)
        {
            this.Strength.Proficiency = savingThrowDto.Strength.Proficiency;
            this.Dexterity.Proficiency = savingThrowDto.Dexterity.Proficiency;
            this.Constitution.Proficiency = savingThrowDto.Constitution.Proficiency;
            this.Intelligence.Proficiency = savingThrowDto.Intelligence.Proficiency;
            this.Wisdom.Proficiency = savingThrowDto.Wisdom.Proficiency;
            this.Charisma.Proficiency = savingThrowDto.Charisma.Proficiency;
        }

        public SavingThrow GetSavingThrow(string name)
        {
            return name.ToLower() switch
            {
                "strength" => this.Strength,
                "dexterity" => this.Dexterity,
                "constitution" => this.Constitution,
                "intelligence" => this.Intelligence,
                "wisdom" => this.Wisdom,
                "charisma" => this.Charisma,
                _ => throw new NotImplementedException(),
            };
        }
    }
}