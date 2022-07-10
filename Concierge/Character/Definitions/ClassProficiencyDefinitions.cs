// <copyright file="ClassProficiencyDefinitions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Definitions
{
    using System.Collections.Generic;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;

    public static class ClassProficiencyDefinitions
    {
        public static List<Proficiency> GetBarbarianProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Shields", ProficiencyTypes.Shield),
                    new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                new Proficiency("Shields", ProficiencyTypes.Shield),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetBardProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                new Proficiency("Hand Crossbow", ProficiencyTypes.Weapon),
                new Proficiency("Longsword", ProficiencyTypes.Weapon),
                new Proficiency("Rapier", ProficiencyTypes.Weapon),
                new Proficiency("Shortsword", ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetClericProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                    new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                    new Proficiency("Shields", ProficiencyTypes.Shield),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                new Proficiency("Shields", ProficiencyTypes.Shield),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetDruidProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                    new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                    new Proficiency("Shields", ProficiencyTypes.Shield),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                new Proficiency("Shields", ProficiencyTypes.Shield),
                new Proficiency("Club", ProficiencyTypes.Weapon),
                new Proficiency("Dagger", ProficiencyTypes.Weapon),
                new Proficiency("Dart", ProficiencyTypes.Weapon),
                new Proficiency("Javelin", ProficiencyTypes.Weapon),
                new Proficiency("Mace", ProficiencyTypes.Weapon),
                new Proficiency("Quarterstaff", ProficiencyTypes.Weapon),
                new Proficiency("Scimitar", ProficiencyTypes.Weapon),
                new Proficiency("Sickle", ProficiencyTypes.Weapon),
                new Proficiency("Sling", ProficiencyTypes.Weapon),
                new Proficiency("Spear", ProficiencyTypes.Weapon),
                new Proficiency("Herbalism Kit", ProficiencyTypes.Tool),
            };
        }

        public static List<Proficiency> GetFighterProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                    new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                    new Proficiency("Shields", ProficiencyTypes.Shield),
                    new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                new Proficiency("Heavy Armor", ProficiencyTypes.Armor),
                new Proficiency("Massive Armor", ProficiencyTypes.Armor),
                new Proficiency("Shields", ProficiencyTypes.Shield),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetMonkProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                    new Proficiency("Shortswords", ProficiencyTypes.Weapon),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                new Proficiency("Shortswords", ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetPaladinProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                    new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                    new Proficiency("Shields", ProficiencyTypes.Shield),
                    new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                new Proficiency("Heavy Armor", ProficiencyTypes.Armor),
                new Proficiency("Massive Armor", ProficiencyTypes.Armor),
                new Proficiency("Shields", ProficiencyTypes.Shield),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetRangerProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                    new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                    new Proficiency("Shields", ProficiencyTypes.Shield),
                    new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency("Medium Armor", ProficiencyTypes.Armor),
                new Proficiency("Shields", ProficiencyTypes.Shield),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.MartialRanged, ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetRogueProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                    new Proficiency("Thieves' Tools", ProficiencyTypes.Tool),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                new Proficiency("Hand Crossbow", ProficiencyTypes.Weapon),
                new Proficiency("Longsword", ProficiencyTypes.Weapon),
                new Proficiency("Rapier", ProficiencyTypes.Weapon),
                new Proficiency("Shortsword", ProficiencyTypes.Weapon),
                new Proficiency("Thieves' Tools", ProficiencyTypes.Tool),
            };
        }

        public static List<Proficiency> GetSorcererProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>();
            }

            return new List<Proficiency>()
            {
                new Proficiency("Dagger", ProficiencyTypes.Weapon),
                new Proficiency("Dart", ProficiencyTypes.Weapon),
                new Proficiency("Sling", ProficiencyTypes.Weapon),
                new Proficiency("Quarterstaff", ProficiencyTypes.Weapon),
                new Proficiency("Light Crossbow", ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetWarlockProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>()
                {
                    new Proficiency("Light Armor", ProficiencyTypes.Armor),
                    new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                    new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
                };
            }

            return new List<Proficiency>()
            {
                new Proficiency("Light Armor", ProficiencyTypes.Armor),
                new Proficiency(Proficiency.SimpleMelee, ProficiencyTypes.Weapon),
                new Proficiency(Proficiency.SimpleRanged, ProficiencyTypes.Weapon),
            };
        }

        public static List<Proficiency> GetWizardProficiencies(bool multiClass)
        {
            if (multiClass)
            {
                return new List<Proficiency>();
            }

            return new List<Proficiency>()
            {
                new Proficiency("Dagger", ProficiencyTypes.Weapon),
                new Proficiency("Dart", ProficiencyTypes.Weapon),
                new Proficiency("Sling", ProficiencyTypes.Weapon),
                new Proficiency("Quarterstaff", ProficiencyTypes.Weapon),
                new Proficiency("Light Crossbow", ProficiencyTypes.Weapon),
            };
        }
    }
}
