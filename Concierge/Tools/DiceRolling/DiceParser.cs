// <copyright file="DiceParser.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRolling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Concierge.Exceptions;
    using Concierge.Tools.DiceRolling.Dice;
    using Concierge.Utility.Extensions;

    public static class DiceParser
    {
        private static readonly Regex patternSplit = new (@"(\+|\-)", RegexOptions.Compiled);
        private static readonly Regex hasNumber = new ("[0-9]", RegexOptions.Compiled);

        public static Stack<object> Parse(string input)
        {
            if (!IsValidInput(input))
            {
                throw new InvalidExpressionException(input);
            }

            try
            {
                var list = SplitAndMaintainDelimiter(input);
                var infixObjectList = Evaluate(list);
                var postfixObjectList = ShuntingYard.ToPostfix(infixObjectList);

                IsValidPostFix(postfixObjectList);

                return postfixObjectList;
            }
            catch (Exception)
            {
                throw new InvalidExpressionException(input);
            }
        }

        private static List<object> Evaluate(List<string> list)
        {
            var objectList = new List<object>();
            int number;
            int sides;

            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].ToLower();
                if (!list[i].Contains('d'))
                {
                    if (int.TryParse(list[i], out int isInt))
                    {
                        objectList.Add(isInt);
                    }
                    else
                    {
                        objectList.Add(list[i]);
                    }

                    continue;
                }

                var tokens = list[i].Split('d').RemoveEmpty();
                if (tokens.Length > 1)
                {
                    number = int.Parse(tokens[0]);
                    sides = int.Parse(tokens[1]);
                }
                else
                {
                    number = 1;
                    sides = int.Parse(tokens[0]);
                }

                var roll = DiceRoll.RollDice(number, sides);
                objectList.Add(new DiceRoll(sides, roll, 0));
            }

            return objectList;
        }

        private static bool IsValidInput(string input)
        {
            return
                !input.IsNullOrWhiteSpace() &&
                !input.Contains('.') &&
                !input.Contains('*') &&
                !input.Contains('/') &&
                hasNumber.Match(input).Success;
        }

        private static void IsValidPostFix(Stack<object> postfix)
        {
            if (postfix.Count > 1)
            {
                return;
            }

            if (postfix.FirstOrDefault() is string)
            {
                throw new Exception();
            }
        }

        private static List<string> SplitAndMaintainDelimiter(string input)
        {
            return patternSplit.Split(input).ToList();
        }
    }
}
