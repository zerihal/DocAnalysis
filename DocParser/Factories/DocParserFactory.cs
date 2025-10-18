using DocParser.Interfaces;
using DocParser.Logging;
using DocParser.Parsers;

namespace DocParser.Factories
{
    /// <summary>
    /// Factory for creating document parser instances.
    /// </summary>
    public static class DocParserFactory
    {
        /// <summary>
        /// Creates an appropriate instance of document parser for the file type.
        /// </summary>
        /// <param name="filename">Filename with extension.</param>
        /// <returns>Instance of <see cref="IDocParser"/> appropriate for the file type.</returns>
        public static IDocParser? CreateDocParserForFile(string filename)
        {
            var fileExt = Path.GetExtension(filename).ToLower();

            switch (fileExt)
            {
                case ".docx":
                    return new WordDocParser();

                case ".txt":
                case ".rtf":
                    return new TextDocParser();

                case ".doc":
                case ".odt":
                    return new GenericDocParser();

                default:
                    Logger.LogWarning("Unsupported file format");
                    break;
            }

            return null;
        }
    }
}
