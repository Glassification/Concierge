using Concierge.Characters;
using Concierge.Characters.Collections;
using Concierge.SavingThrowsNamespace;
using Concierge.SkillsNamespace;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Concierge.Persistence
{
    public static class CharacterLoader
    {
        public static void LoadCharacterSheet(Character character, CcsFile ccsFile)
        {
            try
            {
                XDocument xml = XDocument.Load(ccsFile.AbsolutePath);

                XElement root = xml.Element("Character");
                XElement element;

                //-------------------------------------------------------------------------------------------------------
                // Parse Settings
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Settings");
                Settings.AutosaveEnable = bool.Parse((string)element.Element("AutoSave").Attribute("enabled"));
                Settings.AutosaveInterval = int.Parse((string)element.Element("AutoSave").Attribute("interval"));
                Settings.UseCoinWeight = bool.Parse((string)element.Element("CoinWeight").Attribute("ignore"));
                Settings.UseEncumbrance = bool.Parse((string)element.Element("Encumbrance").Attribute("use"));

                //-------------------------------------------------------------------------------------------------------
                // Parse Attributes
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Attributes");
                character.Attributes.Strength = int.Parse((string)element.Element("Strength").Attribute("value"));
                character.Attributes.Dexterity = int.Parse((string)element.Element("Dexterity").Attribute("value"));
                character.Attributes.Constitution = int.Parse((string)element.Element("Constitution").Attribute("value"));
                character.Attributes.Intelligence = int.Parse((string)element.Element("Intelligence").Attribute("value"));
                character.Attributes.Wisdom = int.Parse((string)element.Element("Wisdom").Attribute("value"));
                character.Attributes.Charisma = int.Parse((string)element.Element("Charisma").Attribute("value"));

                //-------------------------------------------------------------------------------------------------------
                // Parse Saving Throws
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Proficiency").Element("SavingThrows");
                character.SavingThrow.Strength = new Strength(proficiency:(bool)element.Element("Strength").Attribute("proficiency"));
                character.SavingThrow.Dexterity = new Dexterity(proficiency:(bool)element.Element("Dexterity").Attribute("proficiency"));
                character.SavingThrow.Constitution = new Constitution(proficiency:(bool)element.Element("Constitution").Attribute("proficiency"));
                character.SavingThrow.Intelligence = new Intelligence(proficiency:(bool)element.Element("Intelligence").Attribute("proficiency"));
                character.SavingThrow.Wisdom = new Wisdom(proficiency:(bool)element.Element("Wisdom").Attribute("proficiency"));
                character.SavingThrow.Charisma = new Charisma(proficiency:(bool)element.Element("Charisma").Attribute("proficiency"));

                //-------------------------------------------------------------------------------------------------------
                // Parse Skills
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Proficiency").Element("Skills");
                character.Skill.Athletics = new Athletics(proficiency:(bool)element.Element("Athletics").Attribute("proficiency"), expertise: (bool)element.Element("Athletics").Attribute("expertise"));
                character.Skill.Acrobatics = new Acrobatics(proficiency:(bool)element.Element("Acrobatics").Attribute("proficiency"), expertise: (bool)element.Element("Acrobatics").Attribute("expertise"));
                character.Skill.SleightOfHand = new SleightOfHand(proficiency:(bool)element.Element("SleightOfHand").Attribute("proficiency"), expertise: (bool)element.Element("SleightOfHand").Attribute("expertise"));
                character.Skill.Stealth = new Stealth(proficiency:(bool)element.Element("Stealth").Attribute("proficiency"), expertise: (bool)element.Element("Stealth").Attribute("expertise"));
                character.Skill.Arcana = new Arcana(proficiency:(bool)element.Element("Arcana").Attribute("proficiency"), expertise: (bool)element.Element("Arcana").Attribute("expertise"));
                character.Skill.History = new History(proficiency:(bool)element.Element("History").Attribute("proficiency"), expertise: (bool)element.Element("History").Attribute("expertise"));
                character.Skill.Investigation = new Investigation(proficiency:(bool)element.Element("Investigation").Attribute("proficiency"), expertise: (bool)element.Element("Investigation").Attribute("expertise"));
                character.Skill.Nature = new Nature(proficiency: (bool)element.Element("Nature").Attribute("proficiency"), expertise: (bool)element.Element("Nature").Attribute("expertise"));
                character.Skill.Religion = new Religion(proficiency: (bool)element.Element("Religion").Attribute("proficiency"), expertise: (bool)element.Element("Religion").Attribute("expertise"));
                character.Skill.AnimalHandling = new AnimalHandling(proficiency: (bool)element.Element("AnimalHandling").Attribute("proficiency"), expertise: (bool)element.Element("AnimalHandling").Attribute("expertise"));
                character.Skill.Insight = new Insight(proficiency: (bool)element.Element("Insight").Attribute("proficiency"), expertise: (bool)element.Element("Insight").Attribute("expertise"));
                character.Skill.Medicine = new Medicine(proficiency: (bool)element.Element("Medicine").Attribute("proficiency"), expertise: (bool)element.Element("Medicine").Attribute("expertise"));
                character.Skill.Perception = new Perception(proficiency: (bool)element.Element("Perception").Attribute("proficiency"), expertise: (bool)element.Element("Perception").Attribute("expertise"));
                character.Skill.Survival = new Survival(proficiency: (bool)element.Element("Survival").Attribute("proficiency"), expertise: (bool)element.Element("Survival").Attribute("expertise"));
                character.Skill.Deception = new Deception(proficiency: (bool)element.Element("Deception").Attribute("proficiency"), expertise: (bool)element.Element("Deception").Attribute("expertise"));
                character.Skill.Intimidation = new Intimidation(proficiency: (bool)element.Element("Intimidation").Attribute("proficiency"), expertise: (bool)element.Element("Intimidation").Attribute("expertise"));
                character.Skill.Performance = new Performance(proficiency: (bool)element.Element("Performance").Attribute("proficiency"), expertise: (bool)element.Element("Performance").Attribute("expertise"));
                character.Skill.Persuasion = new Persuasion(proficiency: (bool)element.Element("Persuasion").Attribute("proficiency"), expertise: (bool)element.Element("Persuasion").Attribute("expertise"));

                //-------------------------------------------------------------------------------------------------------
                // Parse Equipment
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Proficiency").Element("Equipment");

                var proficiencyArmors = element.Element("Armors").Elements("Armor");
                foreach (var proficiencyArmor in proficiencyArmors)
                {
                    character.Proficiency.Armors.Add(new Guid((string)proficiencyArmor.Attribute("id")), (string)proficiencyArmor.Attribute("value"));
                }

                var proficiencyShields = element.Element("Shields").Elements("Shield");
                foreach (var proficiencyShield in proficiencyShields)
                {
                    character.Proficiency.Shields.Add(new Guid((string)proficiencyShield.Attribute("id")), (string)proficiencyShield.Attribute("value"));
                }

                var proficiencyWeapons = element.Element("Weapons").Elements("Weapon");
                foreach (var proficiencyWeapon in proficiencyWeapons)
                {
                    character.Proficiency.Weapons.Add(new Guid((string)proficiencyWeapon.Attribute("id")), (string)proficiencyWeapon.Attribute("value"));
                }

                var proficiencyTools = element.Element("Tools").Elements("Tool");
                foreach (var proficiencyTool in proficiencyTools)
                {
                    character.Proficiency.Tools.Add(new Guid((string)proficiencyTool.Attribute("id")), (string)proficiencyTool.Attribute("value"));
                }

                //-------------------------------------------------------------------------------------------------------
                // Parse Details
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Details");
                character.Details.Name = (string)element.Element("Name").Attribute("value");
                character.Details.Race = (string)element.Element("Race").Attribute("value");
                character.Details.Background = (string)element.Element("Background").Attribute("value");
                character.Details.Alignment = (string)element.Element("Alignment").Attribute("value");
                character.Details.Experience = (string)element.Element("Experience").Attribute("value");
                character.Details.InitiativeBonus = (int)element.Element("InitiativeBonus").Attribute("value");
                character.Details.PerceptionBonus = (int)element.Element("PerceptionBonus").Attribute("value");
                character.Details.BaseMovement = (int)element.Element("Movement").Attribute("value");
                character.Details.Vision = (string)element.Element("Vision").Attribute("value");

                var classess = element.Element("Classes").Elements("Class");
                foreach (var @class in classess)
                {
                    Class c = new Class(new Guid((string)@class.Attribute("id")));

                    c.Name = (string)@class.Attribute("name");
                    c.Level = (int)@class.Attribute("level");

                    character.Classess.Add(c);
                }

                var languages = element.Element("Languages").Elements("Language");
                foreach (var language in languages)
                {
                    character.Details.Languages.Add(new Guid((string)language.Attribute("id")), (string)language.Attribute("value"));
                }

                //-------------------------------------------------------------------------------------------------------
                // Parse Appearance
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Appearance");
                character.Appearance.Gender = (string)element.Element("Gender").Attribute("value");
                character.Appearance.Age = (string)element.Element("Age").Attribute("value");
                character.Appearance.Height = (string)element.Element("Height").Attribute("value");
                character.Appearance.Weight = (string)element.Element("Weight").Attribute("value");
                character.Appearance.SkinColour = (string)element.Element("SkinColour").Attribute("value");
                character.Appearance.HairColour = (string)element.Element("HairColour").Attribute("value");
                character.Appearance.EyeColour = (string)element.Element("EyeColour").Attribute("value");
                character.Appearance.DistinguishingMarks = (string)element.Element("Marks").Attribute("value");

                //-------------------------------------------------------------------------------------------------------
                // Parse Personality
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Personality");
                character.Personality.Trait1 = (string)element.Element("Trait1").Attribute("value");
                character.Personality.Trait2 = (string)element.Element("Trait2").Attribute("value");
                character.Personality.Ideal = (string)element.Element("Ideal").Attribute("value");
                character.Personality.Bond = (string)element.Element("Bond").Attribute("value");
                character.Personality.Flaw = (string)element.Element("Flaw").Attribute("value");
                character.Personality.Background = (string)element.Element("Background").Attribute("value");
                character.Personality.Notes = (string)element.Element("Notes").Attribute("value");

                //-------------------------------------------------------------------------------------------------------
                // Parse Wealth
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Wealth");
                character.Wealth.Copper = (int)element.Element("Copper").Attribute("value");
                character.Wealth.Silver = (int)element.Element("Silver").Attribute("value");
                character.Wealth.Electrum = (int)element.Element("Electrum").Attribute("value");
                character.Wealth.Gold = (int)element.Element("Gold").Attribute("value");
                character.Wealth.Platinum = (int)element.Element("Platinum").Attribute("value");

                //-------------------------------------------------------------------------------------------------------
                // Parse class resource TODO
                //-------------------------------------------------------------------------------------------------------
                var resources = root.Element("ClassResources").Elements("ClassResource");

                foreach (XElement elem in resources)
                {
                    ClassResource classResource = new ClassResource(new Guid((string)elem.Attribute("id")))
                    {
                        Type = (string)elem.Attribute("type"),
                        Total = (int)elem.Attribute("pool"),
                        Spent = (int)elem.Attribute("spent")
                    };

                    character.ClassResources.Add(classResource);
                };

                //-------------------------------------------------------------------------------------------------------
                // Parse Armor Class
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("ArmorClass");
                character.Armor.Equiped = (string)element.Element("ArmorWorn").Attribute("value");
                character.Armor.Type = (Constants.ArmorType)Enum.Parse(typeof(Constants.ArmorType), (string)element.Element("ArmorType").Attribute("value"));
                character.Armor.ArmorClass = (int)element.Element("ArmorAC").Attribute("value");
                character.Armor.Strength = (int)element.Element("Strength").Attribute("value");
                character.Armor.Weight = (int)element.Element("ArmorWeight").Attribute("value");
                character.Armor.Stealth = (Constants.ArmorStealth)Enum.Parse(typeof(Constants.ArmorStealth), (string)element.Element("Stealth").Attribute("value"));
                character.Armor.Shield = (string)element.Element("Shield").Attribute("value");
                character.Armor.ShieldArmorClass = (int)element.Element("ShieldAC").Attribute("value");
                character.Armor.ShieldWeight = (int)element.Element("ShieldWeight").Attribute("value");
                character.Armor.MiscArmorClass = (int)element.Element("MiscAC").Attribute("value");
                character.Armor.MagicArmorClass = (int)element.Element("MagicAC").Attribute("value");

                //-------------------------------------------------------------------------------------------------------
                // Parse Hit Points
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("HitPoints");
                character.Vitality.MaxHealth = (int)element.Element("MaxHP").Attribute("value");
                character.Vitality.BaseHealth = (int)element.Element("CurrentHP").Attribute("value");
                character.Vitality.TemporaryHealth = (int)element.Element("TemporaryHP").Attribute("value");

                element = element.Element("Conditions");
                character.Vitality.Conditions.Blinded = (string)element.Element("Blinded").Attribute("value");
                character.Vitality.Conditions.Charmed = (string)element.Element("Charmed").Attribute("value");
                character.Vitality.Conditions.Deafened = (string)element.Element("Deafened").Attribute("value");
                character.Vitality.Conditions.Fatigued = (string)element.Element("Fatigued").Attribute("value");
                character.Vitality.Conditions.Frightened = (string)element.Element("Frightened").Attribute("value");
                character.Vitality.Conditions.Grappled = (string)element.Element("Grappled").Attribute("value");
                character.Vitality.Conditions.Incapacitated = (string)element.Element("Incapacitated").Attribute("value");
                character.Vitality.Conditions.Invisible = (string)element.Element("Invisible").Attribute("value");
                character.Vitality.Conditions.Paralyzed = (string)element.Element("Paralyzed").Attribute("value");
                character.Vitality.Conditions.Petrified = (string)element.Element("Petrified").Attribute("value");
                character.Vitality.Conditions.Poisoned = (string)element.Element("Poisoned").Attribute("value");
                character.Vitality.Conditions.Prone = (string)element.Element("Prone").Attribute("value");
                character.Vitality.Conditions.Restrained = (string)element.Element("Restrained").Attribute("value");
                character.Vitality.Conditions.Stunned = (string)element.Element("Stunned").Attribute("value");
                character.Vitality.Conditions.Unconscious = (string)element.Element("Unconscious").Attribute("value");

                element = root.Element("HitPoints").Element("HitDice");
                character.Vitality.HitDice.TotalD6 = (int)element.Element("D6").Attribute("total");
                character.Vitality.HitDice.TotalD8 = (int)element.Element("D8").Attribute("total");
                character.Vitality.HitDice.TotalD10 = (int)element.Element("D10").Attribute("total");
                character.Vitality.HitDice.TotalD12 = (int)element.Element("D12").Attribute("total");
                character.Vitality.HitDice.SpentD6 = (int)element.Element("D6").Attribute("spent");
                character.Vitality.HitDice.SpentD8 = (int)element.Element("D8").Attribute("spent");
                character.Vitality.HitDice.SpentD10 = (int)element.Element("D10").Attribute("spent");
                character.Vitality.HitDice.SpentD12 = (int)element.Element("D12").Attribute("spent");

                //-------------------------------------------------------------------------------------------------------
                // Parse Weapons
                //-------------------------------------------------------------------------------------------------------
                var weapons = root.Element("Weapons").Elements("Weapon");

                foreach (XElement elem in weapons)
                {
                    Weapon weapon = new Weapon(new Guid((string)elem.Attribute("id")))
                    {
                        Name = (string)elem.Attribute("name"),
                        Ability = (Constants.Abilities)Enum.Parse(typeof(Constants.Abilities), (string)elem.Attribute("ability")),
                        Damage = (string)elem.Attribute("dmg"),
                        Misc = (string)elem.Attribute("misc"),
                        DamageType = (Constants.DamageTypes)Enum.Parse(typeof(Constants.DamageTypes), (string)elem.Attribute("dmgType")),
                        Range = (string)elem.Attribute("range"),
                        Note = (string)elem.Attribute("notes"),
                        Weight = (double)elem.Attribute("weight"),
                        WeaponType = (Constants.WeaponTypes)Enum.Parse(typeof(Constants.WeaponTypes), (string)elem.Attribute("weaponType")),
                        ProficiencyOverride = bool.Parse((string)elem.Attribute("override"))
                    };

                    character.Weapons.Add(weapon);
                }

                //-------------------------------------------------------------------------------------------------------
                // Parse Ammo
                //-------------------------------------------------------------------------------------------------------
                var ammunitions = root.Element("Ammunitions").Elements("Ammunition");

                foreach (XElement elem in ammunitions)
                {
                    Ammunition ammunition = new Ammunition(new Guid((string)elem.Attribute("id")))
                    {
                        Name = (string)elem.Attribute("name"),
                        Quantity = (int)elem.Attribute("amount"),
                        Bonus = (string)elem.Attribute("miscDmg"),
                        DamageType = (Constants.DamageTypes)Enum.Parse(typeof(Constants.DamageTypes), (string)elem.Attribute("dmgType")),
                        Used = (int)elem.Attribute("used")
                    };

                    character.Ammunitions.Add(ammunition);
                }

                //-------------------------------------------------------------------------------------------------------
                // Parse Inventory
                //-------------------------------------------------------------------------------------------------------
                var inventories = root.Element("Inventory").Elements("Item");

                foreach (XElement elem in inventories)
                {
                    Inventory inventory = new Inventory(new Guid((string)elem.Attribute("id")))
                    {
                        Name = (string)elem.Attribute("name"),
                        Amount = (int)elem.Attribute("amount"),
                        Weight = (double)elem.Attribute("wgt"),
                        Note = (string)elem.Attribute("note")
                    };

                    character.Inventories.Add(inventory);
                }

                //-------------------------------------------------------------------------------------------------------
                // Parse Abilities
                //-------------------------------------------------------------------------------------------------------
                var abilities = root.Element("Abilities").Elements("Ability");

                foreach (XElement elem in abilities)
                {
                    Ability ability = new Ability(new Guid((string)elem.Attribute("id")))
                    {
                        Name = (string)elem.Attribute("name"),
                        Level = (string)elem.Attribute("level"),
                        Uses = (string)elem.Attribute("uses"),
                        Recovery = (string)elem.Attribute("recovery"),
                        Action = (string)elem.Attribute("action"),
                        Note = (string)elem.Attribute("notes")
                    };

                    character.Abilities.Add(ability);
                }

                //-------------------------------------------------------------------------------------------------------
                // Parse Spell Slots
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Spellcasting").Element("SpellSlots");
                character.SpellSlots.PactTotal = (int)element.Element("Pact").Attribute("total");
                character.SpellSlots.FirstTotal = (int)element.Element("One").Attribute("total");
                character.SpellSlots.SecondTotal = (int)element.Element("Two").Attribute("total");
                character.SpellSlots.ThirdTotal = (int)element.Element("Three").Attribute("total");
                character.SpellSlots.FourthTotal = (int)element.Element("Four").Attribute("total");
                character.SpellSlots.FifthTotal = (int)element.Element("Five").Attribute("total");
                character.SpellSlots.SixthTotal = (int)element.Element("Six").Attribute("total");
                character.SpellSlots.SeventhTotal = (int)element.Element("Seven").Attribute("total");
                character.SpellSlots.EighthTotal = (int)element.Element("Eight").Attribute("total");
                character.SpellSlots.NinethTotal = (int)element.Element("Nine").Attribute("total");

                character.SpellSlots.PactUsed = (int)element.Element("Pact").Attribute("used");
                character.SpellSlots.FirstUsed = (int)element.Element("One").Attribute("used");
                character.SpellSlots.SecondUsed = (int)element.Element("Two").Attribute("used");
                character.SpellSlots.ThirdUsed = (int)element.Element("Three").Attribute("used");
                character.SpellSlots.FourthUsed = (int)element.Element("Four").Attribute("used");
                character.SpellSlots.FifthUsed = (int)element.Element("Five").Attribute("used");
                character.SpellSlots.SixthUsed = (int)element.Element("Six").Attribute("used");
                character.SpellSlots.SeventhUsed = (int)element.Element("Seven").Attribute("used");
                character.SpellSlots.EighthUsed = (int)element.Element("Eight").Attribute("used");
                character.SpellSlots.NinethUsed = (int)element.Element("Nine").Attribute("used");

                //-------------------------------------------------------------------------------------------------------
                // Magic Classes
                //-------------------------------------------------------------------------------------------------------
                var classes = root.Element("Spellcasting").Element("SpellClasses").Elements("SpellClass");

                foreach (XElement elem in classes)
                {
                    MagicClass magic = new MagicClass(new Guid((string)elem.Attribute("id")))
                    {
                        Name = (string)elem.Attribute("class"),
                        Ability = (Constants.Abilities)Enum.Parse(typeof(Constants.Abilities), (string)elem.Attribute("ability")),
                        KnownCantrips = (int)elem.Attribute("cantrips"),
                        KnownSpells = (int)elem.Attribute("known"),
                        Level = (int)elem.Attribute("level")
                    };

                    character.MagicClasses.Add(magic);
                }

                //-------------------------------------------------------------------------------------------------------
                // Spell list
                //-------------------------------------------------------------------------------------------------------
                var spells = root.Element("Spellcasting").Element("SpellList").Elements("Spell");

                foreach (XElement elem in spells)
                {
                    Spell spell = new Spell(new Guid((string)elem.Attribute("id")))
                    {
                        Name = (string)elem.Attribute("name"),
                        Level = (string)elem.Attribute("level"),
                        Page = (string)elem.Attribute("page"),
                        School = (string)elem.Attribute("school"),
                        Ritual = (string)elem.Attribute("ritual"),
                        Components = (string)elem.Attribute("comp"),
                        Concentration = bool.Parse((string)elem.Attribute("concen")),
                        Range = (string)elem.Attribute("range"),
                        Duration = (string)elem.Attribute("duration"),
                        Area = (string)elem.Attribute("area"),
                        Save = (string)elem.Attribute("save"),
                        Damage = (string)elem.Attribute("damage"),
                        Description = (string)elem.Attribute("description"),
                        Prepared = bool.Parse((string)elem.Attribute("prepared"))
                    };

                    character.Spells.Add(spell);
                }

                //-------------------------------------------------------------------------------------------------------
                // Parse Companion TODO
                //-------------------------------------------------------------------------------------------------------
                element = root.Element("Companion");
                character.Companion.Name = (string)element.Element("Name").Attribute("value");
                character.Companion.ArmorClass = (string)element.Element("AC").Attribute("value");
                character.Companion.HitDice = (string)element.Element("HitDice").Attribute("value");
                character.Companion.Health = (string)element.Element("HP").Attribute("value");
                character.Companion.CurrentHealth = (string)element.Element("CurrentHP").Attribute("value");
                character.Companion.Speed = (string)element.Element("Speed").Attribute("value");
                character.Companion.Strength = (int)element.Element("Strength").Attribute("value");
                character.Companion.Dexterity = (int)element.Element("Dexterity").Attribute("value");
                character.Companion.Constitution = (int)element.Element("Constitution").Attribute("value");
                character.Companion.Intelligence = (int)element.Element("Intelligence").Attribute("value");
                character.Companion.Wisdom = (int)element.Element("Wisdom").Attribute("value");
                character.Companion.Charisma = (int)element.Element("Charisma").Attribute("value");
                character.Companion.Perception = (string)element.Element("Perception").Attribute("value");
                character.Companion.Senses = (string)element.Element("Senses").Attribute("value");
                //character.Companion.Attack = new Pair((string)element.Element("Attack").Attribute("one"), (string)element.Element("Attack").Attribute("two"));
                //character.Companion.Type = new Pair((string)element.Element("Type").Attribute("one"), (string)element.Element("Type").Attribute("two"));
                //character.Companion.AtkBonus = new Pair((string)element.Element("AtkBonus").Attribute("one"), (string)element.Element("AtkBonus").Attribute("two"));
                //character.Companion.Damage = new Pair((string)element.Element("Damage").Attribute("one"), (string)element.Element("Damage").Attribute("two"));
                //character.Companion.DmgType = new PresetPair((string)element.Element("DmgType").Attribute("one"), (string)element.Element("DmgType").Attribute("two"));
                //character.Companion.Reach = new Pair((string)element.Element("Reach").Attribute("one"), (string)element.Element("Reach").Attribute("two"));
                //character.Companion.Notes = new Pair((string)element.Element("Notes").Attribute("one"), (string)element.Element("Notes").Attribute("two"));

                //-------------------------------------------------------------------------------------------------------
                // Parse Campain Notes
                //-------------------------------------------------------------------------------------------------------
                var campainChapters = root.Element("CampainNotes").Elements("Chapter");

                foreach (XElement elem in campainChapters)
                {
                    Chapter chapter = new Chapter(new Guid((string)elem.Attribute("id")))
                    {
                        Name = (string)elem.Attribute("name"),
                    };

                    var campainNotes = elem.Elements("Note");

                    foreach (XElement innerElem in campainNotes)
                    {
                        Document document = new Document(new Guid((string)innerElem.Attribute("id")))
                        {
                            Name = (string)innerElem.Attribute("name"),
                            RTF = innerElem.Value
                        };

                        chapter.Documents.Add(document);
                    }

                    character.Chapters.Add(chapter);
                }

                Program.Modified = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                character.Reset();
                Program.Modified = true;
            }
        }
    }
}
