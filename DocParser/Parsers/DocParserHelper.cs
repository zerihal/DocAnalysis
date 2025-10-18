using System.Text.RegularExpressions;

namespace DocParser.Parsers
{
    public static class DocParserHelper
    {
        // Matches http, https, and www. links
        private const string UrlPattern = @"(https?:\/\/[^\s]+|www\.[^\s]+)";

        /// <summary>
        /// Extracts all plaintext URLs (including those without http:) from file text.
        /// </summary>
        /// <param name="text">File text.</param>
        /// <returns>List of URLs found.</returns>
        public static List<string> ExtractUrlsFromText(string text)
        {
            List<string> urls = new List<string>();

            MatchCollection matches = Regex.Matches(text, UrlPattern, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }

        /// <summary>
        /// Tries to obtain document text from a file stream, outputting error message if unobtainable.
        /// </summary>
        /// <param name="fileStream">Filestream to try and obtain file text from.</param>
        /// <param name="error">Error message (or null if no error).</param>
        /// <returns>File text if obtainable, or null if not.</returns>
        public static string TryGetDocText(Stream fileStream, out string? error)
        {
            error = null;

            try
            {
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                error = $"Document unsupported or read error ({e.Message})";
            }

            return string.Empty;
        }

        /// <summary>
        /// Tries to obtain document text from a file, outputting error message if unobtainable.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="error">Error message (or null if no error).</param>
        /// <returns>File text if obtainable, or null if not.</returns>
        public static string TryGetDocText(string filePath, out string? error)
        {
            var docText = string.Empty;
            error = null;
            FileStream fileStream = null;

            if (File.Exists(filePath))
            {
                try
                {
                    fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    docText = TryGetDocText(fileStream, out error);
                }
                catch (Exception e)
                {
                    error = $"Document read error ({e.Message})";
                }
                finally
                {
                    fileStream?.Close();
                }
            }
            else
            {
                error = "File does not exist";
            }

            return docText;
        }
    }
}
