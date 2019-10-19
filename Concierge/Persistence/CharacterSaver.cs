using Concierge.Characters;
using System.Linq;
using System.Xml.Linq;
using Concierge.Utility;
using System;

namespace Concierge.Persistence
{
    public static class CharacterSaver
    {
        public static void SaveCharacterSheet(Character character, CcsFile ccsFile)
        {
            try
            {
                XDocument xml = new XDocument(
                    new XElement("Character",
                        new XElement("Settings",
                            new XElement("AutoSave", new XAttribute("enabled", Settings.AutosaveEnable), new XAttribute("interval", Settings.AutosaveInterval)),
                            new XElement("CoinWeight", new XAttribute("ignore", Settings.UseCoinWeight)),
                            new XElement("Encumbrance", new XAttribute("use", Settings.UseEncumbrance))
                            ),
                        new XElement("Attributes",
                            new XElement("Strength", new XAttribute("value", character.Attributes.Strength)),
                            new XElement("Dexterity", new XAttribute("value", character.Attributes.Dexterity)),
                            new XElement("Constitution", new XAttribute("value", character.Attributes.Constitution)),
                            new XElement("Intelligence", new XAttribute("value", character.Attributes.Intelligence)),
                            new XElement("Wisdom", new XAttribute("value", character.Attributes.Wisdom)),
                            new XElement("Charisma", new XAttribute("value", character.Attributes.Charisma))
                            ),
                        new XElement("Proficiency",
                            new XElement("SavingThrows",
                                new XElement("Strength", new XAttribute("proficiency", character.SavingThrow.Strength.Proficiency)),
                                new XElement("Dexterity", new XAttribute("proficiency", character.SavingThrow.Dexterity.Proficiency)),
                                new XElement("Constitution", new XAttribute("proficiency", character.SavingThrow.Constitution.Proficiency)),
                                new XElement("Intelligence", new XAttribute("proficiency", character.SavingThrow.Intelligence.Proficiency)),
                                new XElement("Wisdom", new XAttribute("proficiency", character.SavingThrow.Wisdom.Proficiency)),
                                new XElement("Charisma", new XAttribute("proficiency", character.SavingThrow.Charisma.Proficiency))
                                ),
                            new XElement("Skills",
                                new XElement("Athletics", new XAttribute("proficiency", character.Skill.Athletics.Proficiency), new XAttribute("expertise", character.Skill.Athletics.Expertise)),
                                new XElement("Acrobatics", new XAttribute("proficiency", character.Skill.Acrobatics.Proficiency), new XAttribute("expertise", character.Skill.Acrobatics.Expertise)),
                                new XElement("SleightOfHand", new XAttribute("proficiency", character.Skill.SleightOfHand.Proficiency), new XAttribute("expertise", character.Skill.SleightOfHand.Expertise)),
                                new XElement("Stealth", new XAttribute("proficiency", character.Skill.Stealth.Proficiency), new XAttribute("expertise", character.Skill.Stealth.Expertise)),
                                new XElement("Arcana", new XAttribute("proficiency", character.Skill.Arcana.Proficiency), new XAttribute("expertise", character.Skill.Arcana.Expertise)),
                                new XElement("History", new XAttribute("proficiency", character.Skill.History.Proficiency), new XAttribute("expertise", character.Skill.History.Expertise)),
                                new XElement("Investigation", new XAttribute("proficiency", character.Skill.Investigation.Proficiency), new XAttribute("expertise", character.Skill.Investigation.Expertise)),
                                new XElement("Nature", new XAttribute("proficiency", character.Skill.Nature.Proficiency), new XAttribute("expertise", character.Skill.Nature.Expertise)),
                                new XElement("Religion", new XAttribute("proficiency", character.Skill.Religion.Proficiency), new XAttribute("expertise", character.Skill.Religion.Expertise)),
                                new XElement("AnimalHandling", new XAttribute("proficiency", character.Skill.AnimalHandling.Proficiency), new XAttribute("expertise", character.Skill.AnimalHandling.Expertise)),
                                new XElement("Insight", new XAttribute("proficiency", character.Skill.Insight.Proficiency), new XAttribute("expertise", character.Skill.Insight.Expertise)),
                                new XElement("Medicine", new XAttribute("proficiency", character.Skill.Medicine.Proficiency), new XAttribute("expertise", character.Skill.Medicine.Expertise)),
                                new XElement("Perception", new XAttribute("proficiency", character.Skill.Perception.Proficiency), new XAttribute("expertise", character.Skill.Perception.Expertise)),
                                new XElement("Survival", new XAttribute("proficiency", character.Skill.Survival.Proficiency), new XAttribute("expertise", character.Skill.Survival.Expertise)),
                                new XElement("Deception", new XAttribute("proficiency", character.Skill.Deception.Proficiency), new XAttribute("expertise", character.Skill.Deception.Expertise)),
                                new XElement("Intimidation", new XAttribute("proficiency", character.Skill.Intimidation.Proficiency), new XAttribute("expertise", character.Skill.Intimidation.Expertise)),
                                new XElement("Performance", new XAttribute("proficiency", character.Skill.Performance.Proficiency), new XAttribute("expertise", character.Skill.Performance.Expertise)),
                                new XElement("Persuasion", new XAttribute("proficiency", character.Skill.Persuasion.Proficiency), new XAttribute("expertise", character.Skill.Persuasion.Expertise))
                                ),
                            new XElement("Equipment",
                                new XElement("Armors",
                                    from armor in character.Proficiency.Armors
                                    select
                                        new XElement("Armor",
                                            new XAttribute("value", armor.Value),
                                            new XAttribute("id", armor.Key)
                                            )
                                ),
                                new XElement("Shields",
                                    from shield in character.Proficiency.Shields
                                    select
                                        new XElement("Shield",
                                            new XAttribute("value", shield.Value),
                                            new XAttribute("id", shield.Key)
                                            )
                                ),
                                new XElement("Weapons",
                                    from weapon in character.Proficiency.Weapons
                                    select
                                        new XElement("Weapon",
                                            new XAttribute("value", weapon.Value),
                                            new XAttribute("id", weapon.Key)
                                            )
                                ),
                                new XElement("Tools",
                                    from tool in character.Proficiency.Tools
                                    select
                                        new XElement("Tool",
                                            new XAttribute("value", tool.Value),
                                            new XAttribute("id", tool.Key)
                                            )
                                ))
                            ),
                        new XElement("Details",
                            new XElement("Name", new XAttribute("value", character.Details.Name)),
                            new XElement("Race", new XAttribute("value", character.Details.Race)),
                            new XElement("Background", new XAttribute("value", character.Details.Background)),
                            new XElement("Alignment", new XAttribute("value", character.Details.Alignment)),
                            new XElement("Classes",
                                    from @class in character.Classess
                                    select
                                        new XElement("Class",
                                            new XAttribute("name", @class.Name),
                                            new XAttribute("level", @class.Level),
                                            new XAttribute("id", @class.ID)
                                            )
                                ),
                            new XElement("Experience", new XAttribute("value", character.Details.Experience)),
                            new XElement("Languages",
                                    from language in character.Details.Languages
                                    select
                                        new XElement("Language",
                                            new XAttribute("value", language.Value),
                                            new XAttribute("id", language.Key)
                                            )
                                ),
                            new XElement("InitiativeBonus", new XAttribute("value", character.Details.InitiativeBonus)),
                            new XElement("PerceptionBonus", new XAttribute("value", character.Details.PerceptionBonus)),
                            new XElement("Movement", new XAttribute("value", character.Details.BaseMovement)),
                            new XElement("Vision", new XAttribute("value", character.Details.Vision))
                            ),
                        new XElement("Appearance",
                            new XElement("Gender", new XAttribute("value", character.Appearance.Gender)),
                            new XElement("Age", new XAttribute("value", character.Appearance.Age)),
                            new XElement("Height", new XAttribute("value", character.Appearance.Height)),
                            new XElement("Weight", new XAttribute("value", character.Appearance.Weight)),
                            new XElement("SkinColour", new XAttribute("value", character.Appearance.SkinColour)),
                            new XElement("HairColour", new XAttribute("value", character.Appearance.HairColour)),
                            new XElement("EyeColour", new XAttribute("value", character.Appearance.EyeColour)),
                            new XElement("Marks", new XAttribute("value", character.Appearance.DistinguishingMarks))
                            ),
                        new XElement("Personality",
                            new XElement("Trait1", new XAttribute("value", character.Personality.Trait1)),
                            new XElement("Trait2", new XAttribute("value", character.Personality.Trait2)),
                            new XElement("Ideal", new XAttribute("value", character.Personality.Ideal)),
                            new XElement("Bond", new XAttribute("value", character.Personality.Bond)),
                            new XElement("Flaw", new XAttribute("value", character.Personality.Flaw)),
                            new XElement("Background", new XAttribute("value", character.Personality.Background)),
                            new XElement("Notes", new XAttribute("value", character.Personality.Notes))
                            ),
                        new XElement("Wealth",
                            new XElement("Copper", new XAttribute("value", character.Wealth.Copper)),
                            new XElement("Silver", new XAttribute("value", character.Wealth.Silver)),
                            new XElement("Electrum", new XAttribute("value", character.Wealth.Electrum)),
                            new XElement("Gold", new XAttribute("value", character.Wealth.Gold)),
                            new XElement("Platinum", new XAttribute("value", character.Wealth.Platinum))
                            ),
                        new XElement("ClassResources",
                                    from classResource in character.ClassResources
                                    select
                                        new XElement("ClassResource",
                                            new XAttribute("type", classResource.Type),
                                            new XAttribute("pool", classResource.Total),
                                            new XAttribute("spent", classResource.Spent),
                                            new XAttribute("id", classResource.ID)
                                            )
                                ),
                        new XElement("ArmorClass",
                            new XElement("ArmorWorn", new XAttribute("value", character.Armor.Equiped)),
                            new XElement("ArmorType", new XAttribute("value", character.Armor.Type)),
                            new XElement("ArmorAC", new XAttribute("value", character.Armor.ArmorClass)),
                            new XElement("Strength", new XAttribute("value", character.Armor.Strength)),
                            new XElement("ArmorWeight", new XAttribute("value", character.Armor.Weight)),
                            new XElement("Stealth", new XAttribute("value", character.Armor.Stealth)),
                            new XElement("Shield", new XAttribute("value", character.Armor.Shield)),
                            new XElement("ShieldAC", new XAttribute("value", character.Armor.ShieldArmorClass)),
                            new XElement("ShieldWeight", new XAttribute("value", character.Armor.ShieldWeight)),
                            new XElement("MiscAC", new XAttribute("value", character.Armor.MiscArmorClass)),
                            new XElement("MagicAC", new XAttribute("value", character.Armor.MagicArmorClass))
                            ),
                        new XElement("HitPoints",
                            new XElement("MaxHP", new XAttribute("value", character.Vitality.MaxHealth)),
                            new XElement("CurrentHP", new XAttribute("value", character.Vitality.BaseHealth)),
                            new XElement("TemporaryHP", new XAttribute("value", character.Vitality.TemporaryHealth)),
                            new XElement("Conditions",
                                new XElement("Blinded", new XAttribute("value", character.Vitality.Conditions.Blinded)),
                                new XElement("Charmed", new XAttribute("value", character.Vitality.Conditions.Charmed)),
                                new XElement("Deafened", new XAttribute("value", character.Vitality.Conditions.Deafened)),
                                new XElement("Fatigued", new XAttribute("value", character.Vitality.Conditions.Fatigued)),
                                new XElement("Frightened", new XAttribute("value", character.Vitality.Conditions.Frightened)),
                                new XElement("Grappled", new XAttribute("value", character.Vitality.Conditions.Grappled)),
                                new XElement("Incapacitated", new XAttribute("value", character.Vitality.Conditions.Incapacitated)),
                                new XElement("Invisible", new XAttribute("value", character.Vitality.Conditions.Invisible)),
                                new XElement("Paralyzed", new XAttribute("value", character.Vitality.Conditions.Paralyzed)),
                                new XElement("Petrified", new XAttribute("value", character.Vitality.Conditions.Petrified)),
                                new XElement("Poisoned", new XAttribute("value", character.Vitality.Conditions.Poisoned)),
                                new XElement("Prone", new XAttribute("value", character.Vitality.Conditions.Prone)),
                                new XElement("Restrained", new XAttribute("value", character.Vitality.Conditions.Restrained)),
                                new XElement("Stunned", new XAttribute("value", character.Vitality.Conditions.Stunned)),
                                new XElement("Unconscious", new XAttribute("value", character.Vitality.Conditions.Unconscious))
                                ),
                            new XElement("HitDice",
                                new XElement("D6", new XAttribute("total", character.Vitality.HitDice.TotalD6), new XAttribute("spent", character.Vitality.HitDice.SpentD6)),
                                new XElement("D8", new XAttribute("total", character.Vitality.HitDice.TotalD8), new XAttribute("spent", character.Vitality.HitDice.SpentD8)),
                                new XElement("D10", new XAttribute("total", character.Vitality.HitDice.TotalD10), new XAttribute("spent", character.Vitality.HitDice.SpentD10)),
                                new XElement("D12", new XAttribute("total", character.Vitality.HitDice.TotalD12), new XAttribute("spent", character.Vitality.HitDice.SpentD12))
                                )
                            ),
                        new XElement("Weapons",
                            from weapon in character.Weapons
                            select
                                new XElement("Weapon",
                                    new XAttribute("name", weapon.Name),
                                    new XAttribute("weaponType", weapon.WeaponType.ToString()),
                                    new XAttribute("ability", weapon.Ability),
                                    new XAttribute("dmg", weapon.Damage),
                                    new XAttribute("misc", weapon.Misc),
                                    new XAttribute("dmgType", weapon.DamageType),
                                    new XAttribute("range", weapon.Range),
                                    new XAttribute("notes", weapon.Note),
                                    new XAttribute("weight", weapon.Weight),
                                    new XAttribute("override", weapon.ProficiencyOverride),
                                    new XAttribute("id", weapon.ID)
                                    )
                            ),
                        new XElement("Ammunitions",
                            from ammo in character.Ammunitions
                            select
                                new XElement("Ammunition",
                                    new XAttribute("name", ammo.Name),
                                    new XAttribute("amount", ammo.Quantity),
                                    new XAttribute("miscDmg", ammo.Bonus),
                                    new XAttribute("dmgType", ammo.DamageType),
                                    new XAttribute("used", ammo.Used),
                                    new XAttribute("id", ammo.ID)
                                )
                            ),
                        new XElement("Inventory",
                            from item in character.Inventories
                            select
                                new XElement("Item",
                                    new XAttribute("name", item.Name),
                                    new XAttribute("amount", item.Amount),
                                    new XAttribute("wgt", item.Weight),
                                    new XAttribute("note", item.Note),
                                    new XAttribute("id", item.ID)
                                )
                            ),
                        new XElement("Abilities",
                            from ability in character.Abilities
                            select
                                new XElement("Ability",
                                    new XAttribute("name", ability.Name),
                                    new XAttribute("level", ability.Level),
                                    new XAttribute("uses", ability.Uses),
                                    new XAttribute("recovery", ability.Recovery),
                                    new XAttribute("action", ability.Action),
                                    new XAttribute("notes", ability.Note),
                                    new XAttribute("id", ability.ID)
                                )
                            ),
                        new XElement("Spellcasting",
                            new XElement("SpellClasses",
                                from magic in character.MagicClasses
                                select
                                    new XElement("SpellClass",
                                        new XAttribute("class", magic.Name),
                                        new XAttribute("ability", magic.Ability),
                                        new XAttribute("cantrips", magic.KnownCantrips),
                                        new XAttribute("known", magic.KnownSpells),
                                        new XAttribute("id", magic.ID)
                                    )
                                ),
                            new XElement("SpellSlots",
                                new XElement("Pact", new XAttribute("total", character.SpellSlots.PactTotal), new XAttribute("used", character.SpellSlots.PactUsed)),
                                new XElement("One", new XAttribute("total", character.SpellSlots.FirstTotal), new XAttribute("used", character.SpellSlots.FirstUsed)),
                                new XElement("Two", new XAttribute("total", character.SpellSlots.SecondTotal), new XAttribute("used", character.SpellSlots.SecondUsed)),
                                new XElement("Three", new XAttribute("total", character.SpellSlots.ThirdTotal), new XAttribute("used", character.SpellSlots.ThirdUsed)),
                                new XElement("Four", new XAttribute("total", character.SpellSlots.FourthTotal), new XAttribute("used", character.SpellSlots.FourthUsed)),
                                new XElement("Five", new XAttribute("total", character.SpellSlots.FifthTotal), new XAttribute("used", character.SpellSlots.FifthUsed)),
                                new XElement("Six", new XAttribute("total", character.SpellSlots.SixthTotal), new XAttribute("used", character.SpellSlots.SixthUsed)),
                                new XElement("Seven", new XAttribute("total", character.SpellSlots.SeventhTotal), new XAttribute("used", character.SpellSlots.SeventhUsed)),
                                new XElement("Eight", new XAttribute("total", character.SpellSlots.EighthTotal), new XAttribute("used", character.SpellSlots.EighthUsed)),
                                new XElement("Nine", new XAttribute("total", character.SpellSlots.NinethTotal), new XAttribute("used", character.SpellSlots.NinethUsed))
                                ),
                            new XElement("SpellList",
                                from spell in character.Spells
                                select
                                    new XElement("Spell",
                                        new XAttribute("name", spell.Name),
                                        new XAttribute("level", spell.Level),
                                        new XAttribute("page", spell.Page),
                                        new XAttribute("school", spell.School),
                                        new XAttribute("ritual", spell.Ritual),
                                        new XAttribute("comp", spell.Components),
                                        new XAttribute("concen", spell.Concentration),
                                        new XAttribute("range", spell.Range),
                                        new XAttribute("duration", spell.Duration),
                                        new XAttribute("area", spell.Area),
                                        new XAttribute("save", spell.Save),
                                        new XAttribute("damage", spell.Damage),
                                        new XAttribute("description", spell.Description),
                                        new XAttribute("prepared", spell.Prepared),
                                        new XAttribute("id", spell.ID)
                                    )
                                )
                            ),
                        new XElement("Companion",
                            new XElement("Name", new XAttribute("value", character.Companion.Name)),
                            new XElement("AC", new XAttribute("value", character.Companion.ArmorClass)),
                            new XElement("HitDice", new XAttribute("value", character.Companion.HitDice)),
                            new XElement("HP", new XAttribute("value", character.Companion.Health)),
                            new XElement("CurrentHP", new XAttribute("value", character.Companion.CurrentHealth)),
                            new XElement("Speed", new XAttribute("value", character.Companion.Speed)),
                            new XElement("Strength", new XAttribute("value", character.Companion.Strength)),
                            new XElement("Dexterity", new XAttribute("value", character.Companion.Dexterity)),
                            new XElement("Constitution", new XAttribute("value", character.Companion.Constitution)),
                            new XElement("Intelligence", new XAttribute("value", character.Companion.Intelligence)),
                            new XElement("Wisdom", new XAttribute("value", character.Companion.Wisdom)),
                            new XElement("Charisma", new XAttribute("value", character.Companion.Charisma)),
                            new XElement("Perception", new XAttribute("value", character.Companion.Perception)),
                            new XElement("Senses", new XAttribute("value", character.Companion.Senses))
                            // new XElement("Attack", new XAttribute("one", character.Companion.Attack.First), new XAttribute("two", character.Companion.Attack.Second)),
                            // new XElement("Type", new XAttribute("one", character.Companion.Type.First), new XAttribute("two", character.Companion.Type.Second)),
                            // new XElement("AtkBonus", new XAttribute("one", character.Companion.AtkBonus.First), new XAttribute("two", character.Companion.AtkBonus.Second)),
                            // new XElement("Damage", new XAttribute("one", character.Companion.Damage.First), new XAttribute("two", character.Companion.Damage.Second)),
                            // new XElement("DmgType", new XAttribute("one", character.Companion.DmgType.First), new XAttribute("two", character.Companion.DmgType.Second)),
                            // new XElement("Reach", new XAttribute("one", character.Companion.Reach.First), new XAttribute("two", character.Companion.Reach.Second)),
                            // new XElement("Notes", new XAttribute("one", character.Companion.Notes.First), new XAttribute("two", character.Companion.Notes.Second))
                            ),
                        new XElement("CampainNotes",
                            from campainChapter in character.Chapters
                            select
                                new XElement("Chapter", new XAttribute("name", campainChapter.Name), new XAttribute("id", campainChapter.ID),
                                    from campainNotes in campainChapter.Documents
                                    select
                                        new XElement("Note", new XAttribute("id", campainNotes.ID),
                                                             new XAttribute("name", campainNotes.Name),
                                                             new XCData(campainNotes.RTF)))
                            )
                        )
                    );

                xml.Save(ccsFile.AbsolutePath);
                Program.Modified = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Program.Modified = true;
            }
        }
    }
}
