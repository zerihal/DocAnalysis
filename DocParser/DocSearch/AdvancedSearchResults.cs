using DocParser.Interfaces;

namespace DocParser.DocSearch
{
    public class AdvancedSearchResults : SearchResultsBase<IAdvancedSearchResult>, IAdvancedSearchResults
    {
        /// <inheritdoc/>
        public IEnumerable<string> SearchStrings { get; }

        public AdvancedSearchResults(IEnumerable<string> searchStrings)
        {
            SearchStrings = searchStrings;
        }
    }
}
