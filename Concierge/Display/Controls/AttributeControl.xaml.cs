﻿// <copyright file="AttributeControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Aspects;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for AttributeControl.xaml.
    /// </summary>
    public partial class AttributeControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(AttributeControl),
                new UIPropertyMetadata("Attribute"));

        public static readonly DependencyProperty BonusProperty =
            DependencyProperty.Register(
                "Bonus",
                typeof(int),
                typeof(AttributeControl),
                new UIPropertyMetadata(1));

        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register(
                "Score",
                typeof(int),
                typeof(AttributeControl),
                new UIPropertyMetadata(1));

        public static readonly DependencyProperty EnableEditProperty =
            DependencyProperty.Register(
                "EnableEdit",
                typeof(Visibility),
                typeof(AttributeControl),
                new UIPropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty DisplaySizeProperty =
            DependencyProperty.Register(
                "DisplaySize",
                typeof(DisplaySize),
                typeof(AttributeControl),
                new UIPropertyMetadata(DisplaySize.Small));

        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AttributeControl));

        public AttributeControl()
        {
            this.InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public int Bonus
        {
            get
            {
                return (int)this.GetValue(BonusProperty);
            }

            set
            {
                this.SetValue(BonusProperty, value);
                this.AttributeBonusField.Text = value.ToString();
            }
        }

        public int Score
        {
            get
            {
                return (int)this.GetValue(ScoreProperty);
            }

            set
            {
                this.SetValue(ScoreProperty, value);
                this.AttributeScoreField.Text = value.ToString();
            }
        }

        public Visibility EnableEdit
        {
            get { return (Visibility)this.GetValue(EnableEditProperty); }
            set { this.SetValue(EnableEditProperty, value); }
        }

        public DisplaySize DisplaySize
        {
            get { return (DisplaySize)this.GetValue(DisplaySizeProperty); }
            set { this.SetValue(DisplaySizeProperty, value); }
        }

        public void Draw(Attribute attribute)
        {
            this.Bonus = attribute.Bonus;
            this.Score = attribute.Score;
        }

        public void InitializeFontSize()
        {
            switch (this.DisplaySize)
            {
                case DisplaySize.Small:
                    this.AttributeTitle.FontSize = 17;
                    this.AttributeBonusText.FontSize = 15;
                    this.AttributeBonusField.FontSize = 25;
                    this.AttributeScoreText.FontSize = 15;
                    this.AttributeScoreField.FontSize = 15;
                    break;
                case DisplaySize.Medium:
                    this.AttributeTitle.FontSize = 20;
                    this.AttributeBonusText.FontSize = 15;
                    this.AttributeBonusField.FontSize = 30;
                    this.AttributeScoreText.FontSize = 15;
                    this.AttributeScoreField.FontSize = 18;
                    break;
                case DisplaySize.Large:
                    Program.Logger.Warning("Large attribute font is not implemented.");
                    break;
            }
        }

        private void EditAttributesButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "We need for bindings.")]
        private void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
