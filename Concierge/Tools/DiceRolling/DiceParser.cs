﻿// <copyright file="DiceParser.cs" company="Thomas Beckett">
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
        private static readonly Regex patternSplit = new (@"(\+|\-|\/|\*)", RegexOptions.Compiled);

        public static Stack<object> Parse(string input)
        {
            if (!IsValid(input))
            {
                throw new InvalidExpressionException(input);
            }

            try
            {
                var list = SplitAndMaintainDelimiter(input);
                var objectList = Evaluate(list);
                var polishObjectList = ShuntingYard.ToPostfix(objectList);

                return polishObjectList;
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
                objectList.Add(new DiceRoll(sides, roll));
            }

            return objectList;
        }

        private static bool IsValid(string input)
        {
            return !input.IsNullOrWhiteSpace() && (!input.Contains('.') && !input.Contains('*') && !input.Contains('/'));
        }

        private static List<string> SplitAndMaintainDelimiter(string input)
        {
            return patternSplit.Split(input).ToList();
        }
    }
}