// <copyright file="ConciergeWindowService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;

    using static Concierge.Interfaces.Components.ConciergeWindow;

    public static class ConciergeWindowService
    {
        public static bool ShowAdd<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return false;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            return conciergeWindow.ShowAdd<T>(item);
        }

        public static void ShowEdit<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            conciergeWindow.ShowEdit<T>(item);
        }

        public static void ShowEdit<T>(T item, bool equippedItem, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            conciergeWindow.ShowEdit<T>(item, equippedItem);
        }

        public static ConciergeWindowResult ShowHeal<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return ConciergeWindowResult.NoResult;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            return conciergeWindow.ShowHeal<T>(item);
        }

        public static ConciergeWindowResult ShowDamage<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return ConciergeWindowResult.NoResult;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            return conciergeWindow.ShowDamage<T>(item);
        }

        public static Color ShowColorWindow(Type typeOfWindow, Color color)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return Colors.Transparent;
            }

            return conciergeWindow.ShowColorWindow(color);
        }

        public static PopupButtons ShowPopup(Type typeOfWindow)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return PopupButtons.None;
            }

            return conciergeWindow.ShowPopup();
        }

        public static void ShowWindow(Type typeOfWindow)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return;
            }

            conciergeWindow.ShowWindow();
        }
    }
}
