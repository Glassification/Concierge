// <copyright file="ConciergeMessageBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;

    public static class ConciergeMessageBox
    {
        public static ConciergeWindowResult Show(
            string message,
            string title,
            ConciergeWindowButtons messageWindowButtons,
            ConciergeWindowIcons messageWindowIcons)
        {
            return new ConciergeMessageWindow().ShowWindow(message, title, messageWindowButtons, messageWindowIcons);
        }
    }
}
