namespace DocParser.EventArguments
{
    public class LinksObtainedEventArgs : EventArgs
    {
        /// <summary>
        /// Collection of document hyperlinks.
        /// </summary>
        public IEnumerable<string> Links { get; }

        public LinksObtainedEventArgs(IEnumerable<string> links)
        {
            Links = links;
        }
    }
}
