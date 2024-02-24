// <copyright file="Attributes.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Aspects
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    /// <summary>
    /// Represents a set of character attributes and associated skills in Dungeons &amp; Dragons.
    /// </summary>
    public sealed class Attributes : ICopyable<Attributes>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Attributes"/> class with default values.
        /// </summary>
        public Attributes()
        {
            this.Strength = new Strength();
            this.Dexterity = new Dexterity();
            this.Constitution = new Constitution();
            this.Intelligence = new Intelligence();
            this.Wisdom = new Wisdom();
            this.Charisma = new Charisma();

            this.Athletics = new Skill(SkillType.Athletics);
            this.Acrobatics = new Skill(SkillType.Acrobatics);
            this.SleightOfHand = new Skill(SkillType.SleightOfHand);
            this.Stealth = new Skill(SkillType.Stealth);
            this.Arcana = new Skill(SkillType.Arcana);
            this.History = new Skill(SkillType.History);
            this.Investigation = new Skill(SkillType.Investigation);
            this.Nature = new Skill(SkillType.Nature);
            this.Religion = new Skill(SkillType.Religion);
            this.AnimalHandling = new Skill(SkillType.AnimalHandling);
            this.Insight = new Skill(SkillType.Insight);
            this.Medicine = new Skill(SkillType.Medicine);
            this.Perception = new Skill(SkillType.Perception);
            this.Survival = new Skill(SkillType.Survival);
            this.Deception = new Skill(SkillType.Deception);
            this.Intimidation = new Skill(SkillType.Intimidation);
            this.Performance = new Skill(SkillType.Performance);
            this.Persuasion = new Skill(SkillType.Persuasion);
        }

        public Strength Strength { get; set; }

        public Dexterity Dexterity { get; set; }

        public Constitution Constitution { get; set; }

        public Intelligence Intelligence { get; set; }

        public Wisdom Wisdom { get; set; }

        public Charisma Charisma { get; set; }

        public Skill Athletics { get; set; }

        public Skill Acrobatics { get; set; }

        public Skill SleightOfHand { get; set; }

        public Skill Stealth { get; set; }

        public Skill Arcana { get; set; }

        public Skill History { get; set; }

        public Skill Investigation { get; set; }

        public Skill Nature { get; set; }

        public Skill Religion { get; set; }

        public Skill AnimalHandling { get; set; }

        public Skill Insight { get; set; }

        public Skill Medicine { get; set; }

        public Skill Perception { get; set; }

        public Skill Survival { get; set; }

        public Skill Deception { get; set; }

        public Skill Intimidation { get; set; }

        public Skill Performance { get; set; }

        public Skill Persuasion { get; set; }

        /// <summary>
        /// Creates a deep copy of the attributes.
        /// </summary>
        /// <returns>A deep copy of <see cref="Attributes"/>.</returns>
        public Attributes DeepCopy()
        {
            return new Attributes()
            {
                Strength = (Strength)this.Strength.DeepCopy(),
                Dexterity = (Dexterity)this.Dexterity.DeepCopy(),
                Constitution = (Constitution)this.Constitution.DeepCopy(),
                Intelligence = (Intelligence)this.Intelligence.DeepCopy(),
                Wisdom = (Wisdom)this.Wisdom.DeepCopy(),
                Charisma = (Charisma)this.Charisma.DeepCopy(),
                Athletics = this.Athletics.DeepCopy(),
                Acrobatics = this.Acrobatics.DeepCopy(),
                SleightOfHand = this.SleightOfHand.DeepCopy(),
                Stealth = this.Stealth.DeepCopy(),
                Arcana = this.Arcana.DeepCopy(),
                History = this.History.DeepCopy(),
                Investigation = this.Investigation.DeepCopy(),
                Nature = this.Nature.DeepCopy(),
                Religion = this.Religion.DeepCopy(),
                AnimalHandling = this.AnimalHandling.DeepCopy(),
                Insight = this.Insight.DeepCopy(),
                Medicine = this.Medicine.DeepCopy(),
                Perception = this.Perception.DeepCopy(),
                Survival = this.Survival.DeepCopy(),
                Deception = this.Deception.DeepCopy(),
                Intimidation = this.Intimidation.DeepCopy(),
                Performance = this.Performance.DeepCopy(),
                Persuasion = this.Persuasion.DeepCopy(),
            };
        }

        /// <summary>
        /// Determines whether any saving throws are currently in an proficient state.
        /// </summary>
        /// <returns>True if any saving throws are currently in an expertise state; otherwise, false.</returns>
        public bool GetState()
        {
            var trueCount = 0;
            trueCount += this.Strength.Proficiency ? 1 : 0;
            trueCount += this.Dexterity.Proficiency ? 1 : 0;
            trueCount += this.Constitution.Proficiency ? 1 : 0;
            trueCount += this.Intelligence.Proficiency ? 1 : 0;
            trueCount += this.Wisdom.Proficiency ? 1 : 0;
            trueCount += this.Charisma.Proficiency ? 1 : 0;

            return trueCount < Attribute.Count;
        }

        /// <summary>
        /// Determines whether any skills are currently in a proficient state.
        /// </summary>
        /// <returns>True if any skills are currently proficient; otherwise, false.</returns>
        public bool GetProficiencyState()
        {
            return this.GetState(false);
        }

        /// <summary>
        /// Determines whether any skills are currently in an expertise state.
        /// </summary>
        /// <returns>True if any skills are currently in an expertise state; otherwise, false.</returns>
        public bool GetExpertiseState()
        {
            return this.GetState(true);
        }

        /// <summary>
        /// Sets the proficiency state of all skills.
        /// </summary>
        /// <param name="state">The state to set (true for proficiency, false otherwise).</param>
        public void SetProficiencyState(bool state)
        {
            this.SetState(false, state);
        }

        /// <summary>
        /// Sets the expertise state of all skills.
        /// </summary>
        /// <param name="state">The state to set (true for expertise, false otherwise).</param>
        public void SetExpertiseState(bool state)
        {
            this.SetState(true, state);
        }

        /// <summary>
        /// Gets the total skill bonus for a given skill and associated bonus.
        /// </summary>
        /// <param name="skill">The skill for which to calculate the bonus.</param>
        /// <param name="bonus">The associated bonus (e.g., proficiency bonus).</param>
        /// <returns>The total skill bonus.</returns>
        public int GetSkillBonus(Skill skill, int bonus)
        {
            return skill.GetBonus(this.GetScore(skill.Type), bonus);
        }

        private bool GetState(bool isExpertise)
        {
            var trueCount = 0;
            trueCount += isExpertise ? (this.Athletics.Expertise ? 1 : 0) : (this.Athletics.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Acrobatics.Expertise ? 1 : 0) : (this.Acrobatics.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.SleightOfHand.Expertise ? 1 : 0) : (this.SleightOfHand.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Stealth.Expertise ? 1 : 0) : (this.Stealth.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Arcana.Expertise ? 1 : 0) : (this.Arcana.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.History.Expertise ? 1 : 0) : (this.History.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Investigation.Expertise ? 1 : 0) : (this.Investigation.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Nature.Expertise ? 1 : 0) : (this.Nature.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Religion.Expertise ? 1 : 0) : (this.Religion.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.AnimalHandling.Expertise ? 1 : 0) : (this.AnimalHandling.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Insight.Expertise ? 1 : 0) : (this.Insight.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Medicine.Expertise ? 1 : 0) : (this.Medicine.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Perception.Expertise ? 1 : 0) : (this.Perception.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Survival.Expertise ? 1 : 0) : (this.Survival.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Deception.Expertise ? 1 : 0) : (this.Deception.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Intimidation.Expertise ? 1 : 0) : (this.Intimidation.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Performance.Expertise ? 1 : 0) : (this.Performance.Proficiency ? 1 : 0);
            trueCount += isExpertise ? (this.Persuasion.Expertise ? 1 : 0) : (this.Persuasion.Proficiency ? 1 : 0);

            return trueCount < Skill.Count;
        }

        private void SetState(bool isExpertise, bool state)
        {
            _ = isExpertise ? this.Athletics.Expertise = state : this.Athletics.Proficiency = state;
            _ = isExpertise ? this.Acrobatics.Expertise = state : this.Acrobatics.Proficiency = state;
            _ = isExpertise ? this.SleightOfHand.Expertise = state : this.SleightOfHand.Proficiency = state;
            _ = isExpertise ? this.Stealth.Expertise = state : this.Stealth.Proficiency = state;
            _ = isExpertise ? this.Arcana.Expertise = state : this.Arcana.Proficiency = state;
            _ = isExpertise ? this.History.Expertise = state : this.History.Proficiency = state;
            _ = isExpertise ? this.Investigation.Expertise = state : this.Investigation.Proficiency = state;
            _ = isExpertise ? this.Nature.Expertise = state : this.Nature.Proficiency = state;
            _ = isExpertise ? this.Religion.Expertise = state : this.Religion.Proficiency = state;
            _ = isExpertise ? this.AnimalHandling.Expertise = state : this.AnimalHandling.Proficiency = state;
            _ = isExpertise ? this.Insight.Expertise = state : this.Insight.Proficiency = state;
            _ = isExpertise ? this.Medicine.Expertise = state : this.Medicine.Proficiency = state;
            _ = isExpertise ? this.Perception.Expertise = state : this.Perception.Proficiency = state;
            _ = isExpertise ? this.Survival.Expertise = state : this.Survival.Proficiency = state;
            _ = isExpertise ? this.Deception.Expertise = state : this.Deception.Proficiency = state;
            _ = isExpertise ? this.Intimidation.Expertise = state : this.Intimidation.Proficiency = state;
            _ = isExpertise ? this.Performance.Expertise = state : this.Performance.Proficiency = state;
            _ = isExpertise ? this.Persuasion.Expertise = state : this.Persuasion.Proficiency = state;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Readability.")]
        private int GetScore(SkillType skillType)
        {
            switch (skillType)
            {
                case SkillType.Athletics:
                    return this.Strength.Score;
                case SkillType.Acrobatics:
                case SkillType.SleightOfHand:
                case SkillType.Stealth:
                    return this.Dexterity.Score;
                case SkillType.Arcana:
                case SkillType.History:
                case SkillType.Investigation:
                case SkillType.Nature:
                case SkillType.Religion:
                    return this.Intelligence.Score;
                case SkillType.AnimalHandling:
                case SkillType.Insight:
                case SkillType.Medicine:
                case SkillType.Perception:
                case SkillType.Survival:
                    return this.Wisdom.Score;
                case SkillType.Deception:
                case SkillType.Intimidation:
                case SkillType.Performance:
                case SkillType.Persuasion:
                    return this.Charisma.Score;
                default:
                    return 0;
            }
        }
    }
}
