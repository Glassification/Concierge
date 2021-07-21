// <copyright file="Skill.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.CharacterSkills
{
    using Concierge.Characters.CharacterSkills.SkillTypes;

    public class Skill
    {
        public Skill()
        {
            this.Athletics = new Athletics();
            this.Acrobatics = new Acrobatics();
            this.SleightOfHand = new SleightOfHand();
            this.Stealth = new Stealth();
            this.Arcana = new Arcana();
            this.History = new History();
            this.Investigation = new Investigation();
            this.Nature = new Nature();
            this.Religion = new Religion();
            this.AnimalHandling = new AnimalHandling();
            this.Insight = new Insight();
            this.Medicine = new Medicine();
            this.Perception = new Perception();
            this.Survival = new Survival();
            this.Deception = new Deception();
            this.Intimidation = new Intimidation();
            this.Performance = new Performance();
            this.Persuasion = new Persuasion();
        }

        public Athletics Athletics { get; set; }

        public Acrobatics Acrobatics { get; set; }

        public SleightOfHand SleightOfHand { get; set; }

        public Stealth Stealth { get; set; }

        public Arcana Arcana { get; set; }

        public History History { get; set; }

        public Investigation Investigation { get; set; }

        public Nature Nature { get; set; }

        public Religion Religion { get; set; }

        public AnimalHandling AnimalHandling { get; set; }

        public Insight Insight { get; set; }

        public Medicine Medicine { get; set; }

        public Perception Perception { get; set; }

        public Survival Survival { get; set; }

        public Deception Deception { get; set; }

        public Intimidation Intimidation { get; set; }

        public Performance Performance { get; set; }

        public Persuasion Persuasion { get; set; }
    }
}
