namespace DocParser.Interfaces
{
    public interface ISearchResultCollection<T> : IEnumerable<T>
    {
        /// <summary>
        /// Total count of discovered instances of the search string.
        /// </summary>
        int DiscoveredCount { get; }

        /// <summary>
        /// Adds a new search result.
        /// </summary>
        /// <param name="item">Search result to add.</param>
        /// <returns><see langword="True"/> if the search result was added or <see langword="false"/> if already present.</returns>
        bool Add(T item);

        /// <summary>
        /// Adds a range of search results.
        /// </summary>
        /// <param name="items">Search results to add.</param>
        /// <returns><see langword="True"/> if any search results were added, otherwise <see langword="false"/>.</returns>
        bool AddRange(IEnumerable<T> items);

        /// <summary>
        /// Removes an existing search result.
        /// </summary>
        /// <param name="item">Search result to remove.</param>
        /// <returns><see langword="True"/> if the search result was removed or <see langword="false"/> if not present.</returns>
        bool Remove(T item);

        /// <summary>
        /// Clears all search results.
        /// </summary>
        void Clear();
    }
}
