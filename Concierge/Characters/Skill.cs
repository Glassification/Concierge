using Concierge.SkillsNamespace;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Skill
    {
        public Skill()
        {
            Athletics = new Athletics();
            Acrobatics = new Acrobatics();
            SleightOfHand = new SleightOfHand();
            Stealth = new Stealth();
            Arcana = new Arcana();
            History = new History();
            Investigation = new Investigation();
            Nature = new Nature();
            Religion = new Religion();
            AnimalHandling = new AnimalHandling();
            Insight = new Insight();
            Medicine = new Medicine();
            Perception = new Perception();
            Survival = new Survival();
            Deception = new Deception();
            Intimidation = new Intimidation();
            Performance = new Performance();
            Persuasion = new Persuasion();

            SetSkillCheck(Constants.Checks.Normal);
        }

        public void SetSkillCheck(Constants.Checks checks)
        {
            Athletics.Checks = checks;
            Acrobatics.Checks = checks;
            SleightOfHand.Checks = checks;
            Stealth.Checks = checks;
            Arcana.Checks = checks;
            History.Checks = checks;
            Investigation.Checks = checks;
            Nature.Checks = checks;
            Religion.Checks = checks;
            AnimalHandling.Checks = checks;
            Insight.Checks = checks;
            Medicine.Checks = checks;
            Perception.Checks = checks;
            Survival.Checks = checks;
            Deception.Checks = checks;
            Intimidation.Checks = checks;
            Performance.Checks = checks;
            Persuasion.Checks = checks;
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
