// <copyright file="DiceRollControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Tools.DiceRoller;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for DiceRollControl.xaml.
    /// </summary>
    public partial class DiceRollControl : UserControl
    {
        public static readonly DependencyProperty DiceNameProperty =
            DependencyProperty.Register(
                "DiceName",
                typeof(string),
                typeof(DiceRollControl),
                new UIPropertyMetadata("d69"));

        public static readonly DependencyProperty CustomSidesVisibilityProperty =
            DependencyProperty.Register(
                "CustomSidesVisibility",
                typeof(Visibility),
                typeof(DiceRollControl),
                new UIPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty DiceSidesProperty =
            DependencyProperty.Register(
                "DiceSides",
                typeof(int),
                typeof(DiceRollControl),
                new UIPropertyMetadata(69));

        public static readonly DependencyProperty DiceToolTipProperty =
            DependencyProperty.Register(
                "DiceToolTip",
                typeof(string),
                typeof(DiceRollControl),
                new UIPropertyMetadata("Roll in ze hay"));

        public static readonly DependencyProperty DiceSymbolProperty =
            DependencyProperty.Register(
                "DiceSymbol",
                typeof(PackIconKind),
                typeof(DiceRollControl),
                new UIPropertyMetadata(PackIconKind.ErrorOutline));

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register(
                "FillBrush",
                typeof(Brush),
                typeof(LabelControl),
                new UIPropertyMetadata(Brushes.Transparent));

        public static readonly RoutedEvent DiceRolledEvent =
            EventManager.RegisterRoutedEvent(
                "DiceRolled",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DiceRollControl));

        public DiceRollControl()
        {
            this.InitializeComponent();
            this.Margin = new Thickness(3, 0, 3, 0);
        }

        public event RoutedEventHandler DiceRolled
        {
            add { this.AddHandler(DiceRolledEvent, value); }
            remove { this.RemoveHandler(DiceRolledEvent, value); }
        }

        public string DiceName
        {
            get { return (string)this.GetValue(DiceNameProperty); }
            set { this.SetValue(DiceNameProperty, value); }
        }

        public string DiceToolTip
        {
            get { return (string)this.GetValue(DiceToolTipProperty); }
            set { this.SetValue(DiceToolTipProperty, value); }
        }

        public int DiceSides
        {
            get { return (int)this.GetValue(DiceSidesProperty); }
            set { this.SetValue(DiceSidesProperty, value); }
        }

        public Visibility CustomSidesVisibility
        {
            get { return (Visibility)this.GetValue(CustomSidesVisibilityProperty); }
            set { this.SetValue(CustomSidesVisibilityProperty, value); }
        }

        public PackIconKind DiceSymbol
        {
            get { return (PackIconKind)this.GetValue(DiceSymbolProperty); }
            set { this.SetValue(DiceSymbolProperty, value); }
        }

        public Brush FillBrush
        {
            get
            {
                return (Brush)this.GetValue(FillBrushProperty);
            }

            set
            {
                this.SetValue(FillBrushProperty, value);
                this.DiceRollGrid.Background = value;
                this.DiceRollBorder.BorderBrush = value;
            }
        }

        public void ResetDiceValue()
        {
            this.DiceNumberUpDown.Value = 1;
            this.DxDieUpDown.Value = 1;
            this.DiceModifierUpDown.Value = 0;
            this.DiceResult.Text = "0";
        }

        public DiceRoll RollDice()
        {
            var diceNumber = this.DiceNumberUpDown.Value;
            var modified = this.DiceModifierUpDown.Value;
            var diceSides = this.CustomSidesVisibility == Visibility.Visible ? this.DxDieUpDown.Value : this.DiceSides;

            var rolledDice = DiceRoll.RollDice(diceNumber, diceSides);
            return new DiceRoll(diceSides, rolledDice, modified);
        }

        private void ButtonRoll_Click(object sender, RoutedEventArgs e)
        {
            var roll = this.RollDice();
            this.DiceResult.Text = roll.Total.ToString();
            try
            {
                Clipboard.SetText(this.DiceResult.Text);
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
            }

            this.RaiseEvent(new RoutedEventArgs(DiceRolledEvent, roll));
        }
    }
}
