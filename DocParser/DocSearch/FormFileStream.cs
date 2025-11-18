using DocParser.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DocParser.DocSearch
{
    /// <inheritdoc cref="IFormFileStream"/>
    public class FormFileStream : IFormFileStream
    {
        /// <inheritdoc/>
        public string FormFileName { get; }

        /// <inheritdoc/>
        public Stream FileStream { get; }

        /// <summary>
        /// Creates a new instance of <see cref="IFormFileStream"/> from input stream and name.
        /// </summary>
        /// <param name="stream">FormFile stream.</param>
        /// <param name="formFileName">FormFile name.</param>
        public FormFileStream(Stream stream, string formFileName)
        {
            FormFileName = formFileName;
            FileStream = stream;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IFormFileStream"/> from <see cref="IFormFile"/>.
        /// </summary>
        /// <param name="formFile"><see cref="IFormFile"/> to create wrapper for.</param>
        public FormFileStream(IFormFile formFile)
        {
            FormFileName = formFile.FileName;
            FileStream = formFile.OpenReadStream();
        }
    }
}
