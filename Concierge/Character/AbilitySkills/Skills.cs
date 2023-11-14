// <copyright file="Skills.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySkills
{
    using System;

    using Concierge.Common;

    public sealed class Skills : ICopyable<Skills>
    {
        public Skills()
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

        public Skills DeepCopy()
        {
            return new Skills()
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

        public Skill GetSkill(string name)
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

        public void SetAllProficency(bool state)
        {
            this.Athletics.Proficiency = state;
            this.Acrobatics.Proficiency = state;
            this.SleightOfHand.Proficiency = state;
            this.Stealth.Proficiency = state;
            this.Arcana.Proficiency = state;
            this.History.Proficiency = state;
            this.Investigation.Proficiency = state;
            this.Nature.Proficiency = state;
            this.Religion.Proficiency = state;
            this.AnimalHandling.Proficiency = state;
            this.Insight.Proficiency = state;
            this.Medicine.Proficiency = state;
            this.Perception.Proficiency = state;
            this.Survival.Proficiency = state;
            this.Deception.Proficiency = state;
            this.Intimidation.Proficiency = state;
            this.Performance.Proficiency = state;
            this.Persuasion.Proficiency = state;
        }

        public void SetAllExpertise(bool state)
        {
            this.Athletics.Expertise = state;
            this.Acrobatics.Expertise = state;
            this.SleightOfHand.Expertise = state;
            this.Stealth.Expertise = state;
            this.Arcana.Expertise = state;
            this.History.Expertise = state;
            this.Investigation.Expertise = state;
            this.Nature.Expertise = state;
            this.Religion.Expertise = state;
            this.AnimalHandling.Expertise = state;
            this.Insight.Expertise = state;
            this.Medicine.Expertise = state;
            this.Perception.Expertise = state;
            this.Survival.Expertise = state;
            this.Deception.Expertise = state;
            this.Intimidation.Expertise = state;
            this.Performance.Expertise = state;
            this.Persuasion.Expertise = state;
        }

        public bool GetProficiencyState()
        {
            var trueCount = 0;

            trueCount += this.Athletics.Proficiency ? 1 : 0;
            trueCount += this.Acrobatics.Proficiency ? 1 : 0;
            trueCount += this.SleightOfHand.Proficiency ? 1 : 0;
            trueCount += this.Stealth.Proficiency ? 1 : 0;
            trueCount += this.Arcana.Proficiency ? 1 : 0;
            trueCount += this.History.Proficiency ? 1 : 0;
            trueCount += this.Investigation.Proficiency ? 1 : 0;
            trueCount += this.Nature.Proficiency ? 1 : 0;
            trueCount += this.Religion.Proficiency ? 1 : 0;
            trueCount += this.AnimalHandling.Proficiency ? 1 : 0;
            trueCount += this.Insight.Proficiency ? 1 : 0;
            trueCount += this.Medicine.Proficiency ? 1 : 0;
            trueCount += this.Perception.Proficiency ? 1 : 0;
            trueCount += this.Survival.Proficiency ? 1 : 0;
            trueCount += this.Deception.Proficiency ? 1 : 0;
            trueCount += this.Intimidation.Proficiency ? 1 : 0;
            trueCount += this.Performance.Proficiency ? 1 : 0;
            trueCount += this.Persuasion.Proficiency ? 1 : 0;

            return trueCount < 18;
        }

        public bool GetExpertiseState()
        {
            var trueCount = 0;

            trueCount += this.Athletics.Expertise ? 1 : 0;
            trueCount += this.Acrobatics.Expertise ? 1 : 0;
            trueCount += this.SleightOfHand.Expertise ? 1 : 0;
            trueCount += this.Stealth.Expertise ? 1 : 0;
            trueCount += this.Arcana.Expertise ? 1 : 0;
            trueCount += this.History.Expertise ? 1 : 0;
            trueCount += this.Investigation.Expertise ? 1 : 0;
            trueCount += this.Nature.Expertise ? 1 : 0;
            trueCount += this.Religion.Expertise ? 1 : 0;
            trueCount += this.AnimalHandling.Expertise ? 1 : 0;
            trueCount += this.Insight.Expertise ? 1 : 0;
            trueCount += this.Medicine.Expertise ? 1 : 0;
            trueCount += this.Perception.Expertise ? 1 : 0;
            trueCount += this.Survival.Expertise ? 1 : 0;
            trueCount += this.Deception.Expertise ? 1 : 0;
            trueCount += this.Intimidation.Expertise ? 1 : 0;
            trueCount += this.Performance.Expertise ? 1 : 0;
            trueCount += this.Persuasion.Expertise ? 1 : 0;

            return trueCount < 18;
        }
    }
}
