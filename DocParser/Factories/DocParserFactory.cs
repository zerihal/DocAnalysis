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
        /// <param name="loadFile">
        /// Loads the specified file to the created instance of <see cref="IDocParser"/> (default is <see langword="true"/>).
        /// </param>
        /// <returns>Instance of <see cref="IDocParser"/> appropriate for the file type.</returns>
        public static IDocParser? CreateDocParserForFile(string filename, bool loadFile = true)
        {
            var fileExt = Path.GetExtension(filename).ToLower();
            IDocParser? docParser = null;

            switch (fileExt)
            {
                case ".docx":
                    docParser = new WordDocParser();
                    break;

                case ".txt":
                case ".rtf":
                    docParser = new TextDocParser();
                    break;

                case ".doc":
                case ".odt":
                    docParser = new GenericDocParser();
                    break;

                default:
                    Logger.LogWarning("Unsupported file format");
                    break;
            }

            if (docParser != null)
            {
                if (loadFile)
                    docParser.LoadFile(filename);

                return docParser;
            }

            return null;
        }
    }
}
