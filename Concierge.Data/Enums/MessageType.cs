// <copyright file="MessageType.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data.Enums
{
    /// <summary>
    /// Represents the type of a message.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The message type is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The message is for informational purposes.
        /// </summary>
        Information,

        /// <summary>
        /// The message is a warning.
        /// </summary>
        Warning,

        /// <summary>
        /// The message indicates an error.
        /// </summary>
        Error,
    }
}
