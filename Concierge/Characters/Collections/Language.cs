using System;

namespace Concierge.Characters.Collections
{
    public class Language
    {
        public Language()
        {
            ID = Guid.NewGuid();
        }

        public Language(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public string Script { get; set; }
        public string Speakers { get; set; }
        public Guid ID { get; }

        public string Description
        {
            get
            {
                return $"{Name} ({Script}), Spoken by: {Speakers}";
            }
        }
    }
}
