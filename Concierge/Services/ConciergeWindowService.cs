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

    /// <summary>
    /// Provides methods for showing various types of Concierge windows.
    /// </summary>
    public static class ConciergeWindowService
    {
        /// <summary>
        /// Shows the window to add an item.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item to add.</param>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="applyEvent">The event handler for applying changes.</param>
        /// <param name="conciergePage">The Concierge page associated with the window.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public static bool ShowAdd<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePages conciergePage)
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

        /// <summary>
        /// Shows the "Edit" window for a specified item.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item to be edited.</param>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="applyEvent">The event handler for applying changes.</param>
        /// <param name="conciergePage">The Concierge page to associate with the window.</param>
        public static void ShowEdit<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePages conciergePage)
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

        /// <summary>
        /// Shows the "Edit" window for a specified item, with an additional sender object.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item to be edited.</param>
        /// <param name="sender">The sender object.</param>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="applyEvent">The event handler for applying changes.</param>
        /// <param name="conciergePage">The Concierge page to associate with the window.</param>
        public static void ShowEdit<T>(T item, object sender, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePages conciergePage)
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

        /// <summary>
        /// Shows the "Edit" window for a specified item, indicating whether it's equipped.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item to be edited.</param>
        /// <param name="equippedItem">Specifies whether the item is equipped.</param>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="applyEvent">The event handler for applying changes.</param>
        /// <param name="conciergePage">The Concierge page to associate with the window.</param>
        public static void ShowEdit<T>(T item, bool equippedItem, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePages conciergePage)
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

        /// <summary>
        /// Shows the "Heal" window for a specified item.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item to be healed.</param>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="applyEvent">The event handler for applying changes.</param>
        /// <param name="conciergePage">The Concierge page to associate with the window.</param>
        /// <returns>The result of showing the window.</returns>
        public static ConciergeResult ShowHeal<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePages conciergePage)
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

        /// <summary>
        /// Shows the "Damage" window for a specified item.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="item">The item to be damaged.</param>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="applyEvent">The event handler for applying changes.</param>
        /// <param name="conciergePage">The Concierge page to associate with the window.</param>
        /// <returns>The result of showing the window.</returns>
        public static ConciergeResult ShowDamage<T>(T item, Type typeOfWindow, ApplyChangesEventHandler applyEvent, ConciergePages conciergePage)
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

        /// <summary>
        /// Shows the "Color" window.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="color">The custom color to be displayed.</param>
        /// <returns>The custom color chosen in the window.</returns>
        public static CustomColor ShowColorWindow(Type typeOfWindow, CustomColor color)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return CustomColor.Invalid;
            }

            return conciergeWindow.ShowColorWindow(color);
        }

        /// <summary>
        /// Shows the "Icon" window.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="iconKind">The initial icon to default to.</param>
        /// <returns>The custom color chosen in the window.</returns>
        public static CustomIcon ShowIconWindow(Type typeOfWindow, CustomIcon iconKind)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return iconKind;
            }

            return conciergeWindow.ShowIconWindow(iconKind);
        }

        /// <summary>
        /// Shows the "UseItem" window.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="usedItem">The item to be used.</param>
        /// <returns>The result of showing the window.</returns>
        public static ConciergeResult ShowUseItemWindow(Type typeOfWindow, UsedItem usedItem)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return ConciergeResult.NoResult;
            }

            return conciergeWindow.ShowUseItemWindow(usedItem);
        }

        /// <summary>
        /// Shows the "AbilityCheck" window for a specified attribute.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="attribute">The attribute to be checked.</param>
        /// <param name="value">The value associated with the attribute.</param>
        /// <returns>The result of showing the window.</returns>
        public static AbilitySave ShowAbilityCheckWindow(Type typeOfWindow, Attribute attribute, int value)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return AbilitySave.None;
            }

            return conciergeWindow.ShowAbilityCheckWindow(attribute, value);
        }

        /// <summary>
        /// Shows the "AbilityCheck" window for a specified skill.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window.</param>
        /// <param name="skill">The skill to be checked.</param>
        /// <param name="value">The value associated with the skill.</param>
        /// <returns>The result of showing the window.</returns>
        public static AbilitySave ShowAbilityCheckWindow(Type typeOfWindow, Skill skill, int value)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return AbilitySave.None;
            }

            return conciergeWindow.ShowAbilityCheckWindow(skill, value);
        }

        /// <summary>
        /// Shows a window of the specified type.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window to show.</param>
        /// <returns>The displayed window or null if the window creation failed.</returns>
        public static object? ShowWindow(Type typeOfWindow)
        {
            var conciergeWindow = Create(typeOfWindow);
            if (conciergeWindow is null)
            {
                return null;
            }

            return conciergeWindow.ShowWindow();
        }

        /// <summary>
        /// Shows a window of the specified type and attaches an event handler to handle changes.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window to show.</param>
        /// <param name="applyEvent">The event handler to handle changes in the window.</param>
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

        /// <summary>
        /// Shows a non-blocking window of the specified type.
        /// </summary>
        /// <param name="typeOfWindow">The type of the window to show.</param>
        /// <returns>The non-blocking window instance if created successfully; otherwise, null.</returns>
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
