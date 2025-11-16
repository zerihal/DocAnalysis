using DocParser.DocSearch;

namespace DocParser.Tests
{
    public class SearcherTests
    {
        [Fact]
        public async Task SingleFileSearcherTest()
        {
            var testFilePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "SearchTest1.docx");
            var searcher = new DocSearcher([testFilePath]);

            // Wait for files to be loaded
            while (!searcher.IsInitialised)
            {
                await Task.Delay(100);
            }

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
            while (!searcher.IsInitialised)
            {
                await Task.Delay(100);
            }

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
    }
}
