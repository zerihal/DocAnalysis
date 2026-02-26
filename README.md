## Assembly for Document Analysis Tools

![CI](https://github.com/zerihal/DocAnalysis/actions/workflows/CI.yml/badge.svg)

Functions include document parsing to extract hyperlinks from single or multiple documents, and document search methods to find single or multiple terms within one or more documents, returning an object with the terms within the sentence, paragraph, and document, giving the index within the paragraph for each. Advanced search functions allow for searching multiple terms and giving a match rating for each paragraphs that contains these (the higher the rating, the better the match over all given terms).

**Example – DocAnalysis Usage**

```csharp
// *** Document parser example code for loading a document and parsing it for links ***

// Create an instance or IDocParser (relevant parser implementation is returned for the file type specified) with document loaded.
var docParser = DocParserFactory.CreateDocParserForFile(@"C\SomePath\SomeDocument.docx")

// Alternatively create instance of IDocParser directly and load the file
var _wordDocParser = new WordDocParser();
var fileLoaded = _wordDocParser.LoadFile(@"C\SomePath\SomeDocument.docx");

// Parse the document for links.
var docLinks = _wordDocParser.GetDocLinks();

// Alternatively get document links async and hook event for results (good for large files).
_wordDocParser.LinksObtainedAsync += (o, e) =>
{
    var links = e.Links;
};

_ = _wordDocParser.GetDocLinksAsync();

// *** Document searcher example code for searching one or more documents for key terms ***

// Create instance of IDocSearcher for files specified (can also be streams or binary objects).
var searcher = new DocSearcher([@"C\SomePath\SomeDocument.docx", @"C\SomePath\SomeDocument2.docx"]);

// Wait for initialisation (or hook initialised event).
while (!searcher.IsInitialised)
{
    await Task.Delay(10);
}

// Get ISearchResults, which contains search results showing matched terms within paragraph, sentence,
// and position (start index) in paragraph.
var searchResults = await searcher.Search("this document");

// Alternatively an advanced search can be performed to look for multiple terms. This will return the
// ISearchResults along with match accuracy and rating per paragraph (the higher the rating, the closer
// the match or more matched terms within the block).
var advSearchResults = await searcher.AdvancedSearch(["this", "document"]);
