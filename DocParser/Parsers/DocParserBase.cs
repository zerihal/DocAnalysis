using DocParser.DocOps;
using DocParser.EventArguments;
using DocParser.Interfaces;

namespace DocParser.Parsers
{
    /// <inheritdoc/>
    public abstract class DocParserBase : DocLoader, IDocParser
    {
        /// <inheritdoc/>
        public event EventHandler<LinksObtainedEventArgs>? LinksObtainedAsync;

        /// <inheritdoc/>
        public abstract IEnumerable<string> GetDocLinks();

        /// <inheritdoc/>
        public async Task GetDocLinksAsync()
        {
            var links = GetDocLinks();
            OnLinksObtained(new LinksObtainedEventArgs(links));
            await Task.CompletedTask;
        }

        private void OnLinksObtained(LinksObtainedEventArgs e) => LinksObtainedAsync?.Invoke(this, e);
    }
}
