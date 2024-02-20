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

    public sealed class DamageCommand : Command
    {
        private readonly Vitality vitality;
        private readonly Health oldHealth;
        private readonly Health newHealth;
        private readonly Spell? concentratedSpell;

        private readonly CharacterSheet character;

        public DamageCommand(Vitality vitality, Health oldHealth, Health newHealth, Spell? concentratedSpell, ConciergePage conciergePage)
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
