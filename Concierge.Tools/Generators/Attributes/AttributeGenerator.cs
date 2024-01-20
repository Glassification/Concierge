// <copyright file="AttributeGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Attributes
{
    using System;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Tools.DiceRoller;

    public sealed class AttributeGenerator : IGenerator
    {
        private readonly int[] attributeRolls = new int[6];

        public AttributeGenerator()
        {
        }

        public IGeneratorResult Generate(IGeneratorSettings generatorSettings)
        {
            Array.Clear(this.attributeRolls);
            while (this.attributeRolls.Sum() <= Constants.MinAttributeTotal)
            {
                for (int i = 0; i < this.attributeRolls.Length; i++)
                {
                    this.attributeRolls[i] = CalculateAttribute();
                }
            }

            return new AttributeResult(this.attributeRolls);
        }

        private static int CalculateAttribute()
        {
            var rolls = DiceRoll.RollDice(4, 6);
            return rolls.Sum() - rolls.Min();
        }
    }
}
