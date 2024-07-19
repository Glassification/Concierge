// <copyright file="ConciergeMessageBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using Concierge.Data.Enums;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;

    /// <summary>
    /// Utility class for displaying message boxes with custom icons and buttons.
    /// </summary>
    public static class ConciergeMessageBox
    {
        /// <summary>
        /// Shows a message box with the specified message, title, custom buttons, and icon.
        /// </summary>
        /// <param name="message">The message to display in the message box.</param>
        /// <param name="title">The title of the message box.</param>
        /// <param name="messageWindowButtons">The custom buttons to display in the message box.</param>
        /// <param name="messageWindowIcons">The custom icon to display in the message box.</param>
        /// <returns>A <see cref="ConciergeResult"/> indicating the result of the user interaction with the message box.</returns>
        public static ConciergeResult Show(
            string message,
            string title,
            ConciergeButtons messageWindowButtons,
            ConciergeIcons messageWindowIcons)
        {
            Program.MessageService.Add(message, IconToType(messageWindowIcons));
            return new ConciergeMessageWindow().ShowWindow(message, title, messageWindowButtons, messageWindowIcons);
        }

        /// <summary>
        /// Shows an error message box with the specified message.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        /// <returns>A <see cref="ConciergeResult"/> indicating the result of the user interaction with the error message box.</returns>
        public static ConciergeResult ShowError(string message)
        {
            Program.MessageService.Add(message, MessageType.Error);
            return new ConciergeMessageWindow().ShowWindow(message, "Error", ConciergeButtons.Ok, ConciergeIcons.Error);
        }

        /// <summary>
        /// Shows a warning message box with the specified message.
        /// </summary>
        /// <param name="message">The warning message to display.</param>
        /// <returns>A <see cref="ConciergeResult"/> indicating the result of the user interaction with the warning message box.</returns>
        public static ConciergeResult ShowWarning(string message)
        {
            Program.MessageService.Add(message, MessageType.Warning);
            return new ConciergeMessageWindow().ShowWindow(message, "Warning", ConciergeButtons.Ok, ConciergeIcons.Warning);
        }

        private static MessageType IconToType(ConciergeIcons messageWindowIcons)
        {
            return messageWindowIcons switch
            {
                ConciergeIcons.Question => MessageType.Question,
                ConciergeIcons.Alert => MessageType.Information,
                ConciergeIcons.Information => MessageType.Information,
                ConciergeIcons.Warning => MessageType.Warning,
                ConciergeIcons.Error => MessageType.Error,
                _ => MessageType.Popup,
            };
        }
    }
}
