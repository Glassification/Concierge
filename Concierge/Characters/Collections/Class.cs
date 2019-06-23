using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class Class
    {
        private int _level;

        public Class()
        {
            _level = 0;
            Name = "";
            ID = Guid.NewGuid();
        }

        public Class(Guid id)
        {
            _level = 0;
            Name = "";
            ID = id;
        }

        public string Name { get; set; }

        public Guid ID { get; private set; }

        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value <= Constants.MAX_LEVEL && value >= 0)
                {
                    if (Program.Character.ValidateClassLevel(value, ID))
                    {
                        _level = value;
                    }
                }
            }
        }
    }
}
