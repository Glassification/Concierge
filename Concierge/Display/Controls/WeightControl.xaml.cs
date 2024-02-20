// <copyright file="WeightControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Data.Units;

    /// <summary>
    /// Interaction logic for WeightDisplay.xaml.
    /// </summary>
    public partial class WeightControl : UserControl
    {
        public WeightControl()
        {
            this.InitializeComponent();
        }

        public void SetWeightValues(CharacterSheet character, UnitTypes unitTypes)
        {
            this.WeightCarriedField.Text = UnitFormat.FormatWeight(unitTypes, character.CarryWeight);
            this.LightWeightField.Text = UnitFormat.FormatWeight(unitTypes, character.LightCapacity, true);
            this.MediumWeightField.Text = UnitFormat.FormatWeight(unitTypes, character.MediumCapacity, true);
            this.HeavyWeightField.Text = UnitFormat.FormatWeight(unitTypes, character.HeavyCapacity, true);
        }

        public void FormatCarryWeight(CharacterSheet character)
        {
            var weight = character.CarryWeight;

            if (weight <= character.LightCapacity)
            {
                this.WeightCarriedField.Foreground = Brushes.Black;
                this.WeightCarriedGrid.Background = ConciergeBrushes.LightCarryCapacity;
                this.WeightCarriedBorder.BorderBrush = ConciergeBrushes.LightCarryCapacity;
            }
            else if (weight > character.LightCapacity && weight <= character.MediumCapacity)
            {
                this.WeightCarriedField.Foreground = ConciergeBrushes.HeavyCarryCapacity;
                this.WeightCarriedGrid.Background = ConciergeBrushes.MediumCarryCapacity;
                this.WeightCarriedBorder.BorderBrush = ConciergeBrushes.MediumCarryCapacity;
            }
            else if (weight > character.MediumCapacity && weight <= character.HeavyCapacity)
            {
                this.WeightCarriedField.Foreground = Brushes.DarkRed;
                this.WeightCarriedGrid.Background = Brushes.Pink;
                this.WeightCarriedBorder.BorderBrush = Brushes.Pink;
            }
            else
            {
                this.WeightCarriedField.Foreground = Brushes.Crimson;
                this.WeightCarriedGrid.Background = Brushes.DimGray;
                this.WeightCarriedBorder.BorderBrush = Brushes.DimGray;
            }
        }
    }
}
