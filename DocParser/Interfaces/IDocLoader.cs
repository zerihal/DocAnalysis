namespace DocParser.Interfaces
{
    /// <summary>
    /// Document loader to load files for parsing.
    /// </summary>
    public interface IDocLoader
    {
        /// <summary>
        /// Raw file bytes from last loaded file (if any).
        /// </summary>
        byte[]? RawFile { get; }

        /// <summary>
        /// Loads the specified file to be used by the parser.
        /// </summary>
        /// <param name="filename">Full filename and path.</param>
        /// <returns><see langword="True"/> if the file was successfully loaded, otherwise <see langword="false"/>.</returns>
        bool LoadFile(string filename);

        /// <summary>
        /// Loads the specified file stream to be used by the parser.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <returns><see langword="True"/> if the file was successfully loaded, otherwise <see langword="false"/>.</returns>
        bool LoadFile(Stream stream);
    }
}
