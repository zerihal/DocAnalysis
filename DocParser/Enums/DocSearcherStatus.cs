namespace DocParser.Enums
{
    /// <summary>
    /// Status of the document searcher.
    /// </summary>
    public enum DocSearcherStatus
    {
        /// <summary>
        /// The searcher is idle.
        /// </summary>
        Idle,
        /// <summary>
        /// The searcher is currently searching.
        /// </summary>
        Searching,
        /// <summary>
        /// The searcher has completed the search.
        /// </summary>
        Complete,
        /// <summary>
        /// An error occurred during searching.
        /// </summary>
        Error
    }
}
