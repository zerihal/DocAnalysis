using System.Net.Http.Headers;

namespace DocParser.Interfaces
{
    /// <summary>
    /// Link information as a result of link checking along with any relevant metadata.
    /// </summary>
    public interface ILinkInfo
    {
        /// <summary>
        /// Indicates whether the link is reachable (i.e., returns a successful HTTP status code).
        /// </summary>
        public bool IsReachable { get; }

        /// <summary>
        /// Http status code returned when attempting to access the link.
        /// </summary>
        public int ResponseStatusCode { get; }

        /// <summary>
        /// Http response headers returned when attempting to access the link (if any).
        /// </summary>
        public HttpResponseHeaders? ResponseHeaders { get; }

        /// <summary>
        /// Http response content type returned when attempting to access the link (if any).
        /// </summary>
        public string ResponseContentType { get; }

        /// <summary>
        /// The final URL after following any redirects (if applicable). If the link is not reachable 
        /// or there are no redirects, this will be the same as the original URL.
        /// </summary>
        public string FinalUrlAfterRedirects { get; }

        /// <summary>
        /// Original URL for this instance of LinkInfo.
        /// </summary>
        public string OriginalUrl { get; }
    }
}
