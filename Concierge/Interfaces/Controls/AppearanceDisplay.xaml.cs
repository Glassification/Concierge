// <copyright file="AppearanceDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;

    /// <summary>
    /// Interaction logic for AppearanceDisplay.xaml.
    /// </summary>
    public partial class AppearanceDisplay : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AppearanceDisplay));

        public AppearanceDisplay()
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

        private void EditAppearanceButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
