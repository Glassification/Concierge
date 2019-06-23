using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class MagicClass
    {
        public MagicClass()
        {
            ID = Guid.NewGuid();
        }

        public MagicClass(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public Constants.Abilities Ability { get; set; }
        public int Level { get; set; }
        public int KnownCantrips { get; set; }
        public int KnownSpells { get; set; }
        public int PreparedSpells { get; set; }
        public Guid ID { get; private set; }
    }
}
