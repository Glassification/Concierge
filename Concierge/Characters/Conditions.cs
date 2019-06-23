using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Conditions
    {
        public const string BlindedDescription = "Automatically fail any ability checks. Attack rolls against you have advantage, your attacks have disadvantage.";
        public const string CharmedDescription = "You cannot attack the charmer. The charmer has advantage on ability checks when interacting socially.";
        public const string DeafenedDescription = "You cannot hear and automatically fails any ability check that requires hearing.";
        public const string EncumbranceDescription = "A carry weight exceding 5 and 10 times Strength will reduce speed by 10 and 20 respectivly.";
        public const string Exausted1 = "Disadvantage on Ability Checks";
        public const string Exausted2 = "Speed halved";
        public const string Exausted3 = "Disadvantage on Attack rolls and Saving Throws";
        public const string Exausted4 = "Hit point maximum halved";
        public const string Exausted5 = "Speed reduced to 0";
        public const string Exausted6 = "Death.";
        public const string FatiguedDescription = "Exaustion levels stack up to 6. A long rest reduces the level by 1.";
        public const string FrightenedDescription = "You have disadvantage on Ability Checks and Attack rolls while the source of fear is within line of sight. You can’t willingly move closer to the source.";
        public const string GrappledDescription = "Your speed becomes 0. It ends when the grappler is incapacitated or you are thrown away.";
        public const string IncapacitatedDescription = "You Cannot take Actions or reactions.";
        public const string InvisibleDescription = "You are impossible to see without the aid of magic or a Special sense. Attacks against you have disadvantage, your attacks have advantage.";
        public const string ParalyzedDescription = "You are incapacitated and automatically fail Strength and Dexterity Saving Throws. Attacks have advantage, and melee are auto crit.";
        public const string PetrifiedDescription = "You are transformed into an inanimate substance and are incapacitated. Resistant to all damage and immune to posion and disease.";
        public const string PoisonedDescription = "You have disadvantage on Attack rolls and Ability Checks";
        public const string ProneDescription = "Your only movement option is to crawl and have disadvantage on attacks. Melee attack is advantage, ranged is disadantage.";
        public const string RestrainedDescription = "Your speed becomes 0. Your attacks have disadvantage, enemies have advantage. Dexterity Saving Throws are disadvantage.";
        public const string StunnedDescription = "You are incapacitated and speak falteringly, and automatically fail Strength and Dexterity Saving Throws. Attacks against have advantage.";
        public const string UnconsciousDescription = "You are incapacitated, drop what you're holding, and fall prone. Attacks against have advantage and hits are auto crit.";

        public Conditions()
        {
            Blinded = "Cured";
            Charmed = "Cured";
            Deafened = "Cured";
            Fatigued = "Cured";
            Frightened = "Cured";
            Grappled = "Cured";
            Incapacitated = "Cured";
            Invisible = "Cured";
            Paralyzed = "Cured";
            Petrified = "Cured";
            Poisoned = "Cured";
            Prone = "Cured";
            Restrained = "Cured";
            Stunned = "Cured";
            Unconscious = "Cured";
        }

        /// =========================================
        /// FormatChecks()
        /// =========================================
        public void FormatChecks()
        {
            if (Fatigued.Equals("Exhaustion 1") || Fatigued.Equals("Exhaustion 2") || Fatigued.Equals("Exhaustion 3") || Fatigued.Equals("Exhaustion 4") || Fatigued.Equals("Exhaustion 5") || Afflicted(Frightened) || Afflicted(Poisoned))
            {
                Program.Character.Skill.SetSkillCheck(Constants.Checks.Disadvantage);
            }

            if (Afflicted(Blinded))
            {
                Program.Character.Skill.SetSkillCheck(Constants.Checks.Fail);
            }

            if (Fatigued.Equals("Exhaustion 3") || Fatigued.Equals("Exhaustion 4") || Fatigued.Equals("Exhaustion 5"))
            {
                Program.Character.SavingThrow.SetSavingThrowCheck(Constants.Checks.Disadvantage);
            }

            if (Afflicted(Restrained))
            {
                Program.Character.SavingThrow.Dexterity.Checks = Constants.Checks.Disadvantage;
            }

            if (Afflicted(Paralyzed) || Afflicted(Stunned))
            {
                Program.Character.SavingThrow.Strength.Checks = Constants.Checks.Fail;
                Program.Character.SavingThrow.Dexterity.Checks = Constants.Checks.Fail;
            }
        }

        /// =========================================
        /// GetDescription()
        /// =========================================
        public string GetDescription(string name)
        {
            string description;
            string str = ", ";

            switch (name)
            {
                case "Blinded":
                    description = BlindedDescription;
                    break;
                case "Charmed":
                    description = CharmedDescription;
                    break;
                case "Deafened":
                    description = DeafenedDescription;
                    break;
                case "Encumbered":
                case "Heavily Encumbered":
                    description = EncumbranceDescription;
                    break;
                case "Frightened":
                    description = FrightenedDescription;
                    break;
                case "Grappled":
                    description = GrappledDescription;
                    break;
                case "Incapacitated":
                    description = IncapacitatedDescription;
                    break;
                case "Invisible":
                    description = InvisibleDescription;
                    break;
                case "Paralyzed":
                    description = ParalyzedDescription;
                    break;
                case "Petrified":
                    description = PetrifiedDescription;
                    break;
                case "Poisoned":
                    description = PoisonedDescription;
                    break;
                case "Prone":
                    description = ProneDescription;
                    break;
                case "Restrained":
                    description = RestrainedDescription;
                    break;
                case "Stunned":
                    description = StunnedDescription;
                    break;
                case "Unconscious":
                    description = UnconsciousDescription;
                    break;
                case "Exhaustion 1":
                    description = Exausted1;
                    break;
                case "Exhaustion 2":
                    description = Exausted1 + str + Exausted2;
                    break;
                case "Exhaustion 3":
                    description = Exausted1 + str + Exausted2 + str + Exausted3;
                    break;
                case "Exhaustion 4":
                    description = Exausted1 + str + Exausted2 + str + Exausted3 + str + Exausted4;
                    break;
                case "Exhaustion 5":
                    description = Exausted1 + str + Exausted2 + str + Exausted3 + str + Exausted4 + str + Exausted5;
                    break;
                case "Exhaustion 6":
                    description = Exausted6;
                    break;
                default:
                    description = "";
                    break;
            }

            return description;
        }

        /// =========================================
        /// ToArray()
        /// =========================================
        public string[] ToArray()
        {
            string[] array = new string[16];

            array[0] = Blinded.Equals("Cured") ? "" : "Blinded";
            array[1] = Charmed.Equals("Cured") ? "" : "Charmed";
            array[2] = Deafened.Equals("Cured") ? "" : "Deafened";
            array[3] = Encumbrance.Equals("Normal") ? "" : Encumbrance;
            array[4] = Fatigued.Equals("Cured") ? "" : Fatigued;
            array[5] = Frightened.Equals("Cured") ? "" : "Frightened";
            array[6] = Grappled.Equals("Cured") ? "" : "Grappled";
            array[7] = Incapacitated.Equals("Cured") ? "" : "Incapacitated";
            array[8] = Invisible.Equals("Cured") ? "" : "Invisible";
            array[9] = Paralyzed.Equals("Cured") ? "" : "Paralyzed";
            array[10] = Petrified.Equals("Cured") ? "" : "Petrified";
            array[11] = Poisoned.Equals("Cured") ? "" : "Poisoned";
            array[12] = Prone.Equals("Cured") ? "" : "Prone";
            array[13] = Restrained.Equals("Cured") ? "" : "Restrained";
            array[14] = Stunned.Equals("Cured") ? "" : "Stunned";
            array[15] = Unconscious.Equals("Cured") ? "" : "Unconscious";

            return array;
        }

        public Conditions Copy()
        {
            Conditions copy = new Conditions();

            copy.Blinded = Blinded;
            copy.Charmed = Charmed;
            copy.Deafened = Deafened;
            copy.Fatigued = Fatigued;
            copy.Frightened = Frightened;
            copy.Grappled = Grappled;
            copy.Incapacitated = Incapacitated;
            copy.Invisible = Invisible;
            copy.Paralyzed = Paralyzed;
            copy.Petrified = Petrified;
            copy.Poisoned = Poisoned;
            copy.Prone = Prone;
            copy.Restrained = Restrained;
            copy.Stunned = Stunned;
            copy.Unconscious = Unconscious;

            return copy;
        }

        /// =========================================
        /// FormatMovement()
        /// =========================================
        public string FormatMovement()
        {
            int movement = Program.Character.Details.Movement;

            if (Fatigued.Equals("Exhaustion 5") || Grappled.Equals("Afflicted") || Restrained.Equals("Afflicted"))
            {
                movement = 0;
            }
            else
            {
                if (Encumbrance.Equals("Encumbered"))
                {
                    movement -= 10;
                }
                else if (Encumbrance.Equals("Heavily Encumbered"))
                {
                    movement -= 20;
                }

                if (Fatigued.Equals("Exhaustion 2") || Fatigued.Equals("Exhaustion 3") || Fatigued.Equals("Exhaustion 4"))
                {
                    movement /= 2;
                }
            }

            return movement.ToString();
        }

        /// =========================================
        /// FormatHealth()
        /// =========================================
        public string FormatHealth()
        {
            string health = Program.Character.Vitality.MaxHealth.ToString();
            int halfHP = Program.Character.Vitality.MaxHealth / 2;

            if (Fatigued.Equals("Exhaustion 4") || Fatigued.Equals("Exhaustion 5"))
            {
                health = halfHP.ToString();

                if (Program.Character.Vitality.CurrentHealth > halfHP)
                {
                    Program.Character.Vitality.CurrentHealth = halfHP;
                }
            }
            else if (Fatigued.Equals("Exhaustion 6"))
            {
                health = "0";
                Program.Character.Vitality.CurrentHealth = 0;
            }

            return health;
        }

        private bool Afflicted(string condition)
        {
            return !condition.Equals("Cured");
        }

        public string Blinded { get; set; }

        public string Charmed { get; set; }

        public string Deafened { get; set; }

        public string Fatigued { get; set; }

        public string Frightened { get; set; }

        public string Grappled { get; set; }

        public string Incapacitated { get; set; }

        public string Invisible { get; set; }

        public string Paralyzed { get; set; }

        public string Petrified { get; set; }

        public string Poisoned { get; set; }

        public string Prone { get; set; }

        public string Restrained { get; set; }

        public string Stunned { get; set; }

        public string Unconscious { get; set; }

        public string Encumbrance
        {
            get
            {
                string str = "Normal";

                if (Program.Character.Armor.Strength > Program.Character.Attributes.Strength)
                {
                    str = "Encumbered";
                }

                if (Settings.UseEncumbrance)
                {
                    if (Program.Character.CarryWeight > Program.Character.LightCarryCapacity && Program.Character.CarryWeight <= Program.Character.MediumCarryCapacity)
                    {
                        str = "Encumbered";
                    }
                    else if (Program.Character.CarryWeight > Program.Character.MediumCarryCapacity)
                    {
                        str = "Heavily Encumbered";
                    }

                }

                return str;
            }
        }
    }
}
