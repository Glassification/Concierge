// <copyright file="AppearanceControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;

    /// <summary>
    /// Interaction logic for AppearanceControl.xaml.
    /// </summary>
    public partial class AppearanceControl : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AppearanceControl));

        public AppearanceControl()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public void SetAppearance(Appearance appearance)
        {
            this.GenderField.Text = appearance.Gender;
            this.AgeField.Text = appearance.Age.ToString();
            this.HeightField.Text = appearance.Height.ToString();
            this.WeightField.Text = appearance.Weight.ToString();
            this.HairColourField.Text = appearance.HairColour.Name;
            this.SkinColourField.Text = appearance.SkinColour.Name;
            this.EyeColourField.Text = appearance.EyeColour.Name;
            this.MarksField.Text = appearance.DistinguishingMarks;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
