using DocParser.Interfaces;
using System.IO.Compression;
using System.Text;

namespace DocParser.DocOps
{
    public class DocLoader : IDocLoader
    {
        private const string OdtExt = ".odt";
        private const string OdtArchiveContentEntry = "content.xml";

        /// <inheritdoc/>
        public byte[]? RawFile { get; protected set; }

        /// <inheritdoc/>
        public virtual bool LoadFile(string filename)
        {
            RawFile = null;

            if (File.Exists(filename))
            {
                var fileBytes = File.ReadAllBytes(filename);

                if (string.Equals(Path.GetExtension(filename), OdtExt, StringComparison.OrdinalIgnoreCase))
                {
                    using var memoryStream = new MemoryStream(fileBytes); // raw ODT bytes
                    using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

                    if (archive.GetEntry(OdtArchiveContentEntry) is ZipArchiveEntry contentEntry)
                    {
                        using var contentStream = contentEntry.Open();
                        using var reader = new StreamReader(contentStream, Encoding.UTF8);
                        RawFile = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                    }
                }
                else
                {
                    RawFile = fileBytes;
                }

                return RawFile?.Length > 0;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual bool LoadFile(Stream fileStream)
        {
            RawFile = null;

            using (var memStream = new MemoryStream())
            {
                try
                {
                    fileStream.CopyTo(memStream);
                    RawFile = memStream.ToArray();
                }
                catch
                {
                    return false;
                }
            }

            return RawFile?.Length > 0;
        }
    }
}
