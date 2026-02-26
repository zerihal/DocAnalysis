using DocParser.Interfaces;
using System.Net.Http.Headers;

namespace DocParser.Utilities
{
    /// <inheritdoc/>
    public class LinkInfo : ILinkInfo
    {
        /// <inheritdoc/>
        public bool IsReachable { get; }

        /// <inheritdoc/>
        public int ResponseStatusCode { get; }

        /// <inheritdoc/>
        public HttpResponseHeaders? ResponseHeaders { get; }

        /// <inheritdoc/>
        public string ResponseContentType { get; }

        /// <inheritdoc/>
        public string FinalUrlAfterRedirects { get; }

        /// <inheritdoc/>
        public string OriginalUrl { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="originalUrl">Original URL.</param>
        /// <param name="isReachable">Flag to indicate whether link can be successfully reached.</param>
        /// <param name="responseStatusCode">HTTP status code.</param>
        /// <param name="responseHeaders">HTTP response headers.</param>
        /// <param name="responseContentType">HTTP response content type.</param>
        /// <param name="finalUrlAfterRedirects">Final URL after any redirects.</param>
        public LinkInfo(string originalUrl, bool isReachable, int responseStatusCode, HttpResponseHeaders responseHeaders, string responseContentType, string finalUrlAfterRedirects)
        {
            OriginalUrl = originalUrl;
            IsReachable = isReachable;
            ResponseStatusCode = responseStatusCode;
            ResponseHeaders = responseHeaders;
            ResponseContentType = responseContentType;
            FinalUrlAfterRedirects = finalUrlAfterRedirects;
        }

        /// <summary>
        /// String representation of the link info, showing the original URL and whether it is active or inactive.
        /// </summary>
        /// <returns>Basic link info string.</returns>
        public override string ToString()
        {
            var active = IsReachable ? "Active" : "Inactive";
            return $"{OriginalUrl} ({active})";
        }

        /// <summary>
        /// Returns an instance of <see cref="ILinkInfo"/> with empty values for all properties other than the 
        /// original URL.
        /// </summary>
        /// <param name="url">Original URL.</param>
        /// <returns>Instance of <see cref="ILinkInfo"/> for non responsive links.</returns>
        public static LinkInfo NoResponseLink(string url)
        {
            return new LinkInfo(url, false, 0, null, string.Empty, string.Empty);
        }
    }
}
