// <copyright file="MessageService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;

    using Concierge.Data;
    using Concierge.Data.Enums;

    /// <summary>
    /// Provides functionality for managing messages and message history.
    /// </summary>
    public class MessageService
    {
        private readonly List<MessageHistory> messages = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        public MessageService()
        {
        }

        /// <summary>
        /// Adds a message to the message history.
        /// </summary>
        /// <param name="message">The message to add.</param>
        public void Add(MessageHistory message)
        {
            this.messages.Add(message);
        }

        /// <summary>
        /// Adds a message with the specified content and type to the message history.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="type">The type of the message.</param>
        public void Add(string message, MessageType type)
        {
            this.messages.Add(new MessageHistory(message, type));
        }

        /// <summary>
        /// Clears the message history.
        /// </summary>
        public void Clear()
        {
            this.messages.Clear();
        }

        /// <summary>
        /// Gets the list of messages in the message history.
        /// </summary>
        /// <returns>The list of messages.</returns>
        public List<MessageHistory> Get()
        {
            return this.messages;
        }
    }
}
