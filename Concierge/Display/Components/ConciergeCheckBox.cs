// <copyright file="ConciergeCheckBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Services;

    public sealed class ConciergeCheckBox : CheckBox
    {
        public ConciergeCheckBox()
            : base()
        {
            this.Margin = new Thickness(0, 15, 0, 0);

            this.Checked += this.SoundEffect_Checked;
            this.Unchecked += this.SoundEffect_Unchecked;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
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

        public void UncheckUnlessNameMatches(string name)
        {
            if (!this.Name.Equals(name))
            {
                this.IsChecked = false;
            }
        }

        private void SoundEffect_Checked(object sender, RoutedEventArgs e)
        {
            if (!this.IsUpdating)
            {
                SoundService.PlayUpdateValue();
            }

            this.IsUpdating = false;
        }

        private void SoundEffect_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!this.IsUpdating)
            {
                SoundService.PlayUpdateValue();
            }

            this.IsUpdating = false;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
