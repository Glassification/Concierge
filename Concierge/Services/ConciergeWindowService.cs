// <copyright file="ConciergeWindowService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Tools;

    using static Concierge.Display.Components.ConciergeWindow;

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

            return conciergeWindow.ShowAdd(item);
        }

        public static bool ShowAdd<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage, ICreature creature)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return false;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            return conciergeWindow.ShowAdd(item, creature);
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

            conciergeWindow.ShowEdit(item);
        }

        public static void ShowEdit<T>(T item, object sender, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            conciergeWindow.ShowEdit(item, sender);
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

            conciergeWindow.ShowEdit(item, equippedItem);
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

            return conciergeWindow.ShowHeal(item);
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

            return conciergeWindow.ShowDamage(item);
        }

        public static CustomColor ShowColorWindow(Type typeOfWindow, CustomColor color)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return CustomColor.Invalid;
            }

            return conciergeWindow.ShowColorWindow(color);
        }

        public static ConciergeWindowResult ShowUseItemWindow(Type typeOfWindow, UsedItem usedItem)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return ConciergeWindowResult.NoResult;
            }

            return conciergeWindow.ShowUseItemWindow(usedItem);
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

        public static object? ShowWindow(Type typeOfWindow)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return null;
            }

            return conciergeWindow.ShowWindow();
        }

        public static void ShowWindow(Type typeOfWindow, ApplyChangesEventHandler applyEvent)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return;
            }

            conciergeWindow.ApplyChanges += applyEvent;

            conciergeWindow.ShowWindow();
        }

        public static ConciergeWindow? ShowNonBlockingWindow(Type typeOfWindow)
        {
            var conciergeWindow = (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
            if (conciergeWindow is null)
            {
                return null;
            }

            return conciergeWindow.ShowNonBlockingWindow();
        }
    }
}
