// <copyright file="ConsoleResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the result of a console operation.
    /// </summary>
    public sealed class ConsoleResult
    {
        private string message = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleResult"/> class with default values.
        /// </summary>
        public ConsoleResult()
            : this(string.Empty, ResultType.Information)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleResult"/> class with specified message and type.
        /// </summary>
        /// <param name="message">The message of the console result.</param>
        /// <param name="type">The type of the console result.</param>
        public ConsoleResult(string message, ResultType type)
        {
            this.Message = message;
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleResult"/> class with specified message, type, and value.
        /// </summary>
        /// <param name="message">The message of the console result.</param>
        /// <param name="type">The type of the console result.</param>
        /// <param name="value">The value associated with the console result.</param>
        public ConsoleResult(string message, ResultType type, object value)
            : this(message, type)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets an empty console result with default information type.
        /// </summary>
        [JsonIgnore]
        public static ConsoleResult Empty => new (Constants.ConsolePrompt, ResultType.Information);

        /// <summary>
        /// Gets or sets the message of the console result.
        /// </summary>
        public string Message
        {
            get => this.Type == ResultType.Information ? this.message : $"   {this.message}";
            set => this.message = value;
        }

        /// <summary>
        /// Gets or sets the type of the console result.
        /// </summary>
        public ResultType Type { get; set; }

        /// <summary>
        /// Gets or initializes the value associated with the console result.
        /// </summary>
        [JsonIgnore]
        public object? Value { get; init; }

        /// <summary>
        /// Gets the text color associated with the console result.
        /// </summary>
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
                    ResultType.NotImplemented => Brushes.IndianRed,
                    ResultType.Information => Brushes.White,
                    _ => Brushes.White,
                };
            }
        }

        /// <summary>
        /// Creates a default error console result for a given command.
        /// </summary>
        /// <param name="command">The command causing the error.</param>
        public static ConsoleResult DefaultError(string command)
        {
            return new ConsoleResult($"Error: '{command.Strip(Constants.ConsolePrompt)}' does not contain a valid command.", ResultType.Error);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}
