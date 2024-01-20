// <copyright file="MessageHistory.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data
{
    using System;
    using System.Windows.Media;
    using Concierge.Common;
    using Concierge.Data.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Represents a historical message with information about its content, type, and timestamp.
    /// </summary>
    public sealed class MessageHistory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHistory"/> class.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="type">The type of the message (e.g., Information, Warning, Error).</param>
        public MessageHistory(string message, MessageType type)
        {
            var icon = GetIcon(type);

            this.Message = message;
            this.Type = type;
            this.Time = ConciergeDateTime.MessageTime;
            this.Icon = icon.Item1;
            this.IconColor = icon.Item2;
        }

        /// <summary>
        /// Gets an empty <see cref="MessageHistory"/> instance.
        /// </summary>
        public static MessageHistory Empty => new (string.Empty, MessageType.Unknown);

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the type of the message (e.g., Information, Warning, Error).
        /// </summary>
        public MessageType Type { get; private set; }

        /// <summary>
        /// Gets the timestamp when the message was created.
        /// </summary>
        public string Time { get; private set; }

        /// <summary>
        /// Gets the icon representing the message.
        /// </summary>
        public PackIconKind Icon { get; private set; }

        /// <summary>
        /// Gets the color of the icon representing the message.
        /// </summary>
        public Brush IconColor { get; private set; }

        private static (PackIconKind, Brush) GetIcon(MessageType type)
        {
            return type switch
            {
                MessageType.Information => ((PackIconKind, Brush))(PackIconKind.InformationSlabBox, Brushes.LightBlue),
                MessageType.Warning => ((PackIconKind, Brush))(PackIconKind.AlertBox, Brushes.Goldenrod),
                MessageType.Error => ((PackIconKind, Brush))(PackIconKind.AlertOctagon, Brushes.IndianRed),
                _ => ((PackIconKind, Brush))(PackIconKind.HelpBox, Brushes.White),
            };
        }
    }
}
