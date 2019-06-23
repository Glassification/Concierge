using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Appearance
    {
        public Appearance()
        {
            Gender = "";
            Age = "";
            Height = "";
            Weight = "";
            SkinColour = "";
            EyeColour = "";
            HairColour = "";
            DistinguishingMarks = "";
        }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string SkinColour { get; set; }

        public string EyeColour { get; set; }

        public string HairColour { get; set; }

        public string DistinguishingMarks { get; set; }
    }
}
