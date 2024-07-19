// <copyright file="RestCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a command that handles resting actions for a character, including restoring vitality, spell slots, companion health, and companion hit dice.
    /// </summary>
    public sealed class RestCommand : Command
    {
        private readonly Vitality oldVitality;
        private readonly Vitality newVitality;
        private readonly Health oldCompanionHealth;
        private readonly HitDice oldCompanionHitDice;
        private readonly Spell? oldConcentratedSpell;
        private readonly Health newCompanionHealth;
        private readonly HitDice newCompanionHitDice;
        private readonly SpellSlots oldSpellSlots;
        private readonly SpellSlots newSpellSlots;

        private readonly CharacterSheet character;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestCommand"/> class.
        /// </summary>
        /// <param name="oldVitality">The character's vitality before resting.</param>
        /// <param name="oldCompanionHealth">The companion's health before resting.</param>
        /// <param name="oldCompanionHitDice">The companion's hit dice before resting.</param>
        /// <param name="oldSpellSlots">The character's spell slots before resting.</param>
        /// <param name="oldConcentratedSpell">The concentrated spell before resting (optional).</param>
        /// <param name="newVitality">The character's vitality after resting.</param>
        /// <param name="newCompanionHealth">The companion's health after resting.</param>
        /// <param name="newCompanionHitDice">The companion's hit dice after resting.</param>
        /// <param name="newSpellSlots">The character's spell slots after resting.</param>
        public RestCommand(
            Vitality oldVitality,
            Health oldCompanionHealth,
            HitDice oldCompanionHitDice,
            SpellSlots oldSpellSlots,
            Spell? oldConcentratedSpell,
            Vitality newVitality,
            Health newCompanionHealth,
            HitDice newCompanionHitDice,
            SpellSlots newSpellSlots)
        {
            this.oldVitality = oldVitality;
            this.newVitality = newVitality;
            this.oldCompanionHealth = oldCompanionHealth;
            this.newCompanionHealth = newCompanionHealth;
            this.oldCompanionHitDice = oldCompanionHitDice;
            this.newCompanionHitDice = newCompanionHitDice;
            this.oldSpellSlots = oldSpellSlots;
            this.newSpellSlots = newSpellSlots;
            this.oldConcentratedSpell = oldConcentratedSpell;
            this.ConciergePage = ConciergePages.None;
            this.character = Program.CcsFile.Character;
        }

        public override void Redo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.newVitality);
            this.character.SpellCasting.SpellSlots.SetProperties<SpellSlots>(this.newSpellSlots);
            this.character.Companion.Health.SetProperties<Health>(this.newCompanionHealth);
            this.character.Companion.HitDice.SetProperties<HitDice>(this.newCompanionHitDice);
            this.character.SpellCasting.ClearConcentration();
        }

        public override void Undo()
        {
            this.character.Vitality.SetProperties<Vitality>(this.oldVitality);
            this.character.SpellCasting.SpellSlots.SetProperties<SpellSlots>(this.oldSpellSlots);
            this.character.Companion.Health.SetProperties<Health>(this.oldCompanionHealth);
            this.character.Companion.HitDice.SetProperties<HitDice>(this.oldCompanionHitDice);
            if (this.oldConcentratedSpell is not null)
            {
                this.oldConcentratedSpell.CurrentConcentration = true;
            }
        }

        public override bool ShouldAdd()
        {
            var oldVitality = JsonConvert.SerializeObject(this.oldVitality, Formatting.Indented);
            var oldSpellSlots = JsonConvert.SerializeObject(this.oldSpellSlots, Formatting.Indented);
            var oldCompanionHealth = JsonConvert.SerializeObject(this.oldCompanionHealth, Formatting.Indented);
            var oldCompanionHitDice = JsonConvert.SerializeObject(this.oldCompanionHitDice, Formatting.Indented);
            var newVitality = JsonConvert.SerializeObject(this.newVitality, Formatting.Indented);
            var newSpellSlots = JsonConvert.SerializeObject(this.newSpellSlots, Formatting.Indented);
            var newCompanionHealth = JsonConvert.SerializeObject(this.newCompanionHealth, Formatting.Indented);
            var newCompanionHitDice = JsonConvert.SerializeObject(this.newCompanionHitDice, Formatting.Indented);

            return !(
                oldVitality.Equals(newVitality) &&
                oldSpellSlots.Equals(newSpellSlots) &&
                oldCompanionHealth.Equals(newCompanionHealth) &&
                oldCompanionHitDice.Equals(newCompanionHitDice) &&
                this.oldConcentratedSpell is null);
        }
    }
}
