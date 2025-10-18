using DocParser.Interfaces;

namespace DocParser.DocSearch
{
    /// <inheritdoc/>
    public class AdvancedSearchResults : SearchResultsBase<IAdvancedSearchResult>, IAdvancedSearchResults
    {
        /// <inheritdoc/>
        public IEnumerable<string> SearchStrings { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="IAdvancedSearchResults"/>.
        /// </summary>
        /// <param name="searchStrings">Search strings that were used to obtain results.</param>
        public AdvancedSearchResults(IEnumerable<string> searchStrings)
        {
            SearchStrings = searchStrings;
        }
    }
}
