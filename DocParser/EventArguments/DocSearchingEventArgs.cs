using DocParser.Enums;

namespace DocParser.EventArguments
{
    /// <summary>
    /// Event arguments for document searching process.
    /// </summary>
    public class DocSearchingEventArgs : EventArgs
    {
        /// <summary>
        /// Status of the document search process.
        /// </summary>
        public DocSearcherStatus Status { get; }

        /// <summary>
        /// Number of documents that have been searched.
        /// </summary>
        public int DocSearchCount { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DocSearchingEventArgs"/>.
        /// </summary>
        /// <param name="status">Document searcher status (<see cref="DocSearcherStatus"/>)</param>
        /// <param name="docSearchCount">Count of documents seached.</param>
        public DocSearchingEventArgs(DocSearcherStatus status, int docSearchCount)
        {
            Status = status;
            DocSearchCount = docSearchCount;
        }
    }
}
