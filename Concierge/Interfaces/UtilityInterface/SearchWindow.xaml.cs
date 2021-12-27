// <copyright file="SearchWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Search;
    using Concierge.Search.Enums;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for SearchWindow.xaml.
    /// </summary>
    public partial class SearchWindow : ConciergeWindow
    {
        private readonly ConciergeSearch conciergeSearch;
        private readonly ConciergeNavigate conciergeNavigate;
        private readonly MainWindow mainWindow;

        public SearchWindow(MainWindow mainWindow)
        {
            this.InitializeComponent();
            this.conciergeSearch = new ConciergeSearch(mainWindow);
            this.conciergeNavigate = new ConciergeNavigate();
            this.SearchResults = new List<SearchResult>();
            this.mainWindow = mainWindow;
            this.SearchDomainComboBox.ItemsSource = Utilities.FormatEnumForDisplay(typeof(SearchDomain));
            this.SearchDomainComboBox.Text = SearchDomain.CurrentPage.ToString().FormatFromEnum();
            this.SearchResultTextBlock.Text = string.Empty;
        }

        private List<SearchResult> SearchResults { get; set; }

        private int SearchIndex { get; set; }

        public void ShowWindow()
        {
            this.SearchTextBox.Focus();
            this.ShowConciergeWindow();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.ClearHighlightedResults();
            this.ClearFields();
            base.OnClosing(e);
        }

        private void ClearFields()
        {
            this.Opacity = 1;
            this.SearchResultTextBlock.Text = string.Empty;
            this.MatchCaseCheckBox.IsChecked = false;
            this.MatchWholeWordCheckBox.IsChecked = false;
            this.UseRegexCheckBox.IsChecked = false;
            this.SearchDomainComboBox.Text = SearchDomain.CurrentPage.ToString().FormatFromEnum();
            this.SearchTextBox.Text = string.Empty;
        }

        private SearchSettings ToSettings()
        {
            return new SearchSettings()
            {
                MatchCase = this.MatchCaseCheckBox.IsChecked ?? false,
                MatchWholeWord = this.MatchWholeWordCheckBox.IsChecked ?? false,
                UseRegex = this.UseRegexCheckBox.IsChecked ?? false,
                SearchDomain = (SearchDomain)Enum.Parse(typeof(SearchDomain), this.SearchDomainComboBox.Text.Strip(" ")),
                TextToSearch = this.SearchTextBox.Text,
            };
        }

        private void Search()
        {
            var settings = this.ToSettings();

            if (this.conciergeSearch.SearchSettings.Equals(settings) && !Program.IsModified)
            {
                return;
            }

            this.SearchResults = this.conciergeSearch.Search(settings);
            this.SearchIndex = 0;
        }

        private void SelectSearchResult()
        {
            if (!this.ValidateRegex())
            {
                return;
            }

            this.Search();

            if (this.SearchResults.IsEmpty())
            {
                return;
            }

            this.Opacity = 0.8;

            var result = this.SearchResults[this.SearchIndex];
            this.ClearHighlightedResults();
            this.mainWindow.MoveSelection(result.ConciergePage.ConciergePage);
            this.conciergeNavigate.Navigate(result);
            this.Focus();
        }

        private void FormatResultText()
        {
            if (this.SearchResults.IsEmpty())
            {
                this.SearchResultTextBlock.Text = "No results found!";
                this.SearchResultTextBlock.Foreground = Brushes.IndianRed;
            }
            else
            {
                this.SearchResultTextBlock.Text = $"Found {this.SearchIndex + 1}/{this.SearchResults.Count} results.";
                this.SearchResultTextBlock.Foreground = Brushes.PaleGreen;
            }
        }

        private void ClearHighlightedResults()
        {
            foreach (var result in this.SearchResults)
            {
                if (result.Item is ConciergeTextBlock)
                {
                    (result.Item as ConciergeTextBlock).ResetHighlight();
                }
            }
        }

        private bool ValidateRegex()
        {
            if ((this.UseRegexCheckBox.IsChecked ?? false) && !this.SearchTextBox.Text.IsValidRegex())
            {
                ConciergeMessageBox.Show(
                    $"The current Regex: {this.SearchTextBox.Text} is invalid.",
                    "Error",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Error);

                return false;
            }

            return true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearHighlightedResults();
            this.ClearFields();
            this.HideConciergeWindow();
        }

        private void FindPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            this.SearchIndex--;
            if (this.SearchIndex < 0)
            {
                this.SearchIndex = this.SearchResults.Count - 1;
            }

            this.SelectSearchResult();
            this.FormatResultText();
        }

        private void FindNextButton_Click(object sender, RoutedEventArgs e)
        {
            this.SearchIndex++;
            if (this.SearchIndex >= this.SearchResults.Count)
            {
                this.SearchIndex = 0;
            }

            this.SelectSearchResult();
            this.FormatResultText();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearHighlightedResults();
            this.ClearFields();
            this.HideConciergeWindow();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.ClearHighlightedResults();
                    this.ClearFields();
                    break;
                case Key.Enter:
                    this.FindNextButton_Click(this.FindNextButton, null);
                    break;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity = 1;
            }
        }

        private void UseRegexCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.MatchWholeWordLabel.IsEnabled = false;
            this.MatchWholeWordCheckBox.IsEnabled = false;
            this.MatchWholeWordLabel.Opacity = 0.5;
            this.MatchWholeWordCheckBox.Opacity = 0.5;
            this.MatchWholeWordCheckBox.IsChecked = false;
        }

        private void UseRegexCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.MatchWholeWordLabel.IsEnabled = true;
            this.MatchWholeWordCheckBox.IsEnabled = true;
            this.MatchWholeWordLabel.Opacity = 1;
            this.MatchWholeWordCheckBox.Opacity = 1;
        }
    }
}
