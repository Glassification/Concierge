// <copyright file="ConciergeRadioButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.Components
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Utility;

    public class ConciergeRadioButton : RadioButton
    {
        public ConciergeRadioButton()
            : base()
        {
            this.Click += this.SoundEffect_Click;
        }

        public bool IsUpdating { get; private set; }

        public void UpdatingValue()
        {
            this.IsUpdating = true;
        }

        public void UpdatedValue()
        {
            this.IsUpdating = false;
        }

        private void SoundEffect_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsUpdating)
            {
                ConciergeSound.UpdateValue();
            }

            this.IsUpdating = false;
        }
    }
}
