// <copyright file="ShuntingYard.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.RollDice
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ShuntingYard
    {
        private static readonly IDictionary<string, Operator> operators = new[]
        {
            new Operator("^", 4, true),
            new Operator("*", 3, false),
            new Operator("/", 3, false),
            new Operator("+", 2, false),
            new Operator("-", 2, false),
        }.ToDictionary(x => x.Symbol);

        public static Stack<object> ToPostfix(List<object> list)
        {
            var stack = new Stack<object>();
            var output = new List<object>();

            foreach (object token in list)
            {
                if (token is int obsoleteInt)
                {
                    output.Add(obsoleteInt);
                }
                else if (token is DiceRoll diceRoll)
                {
                    output.Add(diceRoll);
                }
                else if (token is string str && operators.TryGetValue(str, out Operator? op1))
                {
                    while (stack.Count > 0 && stack.Peek() is string str2 && operators.TryGetValue(str2, out Operator? op2))
                    {
                        int c = op1?.Precedence.CompareTo(op2?.Precedence) ?? 0;
                        if (c < 0 || (!(op1?.RightAssociative ?? false) && c <= 0))
                        {
                            output.Add(stack.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }

                    stack.Push(token);
                }
            }

            while (stack.Count > 0)
            {
                var top = stack.Pop();
                output.Add(top);
            }

            output.Reverse();
            return new Stack<object>(output);
        }
    }
}
