using DocParser.DocOps;
using DocParser.Enums;
using DocParser.EventArguments;
using DocParser.Interfaces;
using DocParser.Strings;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DocParser.DocSearch
{
    /// <inheritdoc/>
    public class DocSearcher : IDocSearcher
    {
        private const string SentenceSplitPattern = @"(?<=[.!?])\s+"; // Split on ". ", "! ", or "? "

        private IEnumerable<object>? _files;
        private IDocLoader _docLoader;

        /// <inheritdoc/>
        public event EventHandler<DocSearcherInitiatisedEventArgs>? Initiatised;

        /// <inheritdoc/>
        public event EventHandler<DocSearchingEventArgs>? SearchingChanged;

        /// <inheritdoc/>
        public bool IsInitialised { get; private set; }

        /// <inheritdoc/>
        public IList<IRawFile> RawFiles { get; private set; } = [];

        /// <inheritdoc/>
        public DocSearchOptions SearcherOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocSearcher"/> class.
        /// </summary>
        public DocSearcher() 
        {
            _docLoader = new DocLoader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocSearcher"/> class with specified files.
        /// </summary>
        /// <param name="files">Files to load.</param>
        /// <remarks>
        /// The collection of files should be of type string (file paths), streams, or IFormFiles. If files
        /// are not of a supported type, no files will be loaded to the RawFiles collection.
        /// </remarks>
        public DocSearcher(IEnumerable<object> files) : this()
        {
            _ = LoadFiles(files);
        }

        /// <inheritdoc/>
        public async Task<bool> LoadFiles(IEnumerable<object> files)
        {
            IsInitialised = false;

            PreLoadFiles(files);

            var filesAllSameType = false;

            if (files.Select(f => f?.GetType()).Distinct().Count() <= 1)
            {
                // Perform valid type check
                if (!IsValidType(files.First().GetType()))
                    return false;

                filesAllSameType = true;
            }

            await LoadFilesInternal(filesAllSameType);

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> LoadFiles<T>(IEnumerable<T> files)
        {
            if (!IsValidType(typeof(T)))
                return false;

            PreLoadFiles((IEnumerable<object>)files);

            await LoadFilesInternal(true);
            return IsInitialised;
        }

        /// <inheritdoc/>
        public async Task<ISearchResults> Search(string searchString)
        {
            OnSearchingChanged(new DocSearchingEventArgs(DocSearcherStatus.Searching, 0));

            var searchResults = new SearchResults(searchString);

            // Do the search and build results
            foreach (var rawFile in RawFiles)
            {
                var fileSearchResults = SearchFile(searchString, rawFile);

                if (fileSearchResults?.Any() ?? false)
                    searchResults.AddRange(fileSearchResults);
            }

            OnSearchingChanged(new DocSearchingEventArgs(DocSearcherStatus.Complete, searchResults.DiscoveredCount));

            await Task.CompletedTask;
            return searchResults;
        }

        /// <inheritdoc/>
        public async Task<IAdvancedSearchResults> AdvancedSearch(string[] searchStrings)
        {
            OnSearchingChanged(new DocSearchingEventArgs(DocSearcherStatus.Searching, 0));

            var advSearchResults = new AdvancedSearchResults(searchStrings);
            
            // Do the search and build results
            foreach (var rawFile in RawFiles)
            {
                var fileAdvSearchResults = AdvancedSearchFile(searchStrings, rawFile);

                if (fileAdvSearchResults?.Any() ?? false)
                    advSearchResults.AddRange(fileAdvSearchResults);
            }
            
            OnSearchingChanged(new DocSearchingEventArgs(DocSearcherStatus.Complete, advSearchResults.DiscoveredCount));
            
            await Task.CompletedTask;
            return advSearchResults;
        }

        /// <summary>
        /// Pre-load files action - clears existing raw files and initialised flag, checks collection and repopulates 
        /// internal files collection.
        /// </summary>
        /// <param name="files">Files to load.</param>
        /// <exception cref="ArgumentException">Thrown if unsupported type or null/invalid input collection.</exception>
        private void PreLoadFiles(IEnumerable<object> files)
        {
            IsInitialised = false;

            // Clear and nullify collections
            RawFiles.Clear();
            _files = null;

            if (files is IEnumerable<object> objectFiles)
            {
                // Assuming _files is List<object> or compatible
                _files = objectFiles.ToList();
            }
            else
            {
                throw new ArgumentException("Unsupported file type. Must be IEnumerable<object>.");
            }
        }

        /// <summary>
        /// Checks whether the type is valid for the DocSearcher.
        /// </summary>
        /// <param name="type">File object type.</param>
        /// <returns><see langword="True"/> if valid/supported, otherwise <see langword="false"/>.</returns>
        private bool IsValidType(Type type)
        {
            // Type check.
            if (type.IsAssignableFrom(typeof(string)) ||
                type.IsAssignableFrom(typeof(Stream)) ||
                typeof(IFormFileStream).IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads files into the searcher (internal).
        /// </summary>
        /// <returns>Completed task.</returns>
        private async Task LoadFilesInternal(bool ofSameType)
        {
            var noFileLoad = false;
            var fileIndex = 0;

            if (_files != null)
            {
                if (ofSameType)
                {
                    // Files are all same type - cast and batch load
                    var fileType = _files.FirstOrDefault()?.GetType();

                    if (fileType != null)
                    {
                        switch (fileType)
                        {
                            case var _ when fileType == typeof(string):
                                foreach (var stringFile in _files.Cast<string>())
                                {
                                    if (_docLoader.LoadFile(stringFile) && _docLoader.RawFile != null)
                                    {
                                        RawFiles.Add(new RawFile(stringFile, _docLoader.RawFile, fileIndex));
                                    }

                                    fileIndex++;
                                }
                                break;

                            case var _ when fileType == typeof(Stream):
                                foreach (var streamFile in _files.Cast<Stream>())
                                {
                                    if (_docLoader.LoadFile(streamFile) && _docLoader.RawFile != null)
                                    {
                                        RawFiles.Add(new RawFile(SR.FileStreamDocName, _docLoader.RawFile, fileIndex));
                                    }

                                    fileIndex++;
                                }
                                break;

                            case var _ when typeof(IFormFileStream).IsAssignableFrom(fileType):
                                foreach (var formFileStream in _files.Cast<IFormFileStream>())
                                {
                                    if (_docLoader.LoadFile(formFileStream.FileStream) && _docLoader.RawFile != null)
                                    {
                                        RawFiles.Add(new RawFile(formFileStream.FormFileName, _docLoader.RawFile, fileIndex));
                                    }
                                }
                                break;

                            default:
                                noFileLoad = true;
                                break;
                        }
                    }
                }
                else
                {
                    // Mixed file types - load individually
                    foreach (var file in _files)
                    {
                        if (file is string stringFile)
                        {
                            // Load string file
                            if (_docLoader.LoadFile(stringFile) && _docLoader.RawFile != null)
                            {
                                RawFiles.Add(new RawFile(stringFile, _docLoader.RawFile, fileIndex));
                            }
                        }
                        else
                        {
                            // Load form file or stream file
                            Stream? fileStream = null;
                            string? fileName = null;

                            if (file is IFormFileStream formFileStream)
                            {
                                fileStream = formFileStream.FileStream;
                                fileName = formFileStream.FormFileName;
                            }
                            else if (file is Stream stream)
                            {
                                fileStream = stream;
                                fileName = SR.FileStreamDocName;
                            }

                            if (fileStream != null && fileName != null)
                            {
                                if (_docLoader.LoadFile(fileStream) && _docLoader.RawFile != null)
                                {
                                    RawFiles.Add(new RawFile(fileName, _docLoader.RawFile, fileIndex));
                                }
                            }
                        }

                        // File index is always incremented regardless of whether raw file could be added or not
                        fileIndex++;
                    }

                    noFileLoad = !RawFiles.Any();
                }
            }
            else
            {
                noFileLoad = true;
            }

            IsInitialised = true;
            OnInitialised(new DocSearcherInitiatisedEventArgs(noFileLoad));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Performs a search on the provided raw file for the specified search string.
        /// </summary>
        /// <param name="searchString">String to search for in document.</param>
        /// <param name="rawFile">Raw file to search.</param>
        /// <returns>Collection of <see cref="ISearchResult"/></returns>
        private IEnumerable<ISearchResult> SearchFile(string searchString, IRawFile rawFile)
        {
            var results = new List<ISearchResult>();
            var paragraphsIndex = new Dictionary<int, string>();
            var paragraphs = new Dictionary<int, string>();
            var paragraphNo = 0;
            var ignoreCase = !SearcherOption.HasFlag(DocSearchOptions.CaseSensitive);
            var matchExact = SearcherOption.HasFlag(DocSearchOptions.MatchExact);

            try
            {
                if (rawFile.FileType == RawFileType.Word)
                {
                    using (var memStream = new MemoryStream(rawFile.Content))
                    {
                        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memStream, false))
                        {
                            // Parse though paragraphs in the document body
                            if (wordDoc.MainDocumentPart?.Document.Body is Body body)
                            {
                                // Get all non-empty paragraphs
                                var allNonEmptyParagraphs = body.Elements<Paragraph>().Select(p => p.InnerText.Trim()).
                                                Where(t => !string.IsNullOrEmpty(t)).ToList();

                                foreach (var para in allNonEmptyParagraphs)
                                {
                                    // Always increment paragraph number
                                    paragraphNo++;

                                    // If paragraph contains search string then add along with paragraph number
                                    if (para.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
                                        paragraphs.Add(paragraphNo, para);
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (var streamReader = new StreamReader(new MemoryStream(rawFile.Content)))
                    {
                        // First check whether there are any instances of the search string in the document - 
                        // if not then no point in parsing.
                        if (streamReader.ReadToEnd().Contains(searchString))
                        {
                            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                            while (streamReader.ReadLine() is string line)
                            {
                                // Ignore empty lines
                                if (line.Length > 0)
                                {
                                    // Always increment paragraph number
                                    paragraphNo++;

                                    // Ignore lines not containing the search string
                                    if (!line.Contains(searchString, ignoreCase ? StringComparison.InvariantCultureIgnoreCase :
                                        StringComparison.InvariantCulture))
                                    {
                                        continue;
                                    }

                                    // Note: ReadLine continues until it hits a newline char, so each is a paragraph in 
                                    // this case.
                                    paragraphs.Add(paragraphNo, line.Trim());
                                }
                            }
                        }
                    }
                }

                foreach (var paragraph in paragraphs)
                {
                    // Get all sentences in the paragraph
                    var sentences = Regex.Split(paragraph.Value, SentenceSplitPattern);

                    // Find all matches in paragraph
                    var matchCollection = matchExact ? GetWholeWordMatches(paragraph.Value, searchString, ignoreCase) :
                        Regex.Matches(paragraph.Value, Regex.Escape(searchString), RegexOptions.IgnoreCase);

                    var matches = matchCollection.Cast<Match>().Select(m => m.Index);

                    // Parse sentences to find matching search results
                    foreach (var sentence in sentences)
                    {
                        // The following assumes no repeated sentences in paragraph - might want to improve this in future!
                        var startIndex = paragraph.Value.IndexOf(sentence);
                        var endIndex = paragraph.Value.IndexOf(sentence) + sentence.Length;
                        var matchesInSentence = matches?.Where(m => m >= startIndex && m <= endIndex);

                        if (matchesInSentence?.Any() ?? false)
                        {
                            foreach (var matchInSentence in matchesInSentence)
                            {
                                results.Add(new SearchResult(sentence, matchInSentence, paragraph.Value, paragraph.Key, -1,
                                    rawFile.FileName, rawFile.FileIndex));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception searching raw file (empty results will be returned) - {e}");
            }

            return results;
        }

        /// <summary>
        /// Performs an advanced search on the provided raw file for multiple search strings.
        /// </summary>
        /// <param name="searchStrings">Strings to search for within file.</param>
        /// <param name="rawFile">Raw file to search.</param>
        /// <returns>Collection of <see cref="IAdvancedSearchResult"/></returns>
        private IEnumerable<IAdvancedSearchResult> AdvancedSearchFile(string[] searchStrings, IRawFile rawFile)
        {
            var advSearchResults = new List<IAdvancedSearchResult>();

            // First get search result for each search string
            var searches = new Dictionary<string, List<ISearchResult>>();

            foreach (var searchString in searchStrings)
            {
                var results = SearchFile(searchString, rawFile)?.ToList();
                if (results?.Any() ?? false)
                    searches.Add(searchString, results);
            }

            // Now build up a dictionary of paragraph number and matched search results
            var paragraphResults = new Dictionary<int, List<ISearchResult>>();

            foreach (var search in searches)
            {
                foreach (var result in search.Value)
                {
                    // Create a new entry if paragraph number not already present
                    if (!paragraphResults.ContainsKey(result.ParagraphNumber))
                        paragraphResults.Add(result.ParagraphNumber, new List<ISearchResult>());

                    // Add result to paragraph entry
                    paragraphResults[result.ParagraphNumber].Add(result);
                }
            }

            // We have a dictionary of paragraphs with all search results in that paragraph, so we can now create a 
            // collection of advanced search results to return.
            foreach (var paraResult in paragraphResults)
            {
                advSearchResults.Add(new AdvancedSearchResult(paraResult.Value, searchStrings));
            }

            return advSearchResults.OrderByDescending(r => r.MatchRating);
        }

        //private bool ContainsWholeWord(string text, string word, bool ignoreCase)
        //{
        //    var regexOpts = RegexOptions.CultureInvariant;

        //    if (ignoreCase)
        //        regexOpts |= RegexOptions.IgnoreCase;

        //    var pattern = $@"\b{Regex.Escape(word)}\b";
        //    return Regex.IsMatch(text, pattern, regexOpts);
        //}

        // TODO: This needs to be tested!
        private MatchCollection GetWholeWordMatches(string text, string word, bool ignoreCase)
        {
            var regexOpts = RegexOptions.CultureInvariant;

            if (ignoreCase)
                regexOpts |= RegexOptions.IgnoreCase;

            var pattern = $@"\b{Regex.Escape(word)}\b";
            return Regex.Matches(text, pattern, regexOpts);
        }

        private void OnInitialised(DocSearcherInitiatisedEventArgs e) => Initiatised?.Invoke(this, e);

        private void OnSearchingChanged(DocSearchingEventArgs e) => SearchingChanged?.Invoke(this, e);

        private enum FileType
        {
            String,
            Stream,
            Unsupported
        }
    }
}
