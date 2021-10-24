// <copyright file="OutlinedTextBlock.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Windows.Media;

    [ContentProperty("Text")]
    public class OutlinedTextBlock : FrameworkElement
    {
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
          "Fill",
          typeof(Brush),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
          "Stroke",
          typeof(Brush),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, StrokePropertyChangedCallback));

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
          "StrokeThickness",
          typeof(double),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender, StrokePropertyChangedCallback));

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text",
          typeof(string),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextInvalidated));

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
          "TextAlignment",
          typeof(TextAlignment),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(
          "TextDecorations",
          typeof(TextDecorationCollection),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
          "TextTrimming",
          typeof(TextTrimming),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
          "TextWrapping",
          typeof(TextWrapping),
          typeof(OutlinedTextBlock),
          new FrameworkPropertyMetadata(TextWrapping.NoWrap, OnFormattedTextUpdated));

        public OutlinedTextBlock()
        {
            this.UpdatePen();
            this.TextDecorations = new TextDecorationCollection();
        }

        public Brush Fill
        {
            get => (Brush)this.GetValue(FillProperty);
            set => this.SetValue(FillProperty, value);
        }

        public FontFamily FontFamily
        {
            get => (FontFamily)this.GetValue(FontFamilyProperty);
            set => this.SetValue(FontFamilyProperty, value);
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)this.GetValue(FontSizeProperty);
            set => this.SetValue(FontSizeProperty, value);
        }

        public FontStretch FontStretch
        {
            get => (FontStretch)this.GetValue(FontStretchProperty);
            set => this.SetValue(FontStretchProperty, value);
        }

        public FontStyle FontStyle
        {
            get => (FontStyle)this.GetValue(FontStyleProperty);
            set => this.SetValue(FontStyleProperty, value);
        }

        public FontWeight FontWeight
        {
            get => (FontWeight)this.GetValue(FontWeightProperty);
            set => this.SetValue(FontWeightProperty, value);
        }

        public Brush Stroke
        {
            get => (Brush)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)this.GetValue(StrokeThicknessProperty);
            set => this.SetValue(StrokeThicknessProperty, value);
        }

        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)this.GetValue(TextAlignmentProperty);
            set => this.SetValue(TextAlignmentProperty, value);
        }

        public TextDecorationCollection TextDecorations
        {
            get => (TextDecorationCollection)this.GetValue(TextDecorationsProperty);
            set => this.SetValue(TextDecorationsProperty, value);
        }

        public TextTrimming TextTrimming
        {
            get => (TextTrimming)this.GetValue(TextTrimmingProperty);
            set => this.SetValue(TextTrimmingProperty, value);
        }

        public TextWrapping TextWrapping
        {
            get => (TextWrapping)this.GetValue(TextWrappingProperty);
            set => this.SetValue(TextWrappingProperty, value);
        }

        private FormattedText FormattedText { get; set; }

        private Geometry TextGeometry { get; set; }

        private Pen Pen { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            this.EnsureGeometry();

            drawingContext.DrawGeometry(null, this.Pen, this.TextGeometry);
            drawingContext.DrawGeometry(this.Fill, null, this.TextGeometry);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.EnsureFormattedText();

            // constrain the formatted text according to the available size
            double w = availableSize.Width;
            double h = availableSize.Height;

            // the Math.Min call is important - without this constraint (which seems arbitrary, but is the maximum allowable text width), things blow up when availableSize is infinite in both directions
            // the Math.Max call is to ensure we don't hit zero, which will cause MaxTextHeight to throw
            this.FormattedText.MaxTextWidth = Math.Min(3579139, w);
            this.FormattedText.MaxTextHeight = Math.Max(0.0001d, h);

            // return the desired size
            return new Size(Math.Ceiling(this.FormattedText.Width), Math.Ceiling(this.FormattedText.Height));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.EnsureFormattedText();

            // update the formatted text with the final size
            this.FormattedText.MaxTextWidth = finalSize.Width;
            this.FormattedText.MaxTextHeight = Math.Max(0.0001d, finalSize.Height);

            // need to re-generate the geometry now that the dimensions have changed
            this.TextGeometry = null;

            return finalSize;
        }

        private static void StrokePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as OutlinedTextBlock)?.UpdatePen();
        }

        private static void OnFormattedTextInvalidated(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
            outlinedTextBlock.FormattedText = null;
            outlinedTextBlock.TextGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private static void OnFormattedTextUpdated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
            outlinedTextBlock.UpdateFormattedText();
            outlinedTextBlock.TextGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private void UpdatePen()
        {
            this.Pen = new Pen(this.Stroke, this.StrokeThickness)
            {
                DashCap = PenLineCap.Round,
                EndLineCap = PenLineCap.Round,
                LineJoin = PenLineJoin.Round,
                StartLineCap = PenLineCap.Round,
            };

            this.InvalidateVisual();
        }

        private void EnsureFormattedText()
        {
            if (this.FormattedText != null)
            {
                return;
            }

            this.FormattedText = new FormattedText(
              this.Text ?? string.Empty,
              CultureInfo.CurrentUICulture,
              this.FlowDirection,
              new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
              this.FontSize,
              Brushes.Black);

            this.UpdateFormattedText();
        }

        private void UpdateFormattedText()
        {
            if (this.FormattedText == null)
            {
                return;
            }

            this.FormattedText.MaxLineCount = this.TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue;
            this.FormattedText.TextAlignment = this.TextAlignment;
            this.FormattedText.Trimming = this.TextTrimming;

            this.FormattedText.SetFontSize(this.FontSize);
            this.FormattedText.SetFontStyle(this.FontStyle);
            this.FormattedText.SetFontWeight(this.FontWeight);
            this.FormattedText.SetFontFamily(this.FontFamily);
            this.FormattedText.SetFontStretch(this.FontStretch);
            this.FormattedText.SetTextDecorations(this.TextDecorations);
        }

        private void EnsureGeometry()
        {
            if (this.TextGeometry != null)
            {
                return;
            }

            this.EnsureFormattedText();
            this.TextGeometry = this.FormattedText.BuildGeometry(new Point(0, 0));
        }
    }
}
