using DocParser.Interfaces;

namespace DocParser.DocSearch
{
    /// <inheritdoc/>
    public class SearchResults : SearchResultsBase<ISearchResult>, ISearchResults
    {
        /// <inheritdoc/>
        public string SearchString { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ISearchResults"/>.
        /// </summary>
        /// <param name="searchString">Search string.</param>
        public SearchResults(string searchString)
        {
            SearchString = searchString;
        }
    }
}
