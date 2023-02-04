// <copyright file="IConciergePage.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using Concierge.Display.Enums;

    public interface IConciergePage
    {
        ConciergePage ConciergePage { get; }

        bool HasEditableDataGrid { get; }

        void Draw();

        void Edit(object itemToEdit);
    }
}
