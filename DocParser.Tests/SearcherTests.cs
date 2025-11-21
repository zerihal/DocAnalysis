using DocParser.DocSearch;
using DocParser.ExtensionMethods;
using DocParser.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace DocParser.Tests
{
    public class SearcherTests
    {
        private const int MaxDelayAwaits = 50; // Each delay is 100ms, so max (100ms x 50) is approx 5 seconds

        [Fact]
        public async Task FormFileSearcerTest()
        {
            var testFilePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "SearchTest1.docx");
            var binFile = File.ReadAllBytes(testFilePath);
            var stream = new MemoryStream(binFile);

            // First check with a binary file with plain text content type
            await DoSearch(stream, "text/plain");

            // Next check with different content type (Word specific)
            stream.Position = 0;
            await DoSearch(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            async Task DoSearch(MemoryStream stream, string contentType)
            {
                // Create an instance of IFormFile
                var formFile = new FormFile(stream, 0, stream.Length, "file", "SearchTest1.docx")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };

                var formFiles = new List<IFormFile> { formFile };
                var formFileStreams = formFiles.ToFormFileStreams();

                var searcher = new DocSearcher(formFileStreams);

                // Wait for files to be loaded
                await SearcherInit(searcher);

                Assert.NotNull(searcher);
                Assert.Single(searcher.RawFiles);

                var searchResults = await searcher.Search("regarding cats");
                Assert.Equal(24, searchResults.DiscoveredCount);
            }
        }

        [Fact]
        public async Task SingleFileSearcherTest()
        {
            var testFilePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "SearchTest1.docx");
            var searcher = new DocSearcher([testFilePath]);

            // Wait for files to be loaded
            await SearcherInit(searcher);

            Assert.True(searcher.IsInitialised);
            Assert.Single(searcher.RawFiles);

            var searchResults = await searcher.Search("regarding cats");
            Assert.Equal(24, searchResults.DiscoveredCount);

            searchResults = null;
            testFilePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "SearchTest3.odt");
            await searcher.LoadFiles([testFilePath]);
            Assert.True(searcher.IsInitialised);
            Assert.Single(searcher.RawFiles);

            searchResults = await searcher.Search("regarding cats");
            Assert.Equal(24, searchResults.DiscoveredCount);
        }

        [Fact]
        public async Task AdvancedSingleFileSearcherTest()
        {
            var advSearch1 = new[] { "cats", "behavior" };
            var advSearch2 = new[] { "cats", "stress" };

            var testFilePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "SearchTest1.docx");
            var searcher = new DocSearcher(new[] { testFilePath });

            // Wait for files to be loaded
            await SearcherInit(searcher);

            Assert.True(searcher.IsInitialised);
            Assert.Single(searcher.RawFiles);

            var searchResults = await searcher.AdvancedSearch(advSearch1);

            // There should be 10 results from this test file and these should be ordered by highest match
            // rating first - check that this is the case (highest match rating for this file is 3.02).
            Assert.Equal(searchResults?.Count(), 10);
            Assert.Equal(searchResults?.FirstOrDefault()?.MatchRating, (decimal)3.02);
        }

        [Fact]
        public async Task MultiFileSearcherTest()
        {
            // ToDo: Test to be completed for mutliple file search with single search string.

            await Task.CompletedTask;
            Assert.True(true);
        }

        [Fact]
        public async Task MultiFileAdvSearcherTest()
        {
            // ToDo: Test to be completed for multiple file search with multiple search terms.

            await Task.CompletedTask;
            Assert.True(true);
        }

        private async Task SearcherInit(IDocSearcher searcher)
        {
            var delayAwaits = 0;
            var delayInterval = 100;

            while (!searcher.IsInitialised)
            {
                if (delayAwaits >= MaxDelayAwaits)
                    break;

                await Task.Delay(delayInterval);
                delayAwaits += delayInterval;
            }

            Assert.True(searcher.IsInitialised, "Searcher initialisation timed out (returned false)");
        }
    }
}
