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
            Fatigued = "Normal";
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
        /// GetDescription()
        /// =========================================
        public string GetDescription(string name)
        {
            const string str = ", ";

            switch (name)
            {
                case "Blinded":
                    return BlindedDescription;
                case "Charmed":
                    return CharmedDescription;
                case "Deafened":
                    return DeafenedDescription;
                case "Encumbered":
                case "Heavily Encumbered":
                    return EncumbranceDescription;
                case "Frightened":
                    return FrightenedDescription;
                case "Grappled":
                    return GrappledDescription;
                case "Incapacitated":
                    return IncapacitatedDescription;
                case "Invisible":
                    return InvisibleDescription;
                case "Paralyzed":
                    return ParalyzedDescription;
                case "Petrified":
                    return PetrifiedDescription;
                case "Poisoned":
                    return PoisonedDescription;
                case "Prone":
                    return ProneDescription;
                case "Restrained":
                    return RestrainedDescription;
                case "Stunned":
                    return StunnedDescription;
                case "Unconscious":
                    return UnconsciousDescription;
                case "One":
                    return Exausted1;
                case "Two":
                    return Exausted1 + str + Exausted2;
                case "Three":
                    return Exausted1 + str + Exausted2 + str + Exausted3;
                case "Four":
                    return Exausted1 + str + Exausted2 + str + Exausted3 + str + Exausted4;
                case "Five":
                    return Exausted1 + str + Exausted2 + str + Exausted3 + str + Exausted4 + str + Exausted5;
                case "Six":
                    return Exausted6;
                default:
                    return string.Empty;
            }
        }

        /// =========================================
        /// ToArray()
        /// =========================================
        public List<KeyValuePair<string, string>> ToArray()
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();

            keyValuePairs.Add(new KeyValuePair<string, string>(Blinded,         "Blinded - "        + GetDescription(Blinded)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Charmed,         "Charmed - "        + GetDescription(Charmed)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Deafened,        "Deafened - "       + GetDescription(Deafened)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Encumbrance,     Encumbrance + " - " + GetDescription(Encumbrance)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Fatigued,        ToInteger(Fatigued) + " - "    + GetDescription(Fatigued)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Frightened,      "Frightened - "     + GetDescription(Frightened)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Grappled,        "Grappled - "       + GetDescription(Grappled)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Incapacitated,   "Incapacitated - "  + GetDescription(Incapacitated)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Invisible,       "Invisible - "      + GetDescription(Invisible)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Paralyzed,       "Paralyzed - "      + GetDescription(Paralyzed)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Petrified,       "Petrified - "      + GetDescription(Petrified)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Poisoned,        "Poisoned - "       + GetDescription(Poisoned)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Prone,           "Prone - "          + GetDescription(Prone)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Restrained,      "Restrained - "     + GetDescription(Restrained)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Stunned,         "Stunned - "        + GetDescription(Stunned)));
            keyValuePairs.Add(new KeyValuePair<string, string>(Unconscious,     "Unconscious - "    + GetDescription(Unconscious)));

            return keyValuePairs.Where(x => !x.Key.Equals("Cured") && !x.Key.Equals("Normal")).ToList();
        }

        private string ToInteger(string str)
        {
            switch (str)
            {
                case "One":
                    return "Exaustion 1";
                case "Two":
                    return "Exaustion 2";
                case "Three":
                    return "Exaustion 3";
                case "Four":
                    return "Exaustion 4";
                case "Five":
                    return "Exaustion 5";
                case "Six":
                    return "Exaustion 6";
                default:
                    return string.Empty;
            }
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
