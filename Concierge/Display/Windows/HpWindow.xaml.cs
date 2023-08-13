// <copyright file="HpWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for HpWindow.xaml.
    /// </summary>
    public partial class HpWindow : ConciergeWindow
    {
        public HpWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.PreviousHeal = 0;
            this.PreviousDamage = 0;
            this.ConciergePage = ConciergePage.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.HpUpDown);
        }

        public override string HeaderText => this.IsHealing ? "Healing" : "Damage";

        public override string WindowName => nameof(HpWindow);

        private int PreviousHeal { get; set; }

        private int PreviousDamage { get; set; }

        private bool IsHealing { get; set; }

        public override ConciergeWindowResult ShowHeal<T>(T vitality)
        {
            if (vitality is not Vitality castItem)
            {
                return ConciergeWindowResult.NoResult;
            }

            this.IsHealing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.HpUpDown.Value = this.PreviousHeal;

            this.ShowConciergeWindow();
            this.SetPreviousValue();

            if (this.Result == ConciergeWindowResult.OK)
            {
                var oldItem = castItem.Health.DeepCopy();
                castItem.Heal(this.HpUpDown.Value);
                Program.UndoRedoService.AddCommand(new EditCommand<Health>(castItem.Health, oldItem, this.ConciergePage));
            }

            return this.Result;
        }

        public override ConciergeWindowResult ShowDamage<T>(T vitality)
        {
            if (vitality is not Vitality castItem)
            {
                return ConciergeWindowResult.NoResult;
            }

            this.IsHealing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.HpUpDown.Value = this.PreviousDamage;

            this.ShowConciergeWindow();
            this.SetPreviousValue();

            if (this.Result == ConciergeWindowResult.OK)
            {
                var oldItem = castItem.Health.DeepCopy();
                castItem.Damage(this.HpUpDown.Value);
                Program.UndoRedoService.AddCommand(new EditCommand<Health>(castItem.Health, oldItem, this.ConciergePage));
            }

            return this.Result;
        }

        private void SetPreviousValue()
        {
            if (this.IsHealing)
            {
                this.PreviousHeal = this.HpUpDown.Value;
            }
            else
            {
                this.PreviousDamage = this.HpUpDown.Value;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }
    }
}
