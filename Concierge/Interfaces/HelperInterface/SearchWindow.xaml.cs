// <copyright file="SearchWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.HelperInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Tools.Searching;
    using Concierge.Tools.Searching.Enums;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for SearchWindow.xaml.
    /// </summary>
    public partial class SearchWindow : Window
    {
        private readonly ConciergeSearch conciergeSearch;
        private readonly MainWindow mainWindow;

        public SearchWindow(MainWindow mainWindow)
        {
            this.InitializeComponent();
            this.conciergeSearch = new ConciergeSearch(mainWindow);
            this.SearchResults = new List<SearchResult>();
            this.mainWindow = mainWindow;
            this.SearchDomainComboBox.ItemsSource = Enum.GetValues(typeof(SearchDomain)).Cast<SearchDomain>();
            this.SearchDomainComboBox.Text = SearchDomain.CurrentPage.ToString();
        }

        private List<SearchResult> SearchResults { get; set; }

        private int SearchIndex { get; set; }

        public void ShowWindow()
        {
            this.WindowState = WindowState.Normal;
            this.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private SearchSettings ToSettings()
        {
            return new SearchSettings()
            {
                MatchCase = this.MatchCaseCheckBox.IsChecked ?? false,
                MatchWholeWord = this.MatchWholeWordCheckBox.IsChecked ?? false,
                SearchDomain = (SearchDomain)Enum.Parse(typeof(SearchDomain), this.SearchDomainComboBox.Text),
                TextToSearch = this.SearchTextBox.Text,
            };
        }

        private void Search()
        {
            var settings = this.ToSettings();

            if (this.conciergeSearch.SearchSettings.Equals(settings) && !Program.Modified)
            {
                return;
            }

            this.SearchResults = this.conciergeSearch.Search(settings, Program.CcsFile.Character);
            this.SearchIndex = 0;
        }

        private void SelectSearchResult()
        {
            this.Search();

            if (this.SearchResults.IsEmpty())
            {
                return;
            }

            var result = this.SearchResults[this.SearchIndex];
            this.mainWindow.PageSelection(result.ConciergePage);
            this.conciergeSearch.Navigate(result);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void FindPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectSearchResult();

            this.SearchIndex--;
            if (this.SearchIndex < 0)
            {
                this.SearchIndex = this.SearchResults.Count - 1;
            }
        }

        private void FindNextButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectSearchResult();

            this.SearchIndex++;
            if (this.SearchIndex >= this.SearchResults.Count)
            {
                this.SearchIndex = 0;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }
    }
}
