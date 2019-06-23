using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class Document
    {
        public Document()
        {
            ID = Guid.NewGuid();
        }

        public Document(Guid id)
        {
            ID = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; set; }
        public string RTF { get; set; }
        public Guid ID { get; private set; }
    }
}
