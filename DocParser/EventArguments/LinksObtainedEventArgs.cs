namespace DocParser.EventArguments
{
    /// <summary>
    /// Event arguments for links obtained event.
    /// </summary>
    public class LinksObtainedEventArgs : EventArgs
    {
        /// <summary>
        /// Collection of document hyperlinks.
        /// </summary>
        public IEnumerable<string> Links { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="LinksObtainedEventArgs"/> class.
        /// </summary>
        /// <param name="links">Collection of hyperlink strings.</param>
        public LinksObtainedEventArgs(IEnumerable<string> links)
        {
            Links = links;
        }
    }
}
