using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class Inventory
    {
        public Inventory()
        {
            ID = Guid.NewGuid();
        }

        public Inventory(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public int Amount { get; set; }
        public double Weight { get; set; }
        public string Note { get; set; }
        public Guid ID { get; private set; }
    }
}
