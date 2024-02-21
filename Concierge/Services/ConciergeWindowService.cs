// <copyright file="ConciergeWindowService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;

    using Concierge.Character.Aspects;
    using Concierge.Character.Enums;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Tools;

    using static Concierge.Display.Components.ConciergeWindow;

    using Attribute = Concierge.Character.Aspects.Attribute;

    public static class ConciergeWindowService
    {
        public static bool ShowAdd<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return false;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            return conciergeWindow.ShowAdd(item);
        }

        public static void ShowEdit<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = Create(typeOfWindow);
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
            var conciergeWindow = Create(typeOfWindow);
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
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            conciergeWindow.ShowEdit(item, equippedItem);
        }

        public static ConciergeResult ShowHeal<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return ConciergeResult.NoResult;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            return conciergeWindow.ShowHeal(item);
        }

        public static ConciergeResult ShowDamage<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePage conciergePage)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return ConciergeResult.NoResult;
            }

            conciergeWindow.ApplyChanges += applyEvent;
            conciergeWindow.ConciergePage = conciergePage;

            return conciergeWindow.ShowDamage(item);
        }

        public static CustomColor ShowColorWindow(Type typeOfWindow, CustomColor color)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return CustomColor.Invalid;
            }

            return conciergeWindow.ShowColorWindow(color);
        }

        public static ConciergeResult ShowUseItemWindow(Type typeOfWindow, UsedItem usedItem)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return ConciergeResult.NoResult;
            }

            return conciergeWindow.ShowUseItemWindow(usedItem);
        }

        public static AbilitySave ShowAbilityCheckWindow(Type typeOfWindow, Attribute attribute, int value)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return AbilitySave.None;
            }

            return conciergeWindow.ShowAbilityCheckWindow(attribute, value);
        }

        public static AbilitySave ShowAbilityCheckWindow(Type typeOfWindow, Skill skill, int value)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return AbilitySave.None;
            }

            return conciergeWindow.ShowAbilityCheckWindow(skill, value);
        }

        public static object? ShowWindow(Type typeOfWindow)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return null;
            }

            return conciergeWindow.ShowWindow();
        }

        public static void ShowWindow(Type typeOfWindow, ApplyChangesEventHandler applyEvent)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return;
            }

            conciergeWindow.ApplyChanges += applyEvent;

            conciergeWindow.ShowWindow();
        }

        public static ConciergeWindow? ShowNonBlockingWindow(Type typeOfWindow)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return null;
            }

            return conciergeWindow.ShowNonBlockingWindow();
        }

        private static ConciergeWindow? Create(Type typeOfWindow)
        {
            return (ConciergeWindow?)Activator.CreateInstance(typeOfWindow);
        }
    }
}
