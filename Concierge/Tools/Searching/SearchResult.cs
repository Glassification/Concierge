// <copyright file="SearchResult.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Searching
{
    using Concierge.Interfaces;

    public class SearchResult
    {
        public SearchResult()
        {
            this.Item = null;
            this.ConciergePage = null;
            this.SearchText = string.Empty;
        }

        public SearchResult(string searchText, object item, IConciergePage conciergePage)
        {
            this.SearchText = searchText;
            this.Item = item;
            this.ConciergePage = conciergePage;
        }

        public object Item { get; set; }

        public IConciergePage ConciergePage { get; set; }

        public string SearchText { get; set; }
    }
}
