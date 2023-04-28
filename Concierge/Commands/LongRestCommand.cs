// <copyright file="LongRestCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;
    using Newtonsoft.Json;

    public sealed class LongRestCommand : Command
    {
        private readonly Vitality oldVitality;
        private readonly Vitality newVitality;
        private readonly Vitality oldCompanionVitality;
        private readonly Vitality newCompanionVitality;
        private readonly SpellSlots oldSpellSlots;
        private readonly SpellSlots newSpellSlots;

        private readonly ConciergeCharacter character;

        public LongRestCommand(
            Vitality oldVitality,
            Vitality oldCompanionVitality,
            SpellSlots oldSpellSlots,
            Vitality newVitality,
            Vitality newCompanionVitality,
            SpellSlots newSpellSlots)
        {
            this.oldVitality = oldVitality;
            this.newVitality = newVitality;
            this.oldCompanionVitality = oldCompanionVitality;
            this.newCompanionVitality = newCompanionVitality;
            this.oldSpellSlots = oldSpellSlots;
            this.newSpellSlots = newSpellSlots;
            this.ConciergePage = ConciergePage.None;
            this.character = Program.CcsFile.Character;
        }

        public override void Redo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.newVitality);
            this.character.SpellSlots.SetProperties<SpellSlots>(this.newSpellSlots);
            this.character.Companion.Vitality.SetProperties<Vitality>(this.newCompanionVitality);
        }

        public override void Undo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.oldVitality);
            this.character.SpellSlots.SetProperties<SpellSlots>(this.oldSpellSlots);
            this.character.Companion.Vitality.SetProperties<Vitality>(this.oldCompanionVitality);
        }

        public override bool ShouldAdd()
        {
            var oldVitality = JsonConvert.SerializeObject(this.oldVitality, Formatting.Indented);
            var oldSpellSlots = JsonConvert.SerializeObject(this.oldSpellSlots, Formatting.Indented);
            var oldCompanion = JsonConvert.SerializeObject(this.oldCompanionVitality, Formatting.Indented);
            var newVitality = JsonConvert.SerializeObject(this.newVitality, Formatting.Indented);
            var newSpellSlots = JsonConvert.SerializeObject(this.newSpellSlots, Formatting.Indented);
            var newCompanion = JsonConvert.SerializeObject(this.newCompanionVitality, Formatting.Indented);

            return !(oldVitality.Equals(newVitality) && oldSpellSlots.Equals(newSpellSlots) && oldCompanion.Equals(newCompanion));
        }
    }
}
