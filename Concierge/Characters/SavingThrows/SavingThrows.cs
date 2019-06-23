using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SavingThrowsNamespace
{
    public abstract class SavingThrows
    {
        public bool Proficiency
        {
            get;
            set;
        }

        public Constants.Checks Checks
        {
            get;
            set;
        }

        public abstract int Bonus
        {
            get;
        }
    }
}
