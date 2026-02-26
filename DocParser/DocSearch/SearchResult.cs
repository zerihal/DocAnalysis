using DocParser.Interfaces;

namespace DocParser.DocSearch
{
    /// <summary>
    /// Implementation of <see cref="ISearchResult"/>.
    /// </summary>
    public class SearchResult : ISearchResult, IEquatable<SearchResult?>
    {
        /// <inheritdoc/>
        public string Sentence { get; }

        /// <inheritdoc/>
        public int Position { get; }

        /// <inheritdoc/>
        public string Paragraph { get; }

        /// <inheritdoc/>
        public int ParagraphNumber { get; }

        /// <inheritdoc/>
        public int Page { get; }

        /// <inheritdoc/>
        public string Document { get; }

        /// <inheritdoc/>
        public int FileIndex { get; }

        /// <summary>
        /// Creates a new instance of <see cref="SearchResult"/>.
        /// </summary>
        /// <param name="sentence">Sentence containing the search string.</param>
        /// <param name="position">Position of the search string within the paragraph.</param>
        /// <param name="paragraph">Paragraph containing the search string.</param>
        /// <param name="paragraphNo">Paragraph number containing the search result.</param>
        /// <param name="page">Page number containing the search result (applicable to PDF only).</param>
        /// <param name="document">The document that the search result was found in.</param>
        public SearchResult(string sentence, int position, string paragraph, int paragraphNo, int page, string document, int fileIndex)
        {
            Sentence = sentence;
            Position = position;
            Paragraph = paragraph;
            ParagraphNumber = paragraphNo;
            Page = page;
            Document = document;
            FileIndex = fileIndex;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as SearchResult);
        }

        /// <inheritdoc/>
        public bool Equals(SearchResult? other)
        {
            return other is not null &&
                   Sentence == other.Sentence &&
                   Position == other.Position &&
                   Page == other.Page &&
                   Document == other.Document;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Sentence?.GetHashCode() ?? 0);
                hash = hash * 23 + Position.GetHashCode();
                hash = hash * 23 + Page.GetHashCode();
                hash = hash * 23 + (Document?.GetHashCode() ?? 0);
                return hash;
            }
#else
            return HashCode.Combine(Sentence, Position, Page, Document);
#endif
        }

        /// <summary>
        /// Default equality operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SearchResult? left, SearchResult? right)
        {
            return EqualityComparer<SearchResult>.Default.Equals(left, right);
        }

        /// <summary>
        /// Default inequality operator.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SearchResult? left, SearchResult? right)
        {
            return !(left == right);
        }
    }
}
