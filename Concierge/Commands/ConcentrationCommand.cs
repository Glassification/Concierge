// <copyright file="ConcentrationCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character.Magic;
    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a command that manages the concentration state of spells.
    /// </summary>
    public sealed class ConcentrationCommand : Command
    {
        private readonly Spell currentConcentration;
        private readonly Spell? newConcentration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcentrationCommand"/> class.
        /// </summary>
        /// <param name="currentConcentration">The spell that currently has concentration.</param>
        /// <param name="newConcentration">The spell that will gain concentration (optional).</param>
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
