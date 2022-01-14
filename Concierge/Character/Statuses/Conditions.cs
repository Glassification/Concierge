// <copyright file="Conditions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Configuration;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Conditions : ICopyable<Conditions>
    {
        public const string BlindedDescription = "Automatically fail any ability checks. Attack rolls against you have advantage, your attacks have disadvantage.";
        public const string CharmedDescription = "You cannot attack the charmer. The charmer has advantage on ability checks when interacting socially.";
        public const string DeafenedDescription = "You cannot hear and automatically fails any ability check that requires hearing.";
        public const string EncumbranceDescription = "A carry weight exceeding 5 and 10 times Strength will reduce speed by 10 and 20 respectively.";
        public const string Exausted1 = "Disadvantage on Ability Checks";
        public const string Exausted2 = "Speed halved";
        public const string Exausted3 = "Disadvantage on Attack rolls and Saving Throws";
        public const string Exausted4 = "Hit point maximum halved";
        public const string Exausted5 = "Speed reduced to 0";
        public const string Exausted6 = "Death.";
        public const string FatiguedDescription = "Exhaustion levels stack up to 6. A long rest reduces the level by 1.";
        public const string FrightenedDescription = "You have disadvantage on Ability Checks and Attack rolls while the source of fear is within line of sight. You can’t willingly move closer to the source.";
        public const string GrappledDescription = "Your speed becomes 0. It ends when the grappler is incapacitated or you are thrown away.";
        public const string IncapacitatedDescription = "You Cannot take Actions or reactions.";
        public const string InvisibleDescription = "You are impossible to see without the aid of magic or a Special sense. Attacks against you have disadvantage, your attacks have advantage.";
        public const string ParalyzedDescription = "You are incapacitated and automatically fail Strength and Dexterity Saving Throws. Attacks have advantage, and melee are auto crit.";
        public const string PetrifiedDescription = "You are transformed into an inanimate substance and are incapacitated. Resistant to all damage and immune to poison and disease.";
        public const string PoisonedDescription = "You have disadvantage on Attack rolls and Ability Checks";
        public const string ProneDescription = "Your only movement option is to crawl and have disadvantage on attacks. Melee attack is advantage, ranged is disadvantage.";
        public const string RestrainedDescription = "Your speed becomes 0. Your attacks have disadvantage, enemies have advantage. Dexterity Saving Throws are disadvantage.";
        public const string StunnedDescription = "You are incapacitated and speak falteringly, and automatically fail Strength and Dexterity Saving Throws. Attacks against have advantage.";
        public const string UnconsciousDescription = "You are incapacitated, drop what you're holding, and fall prone. Attacks against have advantage and hits are auto crit.";

        public Conditions()
        {
            this.Blinded = "Cured";
            this.Charmed = "Cured";
            this.Deafened = "Cured";
            this.Fatigued = "Normal";
            this.Frightened = "Cured";
            this.Grappled = "Cured";
            this.Incapacitated = "Cured";
            this.Invisible = "Cured";
            this.Paralyzed = "Cured";
            this.Petrified = "Cured";
            this.Poisoned = "Cured";
            this.Prone = "Cured";
            this.Restrained = "Cured";
            this.Stunned = "Cured";
            this.Unconscious = "Cured";
        }

        [JsonIgnore]
        public static string Encumbrance
        {
            get
            {
                var str = "Normal";

                if (Program.CcsFile.Character.Armor.Strength > Program.CcsFile.Character.Attributes.Strength)
                {
                    str = "Encumbered";
                }

                if (AppSettingsManager.UserSettings.UseEncumbrance)
                {
                    if (Program.CcsFile.Character.CarryWeight > Program.CcsFile.Character.LightCarryCapacity && Program.CcsFile.Character.CarryWeight <= Program.CcsFile.Character.MediumCarryCapacity)
                    {
                        str = "Encumbered";
                    }
                    else if (Program.CcsFile.Character.CarryWeight > Program.CcsFile.Character.MediumCarryCapacity)
                    {
                        str = "Heavily Encumbered";
                    }
                }

                return str;
            }
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

        /// =========================================
        /// GetDescription()
        /// =========================================
        public static string GetDescription(string name)
        {
            const string Comma = ", ";

            return name switch
            {
                "Blinded" => BlindedDescription,
                "Charmed" => CharmedDescription,
                "Deafened" => DeafenedDescription,
                "Encumbered" or "Heavily Encumbered" => EncumbranceDescription,
                "Frightened" => FrightenedDescription,
                "Grappled" => GrappledDescription,
                "Incapacitated" => IncapacitatedDescription,
                "Invisible" => InvisibleDescription,
                "Paralyzed" => ParalyzedDescription,
                "Petrified" => PetrifiedDescription,
                "Poisoned" => PoisonedDescription,
                "Prone" => ProneDescription,
                "Restrained" => RestrainedDescription,
                "Stunned" => StunnedDescription,
                "Unconscious" => UnconsciousDescription,
                "One" => Exausted1,
                "Two" => Exausted1 + Comma + Exausted2,
                "Three" => Exausted1 + Comma + Exausted2 + Comma + Exausted3,
                "Four" => Exausted1 + Comma + Exausted2 + Comma + Exausted3 + Comma + Exausted4,
                "Five" => Exausted1 + Comma + Exausted2 + Comma + Exausted3 + Comma + Exausted4 + Comma + Exausted5,
                "Six" => Exausted6,
                _ => string.Empty,
            };
        }

        /// =========================================
        /// ToArray()
        /// =========================================
        public List<KeyValuePair<string, string>> ToList()
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(this.Blinded, "Blinded - " + GetDescription(this.Blinded)),
                new KeyValuePair<string, string>(this.Charmed, "Charmed - " + GetDescription(this.Charmed)),
                new KeyValuePair<string, string>(this.Deafened, "Deafened - " + GetDescription(this.Deafened)),
                new KeyValuePair<string, string>(Encumbrance, Encumbrance + " - " + GetDescription(Encumbrance)),
                new KeyValuePair<string, string>(this.Fatigued, ToInteger(this.Fatigued) + " - " + GetDescription(this.Fatigued)),
                new KeyValuePair<string, string>(this.Frightened, "Frightened - " + GetDescription(this.Frightened)),
                new KeyValuePair<string, string>(this.Grappled, "Grappled - " + GetDescription(this.Grappled)),
                new KeyValuePair<string, string>(this.Incapacitated, "Incapacitated - " + GetDescription(this.Incapacitated)),
                new KeyValuePair<string, string>(this.Invisible, "Invisible - " + GetDescription(this.Invisible)),
                new KeyValuePair<string, string>(this.Paralyzed, "Paralyzed - " + GetDescription(this.Paralyzed)),
                new KeyValuePair<string, string>(this.Petrified, "Petrified - " + GetDescription(this.Petrified)),
                new KeyValuePair<string, string>(this.Poisoned, "Poisoned - " + GetDescription(this.Poisoned)),
                new KeyValuePair<string, string>(this.Prone, "Prone - " + GetDescription(this.Prone)),
                new KeyValuePair<string, string>(this.Restrained, "Restrained - " + GetDescription(this.Restrained)),
                new KeyValuePair<string, string>(this.Stunned, "Stunned - " + GetDescription(this.Stunned)),
                new KeyValuePair<string, string>(this.Unconscious, "Unconscious - " + GetDescription(this.Unconscious)),
            };

            return keyValuePairs.Where(x => !x.Key.Equals("Cured") && !x.Key.Equals("Normal")).ToList();
        }

        public Conditions DeepCopy()
        {
            return new Conditions()
            {
                Blinded = this.Blinded,
                Charmed = this.Charmed,
                Deafened = this.Deafened,
                Fatigued = this.Fatigued,
                Frightened = this.Frightened,
                Grappled = this.Grappled,
                Incapacitated = this.Incapacitated,
                Invisible = this.Invisible,
                Paralyzed = this.Paralyzed,
                Petrified = this.Petrified,
                Poisoned = this.Poisoned,
                Prone = this.Prone,
                Restrained = this.Restrained,
                Stunned = this.Stunned,
                Unconscious = this.Unconscious,
            };
        }

        private static string ToInteger(string str)
        {
            return str switch
            {
                "One" => "Exhaustion 1",
                "Two" => "Exhaustion 2",
                "Three" => "Exhaustion 3",
                "Four" => "Exhaustion 4",
                "Five" => "Exhaustion 5",
                "Six" => "Exhaustion 6",
                _ => string.Empty,
            };
        }
    }
}
