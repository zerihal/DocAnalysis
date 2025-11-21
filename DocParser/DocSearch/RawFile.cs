using DocParser.Enums;
using DocParser.Interfaces;
using DocParser.Strings;

namespace DocParser.DocSearch
{
    /// <inheritdoc/>
    public class RawFile : IRawFile
    {
        /// <inheritdoc/>
        public int FileIndex { get; }

        /// <inheritdoc/>
        public string FileName { get; }

        /// <inheritdoc/>
        public string FileExtension { get; }

        /// <inheritdoc/>
        public byte[] Content { get; }

        /// <inheritdoc/>
        public RawFileType FileType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawFile"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="content">File binary content.</param>
        /// <param name="index">File index in collection of files to search.</param>
        public RawFile(string fileName, byte[] content, int index)
        {
            FileIndex = index;
            FileName = fileName;
            FileExtension = FileName == SR.FileStreamDocName ? $".{SR.FileStreamDocName}" : Path.GetExtension(fileName);
            FileType = FileExtension.ToLower() switch
            {
                ".docx" => RawFileType.Word,
                ".pdf" => RawFileType.Pdf,
                _ => RawFileType.TextDoc,
            };
            Content = content;
        }
    }
}
