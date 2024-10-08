﻿// <copyright file="WatermarkAdorner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Adorner for the watermark.
    /// </summary>
    public sealed class WatermarkAdorner : Adorner
    {
        /// <summary>
        /// <see cref="ContentPresenter"/> that holds the watermark.
        /// </summary>
        private readonly ContentPresenter contentPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement"><see cref="UIElement"/> to be adorned.</param>
        /// <param name="watermark">The watermark</param>
        public WatermarkAdorner(UIElement adornedElement, object watermark)
            : base(adornedElement)
        {
            this.IsHitTestVisible = false;
            this.contentPresenter = new ContentPresenter
            {
                Content = watermark,
                Opacity = 0.5,
                Margin = new Thickness(
                    this.Control.Margin.Left + this.Control.Padding.Left,
                    this.Control.Margin.Top + this.Control.Padding.Top,
                    0,
                    0),
            };

            if (this.Control is ItemsControl && this.Control is not ComboBox)
            {
                this.contentPresenter.VerticalAlignment = VerticalAlignment.Center;
                this.contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            }

            var binding = new Binding("IsVisible")
            {
                Source = adornedElement,
                Converter = new BooleanToVisibilityConverter(),
            };
            this.SetBinding(VisibilityProperty, binding);
        }

        /// <summary>
        /// Gets the number of children for the <see cref="ContainerVisual"/>.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the control that is being adorned.
        /// </summary>
        private Control Control
        {
            get { return (Control)this.AdornedElement; }
        }

        /// <summary>
        /// Returns a specified child <see cref="Visual"/> for the parent <see cref="ContainerVisual"/>.
        /// </summary>
        /// <param name="index">A 32-bit signed integer that represents the index value of the child <see cref="Visual"/>. The value of index must be between 0 and <see cref="VisualChildrenCount"/> - 1.</param>
        /// <returns>The child <see cref="Visual"/>.</returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.contentPresenter;
        }

        /// <summary>
        /// Implements any custom measuring behavior for the adorner.
        /// </summary>
        /// <param name="constraint">A size to constrain the adorner to.</param>
        /// <returns>A <see cref="Size"/> object representing the amount of layout space needed by the adorner.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            // Here's the secret to getting the adorner to cover the whole control
            this.contentPresenter.Measure(this.Control.RenderSize);
            return this.Control.RenderSize;
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}