namespace DocParser.Interfaces
{
    /// <summary>
    /// Collection of standard search results.
    /// </summary>
    public interface ISearchResults : ISearchResultCollection<ISearchResult>
    {
        /// <summary>
        /// The search string.
        /// </summary>
        string SearchString { get; }
    }
}
