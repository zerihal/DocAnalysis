﻿using DocParser.EventArguments;

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
        /// Gets the document hyperlinks (sync).
        /// </summary>
        /// <returns>List of document hyperlinks.</returns>
        IEnumerable<string> GetDocLinks();

        /// <summary>
        /// Gets the document hyperlinks (async), firing a LinksObtainedAsync event on completion.
        /// </summary>
        /// <returns>Completed task.</returns>
        Task GetDocLinksAsync();       
    }
}
