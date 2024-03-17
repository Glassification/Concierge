// <copyright file="UseItemWindow.xaml.cs" company="Thomas Beckett">
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
        public UseItemWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.UsedItem = UsedItem.Empty;
        }

        public override string HeaderText => "Use Item";

        public override string WindowName => nameof(UseItemWindow);

        private UsedItem UsedItem { get; set; }

        public override ConciergeResult ShowUseItemWindow(UsedItem usedItem)
        {
            this.UsedItem = usedItem;
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
            this.UsedItem.Attack.ReRoll();
            this.FillFields(this.UsedItem);
        }

        private void RollDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.UsedItem.Damage.ReRoll();
            this.FillFields(this.UsedItem);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
