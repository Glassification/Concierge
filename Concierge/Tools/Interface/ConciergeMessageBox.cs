// <copyright file="ConciergeMessageBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Interface
{
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.UtilityInterface;

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
