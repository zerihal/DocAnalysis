namespace DocParser.Interfaces
{
    public interface ISearchResults : ISearchResultCollection<ISearchResult>
    {
        /// <summary>
        /// The search string.
        /// </summary>
        string SearchString { get; }
    }
}
