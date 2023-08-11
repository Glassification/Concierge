// <copyright file="Operator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    /// <summary>
    /// Represents an operator used in mathematical expressions.
    /// </summary>
    public sealed class Operator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Operator"/> class with the specified properties.
        /// </summary>
        /// <param name="symbol">The symbol of the operator.</param>
        /// <param name="precedence">The precedence of the operator.</param>
        /// <param name="rightAssociative">Indicates whether the operator is right-associative.</param>
        public Operator(string symbol, int precedence, bool rightAssociative)
        {
            this.Symbol = symbol;
            this.Precedence = precedence;
            this.RightAssociative = rightAssociative;
        }

        /// <summary>
        /// Gets or sets the symbol of the operator.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the precedence of the operator.
        /// </summary>
        public int Precedence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operator is right-associative.
        /// </summary>
        public bool RightAssociative { get; set; }
    }
}
