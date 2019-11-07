using Concierge.Utility;
using System;
using System.Collections.Generic;

namespace Concierge.Characters
{
    public class Proficiency
    {
        public Proficiency()
        {
            Armors = new Dictionary<Guid, string>();
            Shields = new Dictionary<Guid, string>();
            Weapons = new Dictionary<Guid, string>();
            Tools = new Dictionary<Guid, string>();
        }

        public string GetProficiencyById(Guid id)
        {
            if (Armors.ContainsKey(id))
            {
                return Armors[id];
            }
            else if (Shields.ContainsKey(id))
            {
                return Shields[id];
            }
            else if (Weapons.ContainsKey(id))
            {
                return Weapons[id];
            }
            else if (Tools.ContainsKey(id))
            {
                return Tools[id];
            }
            else
            {
                return null;
            }
        }

        public void SetProficiencyById(Guid id, string proficiency)
        {
            if (Armors.ContainsKey(id))
            {
                Armors[id] = proficiency;
            }
            else if (Shields.ContainsKey(id))
            {
                Shields[id] = proficiency;
            }
            else if (Weapons.ContainsKey(id))
            {
                Weapons[id] = proficiency;
            }
            else if (Tools.ContainsKey(id))
            {
                Tools[id] = proficiency;
            }
        }

        public void AddProficiencyByPopupButton(Constants.PopupButtons PopupButton, string proficiency)
        {
            switch (PopupButton)
            {
                case Constants.PopupButtons.ArmorProficiency:
                    Armors.Add(Guid.NewGuid(), proficiency);
                    break;
                case Constants.PopupButtons.ShieldProficiency:
                    Shields.Add(Guid.NewGuid(), proficiency);
                    break;
                case Constants.PopupButtons.WeaponProficiency:
                    Weapons.Add(Guid.NewGuid(), proficiency);
                    break;
                case Constants.PopupButtons.ToolProficiency:
                    Tools.Add(Guid.NewGuid(), proficiency);
                    break;
            }
        }

        public Dictionary<Guid, string> Armors { get; set; }
        public Dictionary<Guid, string> Shields { get; set; }
        public Dictionary<Guid, string> Weapons { get; set; }
        public Dictionary<Guid, string> Tools { get; set; }
    }
}
