using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SkillsNamespace
{
    public abstract class Skills
    {
        public bool Proficiency
        {
            get;
            set;
        }

        public bool Expertise
        {
            get;
            set;
        }

        public abstract Constants.Checks Checks
        {
            get;
        }

        public abstract int Bonus
        {
            get;
        }
    }
}
