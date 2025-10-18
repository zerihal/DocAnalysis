using DocParser.Interfaces;
using DocParser.Logging;
using DocParser.Strings;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;

namespace DocParser.Parsers
{
    public class WordDocParser : DocParserBase, IWordDocParser
    {
        /// <inheritdoc/>
        public override IEnumerable<string> GetDocLinks()
        {
            var docLinks = new List<string>();

            if (RawFile?.Length > 0)
            {
                try
                {
                    using (var memStream = new MemoryStream(RawFile))
                    {
                        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(memStream, false))
                        {
                            var mainPart = wordDoc.MainDocumentPart;

                            if (mainPart != null)
                            {
                                // First get the actual hyperlinks
                                foreach (var hyperlink in mainPart.HyperlinkRelationships)
                                {
                                    docLinks.Add(hyperlink.Uri.ToString());
                                }

                                // Now check for plaintext URLs and add them if not already added
                                var body = mainPart.Document.Body;

                                if (body != null)
                                {
                                    var docText = GetDocumentText(body);
                                    var plainTextLinks = DocParserHelper.ExtractUrlsFromText(docText);

                                    foreach (var plainLink in plainTextLinks)
                                    {
                                        // The actual hyperlink should be the full URL, whereas the plaintext one may not be, so
                                        // only add if the plaintext link is not contained within any of the full URLs (or not already
                                        // added of course)
                                        if (!docLinks.Any(l => l.Contains(plainLink, StringComparison.InvariantCultureIgnoreCase)))
                                            docLinks.Add(plainLink);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(string.Format(SR.DocParsingError, "Word"), ex);
                }
            }

            return docLinks;
        }

        /// <summary>
        /// Gets the plaintext from the document body, ensuring that new lines are respected.
        /// </summary>
        /// <param name="body">Document body.</param>
        /// <returns>Plaintext from document body.</returns>
        private string GetDocumentText(Body body)
        {
            var paragraphs = body.Elements<Paragraph>();
            var textBuilder = new StringBuilder();

            foreach (var paragraph in paragraphs)
            {
                textBuilder.AppendLine(paragraph.InnerText);
            }

            return textBuilder.ToString();
        }
    }
}
