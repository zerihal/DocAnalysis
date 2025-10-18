using DocParser.Enums;

namespace DocParser.EventArguments
{
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

        public DocSearchingEventArgs(DocSearcherStatus status, int docSearchCount)
        {
            Status = status;
            DocSearchCount = docSearchCount;
        }
    }
}
