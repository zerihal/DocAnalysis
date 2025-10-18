using DocParser.Enums;
using DocParser.Interfaces;
using DocParser.Strings;

namespace DocParser.DocSearch
{
    public class RawFile : IRawFile
    {
        /// <inheritdoc/>
        public string FileName { get; }

        /// <inheritdoc/>
        public string FileExtension { get; }

        /// <inheritdoc/>
        public byte[] Content { get; }

        /// <inheritdoc/>
        public RawFileType FileType { get; }

        public RawFile(string fileName, byte[] content)
        {
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
