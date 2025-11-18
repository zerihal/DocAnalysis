namespace DocParser.Interfaces
{
    /// <summary>
    /// Simple wrapper for FormFile to expose the file name and stream.
    /// </summary>
    public interface IFormFileStream
    {
        /// <summary>
        /// Form file name.
        /// </summary>
        string FormFileName { get; }

        /// <summary>
        /// Form file stream.
        /// </summary>
        Stream FileStream { get; }
    }
}
