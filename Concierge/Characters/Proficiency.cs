using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Dictionary<Guid, string> Armors { get; set; }
        public Dictionary<Guid, string> Shields { get; set; }
        public Dictionary<Guid, string> Weapons { get; set; }
        public Dictionary<Guid, string> Tools { get; set; }
    }
}
