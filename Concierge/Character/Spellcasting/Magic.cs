// <copyright file="Magic.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public class Magic : ICopyable<Magic>
    {
        public Magic()
        {
            this.MagicClasses = new List<MagicClass>();
            this.Spells = new List<Spell>();
            this.SpellSlots = new SpellSlots();
        }

        public List<MagicClass> MagicClasses { get; set; }

        public List<Spell> Spells { get; set; }

        public SpellSlots SpellSlots { get; set; }

        [JsonIgnore]
        public int CasterLevel
        {
            get
            {
                int level = 0;

                foreach (var magicClass in this.MagicClasses)
                {
                    level += magicClass.Level;
                }

                return level;
            }
        }

        public Magic DeepCopy()
        {
            return new Magic()
            {
                MagicClasses = this.MagicClasses.DeepCopy().ToList(),
                Spells = this.Spells.DeepCopy().ToList(),
                SpellSlots = this.SpellSlots.DeepCopy(),
            };
        }
    }
}
