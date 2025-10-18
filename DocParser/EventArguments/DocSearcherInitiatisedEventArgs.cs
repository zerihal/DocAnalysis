namespace DocParser.EventArguments
{
    public class DocSearcherInitiatisedEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether there was an issue loading files for searching.
        /// </summary>
        public bool UnableToLoadFiles { get; }

        public DocSearcherInitiatisedEventArgs(bool unableToLoadFiles)
        {
            UnableToLoadFiles = unableToLoadFiles;
        }
    }
}
