// <copyright file="AttributeRollWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Windows;

    using Concierge.Display.Components;
    using Concierge.Tools.Enums;
    using Concierge.Tools.Generators.Attributes;

    /// <summary>
    /// Interaction logic for AttributeRollWindow.xaml.
    /// </summary>
    public partial class AttributeRollWindow : ConciergeWindow
    {
        private readonly AttributeGenerator attributeGenerator = new ();

        public AttributeRollWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Roll Attributes";

        public override string WindowName => nameof(AttributeRollWindow);

        public override ConciergeWindow? ShowNonBlockingWindow()
        {
            this.SetValues([0, 0, 0, 0, 0, 0]);
            this.ShowNonBlockingConciergeWindow();

            return this;
        }

        private void SetValues(int[] rolls)
        {
            this.Roll1Label.Text = rolls[0].ToString();
            this.Roll2Label.Text = rolls[1].ToString();
            this.Roll3Label.Text = rolls[2].ToString();
            this.Roll4Label.Text = rolls[3].ToString();
            this.Roll5Label.Text = rolls[4].ToString();
            this.Roll6Label.Text = rolls[5].ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void RollDiceButton_Click(object sender, RoutedEventArgs e)
        {
            var result = this.attributeGenerator.Generate(new AttributeSettings(AbilityScores.Roll));
            if (result is AttributeResult attributeResult)
            {
                this.SetValues(attributeResult.Rolls);
            }
        }

        private void StandardArrayButton_Click(object sender, RoutedEventArgs e)
        {
            var result = this.attributeGenerator.Generate(new AttributeSettings(AbilityScores.StandardArray));
            if (result is AttributeResult attributeResult)
            {
                this.SetValues(attributeResult.Rolls);
            }
        }
    }
}
