﻿// <copyright file="UseItemWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;

    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Tools;

    /// <summary>
    /// Interaction logic for UseItemWindow.xaml.
    /// </summary>
    public partial class UseItemWindow : ConciergeWindow
    {
        private UsedItem usedItem = UsedItem.Empty;

        public UseItemWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Use Item";

        public override string WindowName => nameof(UseItemWindow);

        public override ConciergeResult ShowUseItemWindow(UsedItem usedItem)
        {
            this.usedItem = usedItem;
            this.HeaderTextBlock.Text = usedItem.Name;

            this.FillFields(usedItem);
            this.ShowConciergeWindow();

            return this.Result;
        }

        private void FillFields(UsedItem usedItem)
        {
            this.DamageLabel.Text = usedItem.IsHealing ? "Healing:" : "Damage:";

            this.AttackTextBlock.Text = usedItem.Attack.Dice;
            this.DamageTextBlock.Text = usedItem.Damage.Dice;

            this.AttackTotalTextBlock.Text = $"{usedItem.Attack.Total} To Hit";
            this.DamageTotalTextBlock.Text = $"{usedItem.Damage.Total} {(usedItem.IsHealing ? "Healing" : "Damage")}";

            this.DescriptionTextBlock.Text = usedItem.Description;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void RollAttackButton_Click(object sender, RoutedEventArgs e)
        {
            this.usedItem.Attack.ReRoll();
            this.FillFields(this.usedItem);
        }

        private void RollDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.usedItem.Damage.ReRoll();
            this.FillFields(this.usedItem);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
