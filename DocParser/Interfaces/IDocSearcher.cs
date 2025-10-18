using DocParser.Enums;
using DocParser.EventArguments;

namespace DocParser.Interfaces
{
    /// <summary>
    /// Document searcher for loading documents and performing search operations.
    /// </summary>
    public interface IDocSearcher
    {
        /// <summary>
        /// Event fired when the searcher has been initialised with files.
        /// </summary>
        event EventHandler<DocSearcherInitiatisedEventArgs>? Initiatised;

        /// <summary>
        /// Event fired when the searching state has changed.
        /// </summary>
        event EventHandler<DocSearchingEventArgs>? SearchingChanged;

        /// <summary>
        /// Indicates whether the searcher has been initialised with files.
        /// </summary>
        bool IsInitialised { get; }

        /// <summary>
        /// List of loaded raw files.
        /// </summary>
        IList<IRawFile> RawFiles { get; }

        /// <summary>
        /// Gets or sets how the searcher should match strings.
        /// </summary>
        /// <remarks>
        /// Default is no options, which will match whether search terms are contained within the document
        /// as a whole or partial word and case insensitive.
        /// </remarks>
        DocSearchOptions SearcherOption { get; set; }

        /// <summary>
        /// Loads files into the searcher.
        /// </summary>
        /// <typeparam name="T">Files type.</typeparam>
        /// <param name="files">Collection of files (or streams).</param>
        /// <returns><see langword="True"/> if files loaded, otherwise <see langword="false"/>.</returns>
        Task<bool> LoadFiles<T>(IEnumerable<T> files);

        /// <summary>
        /// Searches the loaded files for the specified search string.
        /// </summary>
        /// <param name="searchString">String to search for.</param>
        /// <returns>Search results collection.</returns>
        Task<ISearchResults> Search(string searchString);

        /// <summary>
        /// Performs an advanced search using multiple search strings.
        /// </summary>
        /// <param name="searchStrings">Strings to search for.</param>
        /// <returns>Collection of advanced search results.</returns>
        Task<IAdvancedSearchResults> AdvancedSearch(string[] searchStrings);
    }

    /// <summary>
    /// Represents options for how the searcher should match strings.
    /// </summary>
    public enum SearchOption
    {
        MatchContainsIgnoreCase,
        MatchExact,
        MatchStartsWith,
        MatchEndsWith
    }
}
