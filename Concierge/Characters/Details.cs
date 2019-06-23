using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Details
    {
        public Details()
        {
            Name = "";
            Race = "";
            Background = "";
            Alignment = "";
            Experience = "";
            Languages = new Dictionary<Guid, string>();
            InitiativeBonus = 0;
            PerceptionBonus = 0;
            Movement = 0;
            Vision = "";
        }

        public string Name { get; set; }

        public string Race { get; set; }

        public string Background { get; set; }

        public string Alignment { get; set; }

        public string Experience { get; set; }

        public Dictionary<Guid, string> Languages { get; set; }

        public int InitiativeBonus { get; set; }

        public int PerceptionBonus { get; set; }

        public int Movement { get; set; }

        public string Vision { get; set; }
    }
}
