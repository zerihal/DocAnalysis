namespace DocParser.EventArguments
{
    /// <summary>
    /// Provides data for an event that occurs when links are obtained from a specific file.
    /// </summary>
    public class FileLinksObtainedEventArgs : LinksObtainedEventArgs
    {
        /// <summary>
        /// Full file path of the document for which links were obtained. This property provides context 
        /// about the source of the links,
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Initializes a new instance of the FileLinksObtainedEventArgs class with the specified file name and
        /// collection of links.
        /// </summary>
        /// <param name="fileName">The name of the file for which links have been obtained.</param>
        /// <param name="links">A collection of links associated with the specified file.</param>
        public FileLinksObtainedEventArgs(string fileName, IEnumerable<string> links) : base(links)
        {
            FileName = fileName;
        }
    }
}
