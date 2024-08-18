// <copyright file="SearchSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Searches
{
    using Concierge.Searches.Enums;

    /// <summary>
    /// Represents settings used for conducting searches, including text to search, matching options, search domain, and whether to use regular expressions.
    /// </summary>
    public sealed class SearchSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSettings"/> class with default values.
        /// </summary>
        public SearchSettings()
            : this(string.Empty, false, false, false, SearchDomain.CurrentPage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSettings"/> class with the specified search parameters.
        /// </summary>
        /// <param name="textToSearch">The text to search for.</param>
        /// <param name="matchCase">A value indicating whether the search should be case-sensitive.</param>
        /// <param name="matchWholeWord">A value indicating whether the search should match whole words only.</param>
        /// <param name="useRegex">A value indicating whether to use regular expressions for the search.</param>
        /// <param name="searchDomain">The domain to search within.</param>
        public SearchSettings(string textToSearch, bool matchCase, bool matchWholeWord, bool useRegex, SearchDomain searchDomain)
        {
            this.TextToSearch = textToSearch;
            this.MatchCase = matchCase;
            this.MatchWholeWord = matchWholeWord;
            this.UseRegex = useRegex;
            this.SearchDomain = searchDomain;
        }

        /// <summary>
        /// Gets or sets the text to search for.
        /// </summary>
        public string TextToSearch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the search should be case-sensitive.
        /// </summary>
        public bool MatchCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the search should match whole words only.
        /// </summary>
        public bool MatchWholeWord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use regular expressions for the search.
        /// </summary>
        public bool UseRegex { get; set; }

        /// <summary>
        /// Gets or sets the domain to search within.
        /// </summary>
        public SearchDomain SearchDomain { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not SearchSettings searchSettings)
            {
                return false;
            }

            var equals = true;
            equals &= searchSettings.MatchCase == this.MatchCase;
            equals &= searchSettings.MatchWholeWord == this.MatchWholeWord;
            equals &= searchSettings.UseRegex == this.UseRegex;
            equals &= searchSettings.SearchDomain == this.SearchDomain;
            equals &= searchSettings.TextToSearch.Equals(this.TextToSearch);

            return equals;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
