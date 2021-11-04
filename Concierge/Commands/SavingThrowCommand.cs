// <copyright file="SavingThrowCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Character.AbilitySavingThrows.SavingThrowTypes;
    using Concierge.Interfaces.Enums;

    public class SavingThrowCommand : Command
    {
        private readonly SavingThrows savingThrows;
        private readonly bool oldValue;
        private readonly bool newValue;

        public SavingThrowCommand(SavingThrows savingThrows, bool oldValue, bool newValue)
        {
            this.ConciergePage = ConciergePage.Overview;
            this.savingThrows = savingThrows;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public override void Redo()
        {
            this.savingThrows.Proficiency = this.newValue;
        }

        public override void Undo()
        {
            this.savingThrows.Proficiency = this.oldValue;
        }
    }
}
