using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class Ability
    {
        public Ability()
        {
            ID = Guid.NewGuid();
        }

        public Ability(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public string Level { get; set; }
        public string Uses { get; set; }
        public string Recovery { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public Guid ID { get; private set; }
    }
}
