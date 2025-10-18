using DocParser.DocSearch;
using DocParser.Interfaces;
using DocParser.Parsers;

namespace DocParser.Tests
{
    public class ParserTests
    {
        private static IList<string> _expectedLinks = new List<string>
        {
            "https://open-meteo.com/",
            "https://www.google.co.uk/",
            "www.bbc.co.uk",
            "https://en.wikipedia.org/wiki/Lists_of_earthquakes"
        };

        private IWordDocParser _wordDocParser = new WordDocParser();
        private ITextDocParser _textDocParser = new TextDocParser();

        [Fact]
        public void WordDocParserTests()
        {
            var testFilePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "WordTestDoc1.docx");

            // First check that the test file exists (should be within test assembly content)
            Assert.True(File.Exists(testFilePath), $"Test file not found: {testFilePath}");

            var fileLoaded = _wordDocParser.LoadFile(testFilePath);
            Assert.True(fileLoaded);

            // The added links have a different evaluated hyperlink than the text value, such as added /
            // or full URL with http://, so these should take preference when being added to the doc links
            var expectedWordLinks = new List<string>(_expectedLinks);
            Assert.NotNull(expectedWordLinks);
            expectedWordLinks.Add("http://www.helloworld.com/");
            expectedWordLinks.Add("http://www.news.sky.com/uk");

            var docLinks = _wordDocParser.GetDocLinks();
            Assert.Equal(expectedWordLinks.Count, docLinks.Count());

            // The doc links returned should contain all the expected links
            foreach (var link in docLinks)
            {
                Assert.Contains(link, expectedWordLinks);
            }
        }

        [Fact]
        public void TextDocParserTests()
        {
            var testFilePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "TextTestDoc1.txt");

            // First check that the test file exists (should be within test assembly content)
            Assert.True(File.Exists(testFilePath), $"Test file not found: {testFilePath}");

            var fileLoaded = _textDocParser.LoadFile(testFilePath);
            Assert.True(fileLoaded);

            var expectedTextLinks = new List<string>(_expectedLinks);
            Assert.NotNull(expectedTextLinks);
            expectedTextLinks.Add("http://www.helloworld.com");
            expectedTextLinks.Add("www.news.sky.com/uk");

            var docLinks = _textDocParser.GetDocLinks();
            Assert.Equal(expectedTextLinks.Count, docLinks.Count());

            // The doc links returned should contain all the expected links
            foreach (var link in docLinks)
            {
                Assert.Contains(link, expectedTextLinks);
            }
        }

        [Fact]
        public void SearchTest()
        {
            var searchResults = new SearchResults("test")
            {
                new SearchResult("This is a test.", 10, "This is a test. Some other text", 1, 1, @"C:\DummyDoc.txt"),
                new SearchResult("This is another test.", 16, "This is another test. Some more other text", 1, 1, @"C:\DummyDoc.txt")
            };

            Assert.Equal(2, searchResults.DiscoveredCount);
            Assert.Equal("This is another test.", searchResults.ToArray()[1].Sentence);
            Assert.Contains(searchResults, result => result.Sentence == "This is a test.");
        }
    }
}
