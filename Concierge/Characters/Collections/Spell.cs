using Concierge.Utility;
using System;

namespace Concierge.Characters.Collections
{
    public class Spell
    {
        public Spell()
        {
            ID = Guid.NewGuid();
        }

        public Spell(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public bool Prepared { get; set; }
        public int Level { get; set; }
        public int Page { get; set; }
        public Constants.ArcaneSchools School { get; set; }
        public bool Ritual { get; set; }
        public string Components { get; set; }
        public bool Concentration { get; set; }
        public string Range { get; set; }
        public string Duration { get; set; }
        public string Area { get; set; }
        public string Save { get; set; }
        public string Damage { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public Guid ID { get; }
    }
}
