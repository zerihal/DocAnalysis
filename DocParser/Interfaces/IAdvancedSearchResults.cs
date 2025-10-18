namespace DocParser.Interfaces
{
    /// <summary>
    /// Collection of advanced search results.
    /// </summary>
    public interface IAdvancedSearchResults : ISearchResultCollection<IAdvancedSearchResult>
    {
        /// <summary>
        /// The search strings.
        /// </summary>
        IEnumerable<string> SearchStrings { get; }
    }
}
