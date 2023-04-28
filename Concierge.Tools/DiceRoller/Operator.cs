// <copyright file="Operator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    public sealed class Operator
    {
        public Operator(string symbol, int precedence, bool rightAssociative)
        {
            this.Symbol = symbol;
            this.Precedence = precedence;
            this.RightAssociative = rightAssociative;
        }

        public string Symbol { get; set; }

        public int Precedence { get; set; }

        public bool RightAssociative { get; set; }
    }
}
