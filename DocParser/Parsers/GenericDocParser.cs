using DocParser.Logging;
using DocParser.Strings;

namespace DocParser.Parsers
{
    /// <summary>
    /// Generic document parser for text-based documents.
    /// </summary>
    public class GenericDocParser : TextDocParser
    {
        /// <inheritdoc/>
        public override string[] ApplicableFileTypes => [".doc", ".odt"];

        /// <summary>
        /// Attempts to obtain the document text from the file stream and then loads it to be used by the parser.
        /// </summary>
        /// <param name="fileStream">File stream.</param>
        /// <returns><see langword="True"/> if the file was successfully loaded, otherwise <see langword="false"/>.</returns>
        public override bool LoadFile(Stream fileStream)
        {
            var docText = DocParserHelper.TryGetDocText(fileStream, out var error);

            if (!string.IsNullOrEmpty(docText))
            {
                return base.LoadFile(docText);
            }

            if (error != null)
                Logger.LogError(error);
            else
                Logger.LogWarning(SR.GenericDocLoadWarning);

            return false;
        }

        /// <summary>
        /// Attempts to obtain the document text from the file and then loads it to be used by the parser.
        /// </summary>
        /// <param name="filename">Full filename and path.</param>
        /// <returns><see langword="True"/> if the file was successfully loaded, otherwise <see langword="false"/>.</returns>
        public override bool LoadFile(string filename)
        {
            var docText = DocParserHelper.TryGetDocText(filename, out var error);

            if (!string.IsNullOrEmpty(docText))
            {
                return base.LoadFile(filename);
            }

            if (error != null)
                Logger.LogError(error);
            else
                Logger.LogWarning(SR.GenericDocLoadWarning);

            return false;           
        }
    }
}
