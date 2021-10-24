// <copyright file="SearchSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Searching
{
    using Concierge.Tools.Searching.Enums;

    public class SearchSettings
    {
        public SearchSettings()
            : this(string.Empty, false, false, false, SearchDomain.CurrentPage)
        {
        }

        public SearchSettings(string textToSearch, bool matchCase, bool matchWholeWord, bool useRegex, SearchDomain searchDomain)
        {
            this.TextToSearch = textToSearch;
            this.MatchCase = matchCase;
            this.MatchWholeWord = matchWholeWord;
            this.UseRegex = useRegex;
            this.SearchDomain = searchDomain;
        }

        public string TextToSearch { get; set; }

        public bool MatchCase { get; set; }

        public bool MatchWholeWord { get; set; }

        public bool UseRegex { get; set; }

        public SearchDomain SearchDomain { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not SearchSettings)
            {
                return false;
            }

            var equals = true;
            var searchSettings = obj as SearchSettings;

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
