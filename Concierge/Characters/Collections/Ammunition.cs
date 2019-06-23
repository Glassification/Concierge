using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class Ammunition
    {
        public Ammunition()
        {
            ID = Guid.NewGuid();
        }

        public Ammunition(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Bonus { get; set; }
        public Constants.DamageTypes DamageType { get; set; }
        public int Used { get; set; }
        public Guid ID { get; private set; }
    }
}
