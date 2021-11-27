// <copyright file="ConciergeMessageBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Interface
{
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.UtilityInterface;

    public static class ConciergeMessageBox
    {
        static ConciergeMessageBox()
        {
            ConciergeMessageWindow = new ConciergeMessageWindow();
        }

        private static ConciergeMessageWindow ConciergeMessageWindow { get; }

        public static ConciergeWindowResult Show(
            string message,
            string title,
            ConciergeWindowButtons messageWindowButtons,
            ConciergeWindowIcons messageWindowIcons)
        {
            return ConciergeMessageWindow.ShowWindow(message, title, messageWindowButtons, messageWindowIcons);
        }
    }
}
