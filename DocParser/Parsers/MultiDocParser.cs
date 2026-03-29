using DocParser.EventArguments;
using DocParser.Factories;
using DocParser.Interfaces;

namespace DocParser.Parsers
{
    /// <summary>
    /// Provides static methods and events for parsing multiple documentation files and extracting their associated
    /// links using registered document parsers.
    /// </summary>
    /// <remarks>
    /// The MultiDocParser class manages a collection of document parsers and coordinates the extraction of links 
    /// from various documentation files. It raises the FileLinksObtained event when links are obtained from a file, 
    /// allowing subscribers to react to link discovery. This class is thread-safe for static method usage.
    /// </remarks>
    public static class MultiDocParser
    {
        private static readonly List<IDocParser> _docParsers = new List<IDocParser>();

        /// <summary>
        /// Occurs when the process of obtaining file links is initiated, allowing subscribers to handle or modify the
        /// file link retrieval operation (if cancellation token specified for <see cref="ParseDocs(IEnumerable{string})"/>).
        /// </summary>
        public static event EventHandler<ObtainingFileLinksEventArgs>? ObtainingFileLinks;

        /// <summary>
        /// Occurs when file links have been successfully obtained for the file currently being processed.
        /// </summary>
        public static event EventHandler<FileLinksObtainedEventArgs>? FileLinksObtained;

        /// <summary>
        /// Asynchronously parses the specified documentation files and retrieves the collection of links found in each
        /// file.
        /// </summary>
        /// <remarks>
        /// If a suitable parser is not found for a given file, a new parser is created using the factory. The method processes
        /// each file independently and aggregates the results. The order of the returned dictionary corresponds to the order 
        /// of the input paths.
        /// </remarks>
        /// <param name="docPaths">
        /// A collection of file paths representing the documentation files to be parsed. Each path should refer to a valid, 
        /// accessible file.
        /// </param>
        /// <param name="cancellationToken">Cancellation token to stop the task and cease parsing files.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a dictionary mapping each input
        /// file path to an enumerable collection of links extracted from that file. If a file contains no links, the
        /// corresponding collection will be empty.
        /// </returns>
        public static async Task<IDictionary<string, IEnumerable<string>>> ParseDocs(IEnumerable<string> docPaths, 
            CancellationToken? cancellationToken = null)
        {
            var filesAndLinks = new Dictionary<string, IEnumerable<string>>();
            
            foreach (var docPath in docPaths)
            {
                if (cancellationToken?.IsCancellationRequested ?? false)
                    break;

                IDocParser? parser = _docParsers.FirstOrDefault(p => p.IsApplicableForFile(docPath));

                if (parser == null)
                {
                    // Create a new parser for the file type, loading the file at same time.
                    parser = DocParserFactory.CreateDocParserForFile(docPath);

                    // Add links obtained event handler for this parser to raise the MultiDocParser's FileLinksObtained event.
                    parser?.LinksObtainedAsync += (sender, args) =>
                    {
                        if (sender is IDocParser p)
                        {
                            OnFileLinksObtained(p, new FileLinksObtainedEventArgs(docPath, args.Links));
                        }
                    };
                }
                else
                {
                    // Parser already created for this file type, so just load the file.
                    parser.LoadFile(docPath);
                }

                if (parser == null)
                {
                    OnFileLinksObtaining(null, new ObtainingFileLinksEventArgs(docPath, false));
                }
                else
                {
                    OnFileLinksObtaining(parser, new ObtainingFileLinksEventArgs(docPath, true));
                    var links = await parser.GetDocLinksAsync();
                    filesAndLinks.Add(docPath, links);
                }
            }
            
            return filesAndLinks;
        }

        private static void OnFileLinksObtained(object sender, FileLinksObtainedEventArgs e)
        {
            FileLinksObtained?.Invoke(sender, e);
        }

        private static void OnFileLinksObtaining(object? sender, ObtainingFileLinksEventArgs e)
        {
            ObtainingFileLinks?.Invoke(sender, e);
        }
    }
}
