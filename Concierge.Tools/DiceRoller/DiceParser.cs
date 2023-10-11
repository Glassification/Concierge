// <copyright file="DiceParser.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Provides methods to parse and evaluate dice expressions, returning the results in postfix notation.
    /// </summary>
    public static class DiceParser
    {
        private static readonly Regex patternSplit = new (@"(\+|\-)", RegexOptions.Compiled);
        private static readonly Regex hasNumber = new ("[0-9]", RegexOptions.Compiled);
        private static readonly Regex hasDice = new ("\\d[Dd]\\d\\+*\\d*", RegexOptions.Compiled);

        private static readonly string[] filter = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "+", "-", "d" };

        /// <summary>
        /// Parses the input dice expression and returns a stack of objects in postfix notation representing the expression.
        /// </summary>
        /// <param name="input">The input dice expression to parse.</param>
        /// <returns>A stack of objects in postfix notation representing the parsed dice expression.</returns>
        /// <exception cref="InvalidExpressionException">Thrown if the input expression is invalid.</exception>
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

        /// <summary>
        /// Finds the dice expressions within the provided input string.
        /// </summary>
        /// <param name="input">The input string containing dice expressions.</param>
        /// <returns>A string of found dice expressions.</returns>
        public static string Find(string input)
        {
            var builder = new StringBuilder();
            var matches = hasDice.Matches(input.Strip(" "));
            if (matches.Count == 0)
            {
                return string.Empty;
            }

            foreach (Match match in matches.Cast<Match>())
            {
                builder.Append(match.Value);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Cleans the input dice expression by removing unwanted characters and ensuring proper formatting.
        /// </summary>
        /// <param name="input">The input dice expression to clean.</param>
        /// <param name="keywords">An array of keywords to consider during cleaning.</param>
        /// <returns>A cleaned and formatted dice expression.</returns>
        public static string Clean(string input, string[] keywords)
        {
            input = input.Strip(StringComparison.InvariantCultureIgnoreCase, keywords);
            input = input.Replace(",", " ");
            if (input.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (int.TryParse(input[i].ToString(), out _))
                {
                    if (i != 0 && !input[i - 1].ToString().IsAny(filter))
                    {
                        input = input.Insert(i, "+");
                        i++;
                    }
                }
            }

            return input;
        }

        /// <summary>
        /// Determines whether the input string is valid for dice parsing.
        /// </summary>
        /// <param name="input">The input string to be checked.</param>
        /// <returns><c>true</c> if the input string is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidInput(string input)
        {
            return
                !input.IsNullOrWhiteSpace() &&
                !input.Contains('.') &&
                !input.Contains('*') &&
                !input.Contains('/') &&
                hasNumber.Match(input).Success;
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
