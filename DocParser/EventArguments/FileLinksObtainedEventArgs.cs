using System;
using System.Collections.Generic;
using System.Text;

namespace DocParser.EventArguments
{
    public class FileLinksObtainedEventArgs : LinksObtainedEventArgs
    {
        public string FileName { get; }

        public FileLinksObtainedEventArgs(string fileName, IEnumerable<string> links) : base(links)
        {
            FileName = fileName;
        }
    }
}
