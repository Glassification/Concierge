// <copyright file="UpdateSettingsCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Interfaces.Enums;

    public class UpdateSettingsCommand : Command
    {
        private readonly SettingsDto oldSettingsDto;
        private readonly SettingsDto newSettingsDto;

        public UpdateSettingsCommand(SettingsDto oldSettingsDto, SettingsDto newSettingsDto)
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
