using DocParser.Interfaces;
using System.Diagnostics;

namespace DocParser.Utilities
{
    public static class LinkUtils
    {
        /// <summary>
        /// HTTP client timeout in seconds for fetching link information. Default is 10 seconds.
        /// </summary>
        public static int TimeoutSeconds { get; set; } = 10;

        /// <summary>
        /// Gets information about a link by performing an HTTP request.
        /// </summary>
        /// <param name="url">Link URL.</param>
        /// <returns>Instance of <see cref="ILinkInfo"/> with information on link status and metadata.</returns>
        public static async Task<ILinkInfo> GetLinkInfoAsync(string url)
        {
            try
            {
                var originalUrl = url;

                if (TryNormalizeHttpUrl(url, out var normalizedUri))
                {
                    if (normalizedUri != null && normalizedUri.ToString() != originalUrl)
                        url = normalizedUri.ToString();
                }

                using var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(TimeoutSeconds)
                };

                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await client.SendAsync(request);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    request = new HttpRequestMessage(HttpMethod.Get, url);
                    response = await client.SendAsync(request);
                }

                if (response != null)
                {
                    return new LinkInfo(originalUrl, response.IsSuccessStatusCode, (int)response.StatusCode,
                        response.Headers, response.Content.Headers.ContentType?.ToString() ?? string.Empty,
                        response.RequestMessage?.RequestUri?.ToString() ?? string.Empty);
                }
                else
                {
                    return LinkInfo.NoResponseLink(originalUrl);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching link info for {url}: {ex.Message}");
                return LinkInfo.NoResponseLink(url);
            }
        }

        /// <summary>
        /// Gets information about a collection of links by performing HTTP requests.
        /// </summary>
        /// <param name="urls">Link URLs.</param>
        /// <returns>Collection of <see cref="ILinkInfo"/> with information on status and metadata.</returns>
        public static async Task<IEnumerable<ILinkInfo>> GetLinksInfoAsync(IEnumerable<string> urls)
        {
            var tasks = urls.Select(GetLinkInfoAsync);
            return await Task.WhenAll(tasks);
        }

        private static bool TryNormalizeHttpUrl(string input, out Uri? uri)
        {
            uri = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            // Protocol-relative: //example.com
            if (input.StartsWith("//"))
                input = "https:" + input;

            // Try absolute first
            if (Uri.TryCreate(input, UriKind.Absolute, out var absolute))
            {
                if (absolute.Scheme == Uri.UriSchemeHttp ||
                    absolute.Scheme == Uri.UriSchemeHttps)
                {
                    uri = absolute;
                    return true;
                }

                return false;
            }

            // Missing scheme like example.com
            if (Uri.TryCreate("https://" + input, UriKind.Absolute, out absolute))
            {
                uri = absolute;
                return true;
            }

            return false;
        }
    }
}
