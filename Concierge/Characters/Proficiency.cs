// <copyright file="Proficiency.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    using System;
    using System.Collections.Generic;

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

        public string GetProficiencyById(Guid id)
        {
            if (this.Armors.ContainsKey(id))
            {
                return this.Armors[id];
            }
            else if (this.Shields.ContainsKey(id))
            {
                return this.Shields[id];
            }
            else
            {
                return this.Weapons.ContainsKey(id) ? this.Weapons[id] : this.Tools.ContainsKey(id) ? this.Tools[id] : null;
            }
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

        public void AddProficiencyByPopupButton(PopupButtons PopupButton, string proficiency)
        {
            switch (PopupButton)
            {
                case PopupButtons.ArmorProficiency:
                    this.Armors.Add(Guid.NewGuid(), proficiency);
                    break;
                case PopupButtons.ShieldProficiency:
                    this.Shields.Add(Guid.NewGuid(), proficiency);
                    break;
                case PopupButtons.WeaponProficiency:
                    this.Weapons.Add(Guid.NewGuid(), proficiency);
                    break;
                case PopupButtons.ToolProficiency:
                    this.Tools.Add(Guid.NewGuid(), proficiency);
                    break;
                default:
                    break;
            }
        }
    }
}
