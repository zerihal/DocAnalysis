namespace DocParser.Interfaces
{
    public interface ISearchResult
    {
        /// <summary>
        /// Sentence containing the search string
        /// </summary>
        string Sentence { get; }

        /// <summary>
        /// Position of the search string (start index) within the sentence.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Paragraph containing the search string.
        /// </summary>
        /// <remarks>
        /// Note: The paragraph may contain multiple instances of the search string. The instance that relates to
        /// this result will be denoted by the <see cref="Position"/>.
        /// </remarks>
        string Paragraph { get; }

        /// <summary>
        /// Paragraph number within the document (if applicable) that contains this search result.
        /// </summary>
        int ParagraphNumber { get; }

        /// <summary>
        /// Page number within the document (if applicable) that contains this search result.
        /// </summary>
        /// <remarks>
        /// Note: This is currently only supported for PDF documents. For other document types this will return -1.
        /// </remarks>
        int Page { get; }

        /// <summary>
        /// The document that the search result was found in.
        /// </summary>
        string Document { get; }
    }
}
