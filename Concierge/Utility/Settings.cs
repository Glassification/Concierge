using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Utility
{
    public static class Settings
    {
        static Settings()
        {
            Default();
        }

        public static void Default()
        {
            AutosaveEnable = false;
            AutosaveInterval = 1;
            UseCoinWeight = false;
            UseEncumbrance = false;
        }

        public static bool AutosaveEnable { get; set; }
        public static int AutosaveInterval { get; set; }
        public static bool UseCoinWeight { get; set; }
        public static bool UseEncumbrance { get; set; }
    }
}
