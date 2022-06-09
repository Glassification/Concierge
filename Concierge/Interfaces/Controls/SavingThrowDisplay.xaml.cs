// <copyright file="SavingThrowDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Concierge.Character.AbilitySavingThrows;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for SavingThrowDisplay.xaml.
    /// </summary>
    public partial class SavingThrowDisplay : UserControl
    {
        public static readonly DependencyProperty SavingThrowNameProperty =
            DependencyProperty.Register(
                "SavingThrowName",
                typeof(string),
                typeof(AttributeDisplay),
                new UIPropertyMetadata("Saving Throw Name"));

        public static readonly DependencyProperty SavingThrowBonusProperty =
            DependencyProperty.Register(
                "SavingThrowBonus",
                typeof(string),
                typeof(AttributeDisplay),
                new UIPropertyMetadata("0"));

        public static readonly RoutedEvent ToggleClickedEvent =
            EventManager.RegisterRoutedEvent(
                "ToggleClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SavingThrowDisplay));

        public SavingThrowDisplay()
        {
            this.InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public event RoutedEventHandler ToggleClicked
        {
            add { this.AddHandler(ToggleClickedEvent, value); }
            remove { this.RemoveHandler(ToggleClickedEvent, value); }
        }

        public string SavingThrowName
        {
            get { return (string)this.GetValue(SavingThrowNameProperty); }
            set { this.SetValue(SavingThrowNameProperty, value); }
        }

        public string SavingThrowBonus
        {
            get
            {
                return (string)this.GetValue(SavingThrowBonusProperty);
            }

            set
            {
                this.SetValue(SavingThrowBonusProperty, value);
                this.SavingThrowBonusField.Text = value;
            }
        }

        public void SetStyle(bool proficiencyFlag, StatusChecks check)
        {
            this.ProficiencyBox.Fill = proficiencyFlag ? ConciergeColors.ProficiencyBrush : Brushes.Transparent;

            SetTextStyleHelper(check, this.SavingThrowNameField);
            SetTextStyleHelper(check, this.SavingThrowBonusField);
        }

        private static void SetTextStyleHelper(StatusChecks check, TextBlock textBlock)
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

        private void ToggleBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Rectangle rectangle)
            {
                return;
            }

            rectangle.Stroke = ConciergeColors.RectangleBorderHighlight;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ToggleBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Rectangle rectangle)
            {
                return;
            }

            rectangle.Stroke = ConciergeColors.RectangleBorder;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SavingThrowProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var savingThrow = Program.CcsFile.Character.SavingThrow;
            var savingThrowCopy = savingThrow.DeepCopy();

            switch (this.SavingThrowName)
            {
                case "Strength":
                    savingThrow.Strength.Proficiency = !savingThrow.Strength.Proficiency;
                    break;
                case "Dexterity":
                    savingThrow.Dexterity.Proficiency = !savingThrow.Dexterity.Proficiency;
                    break;
                case "Constitution":
                    savingThrow.Constitution.Proficiency = !savingThrow.Constitution.Proficiency;
                    break;
                case "Intelligence":
                    savingThrow.Intelligence.Proficiency = !savingThrow.Intelligence.Proficiency;
                    break;
                case "Wisdom":
                    savingThrow.Wisdom.Proficiency = !savingThrow.Wisdom.Proficiency;
                    break;
                case "Charisma":
                    savingThrow.Charisma.Proficiency = !savingThrow.Charisma.Proficiency;
                    break;
            }

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<SavingThrow>(savingThrow, savingThrowCopy, ConciergePage.Overview));
            Program.Modify();
        }
    }
}
