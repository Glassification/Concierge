﻿// <copyright file="ConciergeSettingsDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Dtos
{
    public class ConciergeSettingsDto
    {
        public ConciergeSettingsDto()
        {
        }

        public bool AutosaveEnabled { get; init; }

        public int AutosaveInterval { get; init; }

        public bool CheckVersion { get; init; }

        public bool MuteSounds { get; init; }

        public bool UseCoinWeight { get; init; }

        public bool UseEncumbrance { get; init; }
    }
}