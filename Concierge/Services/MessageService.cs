// <copyright file="MessageService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;

    using Concierge.Data;
    using Concierge.Data.Enums;

    public class MessageService
    {
        private readonly List<MessageHistory> messages = [];

        public MessageService()
        {
        }

        public void Add(MessageHistory message)
        {
            this.messages.Add(message);
        }

        public void Add(string message, MessageType type)
        {
            this.messages.Add(new MessageHistory(message, type));
        }

        public void Clear()
        {
            this.messages.Clear();
        }

        public List<MessageHistory> Get()
        {
            return this.messages;
        }
    }
}
