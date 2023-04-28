// <copyright file="Skill.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySkills
{
    using System;

    using Concierge.Character.AbilitySkills.SkillTypes;
    using Concierge.Common;

    public sealed class Skill : ICopyable<Skill>
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

        public Skill DeepCopy()
        {
            return new Skill()
            {
                Athletics = (Athletics)this.Athletics.DeepCopy(),
                Acrobatics = (Acrobatics)this.Acrobatics.DeepCopy(),
                SleightOfHand = (SleightOfHand)this.SleightOfHand.DeepCopy(),
                Stealth = (Stealth)this.Stealth.DeepCopy(),
                Arcana = (Arcana)this.Arcana.DeepCopy(),
                History = (History)this.History.DeepCopy(),
                Investigation = (Investigation)this.Investigation.DeepCopy(),
                Nature = (Nature)this.Nature.DeepCopy(),
                Religion = (Religion)this.Religion.DeepCopy(),
                AnimalHandling = (AnimalHandling)this.AnimalHandling.DeepCopy(),
                Insight = (Insight)this.Insight.DeepCopy(),
                Medicine = (Medicine)this.Medicine.DeepCopy(),
                Perception = (Perception)this.Perception.DeepCopy(),
                Survival = (Survival)this.Survival.DeepCopy(),
                Deception = (Deception)this.Deception.DeepCopy(),
                Intimidation = (Intimidation)this.Intimidation.DeepCopy(),
                Performance = (Performance)this.Performance.DeepCopy(),
                Persuasion = (Persuasion)this.Persuasion.DeepCopy(),
            };
        }

        public Skills GetSkill(string name)
        {
            return name.ToLower() switch
            {
                "athletics" => this.Athletics,
                "acrobatics" => this.Acrobatics,
                "sleightofhand" => this.SleightOfHand,
                "stealth" => this.Stealth,
                "arcana" => this.Arcana,
                "history" => this.History,
                "investigation" => this.Investigation,
                "nature" => this.Nature,
                "religion" => this.Religion,
                "animalhandling" => this.AnimalHandling,
                "insight" => this.Insight,
                "medicine" => this.Medicine,
                "perception" => this.Perception,
                "survival" => this.Survival,
                "deception" => this.Deception,
                "intimidation" => this.Intimidation,
                "performance" => this.Performance,
                "persuasion" => this.Persuasion,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
