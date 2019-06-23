using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Attributes
    {
        private int _strength;
        private int _dexterity;
        private int _constitution;
        private int _intelligence;
        private int _wisdom;
        private int _charisma;

        public Attributes()
        {
            _strength = 0;
            _dexterity = 0;
            _constitution = 0;
            _intelligence = 0;
            _wisdom = 0;
            _charisma = 0;
        }

        public int Strength
        {
            get
            {
                return _strength;
            }
            set
            {
                if (value >= Constants.MIN_SCORE && value <= Constants.MAX_SCORE)
                    _strength = value;
            }
        }

        public int Dexterity
        {
            get
            {
                return _dexterity;
            }
            set
            {
                if (value >= Constants.MIN_SCORE && value <= Constants.MAX_SCORE)
                    _dexterity = value;
            }
        }

        public int Constitution
        {
            get
            {
                return _constitution;
            }
            set
            {
                if (value >= Constants.MIN_SCORE && value <= Constants.MAX_SCORE)
                    _constitution = value;
            }
        }

        public int Intelligence
        {
            get
            {
                return _intelligence;
            }
            set
            {
                if (value >= Constants.MIN_SCORE && value <= Constants.MAX_SCORE)
                    _intelligence = value;
            }
        }

        public int Wisdom
        {
            get
            {
                return _wisdom;
            }
            set
            {
                if (value >= Constants.MIN_SCORE && value <= Constants.MAX_SCORE)
                    _wisdom = value;
            }
        }

        public int Charisma
        {
            get
            {
                return _charisma;
            }
            set
            {
                if (value >= Constants.MIN_SCORE && value <= Constants.MAX_SCORE)
                    _charisma = value;
            }
        }
    }
}
