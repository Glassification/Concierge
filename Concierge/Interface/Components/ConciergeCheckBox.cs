// <copyright file="ConciergeCheckBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.Components
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Utility;

    public class ConciergeCheckBox : CheckBox
    {
        public ConciergeCheckBox()
            : base()
        {
            this.Checked += this.SoundEffect_Checked;
            this.Unchecked += this.SoundEffect_Unchecked;
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

        private void SoundEffect_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsUpdating)
            {
                ConciergeSound.UpdateValue();
            }

            this.IsUpdating = false;
        }

        private void SoundEffect_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!this.IsUpdating)
            {
                ConciergeSound.UpdateValue();
            }

            this.IsUpdating = false;
        }
    }
}
