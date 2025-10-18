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

        /// <summary>
        /// Creates a new instance of <see cref="SearchResult"/>.
        /// </summary>
        public SearchResult(string sentence, int position, string paragraph, int paragraphNo, int page, string document)
        {
            Sentence = sentence;
            Position = position;
            Paragraph = paragraph;
            ParagraphNumber = paragraphNo;
            Page = page;
            Document = document;
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
            return HashCode.Combine(Sentence, Position, Page, Document);
        }
        
        public static bool operator ==(SearchResult? left, SearchResult? right)
        {
            return EqualityComparer<SearchResult>.Default.Equals(left, right);
        }

        public static bool operator !=(SearchResult? left, SearchResult? right)
        {
            return !(left == right);
        }
    }
}
