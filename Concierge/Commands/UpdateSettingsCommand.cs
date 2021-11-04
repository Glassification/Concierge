// <copyright file="UpdateSettingsCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Dtos;

    public class UpdateSettingsCommand : Command
    {
        private readonly ConciergeSettingsDto oldSettingsDto;
        private readonly ConciergeSettingsDto newSettingsDto;

        public UpdateSettingsCommand(ConciergeSettingsDto oldSettingsDto, ConciergeSettingsDto newSettingsDto)
        {
            this.ConciergePage = ConciergePage.None;
            this.oldSettingsDto = oldSettingsDto;
            this.newSettingsDto = newSettingsDto;
        }

        public override void Redo()
        {
            ConciergeSettings.UpdateSettings(this.newSettingsDto);
        }

        public override void Undo()
        {
            ConciergeSettings.UpdateSettings(this.oldSettingsDto);
        }
    }
}
