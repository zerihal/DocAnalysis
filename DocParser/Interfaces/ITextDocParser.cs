using DocParser.Enums;

namespace DocParser.Interfaces
{
    /// <summary>
    /// Document parser for generic text documents.
    /// </summary>
    public interface ITextDocParser : IDocParser
    {
        /// <summary>
        /// Gets or sets the <see cref="TextDocEncoding"/> to be used (default is UTF8).
        /// </summary>
        TextDocEncoding TextEncoding { get; set; }
    }
}
