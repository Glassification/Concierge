// <copyright file="PersonalityControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;

    /// <summary>
    /// Interaction logic for PersonalityControl.xaml.
    /// </summary>
    public partial class PersonalityControl : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(PersonalityControl));

        public PersonalityControl()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public void SetPersonality(Personality personality)
        {
            this.TraitField1.Text = personality.Trait1;
            this.TraitField2.Text = personality.Trait2;
            this.IdealField.Text = personality.Ideal;
            this.BondField.Text = personality.Bond;
            this.FlawField.Text = personality.Flaw;
            this.NotesField.Text = personality.Notes;
        }

        private void EditPersonalityButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
