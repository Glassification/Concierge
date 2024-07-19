// <copyright file="UpdateSettingsCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a command that updates user settings and allows undoing the changes.
    /// </summary>
    public sealed class UpdateSettingsCommand : Command
    {
        private readonly UserSettingsDto oldSettingsDto;
        private readonly UserSettingsDto newSettingsDto;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSettingsCommand"/> class.
        /// </summary>
        /// <param name="oldSettingsDto">The user settings before the update.</param>
        /// <param name="newSettingsDto">The user settings after the update.</param>
        public UpdateSettingsCommand(UserSettingsDto oldSettingsDto, UserSettingsDto newSettingsDto)
        {
            this.ConciergePage = ConciergePages.None;
            this.oldSettingsDto = oldSettingsDto;
            this.newSettingsDto = newSettingsDto;
        }

        public override void Redo()
        {
            AppSettingsManager.UpdateSettings(this.newSettingsDto, Program.IsDebug);
        }

        public override void Undo()
        {
            AppSettingsManager.UpdateSettings(this.oldSettingsDto, Program.IsDebug);
        }

        public override bool ShouldAdd()
        {
            return !this.newSettingsDto.Equals(this.oldSettingsDto);
        }
    }
}
