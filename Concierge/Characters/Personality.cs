using System;

namespace Concierge.Characters
{
    public class Personality
    {
        public Personality()
        {
            Trait1 = "";
            Trait2 = "";
            Ideal = "";
            Bond = "";
            Flaw = "";
            Background = "";
            Notes = "";
        }

        public string Trait1 { get; set; }

        public string Trait2 { get; set; }

        public string Ideal { get; set; }

        public string Bond { get; set; }

        public string Flaw { get; set; }

        public string Background { get; set; }

        public string Notes { get; set; }
    }
}
