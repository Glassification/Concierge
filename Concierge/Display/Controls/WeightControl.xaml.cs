// <copyright file="WeightDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Utility;
    using Concierge.Utility.Units;
    using Concierge.Utility.Units.Enums;

    /// <summary>
    /// Interaction logic for WeightDisplay.xaml.
    /// </summary>
    public partial class WeightControl : UserControl
    {
        public WeightControl()
        {
            this.InitializeComponent();
        }

        public void SetWeightValues(ConciergeCharacter character, UnitTypes unitTypes)
        {
            this.WeightCarriedField.Text = UnitFormat.FormatWeight(unitTypes, character.CarryWeight);
            this.LightWeightField.Text = UnitFormat.FormatWeight(unitTypes, character.LightCarryCapacity, true);
            this.MediumWeightField.Text = UnitFormat.FormatWeight(unitTypes, character.MediumCarryCapacity, true);
            this.HeavyWeightField.Text = UnitFormat.FormatWeight(unitTypes, character.HeavyCarryCapacity, true);
        }

        public void FormatCarryWeight(ConciergeCharacter character)
        {
            var weight = character.CarryWeight;

            if (weight <= character.LightCarryCapacity)
            {
                this.WeightCarriedField.Foreground = Brushes.Black;
                this.WeightCarriedBox.Background = ConciergeColors.LightCarryCapacity;
            }
            else if (weight > character.LightCarryCapacity && weight <= character.MediumCarryCapacity)
            {
                this.WeightCarriedField.Foreground = ConciergeColors.HeavyCarryCapacity;
                this.WeightCarriedBox.Background = ConciergeColors.MediumCarryCapacity;
            }
            else if (weight > character.MediumCarryCapacity && weight <= character.HeavyCarryCapacity)
            {
                this.WeightCarriedField.Foreground = Brushes.DarkRed;
                this.WeightCarriedBox.Background = Brushes.Pink;
            }
            else
            {
                this.WeightCarriedField.Foreground = Brushes.Red;
                this.WeightCarriedBox.Background = Brushes.DimGray;
            }
        }
    }
}
