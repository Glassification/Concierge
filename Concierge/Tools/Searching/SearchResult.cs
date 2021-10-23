// <copyright file="SearchResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Searching
{
    using System.Text.RegularExpressions;

    using Concierge.Interfaces;

    public class SearchResult
    {
        public SearchResult()
            : this(string.Empty, null, null, null)
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
