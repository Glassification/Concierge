// <copyright file="AttributeGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Attributes
{
    using System;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Tools.DiceRoller;
    using Concierge.Tools.Enums;

    public sealed class AttributeGenerator : IGenerator
    {
        private readonly int[] attributeRolls = new int[6];
        private readonly int[] standardArray = [15, 14, 13, 12, 10, 8];

        public AttributeGenerator()
        {
        }

        public IGeneratorResult Generate(IGeneratorSettings generatorSettings)
        {
            Array.Clear(this.attributeRolls);
            if (generatorSettings is not AttributeSettings settings)
            {
                return new AttributeResult(this.attributeRolls);
            }

            switch (settings.AbilityScores)
            {
                default:
                case AbilityScores.Roll:
                    this.RollDice();
                    return new AttributeResult(this.attributeRolls);
                case AbilityScores.StandardArray:
                    this.StandardArray();
                    return new AttributeResult(this.attributeRolls);
            }
        }

        private void RollDice()
        {
            while (this.attributeRolls.Sum() <= Constants.MinAttributeTotal)
            {
                for (int i = 0; i < this.attributeRolls.Length; i++)
                {
                    var rolls = DiceRoll.RollDice(4, Dice.D6);
                    this.attributeRolls[i] = rolls.Sum() - rolls.Min();
                }
            }
        }

        private void StandardArray()
        {
            Array.Copy(this.standardArray, this.attributeRolls, 6);
        }
    }
}
