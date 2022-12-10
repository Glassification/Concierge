// <copyright file="AttributeResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Attributes
{
    using System;

    public sealed class AttributeResult : IGeneratorResult
    {
        public AttributeResult(int[] rolls)
        {
            if (rolls.Length < 6)
            {
                throw new ArgumentException($"{nameof(rolls)} must have a length greater than 5.");
            }

            this.Rolls = rolls;
        }

        public int[] Rolls { get; set; }
    }
}
