using DocParser.EventArguments;

namespace DocParser.Interfaces
{
    /// <summary>
    /// Document parser for obtaining document hyperlinks.
    /// </summary>
    public interface IDocParser : IDocLoader
    {
        /// <summary>
        /// Event handler for links obtained when requested via async method.
        /// </summary>
        event EventHandler<LinksObtainedEventArgs>? LinksObtainedAsync;

        /// <summary>
        /// File types (extensions) applicable for the doc parser instance.
        /// </summary>
        string[] ApplicableFileTypes { get; }

        /// <summary>
        /// Gets the document hyperlinks (sync).
        /// </summary>
        /// <returns>List of document hyperlinks.</returns>
        IEnumerable<string> GetDocLinks();

        /// <summary>
        /// Checks if this instance of parser is applicable for the given file or extension.
        /// </summary>
        /// <param name="fileOrExt">File or file extension.</param>
        /// <returns>
        /// <see langword="True"/> if the file/extension is applicable for this instance of parser, 
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool IsApplicableForFile(string fileOrExt);

        /// <summary>
        /// Gets the document hyperlinks (async), firing a LinksObtainedAsync event on completion.
        /// </summary>
        /// <returns>List of document hyperlinks.</returns>
        Task<IEnumerable<string>> GetDocLinksAsync();       
    }
}
