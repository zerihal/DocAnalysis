using DocParser.Factories;
using DocParser.Interfaces;

namespace DocParser.Tests
{
    public class ParserFactoryTests
    {
        [Fact]
        public void FactoryTests()
        {
            var testWinWordDoc = @"x:\Docs\TestDoc.docx";
            var testWinTextDoc = @"x:\Docs\TestDoc.txt";
            var testLinuxWordDoc = @"/home/Docs/TestDoc.docx";
            var testLinuxTextDoc = @"/home/Docs/TestDoc.txt";

            // Windows paths
            Assert.IsAssignableFrom<IWordDocParser>(DocParserFactory.CreateDocParserForFile(testWinWordDoc));
            Assert.IsAssignableFrom<ITextDocParser>(DocParserFactory.CreateDocParserForFile(testWinTextDoc));
            Assert.IsNotAssignableFrom<IWordDocParser>(DocParserFactory.CreateDocParserForFile(testWinTextDoc));
            Assert.IsNotAssignableFrom<ITextDocParser>(DocParserFactory.CreateDocParserForFile(testWinWordDoc));

            // Linux paths
            Assert.IsAssignableFrom<IWordDocParser>(DocParserFactory.CreateDocParserForFile(testLinuxWordDoc));
            Assert.IsAssignableFrom<ITextDocParser>(DocParserFactory.CreateDocParserForFile(testLinuxTextDoc));
            Assert.IsNotAssignableFrom<IWordDocParser>(DocParserFactory.CreateDocParserForFile(testLinuxTextDoc));
            Assert.IsNotAssignableFrom<ITextDocParser>(DocParserFactory.CreateDocParserForFile(testLinuxWordDoc));
        }
    }
}
