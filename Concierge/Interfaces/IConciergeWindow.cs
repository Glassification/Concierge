// <copyright file="IConciergeWindow.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces
{
    using Concierge.Interfaces.Enums;

    public interface IConciergeWindow
    {
        ConciergeWindowResult ShowWizardSetup();

        void UpdateCancelButton(string text);
    }
}
