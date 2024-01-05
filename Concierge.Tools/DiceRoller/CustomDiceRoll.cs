// <copyright file="CustomDiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    using System;
    using System.Collections.Generic;

    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents a custom dice roll expression composed of dice rolls and mathematical operations.
    /// </summary>
    public sealed class CustomDiceRoll : IDiceRoll
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDiceRoll"/> class with the specified stack.
        /// </summary>
        /// <param name="input">A string representing dice rolls and operations.</param>
        public CustomDiceRoll(string input)
        {
            var stack = DiceParser.Parse(input);

            this.Dice = string.Empty;
            this.FirstIteration = true;
            this.Stack = stack.Clone();

            this.Total = Math.Max(this.Compute(stack), 0);
        }

        /// <summary>
        /// Gets the textual representation of the custom dice roll expression.
        /// </summary>
        public string Dice { get; private set; }

        /// <summary>
        /// Gets the total sum of the custom dice roll expression.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Gets the maximum possible sum of the custom dice roll expression.
        /// </summary>
        public int Max { get; private set; }

        /// <summary>
        /// Gets the minimum possible sum of the custom dice roll expression.
        /// </summary>
        public int Min { get; private set; }

        private bool FirstIteration { get; set; }

        private Stack<object> Stack { get; set; }

        public override string ToString()
        {
            return $"[{this.Dice}] Total {this.Total}";
        }

        /// <summary>
        /// Re-rolls the dice and computes the total for the custom dice roll.
        /// </summary>
        public void ReRoll()
        {
            this.Dice = string.Empty;
            this.FirstIteration = true;
            this.Total = Math.Max(this.Compute(this.Stack.Clone(), true), 0);
        }

        private static int DoOperation(int first, int second, string operation)
        {
            return operation switch
            {
                "/" => first / second,
                "*" => first * second,
                "-" => first - second,
                _ => first + second,
            };
        }

        private int GetValueAndAddToString(object popped, bool reRoll, bool isSecond = false)
        {
            if (popped is DiceRoll diceRoll)
            {
                if (reRoll)
                {
                    diceRoll.ReRoll();
                }

                if (this.FirstIteration || isSecond)
                {
                    this.Dice += diceRoll.Dice;
                }

                return diceRoll.Total;
            }

            if (popped is int modifer)
            {
                if (this.FirstIteration || isSecond)
                {
                    this.Dice += modifer.ToString();
                }

                return modifer;
            }

            return 0;
        }

        private string GetOperationAndAddToString(object popped)
        {
            if (popped is string operation)
            {
                this.Dice += $" {operation} ";
                return operation;
            }

            return string.Empty;
        }

        private int Compute(Stack<object> stack, bool reRoll = false)
        {
            if (stack.Count == 0)
            {
                throw new EvaluationException();
            }

            if (stack.Count == 1)
            {
                var obj = stack.Pop();

                this.AddToMin(obj, null, string.Empty, reRoll);
                this.AddToMax(obj, null, string.Empty, reRoll);

                stack.Push(this.GetValueAndAddToString(obj, reRoll));
            }
            else
            {
                while (stack.Count >= 3)
                {
                    var first = stack.Pop();
                    var second = stack.Pop();
                    var operation = stack.Pop();

                    this.AddToMin(first, second, operation, reRoll);
                    this.AddToMax(first, second, operation, reRoll);

                    var firstInt = this.GetValueAndAddToString(first, reRoll);
                    var operationString = this.GetOperationAndAddToString(operation);
                    var secondInt = this.GetValueAndAddToString(second, reRoll, true);

                    var result = DoOperation(firstInt, secondInt, operationString);
                    stack.Push(result);

                    this.FirstIteration = false;
                }
            }

            return (int)stack.Pop();
        }

        private void AddToMin(object left, object? right, object operation, bool reRoll)
        {
            var leftValue = 0;
            var rightValue = 0;

            if (reRoll || operation is not string opp)
            {
                return;
            }

            if (left is DiceRoll leftRoll)
            {
                leftValue = leftRoll.Min;
            }
            else if (left is int leftInt)
            {
                leftValue = leftInt;
            }

            if (right is null)
            {
                this.Min += leftValue;
                return;
            }

            if (right is DiceRoll rightRoll)
            {
                rightValue = rightRoll.Min;
            }
            else if (right is int rightInt)
            {
                rightValue = rightInt;
            }

            if (opp.Equals("+"))
            {
                this.Min += leftValue + rightValue;
            }
            else if (opp.Equals("-"))
            {
                this.Min += leftValue - rightValue;
            }
        }

        private void AddToMax(object left, object? right, object operation, bool reRoll)
        {
            var leftValue = 0;
            var rightValue = 0;

            if (reRoll || operation is not string opp)
            {
                return;
            }

            if (left is DiceRoll leftRoll)
            {
                leftValue = leftRoll.Max;
            }
            else if (left is int leftInt)
            {
                leftValue = leftInt;
            }

            if (right is null)
            {
                this.Max += leftValue;
                return;
            }

            if (right is DiceRoll rightRoll)
            {
                rightValue = rightRoll.Max;
            }
            else if (right is int rightInt)
            {
                rightValue = rightInt;
            }

            if (opp.Equals("+"))
            {
                this.Max += leftValue + rightValue;
            }
            else if (opp.Equals("-"))
            {
                this.Max += leftValue - rightValue;
            }
        }
    }
}
