using DocParser.Interfaces;

namespace DocParser.DocSearch
{
    public class SearchResults : SearchResultsBase<ISearchResult>, ISearchResults
    {
        /// <inheritdoc/>
        public string SearchString { get; }

        public SearchResults(string searchString)
        {
            SearchString = searchString;
        }
    }
}
