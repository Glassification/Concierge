// <copyright file="Proficiency.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    using System;
    using System.Collections.Generic;
    using Concierge.Characters.Dto;
    using Concierge.Characters.Enums;

    public class Proficiency
    {
        public Proficiency()
        {
            this.Armors = new Dictionary<Guid, string>();
            this.Shields = new Dictionary<Guid, string>();
            this.Weapons = new Dictionary<Guid, string>();
            this.Tools = new Dictionary<Guid, string>();
        }

        public Dictionary<Guid, string> Armors { get; set; }

        public Dictionary<Guid, string> Shields { get; set; }

        public Dictionary<Guid, string> Weapons { get; set; }

        public Dictionary<Guid, string> Tools { get; set; }

        public ProficiencyDto GetProficiencyById(Guid id)
        {
            return this.Armors.ContainsKey(id)
                ? new ProficiencyDto(this.Armors[id], ProficiencyTypes.Armor)
                : this.Shields.ContainsKey(id)
                    ? new ProficiencyDto(this.Shields[id], ProficiencyTypes.Shield)
                    : this.Weapons.ContainsKey(id) ?
                        new ProficiencyDto(this.Weapons[id], ProficiencyTypes.Weapon) :
                        this.Tools.ContainsKey(id) ?
                            new ProficiencyDto(this.Tools[id], ProficiencyTypes.Tool) :
                            null;
        }

        public void SetProficiencyById(Guid id, string proficiency)
        {
            if (this.Armors.ContainsKey(id))
            {
                this.Armors[id] = proficiency;
            }
            else if (this.Shields.ContainsKey(id))
            {
                this.Shields[id] = proficiency;
            }
            else if (this.Weapons.ContainsKey(id))
            {
                this.Weapons[id] = proficiency;
            }
            else if (this.Tools.ContainsKey(id))
            {
                this.Tools[id] = proficiency;
            }
        }

        public void AddProficiencyByProficiencyType(ProficiencyTypes proficiencyTypes, string proficiency)
        {
            switch (proficiencyTypes)
            {
                case ProficiencyTypes.Armor:
                    this.Armors.Add(Guid.NewGuid(), proficiency);
                    break;
                case ProficiencyTypes.Shield:
                    this.Shields.Add(Guid.NewGuid(), proficiency);
                    break;
                case ProficiencyTypes.Tool:
                    this.Weapons.Add(Guid.NewGuid(), proficiency);
                    break;
                case ProficiencyTypes.Weapon:
                    this.Tools.Add(Guid.NewGuid(), proficiency);
                    break;
                default:
                    break;
            }
        }
    }
}
