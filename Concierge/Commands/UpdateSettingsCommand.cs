// <copyright file="UpdateSettingsCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Interfaces.Enums;

    public sealed class UpdateSettingsCommand : Command
    {
        private readonly UserSettingsDto oldSettingsDto;
        private readonly UserSettingsDto newSettingsDto;

        public UpdateSettingsCommand(UserSettingsDto oldSettingsDto, UserSettingsDto newSettingsDto)
        {
            this.ConciergePage = ConciergePage.None;
            this.oldSettingsDto = oldSettingsDto;
            this.newSettingsDto = newSettingsDto;
        }

        public override void Redo()
        {
            AppSettingsManager.UpdateSettings(this.newSettingsDto);
        }

        public override void Undo()
        {
            AppSettingsManager.UpdateSettings(this.oldSettingsDto);
        }

        public override bool ShouldAdd()
        {
            return !this.newSettingsDto.Equals(this.oldSettingsDto);
        }
    }
}
