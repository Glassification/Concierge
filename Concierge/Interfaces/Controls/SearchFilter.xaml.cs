// <copyright file="SearchFilter.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Interfaces.Components;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for SearchFilter.xaml.
    /// </summary>
    public partial class SearchFilter : UserControl
    {
        private static readonly RoutedEvent FilterChangedEvent =
            EventManager.RegisterRoutedEvent(
                "FilterChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SearchFilter));

        public SearchFilter()
        {
            this.InitializeComponent();
            this.FilterText = string.Empty;
            this.IsFiltering = false;
        }

        public event RoutedEventHandler FilterChanged
        {
            add { this.AddHandler(FilterChangedEvent, value); }
            remove { this.RemoveHandler(FilterChangedEvent, value); }
        }

        public string FilterText
        {
            get
            {
                return this.FilterTextBox.Text;
            }

            set
            {
                this.FilterTextBox.Text = value;
            }
        }

        public bool IsFiltering { get; set; }

        public void SetButtonEnableState(ConciergeDesignButton button)
        {
            var opacity = this.IsFiltering ? 0.5 : 1;

            button.Opacity = opacity;
            button.IsEnabled = !this.IsFiltering;
        }

        private void ClearTextButton_Click(object sender, RoutedEventArgs e)
        {
            this.FilterText = string.Empty;
            this.IsFiltering = false;
            this.RaiseEvent(new RoutedEventArgs(FilterChangedEvent));
        }

        private void FilterTextBox_KeyPress(object sender, KeyEventArgs e)
        {
            this.IsFiltering = !this.FilterText.IsNullOrWhiteSpace();
            this.RaiseEvent(new RoutedEventArgs(FilterChangedEvent));
        }

        private void FilterTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Program.Typing();
        }

        private void FilterTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Program.NotTyping();
        }

        private void FilterTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void FilterTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
