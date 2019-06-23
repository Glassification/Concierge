using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Persistence
{
    public class CcsFile
    {
        public CcsFile()
        {

        }

        public string Name { get; set; }
        public string Path { get; set; }

        public string AbsolutePath
        {
            get
            {
                return Path + Name;
            }
        }
    }
}
