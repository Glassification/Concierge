// <copyright file="SearchResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Searches
{
    using System.Text.RegularExpressions;

    using Concierge.Display.Components;
    using Concierge.Display.Pages;

    /// <summary>
    /// Represents a search result containing information about the matched item, search text, regular expression, and associated Concierge page.
    /// </summary>
    public sealed class SearchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class with default values.
        /// </summary>
        public SearchResult()
            : this(string.Empty, new object(), new Regex(string.Empty), new OverviewPage())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class with the specified search parameters.
        /// </summary>
        /// <param name="searchText">The text used for searching.</param>
        /// <param name="item">The matched item.</param>
        /// <param name="searchRegex">The regular expression used for searching.</param>
        /// <param name="conciergePage">The Concierge page associated with the search result.</param>
        public SearchResult(string searchText, object item, Regex searchRegex, ConciergePage conciergePage)
        {
            this.SearchText = searchText;
            this.Item = item;
            this.ConciergePage = conciergePage;
            this.SearchRegex = searchRegex;
        }

        /// <summary>
        /// Gets or sets the matched item.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the Concierge page associated with the search result.
        /// </summary>
        public ConciergePage ConciergePage { get; set; }

        /// <summary>
        /// Gets or sets the text used for searching.
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets the regular expression used for searching.
        /// </summary>
        public Regex SearchRegex { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SearchResult searchResult)
            {
                return false;
            }

            return this.Item.Equals(searchResult.Item) &&
                this.ConciergePage == searchResult.ConciergePage &&
                this.SearchText.Equals(searchResult.SearchText) &&
                this.SearchRegex.Equals(searchResult.SearchRegex);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
