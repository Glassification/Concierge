// <copyright file="AttributeSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Attributes
{
    using Concierge.Tools.Enums;

    public sealed class AttributeSettings : IGeneratorSettings
    {
        public AttributeSettings(AbilityScores abilityScores)
        {
            this.AbilityScores = abilityScores;
        }

        public AbilityScores AbilityScores { get; private set; }
    }
}
