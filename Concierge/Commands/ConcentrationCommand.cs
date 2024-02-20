// <copyright file="ConcentrationCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character.Magic;
    using Concierge.Display.Enums;

    public sealed class ConcentrationCommand : Command
    {
        private readonly Spell currentConcentration;
        private readonly Spell? newConcentration;

        public ConcentrationCommand(Spell currentConcentration, Spell? newConcentration)
        {
            this.currentConcentration = currentConcentration;
            this.newConcentration = newConcentration;
            this.ConciergePage = ConciergePage.Spellcasting;
        }

        public override void Redo()
        {
            this.Invert();
        }

        public override void Undo()
        {
            this.Invert();
        }

        private void Invert()
        {
            this.currentConcentration.CurrentConcentration = !this.currentConcentration.CurrentConcentration;
            if (this.newConcentration is not null)
            {
                this.newConcentration.CurrentConcentration = !this.newConcentration.CurrentConcentration;
            }
        }
    }
}
