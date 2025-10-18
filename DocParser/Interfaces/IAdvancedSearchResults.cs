namespace DocParser.Interfaces
{
    public interface IAdvancedSearchResults : ISearchResultCollection<IAdvancedSearchResult>
    {
        /// <summary>
        /// The search strings.
        /// </summary>
        IEnumerable<string> SearchStrings { get; }
    }
}
