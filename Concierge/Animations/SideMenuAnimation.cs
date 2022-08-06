// <copyright file="SideMenuAnimation.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Animations
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    public class SideMenuAnimation
    {
        public SideMenuAnimation()
        {
            this.CloseMenu = new Storyboard();
            this.OpenMenu = new Storyboard();
        }

        public Storyboard CloseMenu { get; private set; }

        public Storyboard OpenMenu { get; private set; }
    }
}
