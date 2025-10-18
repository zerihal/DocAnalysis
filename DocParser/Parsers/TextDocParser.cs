using DocParser.Enums;
using DocParser.Interfaces;
using DocParser.Logging;
using DocParser.Strings;
using System.Text;

namespace DocParser.Parsers
{
    /// <summary>
    /// Document parser for text documents.
    /// </summary>
    public class TextDocParser : DocParserBase, ITextDocParser
    {
        /// <inheritdoc/>
        public TextDocEncoding TextEncoding { get; set; } = TextDocEncoding.UTF8;

        /// <inheritdoc/>
        public override IEnumerable<string> GetDocLinks()
        {
            var docLinks = new List<string>();

            if (RawFile?.Length > 0)
            {
                try
                {
                    var fileText = string.Empty;

                    switch (TextEncoding)
                    {
                        case TextDocEncoding.UTF8:
                            fileText = Encoding.UTF8.GetString(RawFile);
                            break;

                        case TextDocEncoding.ASCII:
                            fileText = Encoding.ASCII.GetString(RawFile);
                            break;

                        case TextDocEncoding.Unicode:
                            fileText = Encoding.Unicode.GetString(RawFile);
                            break;

                        default:
                            Logger.LogWarning(SR.TextDocEncodingError);
                            break;
                    }

                    if (!string.IsNullOrEmpty(fileText))
                    {
                        // Get the unique document links
                        docLinks = DocParserHelper.ExtractUrlsFromText(fileText).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(string.Format(SR.DocParsingError, "text"), ex);
                }
            }

            return docLinks;
        }
    }
}
