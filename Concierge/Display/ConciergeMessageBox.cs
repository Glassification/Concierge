// <copyright file="ConciergeMessageBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using Concierge.Data.Enums;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;

    public static class ConciergeMessageBox
    {
        public static ConciergeResult Show(
            string message,
            string title,
            ConciergeButtons messageWindowButtons,
            ConciergeIcons messageWindowIcons)
        {
            Program.MessageService.Add(message, IconToType(messageWindowIcons));
            return new ConciergeMessageWindow().ShowWindow(message, title, messageWindowButtons, messageWindowIcons);
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
