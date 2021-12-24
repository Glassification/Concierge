// <copyright file="Utilities.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Colors;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Extensions;

    public static class Utilities
    {
        public static double CombineFeetAndInches(double feet, double inches)
        {
            return (feet * 12) + inches;
        }

        public static FormattedFeetInchesDto GetSeperateFeetAndInches(double totalInches)
        {
            var feet = (int)(totalInches / 12);
            var inches = totalInches - (feet * 12);

            return new FormattedFeetInchesDto(feet, inches);
        }

        public static void SortListFromDataGrid<T>(ConciergeDataGrid dataGrid, List<T> list, ConciergePage conciergePage)
        {
            if (dataGrid.Items.IsEmpty)
            {
                return;
            }

            var oldList = new List<T>(list);
            list.Clear();

            foreach (var item in dataGrid.Items)
            {
                list.Add((T)item);
            }

            Program.UndoRedoService.AddCommand(
                new ListOrderCommand<T>(
                    list,
                    oldList,
                    new List<T>(list),
                    conciergePage));
            Program.Modify();
        }

        public static List<string> FormatEnumForDisplay(Type enumType)
        {
            var stringArray = Enum.GetNames(enumType);

            for (int i = 0; i < stringArray.Length; i++)
            {
                stringArray[i] = stringArray[i].FormatFromEnum();
            }

            return stringArray.ToList();
        }

        public static T GetElementUnderMouse<T>()
            where T : UIElement
        {
            return FindVisualParent<T>(Mouse.DirectlyOver as UIElement);
        }

        public static T FindVisualParent<T>(UIElement element)
            where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                if (parent is T correctlyTyped)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject child)
                    {
                        if (child is T t)
                        {
                            yield return t;
                        }

                        foreach (T childOfChild in FindVisualChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public static void SetRectangleStyle(Rectangle rectangle, DeathSave deathSave)
        {
            switch (deathSave)
            {
                case DeathSave.None:
                    rectangle.Fill = ConciergeColors.TotalLightBoxBrush;
                    break;
                case DeathSave.Failure:
                    rectangle.Fill = ConciergeColors.FailedSaveBrush;
                    break;
                case DeathSave.Success:
                    rectangle.Fill = ConciergeColors.SucceededSaveBrush;
                    break;
            }
        }

        public static void SetTextStyle(StatusChecks check, TextBlock textBlock)
        {
            switch (check)
            {
                case StatusChecks.Fail:
                    textBlock.TextDecorations = TextDecorations.Strikethrough;
                    textBlock.Foreground = Brushes.DarkGray;
                    textBlock.ToolTip = "Automatic Fail";
                    break;
                case StatusChecks.Advantage:
                    textBlock.TextDecorations = new TextDecorationCollection();
                    textBlock.Foreground = Brushes.Green;
                    textBlock.ToolTip = "Advantage";
                    break;
                case StatusChecks.Disadvantage:
                    textBlock.TextDecorations = new TextDecorationCollection();
                    textBlock.Foreground = Brushes.IndianRed;
                    textBlock.ToolTip = "Disadvantage";
                    break;
                case StatusChecks.Normal:
                default:
                    textBlock.TextDecorations = new TextDecorationCollection();
                    textBlock.Foreground = Brushes.White;
                    textBlock.ToolTip = null;
                    break;
            }
        }

        public static void SetProficiencyBoxStyle(bool flag, Rectangle rectangle)
        {
            rectangle.Fill = flag ? ConciergeColors.ProficiencyBrush : Brushes.Transparent;
        }

        public static Brush SetHealthStyle(Vitality vitality)
        {
            int third = vitality.Health.MaxHealth / 3;
            int hp = vitality.CurrentHealth;

            return hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;
        }

        public static int IncrementUsedSlots(int used, int total)
        {
            if (used < total)
            {
                ConciergeSound.UpdateValue();

                return used + 1;
            }

            return used;
        }

        public static void SetCursor(int spent, int total, Func<int, int, bool> func, Cursor cursor)
        {
            if (func(spent, total))
            {
                Mouse.OverrideCursor = cursor;
            }
        }

        public static void SetBorderColour(int spent, int total, Grid grid, Border border, string currentName)
        {
            border.BorderBrush =
                spent != total && currentName.Equals(grid.Name)
                ? ConciergeColors.RectangleBorderHighlight
                : grid.Background;
        }

        public static T GetPropertyValue<T>(object source, string propertyName)
        {
            return (T)source.GetType().GetProperty(propertyName).GetValue(source, null);
        }

        public static int CalculateBonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        public static int CalculateBonusFromAbility(Abilities ability, ConciergeCharacter character)
        {
            int bonus = character.ProficiencyBonus;

            return ability switch
            {
                Abilities.STR => CalculateBonus(character.Attributes.Strength) + bonus,
                Abilities.DEX => CalculateBonus(character.Attributes.Dexterity) + bonus,
                Abilities.CON => CalculateBonus(character.Attributes.Constitution) + bonus,
                Abilities.INT => CalculateBonus(character.Attributes.Intelligence) + bonus,
                Abilities.WIS => CalculateBonus(character.Attributes.Wisdom) + bonus,
                Abilities.CHA => CalculateBonus(character.Attributes.Charisma) + bonus,
                Abilities.NONE => bonus,
                _ => throw new NotImplementedException(),
            };
        }

        public static string FormatName(string name)
        {
            var ch = name.ToArray();
            int offset = 0;

            for (int i = 1; i < ch.Length; i++)
            {
                if (char.IsUpper(ch[i]))
                {
                    name = name.Insert(i + offset, " ");
                    offset++;
                }
            }

            return name;
        }

        public static bool ValidateClassLevel(ConciergeCharacter character, Guid id, int newValue)
        {
            var totalLevel =
                (character.Properties.Class1.Id.Equals(id) ? 0 : character.Properties.Class1.Level) +
                (character.Properties.Class2.Id.Equals(id) ? 0 : character.Properties.Class2.Level) +
                (character.Properties.Class3.Id.Equals(id) ? 0 : character.Properties.Class3.Level);

            totalLevel += newValue;

            return totalLevel is <= Constants.MaxLevel and >= 0;
        }

        public static Brush SetUsedTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkRed : Brushes.White;
        }

        public static Brush SetUsedBoxStyle(int total, int used)
        {
            return total <= used ? Brushes.IndianRed : ConciergeColors.UsedBoxBrush;
        }

        public static Brush SetTotalTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkGray : Brushes.White;
        }

        public static Brush SetTotalBoxStyle(int total, int used)
        {
            return total <= used ? ConciergeColors.TotalDarkBoxBrush : ConciergeColors.TotalLightBoxBrush;
        }
    }
}
