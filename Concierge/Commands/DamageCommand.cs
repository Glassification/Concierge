// <copyright file="DamageCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a command that applies damage to a character's health and handles concentration for spells.
    /// </summary>
    public sealed class DamageCommand : Command
    {
        private readonly Vitality vitality;
        private readonly Health oldHealth;
        private readonly Health newHealth;
        private readonly Spell? concentratedSpell;

        private readonly CharacterSheet character;

        /// <summary>
        /// Initializes a new instance of the <see cref="DamageCommand"/> class.
        /// </summary>
        /// <param name="vitality">The vitality of the character receiving damage.</param>
        /// <param name="oldHealth">The previous health state of the character.</param>
        /// <param name="newHealth">The new health state of the character after taking damage.</param>
        /// <param name="concentratedSpell">The concentrated spell associated with the character (optional).</param>
        /// <param name="conciergePage">The ConciergePage associated with this command.</param>
        public DamageCommand(Vitality vitality, Health oldHealth, Health newHealth, Spell? concentratedSpell, ConciergePages conciergePage)
        {
            this.vitality = vitality;
            this.oldHealth = oldHealth;
            this.newHealth = newHealth;
            this.concentratedSpell = concentratedSpell;
            this.character = Program.CcsFile.Character;

            this.ConciergePage = conciergePage;
        }

        public override void Redo()
        {
            this.vitality.Health.SetProperties<Vitality>(this.newHealth);
            if (this.concentratedSpell is not null)
            {
                this.character.SpellCasting.ClearConcentration();
            }
        }

        public override void Undo()
        {
            this.vitality.Health.SetProperties<Vitality>(this.oldHealth);
            if (this.concentratedSpell is not null)
            {
                this.character.SpellCasting.SetConcentration(this.concentratedSpell);
            }
        }
    }
}
