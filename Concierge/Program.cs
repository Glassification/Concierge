using Concierge.Characters;
using Concierge.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge
{
    public static class Program
    {
        static Program()
        {
            Character = new Character();
            CcsFile = null;
            Modified = true;
            Typing = false;
        }

        public static Character Character { get; private set; }
        public static CcsFile CcsFile { get; set; }
        public static bool Typing { get; set; }
        public static bool Modified { get; set; }
    }
}
