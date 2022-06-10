// <copyright file="HitDiceDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for HitDiceDisplay.xaml.
    /// </summary>
    public partial class HitDiceDisplay : UserControl
    {
        public static readonly DependencyProperty ConciergePageProperty =
            DependencyProperty.Register(
                "ConciergePage",
                typeof(ConciergePage),
                typeof(HitDiceDisplay),
                new UIPropertyMetadata(ConciergePage.Overview));

        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HitDiceDisplay));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ValueChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HitDiceDisplay));

        public HitDiceDisplay()
        {
            this.InitializeComponent();
            this.CurrentHitDiceBox = string.Empty;
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }

        public ConciergePage ConciergePage
        {
            get { return (ConciergePage)this.GetValue(ConciergePageProperty); }
            set { this.SetValue(ConciergePageProperty, value); }
        }

        private string CurrentHitDiceBox { get; set; }

        public void DrawSpentHitDice(HitDice hitDice)
        {
            this.DrawSpentHitDiceHelper(this.D6SpentField, this.D6SpentBox, this.D6Border, hitDice.SpentD6, hitDice.TotalD6);
            this.DrawSpentHitDiceHelper(this.D8SpentField, this.D8SpentBox, this.D8Border, hitDice.SpentD8, hitDice.TotalD8);
            this.DrawSpentHitDiceHelper(this.D10SpentField, this.D10SpentBox, this.D10Border, hitDice.SpentD10, hitDice.TotalD10);
            this.DrawSpentHitDiceHelper(this.D12SpentField, this.D12SpentBox, this.D12Border, hitDice.SpentD12, hitDice.TotalD12);
        }

        public void DrawTotalHitDice(HitDice hitDice)
        {
            this.DrawTotalHitDiceHelper(this.D6TotalField, this.D6TotalBox, hitDice.SpentD6, hitDice.TotalD6);
            this.DrawTotalHitDiceHelper(this.D8TotalField, this.D8TotalBox, hitDice.SpentD8, hitDice.TotalD8);
            this.DrawTotalHitDiceHelper(this.D10TotalField, this.D10TotalBox, hitDice.SpentD10, hitDice.TotalD10);
            this.DrawTotalHitDiceHelper(this.D12TotalField, this.D12TotalBox, hitDice.SpentD12, hitDice.TotalD12);
        }

        private void DrawSpentHitDiceHelper(TextBlock spentField, Grid spentBox, Border border, int spent, int total)
        {
            spentField.Text = spent.ToString();
            spentField.Foreground = DisplayUtility.SetUsedTextStyle(total, spent);
            spentBox.Background = DisplayUtility.SetUsedBoxStyle(total, spent);
            DisplayUtility.SetBorderColour(spent, total, spentBox, border, this.CurrentHitDiceBox);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Consistency")]
        private void DrawTotalHitDiceHelper(TextBlock totalField, Grid totalBox, int spent, int total)
        {
            totalField.Text = total.ToString();
            totalField.Foreground = DisplayUtility.SetTotalTextStyle(total, spent);
            totalBox.Background = DisplayUtility.SetTotalBoxStyle(total, spent);
        }

        private HitDice GetHitDice()
        {
            return this.ConciergePage switch
            {
                ConciergePage.Companion => Program.CcsFile.Character.Companion.Vitality.HitDice,
                _ => Program.CcsFile.Character.Vitality.HitDice,
            };
        }

        private void SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2 || sender is not Grid usedBox)
            {
                return;
            }

            var hitDice = this.GetHitDice();
            var oldItem = hitDice.DeepCopy();
            switch (usedBox.Name)
            {
                case "D6SpentBox":
                    hitDice.SpentD6 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD6, hitDice.TotalD6);
                    DisplayUtility.SetCursor(hitDice.SpentD6, hitDice.TotalD6, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D8SpentBox":
                    hitDice.SpentD8 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD8, hitDice.TotalD8);
                    DisplayUtility.SetCursor(hitDice.SpentD8, hitDice.TotalD8, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D10SpentBox":
                    hitDice.SpentD10 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD10, hitDice.TotalD10);
                    DisplayUtility.SetCursor(hitDice.SpentD10, hitDice.TotalD10, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D12SpentBox":
                    hitDice.SpentD12 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD12, hitDice.TotalD12);
                    DisplayUtility.SetCursor(hitDice.SpentD12, hitDice.TotalD12, (x, y) => x == y, Cursors.Arrow);
                    break;
            }

            Program.UndoRedoService.AddCommand(new EditCommand<HitDice>(hitDice, oldItem, this.ConciergePage));
            Program.Modify();

            this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            var hitDice = this.GetHitDice();
            this.CurrentHitDiceBox = grid.Name;
            switch (grid.Name)
            {
                case "D6SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD6, hitDice.TotalD6, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD6, hitDice.TotalD6, grid, this.D6Border, this.CurrentHitDiceBox);
                    break;
                case "D8SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD8, hitDice.TotalD8, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD8, hitDice.TotalD8, grid, this.D8Border, this.CurrentHitDiceBox);
                    break;
                case "D10SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD10, hitDice.TotalD10, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD10, hitDice.TotalD10, grid, this.D10Border, this.CurrentHitDiceBox);
                    break;
                case "D12SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD12, hitDice.TotalD12, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD12, hitDice.TotalD12, grid, this.D12Border, this.CurrentHitDiceBox);
                    break;
            }
        }

        private void SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            switch (grid.Name)
            {
                case "D6SpentBox":
                    this.D6Border.BorderBrush = grid.Background;
                    break;
                case "D8SpentBox":
                    this.D8Border.BorderBrush = grid.Background;
                    break;
                case "D10SpentBox":
                    this.D10Border.BorderBrush = grid.Background;
                    break;
                case "D12SpentBox":
                    this.D12Border.BorderBrush = grid.Background;
                    break;
            }

            this.CurrentHitDiceBox = string.Empty;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EditHitDiceButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
