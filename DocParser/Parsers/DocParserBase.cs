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
        public abstract string[] ApplicableFileTypes { get; }

        /// <inheritdoc/>
        public abstract IEnumerable<string> GetDocLinks();

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetDocLinksAsync()
        {
            var links = GetDocLinks();
            OnLinksObtained(new LinksObtainedEventArgs(links));
            await Task.CompletedTask;
            return links;
        }

        /// <inheritdoc/>
        public virtual bool IsApplicableForFile(string fileOrExt)
        {
            return ApplicableFileTypes.Contains(Path.GetExtension(fileOrExt).ToLowerInvariant());
        }

        private void OnLinksObtained(LinksObtainedEventArgs e) => LinksObtainedAsync?.Invoke(this, e);
    }
}
