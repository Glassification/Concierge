using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class ClassResource
    {
        public ClassResource()
        {

        }

        public ClassResource(Guid id)
        {

        }

        public string Type { get; set; }
        public int Total { get; set; }
        public int Spent { get; set; }
        public Guid ID { get; private set; }
    }
}
