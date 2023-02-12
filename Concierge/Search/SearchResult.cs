// <copyright file="SearchResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Search
{
    using System.Text.RegularExpressions;

    using Concierge.Display;
    using Concierge.Display.Pages;

    public sealed class SearchResult
    {
        public SearchResult()
            : this(string.Empty, new object(), new Regex(string.Empty), new OverviewPage())
        {
        }

        public SearchResult(string searchText, object item, Regex searchRegex, IConciergePage conciergePage)
        {
            this.SearchText = searchText;
            this.Item = item;
            this.ConciergePage = conciergePage;
            this.SearchRegex = searchRegex;
        }

        public object Item { get; set; }

        public IConciergePage ConciergePage { get; set; }

        public string SearchText { get; set; }

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
