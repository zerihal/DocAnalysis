namespace DocParser.EventArguments
{
    /// <summary>
    /// Event arguments for when a document searcher has been initialised.
    /// </summary>
    public class DocSearcherInitiatisedEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether there was an issue loading files for searching.
        /// </summary>
        public bool UnableToLoadFiles { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DocSearcherInitiatisedEventArgs"/>.
        /// </summary>
        /// <param name="unableToLoadFiles">Flag for searcher was unable to load files.</param>
        public DocSearcherInitiatisedEventArgs(bool unableToLoadFiles)
        {
            UnableToLoadFiles = unableToLoadFiles;
        }
    }
}
