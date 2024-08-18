// <copyright file="SearchWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Journals;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Searches;
    using Concierge.Searches.Enums;

    /// <summary>
    /// Interaction logic for SearchWindow.xaml.
    /// </summary>
    public partial class SearchWindow : ConciergeWindow
    {
        private readonly Search search;
        private readonly Navigate navigate;
        private readonly MainWindow mainWindow;

        private int searchIndex;
        private SearchResult? previousResult;
        private List<SearchResult> searchResults = [];

        public SearchWindow()
        {
            if (Program.MainWindow is null)
            {
                throw new NullValueException(nameof(Program.MainWindow));
            }

            this.InitializeComponent();
            this.UseRoundedCorners();

            this.search = new Search(Program.MainWindow);
            this.navigate = new Navigate();
            this.mainWindow = Program.MainWindow;
            this.SearchDomainComboBox.ItemsSource = ComboBoxGenerator.SearchDomainComboBox();
            this.SearchDomainComboBox.Text = SearchDomain.CurrentPage.PascalCase();
            this.SearchResultTextBlock.Text = string.Empty;
        }

        public override string HeaderText => "Search";

        public override string WindowName => nameof(SearchWindow);

        private SearchResult? CurrentResult => this.searchResults.Count > 0 ? this.searchResults[this.searchIndex] : null;

        public override object? ShowWindow()
        {
            this.SearchTextBox.Focus();
            this.ShowConciergeWindow();

            return null;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.ClearHighlightedResults();
            this.ClearFields();
            base.OnClosing(e);
        }

        protected override void ReturnAndClose()
        {
            // Do nothing
        }

        private void ClearFields()
        {
            this.Opacity = 1;
            this.SearchResultTextBlock.Text = string.Empty;
            this.MatchCaseCheckBox.IsChecked = false;
            this.MatchWholeWordCheckBox.IsChecked = false;
            this.UseRegexCheckBox.IsChecked = false;
            this.SearchDomainComboBox.Text = SearchDomain.CurrentPage.PascalCase();
            this.SearchTextBox.Text = string.Empty;
        }

        private SearchSettings ToSettings()
        {
            return new SearchSettings()
            {
                MatchCase = this.MatchCaseCheckBox.IsChecked ?? false,
                MatchWholeWord = this.MatchWholeWordCheckBox.IsChecked ?? false,
                UseRegex = this.UseRegexCheckBox.IsChecked ?? false,
                SearchDomain = this.SearchDomainComboBox.Text.ToEnum<SearchDomain>(),
                TextToSearch = this.SearchTextBox.Text,
            };
        }

        private void Search()
        {
            var settings = this.ToSettings();
            if (this.search.SearchSettings.Equals(settings))
            {
                return;
            }

            this.searchResults = this.search.Start(settings);
            this.searchIndex = 0;
            this.previousResult = null;
        }

        private void SelectSearchResult()
        {
            if (!this.ValidateRegex())
            {
                return;
            }

            this.Search();
            if (this.searchResults.IsEmpty())
            {
                return;
            }

            this.Opacity = 0.8;
            var result = this.searchResults[this.searchIndex];

            this.ClearHighlightedResults();
            this.mainWindow.MoveSelection(result.ConciergePage.ConciergePages);
            this.navigate.Go(result);
            this.Focus();
        }

        private void FormatResultText()
        {
            if (this.searchResults.IsEmpty())
            {
                this.SearchResultTextBlock.Text = "No results found!";
                this.SearchResultTextBlock.Foreground = Brushes.IndianRed;
            }
            else
            {
                this.SearchResultTextBlock.Text = $"Found {this.searchIndex + 1}/{this.searchResults.Count} results.";
                this.SearchResultTextBlock.Foreground = Brushes.PaleGreen;
            }
        }

        private void ClearHighlightedResults()
        {
            foreach (var result in this.searchResults)
            {
                if (result.Item is ConciergeTextBlock item)
                {
                    item.ResetHighlight();
                }
            }

            if (this.previousResult?.Item is Document || this.CurrentResult?.Item is Document)
            {
                Program.MainWindow?.JournalPage.ClearHighlightSelection();
            }
        }

        private bool ValidateRegex()
        {
            if ((this.UseRegexCheckBox.IsChecked ?? false) && !this.SearchTextBox.Text.IsValidRegex())
            {
                ConciergeMessageBox.ShowError($"The current Regex: {this.SearchTextBox.Text} is invalid.");
                return false;
            }

            return true;
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            this.ClearHighlightedResults();
            this.ClearFields();
            this.CloseConciergeWindow();
        }

        private void FindPreviousButton_Click(object? sender, RoutedEventArgs e)
        {
            this.previousResult = this.searchResults.Count > 0 ? this.searchResults[this.searchIndex] : null;
            this.searchIndex--;
            if (this.searchIndex < 0)
            {
                this.searchIndex = this.searchResults.Count - 1;
            }

            this.SelectSearchResult();
            this.FormatResultText();
        }

        private void FindNextButton_Click(object? sender, RoutedEventArgs e)
        {
            this.previousResult = this.searchResults.Count > 0 ? this.searchResults[this.searchIndex] : null;
            this.searchIndex++;
            if (this.searchIndex >= this.searchResults.Count)
            {
                this.searchIndex = 0;
            }

            this.SelectSearchResult();
            this.FormatResultText();
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            this.ClearHighlightedResults();
            this.ClearFields();
            this.CloseConciergeWindow();
        }

        private void Window_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.ClearHighlightedResults();
                    this.ClearFields();
                    break;
                case Key.Enter:
                    this.FindNextButton_Click(this.FindNextButton, new RoutedEventArgs());
                    break;
            }
        }

        private void Window_MouseMove(object? sender, MouseEventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity = 1;
            }
        }

        private void UseRegexCheckBox_Checked(object? sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.MatchWholeWordLabel, false);
            DisplayUtility.SetControlEnableState(this.MatchWholeWordCheckBox, false);

            this.MatchWholeWordCheckBox.IsChecked = false;
        }

        private void UseRegexCheckBox_Unchecked(object? sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.MatchWholeWordLabel, true);
            DisplayUtility.SetControlEnableState(this.MatchWholeWordCheckBox, true);
        }
    }
}
