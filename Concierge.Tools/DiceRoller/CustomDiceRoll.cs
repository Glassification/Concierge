// <copyright file="CustomDiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    using System;
    using System.Collections.Generic;

    using Concierge.Common.Exceptions;

    public sealed class CustomDiceRoll : IDiceRoll
    {
        public CustomDiceRoll(Stack<object> stack)
        {
            this.Dice = string.Empty;
            this.FirstIteration = true;

            this.Total = Math.Max(this.Compute(stack), 0);
        }

        public string Dice { get; private set; }

        public int Total { get; private set; }

        private bool FirstIteration { get; set; }

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

        private int GetValueAndAddToString(object popped, bool isSecond = false)
        {
            if (popped is DiceRoll diceRoll)
            {
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

        private int Compute(Stack<object> stack)
        {
            if (stack.Count == 0)
            {
                throw new EvaluationException();
            }

            if (stack.Count == 1)
            {
                stack.Push(this.GetValueAndAddToString(stack.Pop()));
            }
            else
            {
                while (stack.Count >= 3)
                {
                    var first = stack.Pop();
                    var second = stack.Pop();
                    var operation = stack.Pop();

                    var firstInt = this.GetValueAndAddToString(first);
                    var operationString = this.GetOperationAndAddToString(operation);
                    var secondInt = this.GetValueAndAddToString(second, true);

                    var result = DoOperation(firstInt, secondInt, operationString);
                    stack.Push(result);

                    this.FirstIteration = false;
                }
            }

            return (int)stack.Pop();
        }
    }
}
