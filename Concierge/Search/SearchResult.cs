// <copyright file="SearchResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Search
{
    using System.Text.RegularExpressions;

    using Concierge.Interfaces;
    using Concierge.Interfaces.OverviewPageInterface;

    public class SearchResult
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
    }
}
