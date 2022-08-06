// <copyright file="WindowAnimation.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Animations
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    public class WindowAnimation
    {
        public const double DefaultAnimationSpeed = 0.15;

        public WindowAnimation(double animationSpeed, EventHandler completedEvent)
        {
            animationSpeed = ValidateAnimationSpeed(animationSpeed);

            this.Open = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(animationSpeed)),
                FillBehavior = FillBehavior.Stop,
            };
            this.Close = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(animationSpeed)),
                FillBehavior = FillBehavior.Stop,
            };

            this.Close.Completed += completedEvent;

            this.Open.Freeze();
            this.Close.Freeze();
        }

        public DoubleAnimation Close { get; private set; }

        public DoubleAnimation Open { get; private set; }

        private static double ValidateAnimationSpeed(double speed)
        {
            return speed > 0 ? speed : DefaultAnimationSpeed;
        }
    }
}
