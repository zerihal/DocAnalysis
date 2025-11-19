using DocParser.DocSearch;
using DocParser.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DocParser.ExtensionMethods
{
    /// <summary>
    /// File extension methods.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Converts a collection of IFormFile to a collection of <see cref="IFormFileStream"/> for processing by 
        /// instance of <see cref="IDocSearcher"/>.
        /// </summary>
        /// <param name="files">Collection of <see cref="IFormFile"/>.</param>
        /// <returns>Collection of <see cref="IFormFileStream"/>.</returns>
        public static IEnumerable<IFormFileStream> ToFormFileStreams(this IEnumerable<IFormFile> files)
        {
            var filesList = new List<IFormFileStream>();

            foreach (var file in files)
                filesList.Add(new FormFileStream(file));

            return filesList;
        }
    }
}
