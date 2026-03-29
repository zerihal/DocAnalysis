namespace DocParser.EventArguments
{
    /// <summary>
    /// Provides data for events that occur when file links are being obtained.
    /// </summary>
    public class ObtainingFileLinksEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the full file system path associated with the current instance.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets a value indicating whether a supported parser is available for the current file type.
        /// </summary>
        public bool SupportedParserAvailable { get; }

        /// <summary>
        /// Initializes a new instance of the ObtainingFileLinksEventArgs class with the specified file path.
        /// </summary>
        /// <param name="filePath">The full path of the file for which links are being obtained.</param>
        /// <param name="supportedParserAvailable">Indicate whether supported file parser found.</param>
        public ObtainingFileLinksEventArgs(string filePath, bool supportedParserAvailable)
        {
            FilePath = filePath;
            SupportedParserAvailable = supportedParserAvailable;
        }
    }
}
