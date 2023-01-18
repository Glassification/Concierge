// <copyright file="ConsoleResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System.Windows.Media;

    using Concierge.Console.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public sealed class ConsoleResult
    {
        private string message = string.Empty;

        public ConsoleResult(string message, ResultType type)
        {
            this.Message = message;
            this.Type = type;
        }

        public ConsoleResult(string message, ResultType type, object value)
            : this(message, type)
        {
            this.Value = value;
        }

        [JsonIgnore]
        public static ConsoleResult Empty => new (Constants.ConsolePrompt, ResultType.Information);

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

        [JsonIgnore]
        public object? Value { get; init; }

        [JsonIgnore]
        public Brush TextColor
        {
            get
            {
                return this.Type switch
                {
                    ResultType.Success => Brushes.LightGreen,
                    ResultType.Warning => Brushes.Orange,
                    ResultType.Error => Brushes.IndianRed,
                    ResultType.Information => Brushes.White,
                    _ => Brushes.White,
                };
            }
        }

        public static ConsoleResult Default(string command)
        {
            return new ConsoleResult($"Error: '{command.Strip(Constants.ConsolePrompt)}' does not contain a valid command.", ResultType.Error);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}
