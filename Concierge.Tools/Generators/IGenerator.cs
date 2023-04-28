// <copyright file="IGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators
{
    public interface IGenerator
    {
        IGeneratorResult Generate(IGeneratorSettings generatorSettings);
    }
}
