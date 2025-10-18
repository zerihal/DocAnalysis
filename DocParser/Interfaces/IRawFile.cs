using DocParser.Enums;

namespace DocParser.Interfaces
{
    /// <summary>
    /// Raw file for parsing or searching.
    /// </summary>
    public interface IRawFile
    {
        /// <summary>
        /// File name without path.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// File extension.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Raw file content as byte array.
        /// </summary>
        byte[] Content { get; }

        /// <summary>
        /// Type of the raw file.
        /// </summary>
        RawFileType FileType { get; }
    }
}
