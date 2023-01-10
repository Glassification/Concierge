// <copyright file="ConsoleResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System.Windows.Media;

    using Concierge.Console.Enums;

    public sealed class ConsoleResult
    {
        private string message = string.Empty;

        public ConsoleResult(string message, ResultType type)
        {
            this.Message = message;
            this.Type = type;
        }

        public static ConsoleResult Empty => new ("No Result", ResultType.Error);

        public string Message
        {
            get
            {
                return this.Type == ResultType.Information ? this.message : $"   {this.message}";
            }

            set
            {
                this.message = value;
            }
        }

        public ResultType Type { get; set; }

        public Brush TextColor
        {
            get
            {
                return this.Type switch
                {
                    ResultType.Success => Brushes.LightGreen,
                    ResultType.Warning => Brushes.Orange,
                    ResultType.Error => Brushes.IndianRed,
                    _ => Brushes.White,
                };
            }
        }
    }
}
